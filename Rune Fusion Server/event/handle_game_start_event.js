import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
export function handle_game_start_event(io, socket, roomsPlaying) {
    //fake data
    roomsPlaying[socket.roomId] = {
        player1: {
            socket: roomsPlaying[socket.roomId].player1.socket,
            data: roomsPlaying[socket.roomId].player1.data,
            monsters: [
                {
                    id: 0,
                    id_in_battle: "11",
                    speed: 100,
                },
                {
                    id: 1,
                    id_in_battle: "12",
                    speed: 102,
                },
                {
                    id: 2,
                    id_in_battle: "13",
                    speed: 96,
                },
            ],
            ready: false,
        },
        player2: {
            socket: roomsPlaying[socket.roomId].player2.socket,
            data: roomsPlaying[socket.roomId].player2.data,
            monsters: [
                {
                    id: 3,
                    id_in_battle: "21",
                    speed: 112,
                },
                {
                    id: 4,
                    id_in_battle: "22",
                    speed: 108,
                },
                {
                    id: 5,
                    id_in_battle: "23",
                    speed: 116,
                },
            ],
            ready: false,
        },
        turn_base_data: [
            { id_in_battle: "11", speed: 100, progress: 0 },
            { id_in_battle: "12", speed: 102, progress: 0 },
            { id_in_battle: "13", speed: 96, progress: 0 },
            { id_in_battle: "21", speed: 112, progress: 0 },
            { id_in_battle: "22", speed: 108, progress: 0 },
            { id_in_battle: "23", speed: 116, progress: 0 },
            { id_in_battle: "01", speed: 122, progress: 0 }, // player1
            { id_in_battle: "02", speed: 136, progress: 0 }, // player2
        ], // id, speed, progress
    };
    //send monster list
    const mosterList = {
        player1: roomsPlaying[socket.roomId].player1.monsters,
        player2: roomsPlaying[socket.roomId].player2.monsters,
    };
    io.to(socket.roomId).emit(EVENTS.GAME.MONSTER_LIST, mosterList);

    // send start map
    const mapData = {
        rows: 5,
        cols: 6,
        numTypes: 5,
    };
    io.to(socket.roomId).emit(
        EVENTS.RUNE.GENERATE_START_MAP,
        generateRuneMap(mapData)
    );

    //send turn update
    update_turn_monster(roomsPlaying[socket.roomId]);
    io.to(socket.roomId).emit(
        EVENTS.GAME.TURN_BASE_LIST_PUSH_DATA,
        roomsPlaying[socket.roomId].turn_base_data
    );
}
