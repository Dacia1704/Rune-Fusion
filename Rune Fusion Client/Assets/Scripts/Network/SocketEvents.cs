public static class SocketEvents
{
    public static class Rune
    {
        public const string CONSUME = "rune_consume";
        public const string GENERATE_START_MAP = "generate_start_rune";
        public const string NEW_REQUEST = "new_rune_request";
        public const string NEW_RESPONSE = "new_rune_response";
        public const string SWAP_RUNE = "swap_rune";
        public const string OPPONENT_SWAP_RUNE = "opponent_swap_rune";
    }

    public static class Player
    {
        public const string FIND_MATCH = "find_match";
        public const string MATCH_FOUND = "match_found";
        public const string CURRENT_TURN = "current_turn";
        public const string TURN_REQUEST = "turn_request";
        public const string TURN_RESPONSE = "turn_response";
    }

    public static class Game
    {
        public const string GAME_START_REQUEST = "game_start_request";
        public const string TURN_BASE_LIST_PUSH_DATA = "turn_base_list_push_data";
        public const string TURN_BASE_LIST_REQUEST = "turn_base_list_request";
    }

    public static class Monster
    {
        public const string MONSTER_LIST = "monster_list";
        public const string MONSTER_ACTION_REQUEST = "monster_action_request";
        public const string MONSTER_ACTION_RESPONSE = "monster_action_response";
        public const string UPDATE_EFFECT_REQUEST = "update_effect_request";
        public const string UPDATE_EFFECT_RESPONSE = "update_effect_response";
    }
}