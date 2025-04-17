import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";

mongoose.connect(config.mongoURI + "RuneFushion");

export default (app) => {
    app.post("/register", async (req, res) => {
        const { rUsername, rPassword } = req.body;

        if (rUsername == null || rPassword == null) {
            res.status(401).json({ error: "Invalid cradentials" });
            return;
        }
        let userAccount = await Account.findOne({ username: rUsername });
        if (userAccount == null) {
            console.log("Create new account...");
            let newAccount = new Account({
                username: rUsername,
                password: rPassword,
                own_monster_list: [
                    {
                        id: 0,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                    {
                        id: 1,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                    {
                        id: 2,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                    {
                        id: 3,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                    {
                        id: 4,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                    {
                        id: 5,
                        talent_point: {
                            attack: 0,
                            defend: 0,
                            health: 0,
                            speed: 0,
                            accuracy: 0,
                            resistance: 0,
                        },
                    },
                ],
            });
            await newAccount.save();
            res.status(200).json({
                user: newAccount,
                message: "Create account successful",
            });
            return;
        } else {
            console.log("Already exist this account");
            res.status(401).json({
                error: "Already exist this account",
            });
        }
    });
};
