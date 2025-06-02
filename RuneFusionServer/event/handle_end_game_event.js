import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import EVENTS from "./event.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_end_game_event(io, socket, roomsPlaying) {
    const roomId = socket.roomId;
    const player1_id = roomsPlaying[roomId].player1.data.id;
    const player2_id = roomsPlaying[roomId].player2.data.id;
    const socket_player1_id = roomsPlaying[roomId].player1.socket.id;
    const winner_id = socket_player1_id == socket.id ? player2_id : player1_id; // loser is player request data
    const loser_id = socket_player1_id == socket.id ? player1_id : player2_id;
    const response = {
        winner_id: winner_id,
        loser_id: loser_id,
        winner_gold: 500,
        loser_gold: 150,
    };
    io.to(roomId).emit(EVENTS.GAME.END_GAME_RESPONSE, response);

    //update resource for winner and loser
    try {
        const winner = await Account.findOne({ _id: winner_id });
        await Account.updateOne(
            { _id: winner._id },
            {
                $set: {
                    gold: winner.gold + 500,
                },
            }
        );

        const loser = await Account.findOne({ _id: loser_id });
        await Account.updateOne(
            { _id: loser._id },
            {
                $set: {
                    gold: loser.gold + 150,
                },
            }
        );
    } catch (error) {
        console.error(error);
    }
}
