import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
import { archerMonsterData, armoredAxemanData, knightData, lancerData, priestData, wizardData } from "../model/defaultMonsterData.js";
export function handle_pick_monster_event(io, socket, roomsPlaying, data) {
    let monsterData = JSON.parse(data);
    // console.log(monsterData);

    let indexMonsterDataPlayer1Unlock = roomsPlaying[socket.roomId].player1.monsters.findIndex((monster) => monster.is_locked == false);
    let indexMonsterDataPlayer2Unlock = roomsPlaying[socket.roomId].player2.monsters.findIndex((monster) => monster.is_locked == false);

    monsterData.player1.forEach((mon) => {
        if (mon != null && mon != undefined && indexMonsterDataPlayer1Unlock != -1) {
            console.log("index: " + indexMonsterDataPlayer1Unlock);
            roomsPlaying[socket.roomId].player1.monsters[indexMonsterDataPlayer1Unlock].data = mon;
            roomsPlaying[socket.roomId].monster_base_data.push(mon);
            roomsPlaying[socket.roomId].turn_base_data.push({
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[indexMonsterDataPlayer1Unlock].id_in_battle,
                speed: mon.stats.speed,
                progress: 0,
            });
            indexMonsterDataPlayer1Unlock++;
        }
    });
    monsterData.player2.forEach((mon) => {
        if (mon != null && mon != undefined && indexMonsterDataPlayer2Unlock != -1) {
            console.log("index: " + indexMonsterDataPlayer2Unlock);
            roomsPlaying[socket.roomId].player2.monsters[indexMonsterDataPlayer2Unlock].data = mon;
            roomsPlaying[socket.roomId].monster_base_data.push(mon);
            roomsPlaying[socket.roomId].turn_base_data.push({
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[indexMonsterDataPlayer2Unlock].id_in_battle,
                speed: mon.stats.speed,
                progress: 0,
            });
            indexMonsterDataPlayer2Unlock++;
        }
    });
    console.log(roomsPlaying[socket.roomId].player1.monsters[0].data?.id + " " + roomsPlaying[socket.roomId].player1.monsters[1].data?.id + " " + roomsPlaying[socket.roomId].player1.monsters[2].data?.id);
    console.log(roomsPlaying[socket.roomId].player2.monsters[0].data?.id + " " + roomsPlaying[socket.roomId].player2.monsters[1].data?.id + " " + roomsPlaying[socket.roomId].player2.monsters[2].data?.id);
    const monsterIdsPlayer1 = monsterData.player1.map((mon) => (mon == null || mon == undefined ? -1 : mon.id));
    const monsterIdsPlayer2 = monsterData.player2.map((mon) => (mon == null || mon == undefined ? -1 : mon.id));

    socket.to(socket.roomId).emit(EVENTS.GAME.PICK_MONSTER_PUSH, {
        player1: monsterIdsPlayer1,
        player2: monsterIdsPlayer2,
    });
}
