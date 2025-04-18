import EVENTS from "./event.js";
import generateRuneMap from "../game_logics/game_map.js";
import update_turn_monster from "../game_logics/turn_monster.js";
import mongoose from "mongoose";
import config from "../config/keys.js";
import Account from "../model/Account.js";
import Monster from "../model/Monster.js";

mongoose.connect(config.mongoURI + "RuneFushion");

function CalculateStatByTalentPoint(stat, talent_point) {
    let add = 0;
    for (let i = 1; i <= talent_point; i++) {
        if (i <= 10) {
            add += stat * 0.05;
        } else {
            const x = Math.ceil(i - 10 / 5);
            add += stat * (0.05 / Math.pow(2, x));
        }
    }
    return stat + add;
}

export async function handle_game_start_event(io, socket, roomsPlaying) {
    const player1 = await Account.findOne({ _id: roomsPlaying[socket.roomId].player1.data.id });
    roomsPlaying[socket.roomId].player1.monsters.forEach((mon) => {
        const mon_talent_point = player1.own_monster_list.find((m) => m.id === mon.data.id).talent_point;
        mon.data.hp = CalculateStatByTalentPoint(mon.data.hp, mon_talent_point.talent_point);
        mon.data.attack = CalculateStatByTalentPoint(mon.data.attack, mon.talent_point);
        mon.data.defense = CalculateStatByTalentPoint(mon.data.defense, mon.talent_point);
        mon.data.speed = CalculateStatByTalentPoint(mon.data.speed, mon.talent_point);
        mon.data.accuracy = CalculateStatByTalentPoint(mon.data.accuracy, mon.talent_point);
        mon.data.resistance = CalculateStatByTalentPoint(mon.data.resistance, mon.talent_point);
    });

    const player2 = await Account.findOne({ _id: roomsPlaying[socket.roomId].player2.data.id });
    roomsPlaying[socket.roomId].player2.monsters.forEach((mon) => {
        const mon_talent_point = player2.own_monster_list.find((m) => m.id === mon.data.id).talent_point;
        mon.data.hp = CalculateStatByTalentPoint(mon.data.hp, mon_talent_point.talent_point);
        mon.data.attack = CalculateStatByTalentPoint(mon.data.attack, mon.talent_point);
        mon.data.defense = CalculateStatByTalentPoint(mon.data.defense, mon.talent_point);
        mon.data.speed = CalculateStatByTalentPoint(mon.data.speed, mon.talent_point);
        mon.data.accuracy = CalculateStatByTalentPoint(mon.data.accuracy, mon.talent_point);
        mon.data.resistance = CalculateStatByTalentPoint(mon.data.resistance, mon.talent_point);
    });
    //send monster list
    const mosterList = {
        player1: [
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[0].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[0].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[1].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[1].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player1.monsters[2].id_in_battle,
                id: roomsPlaying[socket.roomId].player1.monsters[2].data.id,
            },
        ],
        player2: [
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[0].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[0].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[1].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[1].data.id,
            },
            {
                id_in_battle: roomsPlaying[socket.roomId].player2.monsters[2].id_in_battle,
                id: roomsPlaying[socket.roomId].player2.monsters[2].data.id,
            },
        ],
    };
    io.to(socket.roomId).emit(EVENTS.MONSTER.MONSTER_LIST, mosterList);

    // send start map
    const mapData = {
        rows: 5,
        cols: 6,
        numTypes: 5,
    };
    io.to(socket.roomId).emit(EVENTS.RUNE.GENERATE_START_MAP, generateRuneMap(mapData));
    //send turn update
    update_turn_monster(roomsPlaying[socket.roomId]);
    io.to(socket.roomId).emit(EVENTS.GAME.TURN_BASE_LIST_PUSH_DATA, roomsPlaying[socket.roomId].turn_base_data);
}
