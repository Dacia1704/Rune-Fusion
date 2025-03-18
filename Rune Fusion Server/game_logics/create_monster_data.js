import mongoose from "mongoose";
import config from "../config/keys.js";
import Monster from "../model/Monster.js";

mongoose.connect(config.mongoURI + "monsters");

export default () => {
    console.log("Save data monster...");
    let monster1 = new Monster({
        id: "0",
        name: "Archer",
        type: "0",
        stats: {
            attack: 1200,
            defense: 100,
            health: 5600,
            speed: 100,
            accuracy: 0.1,
            resistance: 0.1,
        },
    });
    let monster2 = new Monster({
        id: "1",
        name: "ArmoredAxeman",
        type: "3",
        stats: {
            attack: 500,
            defense: 250,
            health: 7000,
            speed: 102,
            accuracy: 0.2,
            resistance: 0.2,
        },
    });
    let monster3 = new Monster({
        id: "2",
        name: "Knight",
        type: "0",
        stats: {
            attack: 1000,
            defense: 120,
            health: 6000,
            speed: 96,
            accuracy: 0.1,
            resistance: 0.1,
        },
    });
    let monster4 = new Monster({
        id: "3",
        name: "Lancer",
        type: "0",
        stats: {
            attack: 1000,
            defense: 120,
            health: 6000,
            speed: 96,
            accuracy: 0.1,
            resistance: 0.1,
        },
    });
    monster1.save();
};
