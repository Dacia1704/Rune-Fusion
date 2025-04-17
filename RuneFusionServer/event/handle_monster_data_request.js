import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import Monster from "../model/Monster.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_monster_data_request(io, socket, data) {
    try {
        const playerData = JSON.parse(data);
        const player = await Account.findOne({ _id: playerData.id });
        const monsters = await Monster.find({});
        const response = {
            own_monster_list: player.own_monster_list,
            monsters: monsters,
        };
        socket.emit("monster_data_response", response);
    } catch (error) {
        console.error("Error getting monster data:", error);
    }
}
