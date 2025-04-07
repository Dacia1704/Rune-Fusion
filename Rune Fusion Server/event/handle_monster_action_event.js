import monster_action_to_ally_caculation from "../game_logics/monster_action_ally_caculation.js";
import monster_action_to_opponent_caculation from "../game_logics/monster_action_opponent_caculation.js";
import monster_action_to_self_caculation from "../game_logics/monster_action_self_calculation.js";
import { effectType, skillArea, skillTargetType } from "../model/Monster.js";
import EVENTS from "./event.js";

// dam_formula =  base_atk * skillModifier * buff * (1 - effectiveDef/(effectiveDef+100))
// effectiveDef = bas_def/(1+ penetration) - penetration làm giảm khả năng giảm sát thương của def
export function handle_monster_action_event(
    io,
    socket,
    roomsPlaying,
    actionData
) {
    const data = JSON.parse(actionData);
    console.log(data);
    const monsterPlayer =
        data.monster_id[0] === "1"
            ? roomsPlaying[socket.roomId].player1.monsters.find(
                  (monster) => monster.id_in_battle === data.monster_id
              )
            : roomsPlaying[socket.roomId].player2.monsters.find(
                  (monster) => monster.id_in_battle === data.monster_id
              );
    console.log("monsterPlayer: " + monsterPlayer.id_in_battle);
    let monsterAlly =
        data.monster_id[0] === "0"
            ? roomsPlaying[socket.roomId].player1.monsters
            : roomsPlaying[socket.roomId].player2.monsters;
    let monsterTarget = [];
    data.monster_target_id.forEach((monsterId) => {
        data.monster_target_id.forEach((monsterId) => {
            const monster1 = roomsPlaying[socket.roomId].player1.monsters.find(
                (monster) => monster.id_in_battle === monsterId
            );
            if (monster1) {
                monsterTarget.push(monster1);
            }

            const monster2 = roomsPlaying[socket.roomId].player2.monsters.find(
                (monster) => monster.id_in_battle === monsterId
            );
            if (monster2) {
                monsterTarget.push(monster2);
            }
        });
    });
    console.log("monsterTarget: " + monsterTarget.length);
    let actionResponse = [];

    const skill = monsterPlayer.data.skill.find(
        (element) => element.id === data.skill_id
    );
    skill.action_list.forEach((action) => {
        let monstersAffect = [];
        if (action.target_type === skillTargetType.OPPONENT) {
            actionResponse.push(
                monster_action_to_opponent_caculation(
                    monsterPlayer,
                    monsterTarget,
                    action
                )
            );
        } else if (action.target_type === skillTargetType.ALLY) {
            actionResponse.push(
                monster_action_to_ally_caculation(
                    monsterPlayer,
                    monsterAlly,
                    action
                )
            );
        } else {
            actionResponse.push(
                monster_action_to_self_caculation(monsterPlayer, action)
            );
        }
    });

    const response = {
        monster_id: data.monster_id,
        monster_target_id: data.monster_target_id,
        skill_id: data.skill_id,
        action_affect_list: actionResponse,
    };
    console.log("send action");
    io.in(socket.roomId).emit(EVENTS.MONSTER.MONSTER_ACTION_RESPONSE, response);
}
