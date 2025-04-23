import mongoose from "mongoose";
import { talentPointSchema } from "./Monster.js";
const { Schema } = mongoose;

const ownMonsterSchema = new Schema({
    id: Number,
    talent_point: talentPointSchema,
});
const accountSchema = new Schema({
    username: String,
    password: String,
    own_monster_list: [ownMonsterSchema],
    gold: Number,
    scroll: Number,
});

const Account = mongoose.model("accounts", accountSchema);
export default Account;
