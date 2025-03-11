import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";

mongoose.connect(config.mongoURI);

export default (app) => {
  app.get("/account", async (req, res) => {
    const { rUsername, rPassword } = req.query;

    if (rUsername == null || rPassword == null) {
      res.send("Invalid cradentials");
      return;
    }
    console.log(mongoose.connection.readyState);
    let userAccount = await Account.findOne({ username: rUsername });
    if (userAccount == null) {
      console.log("Create new account...");
      let newAccount = new Account({
        username: rUsername,
        password: rPassword,

        lastAuthentication: Date.now(),
      });
      await newAccount.save();
      res.send(newAccount);
      return;
    } else {
      console.log("Retrieving account.....");
      if (rPassword == userAccount.password) {
        userAccount.lastAuthentication = Date.now();
        await userAccount.save();
        res.send(userAccount);
      }
    }
  });
};
