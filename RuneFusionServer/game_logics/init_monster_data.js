import mongoose from "mongoose";
import config from "../config/keys.js";
import Monster from "../model/Monster.js";
mongoose.connect(config.mongoURI + "RuneFushion");
export async function init_monster_data(monsterData) {
    try {
        const existing = await Monster.findOne({ id: monsterData.id });

        if (!existing) {
            const savedMonster = await monsterData.save();
            console.log("Monster data initialized:", savedMonster.name);
        } else {
            console.log("Monster already exists:", existing.name);
        }
    } catch (err) {
        console.error("Failed to init monster data:", err);
    }
}
