import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";
mongoose.connect(config.mongoURI + "RuneFushion");
export async function updateMonsterTalentPointInAccount(userId, monsterId, newTalentPoint) {
    try {
        const result = await Account.updateOne(
            { _id: userId },
            {
                $set: {
                    "own_monster_list.$[m].talent_point": newTalentPoint,
                },
            },
            {
                arrayFilters: [{ "m.id": monsterId }],
            }
        );

        if (result.modifiedCount > 0) {
            console.log("Monster updated in account!");
        } else {
            console.log("Monster or account not found.");
        }
    } catch (err) {
        console.error("Update failed:", err);
    }
}
