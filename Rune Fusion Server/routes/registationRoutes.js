import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";

mongoose.connect(config.mongoURI + "loginDb");

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
