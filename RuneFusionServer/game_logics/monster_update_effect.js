import { effectType, monsterId } from "../model/Monster.js";
import Monster from "../model/Monster.js";

export default async function monster_update_effect(monster) {
    let effect_list = monster.data.stats.effect_list;
    let response = {
        id_in_battle: monster.id_in_battle,
        dam: 0,
        effect_list: [],
    };
    console.log(effect_list);
    if (effect_list.length === 0) {
        return response;
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
    console.log(response);
    return response;
}
