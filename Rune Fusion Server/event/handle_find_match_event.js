import EVENTS from "./event.js";
import getRandomInt from "../utils/random.js";
import generateRuneMap from "../game_logics/game_map.js";

export function handle_find_match_event(io, socket, playerData, queuePlayerWaiting, roomsPlaying) {
    const data = JSON.parse(playerData);
    console.log(data);
    queuePlayerWaiting.push({ socket, data });
    if (queuePlayerWaiting.length >= 2) {
        const player1 = queuePlayerWaiting.shift();
        const player2 = queuePlayerWaiting.shift();
        const roomId = `room-${player1.socket.id}-${player2.socket.id}`;
        roomsPlaying[roomId] = { player1, player2 };

        roomsPlaying[roomId] = {
            player1: {
                socket: player1.socket,
                data: player1.data,
                monsters: [
                    {
                        id_in_battle: "11",
                        data: null,
                        is_locked: false,
                    },
                    {
                        id_in_battle: "12",
                        data: null,
                        is_locked: false,
                    },
                    {
                        id_in_battle: "13",
                        data: null,
                        is_locked: false,
                    },
                ],

                ready: false,
            },
            player2: {
                socket: player2.socket,
                data: player2.data,
                monsters: [
                    {
                        id_in_battle: "21",
                        data: null,
                        is_locked: false,
                    },
                    {
                        id_in_battle: "22",
                        data: null,
                        is_locked: false,
                    },
                    {
                        id_in_battle: "23",
                        data: null,
                        is_locked: false,
                    },
                ],
                ready: false,
            },
            turn_base_data: [], // id_in_battle, speed, progress
            monster_base_data: [],
        };

        player1.socket.join(roomId);
        player2.socket.join(roomId);
        player1.socket.roomId = roomId;
        player2.socket.roomId = roomId;

        const player1Data = {
            id: player1.data.id,
            playername: player1.data.playername,
            playerindex: 0,
        };
        const player2Data = {
            id: player2.data.id,
            playername: player2.data.playername,
            playerindex: 1,
        };

        io.to(roomId).emit(EVENTS.PLAYER.MATCH_FOUND, {
            roomId: roomId,
            player1: player1Data,
            player2: player2Data,
        });

        console.log(`Ghép cặp: ${player1.data.playername} vs ${player2.data.playername} (Room: ${roomId})`);

        io.to(roomId).emit(EVENTS.PLAYER.CURRENT_TURN, getRandomInt(0, 1));
    }
}
