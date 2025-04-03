const EVENTS = {
    RUNE: {
        CONSUME: "rune_consume",
        GENERATE_START_MAP: "generate_start_rune",
        NEW_REQUEST: "new_rune_request",
        NEW_RESPONSE: "new_rune_response",
        SWAP_RUNE: "swap_rune",
        OPPONENT_SWAP_RUNE: "opponent_swap_rune",
    },
    PLAYER: {
        FIND_MATCH: "find_match",
        MATCH_FOUND: "match_found",
        CURRENT_TURN: "current_turn",
        TURN_REQUEST: "turn_request",
        TURN_RESPONSE: "turn_response",
        PICK_MONSTER: "pick_monster",
    },
    GAME: {
        GAME_START_REQUEST: "game_start_request",
        TURN_BASE_LIST_PUSH_DATA: "turn_base_list_push_data",
        TURN_BASE_LIST_REQUEST: "turn_base_list_request",
    },
    MONSTER: {
        MONSTER_LIST: "monster_list",
        MONSTER_ACTION_REQUEST: "monster_action_request",
        MONSTER_ACTION_RESPONSE: "monster_action_response",
    },
};
export default EVENTS;
