import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
import { archerMonsterData, armoredAxemanData, knightData, lancerData, priestData, wizardData } from "../model/defaultMonsterData.js";
export function handle_game_start_event(io, socket, roomsPlaying) {
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

    //send point init
    const maxPoint = 100;
    const initPoint = Math.floor(maxPoint);
    roomsPlaying[socket.roomId].player1.rune_points = [initPoint, initPoint, initPoint, initPoint, initPoint];
    roomsPlaying[socket.roomId].player2.rune_points = [initPoint, initPoint, initPoint, initPoint, initPoint];
    const point = {
        player1: roomsPlaying[socket.roomId].player1.rune_points,
        player2: roomsPlaying[socket.roomId].player2.rune_points,
    };
    io.to(socket.roomId).emit(EVENTS.GAME.POINT_INIT, point);
}
