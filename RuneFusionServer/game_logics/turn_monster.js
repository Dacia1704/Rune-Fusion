const base_speed_factor = 100;
export default function update_turn_monster(roomData) {
    let turn_base_list = roomData.turn_base_data;
    if (turn_base_list[0].progress == 1) {
        turn_base_list[0].progress = 0;
    }
    const maxSpeed = Math.max(
        ...turn_base_list.map((monster) => monster.speed)
    );
    let temp_turn_base_list = [];
    turn_base_list.forEach((element) => {
        const timeToReachFull =
            (1 - element.progress) /
            ((element.speed / maxSpeed) * base_speed_factor);
        temp_turn_base_list.push(timeToReachFull);
    });

    const minTime = Math.min(...temp_turn_base_list);

    turn_base_list = turn_base_list.map((element) => ({
        id_in_battle: element.id_in_battle,
        speed: element.speed,
        progress:
            element.progress +
            (element.speed / maxSpeed) * minTime * base_speed_factor,
    }));

    turn_base_list.sort((a, b) => b.progress - a.progress || b.speed - a.speed); // if equal progress, compare speed

    console.log(
        "turn_base_list: " + turn_base_list.map((e) => e.id_in_battle).join(" ")
    );

    roomData.turn_base_data = turn_base_list;
}
