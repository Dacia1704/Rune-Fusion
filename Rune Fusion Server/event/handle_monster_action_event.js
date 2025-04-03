import monster_action_caculation from "../game_logics/monster_action_caculation.js";
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
    let monsterTarget = [];
    data.monster_target_id.forEach((monsterId) => {
        if (monsterId[0] == "1") {
            monsterTarget.push(
                roomsPlaying[socket.roomId].player1.monsters.find(
                    (monster) => monster.id_in_battle === monsterId
                )
            );
        } else {
            monsterTarget.push(
                roomsPlaying[socket.roomId].player2.monsters.find(
                    (monster) => monster.id_in_battle === monsterId
                )
            );
        }
    });
    console.log("monsterTarget: " + monsterTarget.length);
    let actionResponse = [];

    const skill = monsterPlayer.data.skill.find(
        (element) => element.id === data.skill_id
    );
    skill.action_list.forEach((action) => {
        let monstersAffect = [];
        if (action.target_type === skillTargetType.OPPONENT) {
            if (action.area_effect === skillArea.SINGLE) {
                actionResponse.push(
                    monster_action_caculation(
                        monsterPlayer,
                        monsterTarget,
                        action
                    )
                );
            } else if (action.area_effect == skillArea.ALL) {
                actionResponse.push(
                    monster_action_caculation(
                        monsterPlayer,
                        monsterTarget,
                        action
                    )
                );
            }
        } else if (action.target_type === skillTargetType.ALLY) {
            if (action.area_effect === skillArea.SINGLE) {
            } else if (action.area_effect == skillArea.ALL) {
            } else {
            }
        } else {
            // self
        }
    });

    const response = {
        monster_id: data.monster_id,
        monster_target_id: data.monster_target_id,
        skill_id: data.skill_id,
        action_affect_list: actionResponse,
    };
    console.log("monster_action_response:\n" + response.monster_id);
    io.in(socket.roomId).emit(EVENTS.MONSTER.MONSTER_ACTION_RESPONSE, response);
}
