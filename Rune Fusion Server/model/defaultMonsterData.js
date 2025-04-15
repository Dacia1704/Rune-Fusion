import Monster, { effectType, monsterId, monsterType, skillArea, skillRandomType, skillTargetType } from "./Monster.js";

export const archerMonsterData = new Monster({
    id: monsterId.ARCHER,
    name: "Archer",
    type: monsterType.PHYSIC_ATTACK,
    stats: {
        attack: 1200,
        defend: 100,
        health: 5600,
        speed: 100,
        accuracy: 0.5,
        resistance: 0.1,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0.4,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.DEFEND_DECREMENT,
                        duration: 1,
                    },
                    effectiveness: 0.7,
                },
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0.5,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.DEFEND_DECREMENT,
                        duration: 1,
                    },
                    effectiveness: 0.7,
                },
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0.6,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.DEFEND_DECREMENT,
                        duration: 1,
                    },
                    effectiveness: 0.7,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0.4,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.NONE,
                        duration: 0,
                    },
                    effectiveness: 0,
                },
            ],
        },
    ],
});

export const armoredAxemanData = new Monster({
    id: monsterId.ARMORED_AXEMAN,
    name: "Armored Axeman",
    type: monsterType.HEALTH,
    stats: {
        attack: 500,
        defend: 250,
        health: 10000,
        speed: 102,
        accuracy: 0.5,
        resistance: 0.4,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.ATTACK_DECREMENT,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1.1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.TAUNT,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
            ],
        },
    ],
});

export const knightData = new Monster({
    id: monsterId.KNIGHT,
    name: "Knight",
    type: monsterType.PHYSIC_ATTACK,
    stats: {
        attack: 1000,
        defend: 120,
        health: 4000,
        speed: 86,
        accuracy: 0.4,
        resistance: 0.3,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.SPEED_DECREMENT,
                        duration: 1,
                    },
                    effectiveness: 0.6,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.SELF,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.ATTACK_INCREMENT,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1.5,
                    penetration: 0.5,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.BURN,
                        duration: 2,
                    },
                    effectiveness: 0.8,
                },
            ],
        },
    ],
});

export const lancerData = new Monster({
    id: monsterId.LANCER,
    name: "Lancer",
    type: monsterType.DEFEND,
    stats: {
        attack: 350,
        defend: 420,
        health: 7500,
        speed: 112,
        accuracy: 0.6,
        resistance: 0.5,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.ATTACK_DECREMENT,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.ALL,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1.1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.SPEED_DECREMENT,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
            ],
        },
    ],
});

export const priestData = new Monster({
    id: monsterId.PRIEST,
    name: "Priest",
    type: monsterType.HEALTH,
    stats: {
        attack: 450,
        defend: 300,
        health: 8000,
        speed: 108,
        accuracy: 0.3,
        resistance: 0.6,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.NONE,
                        duration: 0,
                    },
                    effectiveness: 0,
                },
                {
                    target_type: skillTargetType.ALLY,
                    area_effect: skillArea.RANDOM,
                    random_type: skillRandomType.LOWEST_HP,
                    percent_attack: 0,
                    penetration: 0,
                    percent_health: 0.2,
                    effect_skill: {
                        effect_type: effectType.HEAL,
                        duration: 1,
                    },
                    effectiveness: 1,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.ALLY,
                    area_effect: skillArea.ALL,
                    random_type: skillRandomType.NONE,
                    percent_attack: 0,
                    penetration: 0,
                    percent_health: 0.4,
                    effect_skill: {
                        effect_type: effectType.HEAL,
                        duration: 1,
                    },
                    effectiveness: 1,
                },
            ],
        },
    ],
});

export const wizardData = new Monster({
    id: monsterId.WIZARD,
    name: "Wizard",
    type: monsterType.MAGIC_ATTACK,
    stats: {
        attack: 700,
        defend: 220,
        health: 3800,
        speed: 104,
        accuracy: 0.7,
        resistance: 0.1,
        effect_list: [],
    },
    talentPoint: {
        attack: 0,
        defend: 0,
        health: 0,
        speed: 0,
        accuracy: 0,
        resistance: 0,
    },
    skill: [
        {
            id: "0",
            name: "Basic Attack",
            description: "Basic Attack",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.BURN,
                        duration: 2,
                    },
                    effectiveness: 1,
                },
            ],
        },
        {
            id: "1",
            name: "Ultimate Skill",
            description: "Ultimate Skill",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.ALL,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1.2,
                    penetration: 0,
                    percent_health: 0,
                    effect_skill: {
                        effect_type: effectType.FROZEN,
                        duration: 1,
                    },
                    effectiveness: 0.7,
                },
            ],
        },
    ],
});
