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
                    id: "11",
                    speed: 100,
                },
                {
                    id: "12",
                    speed: 102,
                },
                {
                    id: "13",
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
                    id: "21",
                    speed: 112,
                },
                {
                    id: "22",
                    speed: 108,
                },
                {
                    id: "23",
                    speed: 116,
                },
            ],
            ready: false,
        },
        turn_base_data: [
            { id: "11", speed: 100, progress: 0 },
            { id: "12", speed: 102, progress: 0 },
            { id: "13", speed: 96, progress: 0 },
            { id: "21", speed: 112, progress: 0 },
            { id: "22", speed: 108, progress: 0 },
            { id: "23", speed: 116, progress: 0 },
            { id: "01", speed: 122, progress: 0 }, // player1
            { id: "02", speed: 136, progress: 0 }, // player2
        ], // id, speed, progress
    };

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
