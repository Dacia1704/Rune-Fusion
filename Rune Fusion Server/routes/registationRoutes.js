import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/account.js";

mongoose.connect(config.mongoURI);

export default (app) => {
  app.post("/register", async (req, res) => {
    const { rUsername, rPassword } = req.body;

    if (rUsername == null || rPassword == null) {
      res.send("Invalid cradentials");
      return;
    }
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
      console.log("Already exist this account");
      res.send("Already Exist");
    }
  });
};
