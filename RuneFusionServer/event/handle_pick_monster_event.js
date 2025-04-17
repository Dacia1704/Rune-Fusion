import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
import mongoose from "mongoose";
import config from "../config/keys.js";
import Monster from "../model/Monster.js";

mongoose.connect(config.mongoURI + "RuneFushion");
export async function handle_pick_monster_event(io, socket, roomsPlaying, data) {
    let monsterData = JSON.parse(data);

    let indexMonsterDataPlayer1Unlock = roomsPlaying[socket.roomId].player1.monsters.findIndex((monster) => monster.is_locked == false);
    let indexMonsterDataPlayer2Unlock = roomsPlaying[socket.roomId].player2.monsters.findIndex((monster) => monster.is_locked == false);

    for (const id of monsterData.player1) {
        const mon = await Monster.findOne({ id: id });
        if (id != -1 && indexMonsterDataPlayer1Unlock != -1) {
            console.log("index: " + indexMonsterDataPlayer1Unlock);
            roomsPlaying[socket.roomId].player1.monsters[indexMonsterDataPlayer1Unlock].data = mon;
            const monsterExists = roomsPlaying[socket.roomId].monster_base_data.some((m) => m.id === mon.id);
            if (!monsterExists) {
                roomsPlaying[socket.roomId].monster_base_data.push(mon);
            }
            indexMonsterDataPlayer1Unlock++;
        }
    }
    for (const id of monsterData.player2) {
        const mon = await Monster.findOne({ id: id });
        if (id != -1 && mon != undefined && indexMonsterDataPlayer2Unlock != -1) {
            console.log("index: " + indexMonsterDataPlayer2Unlock);
            roomsPlaying[socket.roomId].player2.monsters[indexMonsterDataPlayer2Unlock].data = mon;

            const monsterExists = roomsPlaying[socket.roomId].monster_base_data.some((m) => m.id === mon.id);
            if (!monsterExists) {
                roomsPlaying[socket.roomId].monster_base_data.push(mon);
            }
            indexMonsterDataPlayer2Unlock++;
        }
    }
    console.log(roomsPlaying[socket.roomId].player1.monsters[0].data?.id + " " + roomsPlaying[socket.roomId].player1.monsters[1].data?.id + " " + roomsPlaying[socket.roomId].player1.monsters[2].data?.id);
    console.log(roomsPlaying[socket.roomId].player2.monsters[0].data?.id + " " + roomsPlaying[socket.roomId].player2.monsters[1].data?.id + " " + roomsPlaying[socket.roomId].player2.monsters[2].data?.id);
    // const monsterIdsPlayer1 = monsterData.player1.map((mon) => (mon == null || mon == undefined ? -1 : mon.id));
    // const monsterIdsPlayer2 = monsterData.player2.map((mon) => (mon == null || mon == undefined ? -1 : mon.id));

    const picked_monsters = [];
    roomsPlaying[socket.roomId].player1.monsters.forEach((mon) => {
        if (mon.data != null) {
            picked_monsters.push(mon.data.id);
        }
    });
    roomsPlaying[socket.roomId].player2.monsters.forEach((mon) => {
        if (mon.data != null) {
            picked_monsters.push(mon.data.id);
        }
    });
    socket.to(socket.roomId).emit(EVENTS.GAME.PICK_MONSTER_PUSH, {
        player1: monsterData.player1,
        player2: monsterData.player2,
        picked_monsters: picked_monsters,
    });
}
