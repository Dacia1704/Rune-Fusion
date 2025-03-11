import { Server } from "socket.io";
import generateRuneMap, { generateNewRune } from "./game_logics/game_map.js";
import EVENTS from "./event.js";
import express from "express";
import config from "./config/keys.js";
import authenticationRoutes from "./routes/authenticationRoutes.js";

//login
const app = express();
authenticationRoutes(app);
const server = app.listen(config.port, () => {
  console.log(`Express server is running on port ${config.port}`);
});

//socket logic game
const io = new Server(server);
io.on("connection", (socket) => {
  console.log("Connection");

  socket.on(EVENTS.RUNE.GENERATE_START_REQUEST, (data) => {
    const mapData = JSON.parse(data);
    socket.emit(EVENTS.RUNE.GENERATE_START_RESPONSE, generateRuneMap(mapData));
  });

  socket.on(EVENTS.RUNE.NEW_REQUEST, (data) => {
    const mapData = JSON.parse(data);
    socket.emit(EVENTS.RUNE.NEW_RESPONSE, generateNewRune(mapData));
  });

  socket.on("disconnect", (data) => {
    console.log("Disconnect");
  });
});
console.log("Server Start");
