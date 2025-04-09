import { effectType } from "../model/Monster.js";

export default function monster_action_to_opponent_caculation(monsterPlayer, monsterOpponent, action) {
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
        let buffDef = 1;
        if (monsterPlayer.data.stats.effect_list.includes(effectType.DEFEND_INCREMENT)) {
            buffDef = 1.5;
        }
        if (monsterPlayer.data.stats.effect_list.includes(effectType.DEFEND_DECREMENT)) {
            buffDef = 0.5;
        }
        const effectiveDef = (opponent.data.stats.defend * buffDef) / (1 + action.penetration);
        let buffAtk = 1;
        if (monsterPlayer.data.stats.effect_list.includes(effectType.ATTACK_INCREMENT)) {
            buffAtk = 1.5;
        }
        if (monsterPlayer.data.stats.effect_list.includes(effectType.ATTACK_DECREMENT)) {
            buffAtk = 0.7;
        }
        monster_affect.dam = Math.floor(monsterPlayer.data.stats.attack * action.percent_attack * buffAtk * (1 - effectiveDef / (effectiveDef + 400)));
        // cacul buff, debuff
        if (action.effect_skill.effect_type !== effectType.NONE && action.effect_skill.effect_type !== effectType.HEAL) {
            const ratio = Math.max(0.15, monsterPlayer.data.stats.accuracy - opponent.data.stats.resistance);
            const randomNum = Math.random() * (1.01 - 0.01) + 0.01;
            if (randomNum < ratio) {
                monster_affect.effect = {
                    effect_type: action.effect_skill.effect_type,
                    duration: action.effect_skill.duration,
                };
                if (action.effect_skill.effect_type === effectType.BURN) {
                    opponent.data.stats.effect_list.push(action.effect_skill);
                } else {
                    const existingEffectIndex = opponent.data.stats.effect_list.findIndex((effect) => effect.effect_type === action.effect_skill.effect_type);

                    if (existingEffectIndex !== -1) {
                        if (opponent.data.stats.effect_list[existingEffectIndex].duration < action.effect_skill.duration) {
                            opponent.data.stats.effect_list[existingEffectIndex].duration = action.effect_skill.duration;
                        }
                    } else {
                        opponent.data.stats.effect_list.push(action.effect_skill);
                    }
                }
            }
        }
        action_affect.push(monster_affect);
    });
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
