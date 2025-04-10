import { Server } from "socket.io";
import generateRuneMap, { generateNewRune } from "./game_logics/game_map.js";
import EVENTS from "./event/event.js";
import express from "express";
import config from "./config/keys.js";
import authenticationRoutes from "./routes/authenticationRoutes.js";
import registationRoutes from "./routes/registationRoutes.js";
import bodyParser from "body-parser";
import jwt from "jsonwebtoken";
import getRandomInt from "./utils/random.js";
import { handle_find_match_event } from "./event/handle_find_match_event.js";
import { handle_game_start_event } from "./event/handle_game_start_event.js";
import update_turn_monster from "./game_logics/turn_monster.js";
import { handle_monster_action_event } from "./event/handle_monster_action_event.js";
import monster_update_effect from "./game_logics/monster_update_effect.js";
import { handle_pick_monster_event } from "./event/handle_pick_monster_event.js";
//login
const app = express();
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
authenticationRoutes(app);
registationRoutes(app);
const server = app.listen(config.port, () => {
    console.log(`Express server is running on port ${config.port}`);
});

//socket logic game
const io = new Server(server);
// middleware được gọi khi 1 socket cố kết nối với server
io.use((socket, next) => {
    const token = socket.handshake.query.token;
    if (!token) {
        return next(new Error("Authentication error"));
    }

    jwt.verify(token, config.jwtSecret, (err, decoded) => {
        if (err) {
            return next(new Error("Invalid token"));
        }
        socket.username = decoded.username;
        next();
    });
});

let queuePlayerWaiting = [];
let roomsPlaying = {};
io.on("connection", (socket) => {
    console.log(`Player connect: ${socket.id}`);

    socket.on(EVENTS.PLAYER.FIND_MATCH, (playerData) => {
        handle_find_match_event(io, socket, playerData, queuePlayerWaiting, roomsPlaying);
    });
    socket.on(EVENTS.GAME.PICK_MONSTER_POST, (data) => {
        console.log(data);
        handle_pick_monster_event(io, socket, roomsPlaying, data);
    });
    socket.on(EVENTS.GAME.PICK_MONSTER_CONFIRM_POST, (data) => {
        socket.to(socket.roomId).emit(EVENTS.GAME.PICK_MONSTER_CONFIRM_PUSH);
        let isPickAll = true;
        roomsPlaying[socket.roomId].player1.monsters.forEach((mon) => {
            if (mon.data == null) {
                isPickAll = false;
            } else {
                mon.is_locked = true;
            }
        });
        roomsPlaying[socket.roomId].player2.monsters.forEach((mon) => {
            if (mon.data == null) {
                isPickAll = false;
            } else {
                mon.is_locked = true;
            }
        });
        if (isPickAll) {
            const fastestMonster1 = roomsPlaying[socket.roomId].player1.monsters.reduce((fastest, mon) => {
                if (!mon || !mon.data) return fastest;
                return !fastest || mon.data.stats.speed > fastest.data.stats.speed ? mon : fastest;
            }, null);
            const fastestMonster2 = roomsPlaying[socket.roomId].player2.monsters.reduce((fastest, mon) => {
                if (!mon || !mon.data) return fastest;
                return !fastest || mon.data.stats.speed > fastest.data.stats.speed ? mon : fastest;
            }, null);

            roomsPlaying[socket.roomId].turn_base_data.push({
                id_in_battle: "01",
                speed: fastestMonster1.data.stats.speed + 20,
                progress: 0,
            });
            roomsPlaying[socket.roomId].turn_base_data.push({
                id_in_battle: "02",
                speed: fastestMonster2.data.stats.speed + 20,
                progress: 0,
            });
            console.log(roomsPlaying[socket.roomId].player1.monsters[0].data.id + " " + roomsPlaying[socket.roomId].player1.monsters[1].data.id + " " + roomsPlaying[socket.roomId].player1.monsters[2].data.id);
            console.log(roomsPlaying[socket.roomId].player2.monsters[0].data.id + " " + roomsPlaying[socket.roomId].player2.monsters[1].data.id + " " + roomsPlaying[socket.roomId].player2.monsters[2].data.id);
            io.in(socket.roomId).emit(EVENTS.GAME.END_PICK_MONSTER);
        }
    });

    socket.on(EVENTS.GAME.GAME_START_REQUEST, (data) => {
        if (roomsPlaying[socket.roomId].player1.socket == socket) {
            roomsPlaying[socket.roomId].player1.ready = true;
        }
        if (roomsPlaying[socket.roomId].player2.socket == socket) {
            roomsPlaying[socket.roomId].player2.ready = true;
        }

        if (roomsPlaying[socket.roomId].player1.ready && roomsPlaying[socket.roomId].player2.ready) {
            // gửi map
            // gửi turn list
            handle_game_start_event(io, socket, roomsPlaying);
        }
    });

    socket.on(EVENTS.RUNE.SWAP_RUNE, (data) => {
        const swapRuneData = JSON.parse(data);
        console.log(socket.roomId);
        socket.to(socket.roomId).emit(EVENTS.RUNE.OPPONENT_SWAP_RUNE, swapRuneData);
        console.log(swapRuneData);
    });

    socket.on(EVENTS.RUNE.NEW_REQUEST, (data) => {
        const mapData = JSON.parse(data);
        io.to(socket.roomId).emit(EVENTS.RUNE.NEW_RESPONSE, generateNewRune(mapData));
    });

    socket.on(EVENTS.GAME.TURN_BASE_LIST_REQUEST, (data) => {
        console.log("turn request");
        update_turn_monster(roomsPlaying[socket.roomId]);
        io.to(socket.roomId).emit(EVENTS.GAME.TURN_BASE_LIST_PUSH_DATA, roomsPlaying[socket.roomId].turn_base_data);
    });

    socket.on(EVENTS.MONSTER.MONSTER_ACTION_REQUEST, (data) => {
        handle_monster_action_event(io, socket, roomsPlaying, data);
    });

    socket.on(EVENTS.MONSTER.UPDATE_EFFECT_REQUEST, (data) => {
        const monsterData = data;
        let monster;
        if (monsterData[0] == "1") {
            console.log("player 1");
            monster = roomsPlaying[socket.roomId].player1.monsters.find((monster) => monster.id_in_battle == monsterData);
        } else {
            console.log("player 2");
            monster = roomsPlaying[socket.roomId].player2.monsters.find((monster) => monster.id_in_battle == monsterData);
        }
        console.log("monster: " + monster);
        const response = monster_update_effect(monster);
        io.in(socket.roomId).emit(EVENTS.MONSTER.UPDATE_EFFECT_RESPONSE, response);
    });

    socket.on("disconnect", (data) => {
        console.log("Disconnect");
        for (let roomId in roomsPlaying) {
            if (roomsPlaying[roomId].player1.socket.id === socket.id) {
                delete roomsPlaying[roomId];
                break;
            }
            if (roomsPlaying[roomId].player2.socket.id === socket.id) {
                delete roomsPlaying[roomId];
                break;
            }
        }
    });
});
console.log("Server Start");
