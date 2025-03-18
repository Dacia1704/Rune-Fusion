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

    // socket.on(EVENTS.PLAYER.FIND_MATCH, (playerData) => {
    //   // const data = JSON.parse(playerData);
    //   // console.log(data);
    //   // queuePlayerWaiting.push({ socket, data });
    //   // if (queuePlayerWaiting.length >= 2) {
    //   //   const player1 = queuePlayerWaiting.shift();
    //   //   const player2 = queuePlayerWaiting.shift();
    //   //   const roomId = `room-${player1.socket.id}-${player2.socket.id}`;
    //   //   roomsPlaying[roomId] = { player1, player2 };

    //   //   roomsPlaying[roomId] = {
    //   //     player1: {
    //   //       socket: player1.socket,
    //   //       data: player1.data,
    //   //       monsters: [],
    //   //     },
    //   //     player2: {
    //   //       socket: player2.socket,
    //   //       data: player2.data,
    //   //       monsters: [],
    //   //     },
    //   //     turn_base_data:[]
    //   //   };

    //   //   player1.socket.join(roomId);
    //   //   player2.socket.join(roomId);
    //   //   player1.socket.roomId = roomId;
    //   //   player2.socket.roomId = roomId;

    //   //   const player1Data = {
    //   //     id: player1.data.id,
    //   //     playername: player1.data.playername,
    //   //     playerindex: 0,
    //   //   };
    //   //   const player2Data = {
    //   //     id: player2.data.id,
    //   //     playername: player2.data.playername,
    //   //     playerindex: 1,
    //   //   };
    //   //   io.to(roomId).emit(EVENTS.PLAYER.MATCH_FOUND, {
    //   //     roomId: roomId,
    //   //     player1: player1Data,
    //   //     player2: player2Data,
    //   //   });

    //   //   console.log(
    //   //     `Ghép cặp: ${player1.data.playername} vs ${player2.data.playername} (Room: ${roomId})`
    //   //   );

    //   //   const mapData = {
    //   //     rows: 5,
    //   //     cols: 6,
    //   //     numTypes: 5,
    //   //   };
    //   //   io.to(roomId).emit(
    //   //     EVENTS.RUNE.GENERATE_START_MAP,
    //   //     generateRuneMap(mapData)
    //   //   );
    //   //   console.log(generateRuneMap(mapData));

    //   //   io.to(roomId).emit(EVENTS.PLAYER.CURRENT_TURN, getRandomInt(0, 1));
    //   // }
    // });

    socket.on(EVENTS.PLAYER.FIND_MATCH, (playerData) => {
        handle_find_match_event(
            io,
            socket,
            playerData,
            queuePlayerWaiting,
            roomsPlaying
        );
    });

    socket.on(EVENTS.GAME.GAME_START_REQUEST, (data) => {
        if (roomsPlaying[socket.roomId].player1.socket == socket) {
            roomsPlaying[socket.roomId].player1.ready = true;
        }
        if (roomsPlaying[socket.roomId].player2.socket == socket) {
            roomsPlaying[socket.roomId].player2.ready = true;
        }

        if (
            roomsPlaying[socket.roomId].player1.ready &&
            roomsPlaying[socket.roomId].player2.ready
        ) {
            // gửi map
            // gửi turn list
            handle_game_start_event(io, socket, roomsPlaying);
        }
    });

    socket.on(EVENTS.RUNE.SWAP_RUNE, (data) => {
        const swapRuneData = JSON.parse(data);
        console.log(socket.roomId);
        socket
            .to(socket.roomId)
            .emit(EVENTS.RUNE.OPPONENT_SWAP_RUNE, swapRuneData);
        console.log(swapRuneData);
    });

    socket.on(EVENTS.RUNE.NEW_REQUEST, (data) => {
        const mapData = JSON.parse(data);
        io.to(socket.roomId).emit(
            EVENTS.RUNE.NEW_RESPONSE,
            generateNewRune(mapData)
        );
    });

    socket.on(EVENTS.GAME.TURN_BASE_LIST_REQUEST, (data) => {
        console.log("turn request");
        update_turn_monster(roomsPlaying[socket.roomId]);
        io.to(socket.roomId).emit(
            EVENTS.GAME.TURN_BASE_LIST_PUSH_DATA,
            roomsPlaying[socket.roomId].turn_base_data
        );
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
