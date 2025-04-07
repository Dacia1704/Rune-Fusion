import { effectType, skillArea } from "../model/Monster.js";

export default function monster_action_to_ally_caculation(
    monsterPlayer,
    monsterTarget,
    action
) {
    let action_affect = [];
    if (action.area_effect === skillArea.RANDOM) {
        const weakest = monsterTarget.reduce((min, monster) => {
            return monster.data.health < min.data.health ? monster : min;
        });
        let monster_affect = {
            id_in_battle: weakest.id_in_battle,
            dam: 0,
            effect: {
                effect_type: action.effect_skill.effect_type,
                duration: action.effect_skill.duration,
            },
        };
        if (action.effect_skill.effect_type === effectType.HEAL) {
            monster_affect.dam =
                -1 *
                Math.floor(
                    monsterPlayer.data.stats.health * action.percent_health
                );
        }
        console.log(monster_affect);
        action_affect.push(monster_affect);
    } else {
        monsterTarget.forEach((target) => {
            let monster_affect = {
                id_in_battle: target.id_in_battle,
                dam: 0,
                effect: {
                    effect_type: action.effect_skill.effect_type,
                    duration: action.effect_skill.duration,
                },
            };
            if (action.effect_skill.effect_type === effectType.HEAL) {
                monster_affect.dam =
                    -1 *
                    Math.floor(
                        monsterPlayer.data.stats.health * action.percent_health
                    );
            }
            action_affect.push(monster_affect);
        });
    }
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
