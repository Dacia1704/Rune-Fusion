import mongoose, { get } from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import Monster from "../model/Monster.js";
import getRandomInt from "../utils/random.js";
import EVENTS from "./event.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export async function handle_summon_event(io, socket, data) {
    try {
        const summonData = JSON.parse(data);
        const player = await Account.findOne({ _id: summonData.player_id });
        const monsters = await Monster.find({});
        const rate = 0.3;
        let summonResult = [];
        let amountGold = 0;
        for (let i = 0; i < summonData.summon_times; i++) {
            const randomNum = Math.random() * (1.01 - 0.01) + 0.01;
            if (randomNum < rate) {
                const randomMonsterIndex = getRandomInt(0, monsters.length - 1);
                summonResult.push({
                    monster_id: randomMonsterIndex,
                    gold: 0,
                });
            } else {
                const randomGold = getRandomInt(500, 1000);
                summonResult.push({
                    monster_id: -1,
                    gold: randomGold,
                });
            }
            amountGold += summonResult[summonResult.length - 1].gold;
            if (summonResult[summonResult.length - 1].monster_id !== -1) {
                if (player.own_monster_list.some((monster) => monster.id === summonResult[summonResult.length - 1].monster_id)) {
                    amountGold += 2000;
                } else {
                    player.own_monster_list.push({
                        id: summonResult[summonResult.length - 1].monster_id,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    });
                }
            }
        }
        await Account.updateOne(
            { _id: player._id },
            {
                $set: {
                    own_monster_list: player.own_monster_list,
                    gold: player.gold + amountGold,
                    scroll: player.scroll - summonData.summon_times,
                },
            }
        );
        const resourceData = {
            gold: player.gold + amountGold,
            scroll: player.scroll - summonData.summon_times,
        };
        socket.emit(EVENTS.GAME.UPDATE_RESOURCE_RESPONSE, resourceData);
        const response = {
            own_monster_list: player.own_monster_list,
            monsters: monsters,
            summon_results: summonResult,
        };
        socket.emit(EVENTS.GAME.SUMMON_RESPONSE, response);
    } catch (error) {
        console.error("Error getting summon data:", error);
    }
}
