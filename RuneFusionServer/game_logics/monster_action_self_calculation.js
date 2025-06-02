import { effectType } from "../model/Monster.js";

export default function monster_action_to_self_caculation(monsterPlayer, action, monster_base_data) {
    let action_affect = [];
    const base_data = monster_base_data.find((monster) => monster.id === monsterPlayer.data.id);
    let monster_affect = {
        id_in_battle: monsterPlayer.id_in_battle,
        dam: 0,
        effect: {
            effect_type: action.effect_skill.effect_type,
            duration: action.effect_skill.duration,
        },
    };
    const existingEffectIndex = monsterPlayer.data.stats.effect_list.findIndex((effect) => effect.effect_type === action.effect_skill.effect_type);
    if (existingEffectIndex !== -1) {
        if (monsterPlayer.data.stats.effect_list[existingEffectIndex].duration < action.effect_skill.duration) {
            monsterPlayer.data.stats.effect_list[existingEffectIndex].duration = action.effect_skill.duration;
        }
    } else {
        monsterPlayer.data.stats.effect_list.push(action.effect_skill);
    }
    if (action.effect_skill.effect_type === effectType.HEAL) {
        monster_affect.dam = -1 * Math.floor(base_data.stats.health * action.effectiveness);
    }
    action_affect.push(monster_affect);
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
