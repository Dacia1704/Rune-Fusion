import { effectType } from "../model/Monster.js";

export default function monster_action_to_opponent_caculation(
    monsterPlayer,
    monsterOpponent,
    action
) {
    let action_affect = [];
    monsterOpponent.forEach((opponent) => {
        let monster_affect = {
            id_in_battle: opponent.id_in_battle,
            dam: 0,
            effect: {
                effect_type: effectType.NONE,
                duration: 0,
            },
        };
        // cacul dam
        const effectiveDef =
            opponent.data.stats.defend / (1 + action.penetration);
        let buff = 1;
        if (
            monsterPlayer.data.stats.effect_list.includes(
                effectType.ATTACK_INCREMENT
            )
        ) {
            buff = 1.5;
        }
        if (
            monsterPlayer.data.stats.effect_list.includes(
                effectType.ATTACK_DECREMENT
            )
        ) {
            buff = 0.7;
        }
        monster_affect.dam = Math.floor(
            monsterPlayer.data.stats.attack *
                action.percent_attack *
                buff *
                (1 - effectiveDef / (effectiveDef + 400))
        );
        // cacul buff, debuff
        const ratio = Math.max(
            0.15,
            monsterPlayer.data.stats.accuracy - opponent.data.stats.resistance
        );
        const randomNum = Math.random() * (1.01 - 0.01) + 0.01;
        if (randomNum < ratio) {
            monster_affect.effect = {
                effect_type: action.effect_skill.effect_type,
                duration: action.effect_skill.duration,
            };
        }
        action_affect.push(monster_affect);
    });
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
