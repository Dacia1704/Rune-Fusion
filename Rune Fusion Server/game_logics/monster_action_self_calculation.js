import { effectType } from "../model/Monster.js";

export default function monster_action_to_self_caculation(
    monsterPlayer,
    action
) {
    let action_affect = [];
    let monster_affect = {
        id_in_battle: monsterPlayer.id_in_battle,
        dam: 0,
        effect: {
            effect_type: action.effect_skill.effect_type,
            duration: action.effect_skill.duration,
        },
    };
    if (action.effect_skill.effect_type === effectType.HEAL) {
        monster_affect.dam =
            -1 *
            Math.floor(monsterPlayer.data.stats.health * action.effectiveness);
    }
    action_affect.push(monster_affect);
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
