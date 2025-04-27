import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import EVENTS from "./event.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_buy_request_event(io, socket, data) {
    try {
        const buyData = JSON.parse(data);
        const player = await Account.findOne({ _id: buyData.player_id });

        await Account.updateOne(
            { _id: player._id },
            {
                $set: {
                    gold: player.gold - buyData.gold_price,
                    scroll: player.scroll + buyData.scroll_amount,
                },
            }
        );
        const response = {
            gold: player.gold - buyData.gold_price,
            scroll: player.scroll + buyData.scroll_amount,
        };
        socket.emit(EVENTS.GAME.UPDATE_RESOURCE_RESPONSE, response);
    } catch (error) {
        console.error("Error player data:", error);
    }
}
