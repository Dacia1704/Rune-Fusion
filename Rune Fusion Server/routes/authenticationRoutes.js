import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";
import jwt from "jsonwebtoken";
mongoose.connect(config.mongoURI + "loginDb");

export default (app) => {
    app.post("/account", async (req, res) => {
        const { rUsername, rPassword } = req.body;

        if (rUsername == null || rPassword == null) {
            res.status(401).json({
                error: "Invalid cradentials",
            });
            return;
        }
        let userAccount = await Account.findOne({ username: rUsername });
        if (userAccount == null) {
            console.log("Don't exist this account");
            res.status(401).json({
                error: "Dont Exist",
            });
            return;
        } else {
            if (rPassword == userAccount.password) {
                const jwtToken = jwt.sign({ rUsername }, config.jwtSecret, {
                    expiresIn: "1h",
                });
                res.status(200).json({
                    token: jwtToken,
                    user: userAccount,
                    message: "Login successful",
                });
                console.log("Login success");
                return;
            }
        }

        res.status(401).json({
            error: "Wrong Username/Password",
        });
        return;
    });
};
