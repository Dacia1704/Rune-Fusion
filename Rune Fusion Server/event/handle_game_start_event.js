import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
import { archerMonsterData, armoredAxemanData, knightData, lancerData, priestData, wizardData } from "../model/defaultMonsterData.js";
export function handle_game_start_event(io, socket, roomsPlaying) {
    //fake data
    // roomsPlaying[socket.roomId] = {
    //     player1: {
    //         socket: roomsPlaying[socket.roomId].player1.socket,
    //         data: roomsPlaying[socket.roomId].player1.data,
    //         monsters: [
    //             {
    //                 id_in_battle: "11",
    //                 data: archerMonsterData,
    //             },
    //             {
    //                 id_in_battle: "12",
    //                 data: armoredAxemanData,
    //             },
    //             {
    //                 id_in_battle: "13",
    //                 data: knightData,
    //             },
    //         ],
    //         ready: false,
    //     },
    //     player2: {
    //         socket: roomsPlaying[socket.roomId].player2.socket,
    //         data: roomsPlaying[socket.roomId].player2.data,
    //         monsters: [
    //             {
    //                 id_in_battle: "21",
    //                 data: lancerData,
    //             },
    //             {
    //                 id_in_battle: "22",
    //                 data: priestData,
    //             },
    //             {
    //                 id_in_battle: "23",
    //                 data: wizardData,
    //             },
    //         ],
    //         ready: false,
    //     },
    //     turn_base_data: [
    //         {
    //             id_in_battle: "11",
    //             speed: archerMonsterData.stats.speed,
    //             progress: 0,
    //         },
    //         {
    //             id_in_battle: "12",
    //             speed: armoredAxemanData.stats.speed,
    //             progress: 0,
    //         },
    //         { id_in_battle: "13", speed: knightData.stats.speed, progress: 0 },
    //         { id_in_battle: "21", speed: lancerData.stats.speed, progress: 0 },
    //         { id_in_battle: "22", speed: priestData.stats.speed, progress: 0 },
    //         { id_in_battle: "23", speed: wizardData.stats.speed, progress: 0 },
    //         { id_in_battle: "01", speed: 122, progress: 0 }, // player1
    //         { id_in_battle: "02", speed: 132, progress: 0 }, // player2
    //     ], // id, speed, progress
    // };

    //send monster list
    const mosterList = {
        player1: [
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[0].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[0].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[1].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[1].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[2].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[2].data.id,
            },
        ],
        player2: [
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[0].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[0].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[1].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[1].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[2].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[2].data.id,
            },
        ],
    };
    io.to(socket.roomId).emit(EVENTS.MONSTER.MONSTER_LIST, mosterList);

    // send start map
    const mapData = {
        rows: 5,
        cols: 6,
        numTypes: 5,
    };
    io.to(socket.roomId).emit(EVENTS.RUNE.GENERATE_START_MAP, generateRuneMap(mapData));

    //send turn update
    update_turn_monster(roomsPlaying[socket.roomId]);
    io.to(socket.roomId).emit(EVENTS.GAME.TURN_BASE_LIST_PUSH_DATA, roomsPlaying[socket.roomId].turn_base_data);
}
