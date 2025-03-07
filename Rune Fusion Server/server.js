import { Server } from "socket.io";
import generateRuneMap from "./game_logics/game_map.js";

const io = new Server(3000);

io.on("connection", (socket) => {
  console.log("Connection");

  socket.on("genMap", (data) => {
    const mapData = JSON.parse(data);
    socket.emit("genMapResponse", generateRuneMap(mapData));
  });

  socket.on("disconnect", (data) => {
    console.log("Disconnect");
  });
});
console.log("Server Start");
