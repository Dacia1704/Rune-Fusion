import mongoose from "mongoose";
const { Schema } = mongoose;

const accountSchema = new Schema({
  username: String,
  password: String,

  lastAuthentication: Date,
});

const Account = mongoose.model("accounts", accountSchema);
export default Account;
