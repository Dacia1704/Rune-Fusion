import mongoose, { get } from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import Monster from "../model/Monster.js";
import getRandomInt from "../utils/random.js";
import EVENTS from "./event.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_monster_own_request(io, socket, data) {
    try {
        const summonData = JSON.parse(data);
        const player = await Account.findOne({ _id: summonData.player_id });
        const response = {
            own_monster_list: player.own_monster_list,
        };
        socket.emit(EVENTS.MONSTER.MONSTER_OWN_RESPONSE, response);
    } catch (error) {
        console.error("Error getting monster own data:", error);
    }
}
