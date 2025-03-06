import { Server } from "socket.io";
import { generateRuneMap } from "./controller/gameMap.ts";

const io = new Server(3000);

io.on("connection", (socket) => {
  console.log("Connection");

  socket.on("genMap", (data) => {
    console.log(data);
    socket.emit("genMapResponse", generateRuneMap(data));
  });

  socket.on("disconnect", (data) => {
    console.log("Disconnect");
  });
});

console.log("Server Start");
