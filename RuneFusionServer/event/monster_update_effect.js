import { effectType, monsterId } from "../model/Monster.js";
import Monster from "../model/Monster.js";
import EVENTS from "../event/event.js";
export default async function monster_update_effect(io, socket, monster) {
    try {
        let effect_list = monster.data.stats.effect_list;
        let response = {
            id_in_battle: monster.id_in_battle,
            dam: 0,
            effect_list: [],
        };
        console.log(effect_list);
        if (effect_list.length === 0) {
            io.in(socket.roomId).emit(EVENTS.MONSTER.UPDATE_EFFECT_RESPONSE, response);
            return;
        }
        let base_data = await Monster.findOne({ id: monster.data.id });
        effect_list.forEach((element) => {
            if (element.effect_type === effectType.BURN) {
                response.dam += base_data.stats.health * 0.05;
            }
            let duration = element.duration - 1;

            if (duration > 0) {
                response.effect_list.push({
                    effect_type: element.effect_type,
                    duration: duration,
                });
            }
        });
        io.in(socket.roomId).emit(EVENTS.MONSTER.UPDATE_EFFECT_RESPONSE, response);
    } catch (error) {
        console.error("Error monster data:", error);
    }
}
