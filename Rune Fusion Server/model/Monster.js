import mongoose from "mongoose";
const { Schema } = mongoose;

const monsterSchema = new Schema({
    id: String,
    name: String,
    type: Number,
    stats: {
        attack: Number,
        speed: Number,
        defend: Number,
        health: Number,
        accuracy: Number,
        resistance: Number,
    },
});

const Monster = mongoose.model("monsters", monsterSchema);
export default Monster;
