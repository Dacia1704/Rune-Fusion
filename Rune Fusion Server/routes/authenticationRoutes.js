import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";

mongoose.connect(config.mongoURI);

export default (app) => {
  app.post("/account", async (req, res) => {
    const { rUsername, rPassword } = req.body;

    if (rUsername == null || rPassword == null) {
      res.send("Invalid cradentials");
      return;
    }
    let userAccount = await Account.findOne({ username: rUsername });
    if (userAccount == null) {
      console.log("Don't exist this account");
      res.send("Dont Exist");
      return;
    } else {
      if (rPassword == userAccount.password) {
        userAccount.lastAuthentication = Date.now();
        await userAccount.save();
        res.send(userAccount);
        console.log("Login success");
        return;
      }
    }

    res.send("Wrong Username/Password");
    return;
  });
};
