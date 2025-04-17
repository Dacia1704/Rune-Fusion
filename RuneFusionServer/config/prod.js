const prodConfig = {
    port: process.env.PORT,
    mongoURI: process.env.MONGO_URI,
    jwtSecret: process.env.JWT_SECRET,
};

export default prodConfig;
