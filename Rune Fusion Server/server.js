import { Server } from "socket.io";
import generateRuneMap, { generateNewRune } from "./game_logics/game_map.js";
import EVENTS from "./event.js";

const io = new Server(3000);

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
