import { Server } from "socket.io";

const io = new Server(3000);

io.on("connection", (socket) => {
  console.log("Connection");

  socket.on("test", (data) => {
    console.log(data);

    var test = { text: "Hi" };
    socket.emit("hi", test);
  });

  socket.on("disconnect", (data) => {
    console.log("Disconnect");
  });
});

console.log("Server Start");
