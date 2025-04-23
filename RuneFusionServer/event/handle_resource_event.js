import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import EVENTS from "./event.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_resource_event(io, socket, data) {
    try {
        const playerData = JSON.parse(data);
        const player = await Account.findOne({ _id: playerData.player_id });
        const response = {
            gold: player.gold,
            scroll: player.scroll,
        };
        socket.emit(EVENTS.GAME.UPDATE_RESOURCE_RESPONSE, response);
    } catch (error) {
        console.error("Error getting monster data:", error);
    }
}
