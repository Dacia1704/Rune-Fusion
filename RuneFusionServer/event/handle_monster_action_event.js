import Clamp from "../game_logics/function.js";
import monster_action_to_ally_caculation from "../game_logics/monster_action_ally_caculation.js";
import monster_action_to_opponent_caculation from "../game_logics/monster_action_opponent_caculation.js";
import monster_action_to_self_caculation from "../game_logics/monster_action_self_calculation.js";
import { effectType, skillArea, skillTargetType } from "../model/Monster.js";
import EVENTS from "./event.js";

// dam_formula =  base_atk * skillModifier * buff * (1 - effectiveDef/(effectiveDef+100))
// effectiveDef = bas_def/(1+ penetration) - penetration làm giảm khả năng giảm sát thương của def
export function handle_monster_action_event(io, socket, roomsPlaying, actionData) {
    const data = JSON.parse(actionData);
    console.log(data);
    const monsterPlayer = data.monster_id[0] === "1" ? roomsPlaying[socket.roomId].player1.monsters.find((monster) => monster.id_in_battle === data.monster_id) : roomsPlaying[socket.roomId].player2.monsters.find((monster) => monster.id_in_battle === data.monster_id);
    const skill = monsterPlayer.data.skill.find((element) => element.id === data.skill_id);
    let monsterAlly = data.monster_id[0] === "1" ? roomsPlaying[socket.roomId].player1.monsters : roomsPlaying[socket.roomId].player2.monsters;
    let monsterTarget = [];
    const monster_base_data = roomsPlaying[socket.roomId].monster_base_data;
    data.monster_target_id.forEach((monsterId) => {
        data.monster_target_id.forEach((monsterId) => {
            const monster1 = roomsPlaying[socket.roomId].player1.monsters.find((monster) => monster.id_in_battle === monsterId);
            if (monster1) {
                monsterTarget.push(monster1);
            }

            const monster2 = roomsPlaying[socket.roomId].player2.monsters.find((monster) => monster.id_in_battle === monsterId);
            if (monster2) {
                monsterTarget.push(monster2);
            }
        });
    });
    let monstersOpponent = data.monster_id[0] === "2" ? roomsPlaying[socket.roomId].player1.monsters : roomsPlaying[socket.roomId].player2.monsters;
    console.log("monsterTarget: " + monsterTarget.length);
    let actionResponse = [];

    skill.action_list.forEach((action) => {
        let monstersAffect = [];
        if (action.target_type === skillTargetType.OPPONENT) {
            actionResponse.push(monster_action_to_opponent_caculation(monsterPlayer, action.area_effect === skillArea.ALL ? monstersOpponent : monsterTarget, action, monster_base_data));
        } else if (action.target_type === skillTargetType.ALLY) {
            actionResponse.push(monster_action_to_ally_caculation(monsterPlayer, monsterAlly, action, monster_base_data));
        } else {
            actionResponse.push(monster_action_to_self_caculation(monsterPlayer, action, monster_base_data, monster_base_data));
        }

        // let monster_affect = {
        //     id_in_battle: weakest.id_in_battle,
        //     dam: 0,
        //     effect: {
        //         effect_type: action.effect_skill.effect_type,
        //         duration: action.effect_skill.duration,
        //     },
        // };
        monstersAffect.forEach((monster_affect_data) => {
            if (monster_affect_data.id_in_battle[0] === "1") {
                const monster1 = roomsPlaying[socket.roomId].player1.monsters.find((monster) => monster.id_in_battle === monster.id_in_battle);
                const base_data = roomsPlaying[socket.roomId].monster_base_data.find((monster) => monster.id === monster1.id);
                monster1.data.health = Clamp(monster1.data.health - monster_affect_data.dam, 0, base_data.stats.health);
            } else {
                const monster2 = roomsPlaying[socket.roomId].player2.monsters.find((monster) => monster.id_in_battle === monster.id_in_battle);
                const base_data = roomsPlaying[socket.roomId].monster_base_data.find((monster) => monster.id === monster2.id);
                monster2.data.health = Clamp(monster2.data.health - monster_affect_data.dam, 0, base_data.stats.health);
            }
        });
    });

    const response = {
        monster_id: data.monster_id,
        monster_target_id: data.monster_target_id,
        skill_id: data.skill_id,
        action_affect_list: actionResponse,
    };
    console.log(response);
    io.in(socket.roomId).emit(EVENTS.MONSTER.MONSTER_ACTION_RESPONSE, response);
}
