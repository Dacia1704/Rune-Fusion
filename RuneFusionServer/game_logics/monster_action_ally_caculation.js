import { effectType, skillArea } from "../model/Monster.js";

export default function monster_action_to_ally_caculation(monsterPlayer, monsterTarget, action, monster_base_data) {
    let action_affect = [];
    const base_data = monster_base_data.find((monster) => monster.id === monsterPlayer.data.id);
    if (action.area_effect === skillArea.RANDOM) {
        const weakest = monsterTarget.reduce((min, monster) => {
            const base = monster_base_data.find((m) => m.id === monster.data.id);
            const isInjured = monster.data.health < base.stats.health;

            const minBase = monster_base_data.find((m) => m.id === min.data.id);
            const minIsInjured = min.data.health < minBase.stats.health;

            if (isInjured && !minIsInjured) {
                // Ưu tiên quái bị thương hơn quái không bị thương
                return monster;
            }

            if (isInjured && minIsInjured) {
                // Cả hai đều bị thương → chọn con hp thấp hơn
                return monster.data.health < min.data.health ? monster : min;
            }

            if (!isInjured && !minIsInjured) {
                // Cả hai đều khỏe → chọn con có base HP thấp hơn
                return base.stats.health < minBase.stats.health ? monster : min;
            }

            return min;
        });
        let monster_affect = {
            id_in_battle: weakest.id_in_battle,
            dam: 0,
            effect: {
                effect_type: action.effect_skill.effect_type,
                duration: action.effect_skill.duration,
            },
        };
        const existingEffectIndex = weakest.data.stats.effect_list.findIndex((effect) => effect.effect_type === action.effect_skill.effect_type);
        if (existingEffectIndex !== -1) {
            if (weakest.data.stats.effect_list[existingEffectIndex].duration < action.effect_skill.duration) {
                weakest.data.stats.effect_list[existingEffectIndex].duration = action.effect_skill.duration;
            }
        } else {
            weakest.data.stats.effect_list.push(action.effect_skill);
        }

        if (action.effect_skill.effect_type === effectType.HEAL) {
            monster_affect.dam = -1 * Math.floor(base_data.stats.health * action.percent_health);
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
            const existingEffectIndex = target.data.stats.effect_list.findIndex((effect) => effect.effect_type === action.effect_skill.effect_type);
            if (existingEffectIndex !== -1) {
                if (target.data.stats.effect_list[existingEffectIndex].duration < action.effect_skill.duration) {
                    target.data.stats.effect_list[existingEffectIndex].duration = action.effect_skill.duration;
                }
            } else {
                target.data.stats.effect_list.push(action.effect_skill);
            }

            if (action.effect_skill.effect_type === effectType.HEAL) {
                monster_affect.dam = -1 * Math.floor(base_data.stats.health * action.percent_health);
            }
            if (action.effect_skill.effect_type === effectType.HEAL) {
                monster_affect.dam = -1 * Math.floor(base_data.stats.health * action.percent_health);
            }
            action_affect.push(monster_affect);
        });
    }
    console.log("action_affect: " + action_affect.length);

    return action_affect;
}
