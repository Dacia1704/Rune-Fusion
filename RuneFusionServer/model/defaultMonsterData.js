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
            name: "Triple Arrow Break",
            description: "Attacks a single enemy 3 times in a row, dealing 40%, 50%, and 60% of the caster's ATK respectively. Each hit has a 70% chance to apply Defend Decrement for 1 turn.",
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
            // point_cost: ,
        },
        {
            id: "1",
            name: "Heartpiercer",
            description: "Fires a deadly powerful arrow that pierces through enemy defenses, dealing 150% of the caster's ATK and ignoring 30% of the target's DEF",
            action_list: [
                {
                    target_type: skillTargetType.OPPONENT,
                    area_effect: skillArea.SINGLE,
                    random_type: skillRandomType.NONE,
                    percent_attack: 1.5,
                    penetration: 0.3,
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
            name: "Crippling Blow",
            description: "Swings a heavy axe at a single enemy, dealing 100% of ATK. Has a 100% chance to apply Attack Decrement for 2 turns.",
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
            name: "Unbreakable Roar",
            description: "Lets out a thunderous roar, striking a single enemy for 110% of ATK and forcing them to attack the Axeman for 2 turns with Taunt.",
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
            name: "Weighted Strike",
            description: "Delivers a heavy blow to a single enemy, dealing 100% of ATK with a 60% chance to apply Speed Decrement for 1 turn.",
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
            name: "Blazing Crusade",
            description: "Empowers self with Attack Increment for 2 turns, then strikes a single enemy for 150% of ATK, ignoring 50% of DEF. Has an 80% chance to apply Burn for 2 turns.",
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
            name: "Piercing Lunge",
            description: "Strikes a single enemy for 100% ATK and applies Attack Decrement for 2 turns.",
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
            name: "Whirlwind Formation",
            description: "Unleashes a sweeping strike on all enemies, dealing 110% of ATK and applying Speed Decrement for 2 turns.",
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
            name: "Healing Touch",
            description: "Deals 100% of ATK damage to a single enemy and heals the ally with the lowest HP by 20% of their total health.",
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
            name: "Divine Blessing",
            description: "Heals all allies for 40% of their health.",
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
            name: "Ignite",
            description: "Deals 100% of ATK damage to a single enemy and applies Burn for 2 turns.",
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
            name: "Frozen Abyss",
            description: "Deals 120% of ATK damage to all enemies and applies Frozen for 1 turn.",
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
