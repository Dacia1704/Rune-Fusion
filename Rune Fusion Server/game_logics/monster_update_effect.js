import { archerMonsterData, armoredAxemanData, knightData, lancerData, priestData, wizardData } from "../model/defaultMonsterData.js";
import { effectType, monsterId } from "../model/Monster.js";

export default function monster_update_effect(monster) {
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
    let base_data;
    switch (monster.data.id) {
        case monsterId.ARCHER:
            base_data = archerMonsterData;
            break;
        case monsterId.ARMORED_AXEMAN:
            base_data = armoredAxemanData;
            break;
        case monsterId.KNIGHT:
            base_data = knightData;
            break;
        case monsterId.LANCER:
            base_data = lancerData;
            break;
        case monsterId.PRIEST:
            base_data = priestData;
            break;
        case monsterId.WIZARD:
            base_data = wizardData;
            break;
        default:
            break;
    }
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
