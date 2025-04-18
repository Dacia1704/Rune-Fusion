import mongoose from "mongoose";
const { Schema } = mongoose;

export const monsterId = {
    ARCHER: 0,
    ARMORED_AXEMAN: 1,
    KNIGHT: 2,
    LANCER: 3,
    PRIEST: 4,
    WIZARD: 5,
};
export const monsterType = {
    PHYSIC_ATTACK: 0,
    MAGIC_ATTACK: 1,
    HEALTH: 2,
    DEFEND: 3,
};
export const skillTargetType = {
    ALLY: 0,
    OPPONENT: 1,
    SELF: 2,
};
export const skillArea = {
    ALL: 0,
    SINGLE: 1,
    RANDOM: 2,
};
export const skillRandomType = {
    NONE: 0,
    HIGHEST_HP: 1,
    LOWEST_HP: 2,
};
export const effectType = {
    NONE: 0,
    HEAL: 1,
    BURN: 2,
    SPEED_INCREMENT: 3,
    ATTACK_INCREMENT: 4,
    DEFEND_INCREMENT: 5,
    SPEED_DECREMENT: 6,
    ATTACK_DECREMENT: 7,
    DEFEND_DECREMENT: 8,
    TAUNT: 9,
    FROZEN: 10,
};

const effectSkill = new Schema({
    effect_type: Number,
    duration: Number,
});
const skillSchema = new Schema({
    target_type: Number,
    area_effect: Number,
    random_type: Number,
    percent_attack: Number,
    penetration: Number,
    percent_health: Number,
    effect_skill: effectSkill,
    effectiveness: Number,
});

const monsterSkillSchema = new Schema({
    id: String,
    name: String,
    description: String,
    action_list: [skillSchema],
    point_cost: Number,
});

const monsterStats = new Schema({
    attack: Number,
    defend: Number,
    health: Number,
    speed: Number,
    accuracy: Number,
    resistance: Number,
    effect_list: [effectSkill],
});

export const talentPointSchema = new Schema({
    id: Number,
    attack: Number,
    defend: Number,
    health: Number,
    speed: Number,
    accuracy: Number,
    resistance: Number,
});
const monsterSchema = new Schema({
    id: Number,
    name: String,
    type: Number,
    stats: monsterStats,
    talentPoint: talentPointSchema,
    skill: [monsterSkillSchema],
});

const Monster = mongoose.model("monsters", monsterSchema);
export default Monster;
