const prodConfig = {
  port: process.env.PORT,
  mongoURI: process.env.MONGO_URI,
  jwtSecret: process.env.jwtSecret,
};

export default prodConfig;
