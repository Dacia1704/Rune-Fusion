﻿public static class SocketEvents
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
        public const string USE_SHIELD_PUSH = "use_shield_request";
        public const string USE_SHIELD_RESPONSE = "use_shield_response";
    }

    public static class Game
    {
        public const string GAME_START_REQUEST = "game_start_request";
        public const string TURN_BASE_LIST_PUSH_DATA = "turn_base_list_push_data";
        public const string TURN_BASE_LIST_REQUEST = "turn_base_list_request";
        public const string PICK_MONSTER_POST = "pick_monster_post";
        public const string PICK_MONSTER_PUSH = "pick_monster_push";
        public const string PICK_MONSTER_CONFIRM_POST = "pick_monster_confirm_post";
        public const string PICK_MONSTER_CONFIRM_PUSH = "pick_monster_confirm_push";
        public const string END_PICK_MONSTER = "end_pick_monster";
        public const string POINT_INIT_REQUEST= "point_init_request";
        public const string POINT_INIT_RESPONSE= "point_init_response";
        public const string POINT_UPDATE_POST = "point_update_post";
        public const string POINT_UPDATE_PUSH = "point_update_push";
        public const string TALENT_POINT_UPDATE_REQUEST = "talent_point_update_request";
        public const string TALENT_POINT_UPDATE_RESPONSE = "talent_point_update_response";
        public const string MONSTER_DATA_REQUEST = "monster_data_request";
        public const string MONSTER_DATA_RESPONSE = "monster_data_response";
        public const string SUMMON_REQUEST = "summon_request";
        public const string SUMMON_RESPONSE = "summon_response";
        public const string UPDATE_RESOURCE_RESPONSE = "update_resource_response";
        public const string UPDATE_RESOURCE_REQUEST = "update_resource_request";
        public const string BUY_DATA_PUSH = "buy_data_push";
        public const string END_GAME_REQUEST = "end_game_request";
        public const string END_GAME_RESPONSE = "end_game_response";
    }

    public static class Monster
    {
        public const string MONSTER_LIST = "monster_list";
        public const string MONSTER_OWN_RESQUEST = "monster_own_request";
        public const string MONSTER_OWN_RESPONSE = "monster_own_response";
        public const string MONSTER_ACTION_REQUEST = "monster_action_request";
        public const string MONSTER_ACTION_RESPONSE = "monster_action_response";
        public const string UPDATE_EFFECT_REQUEST = "update_effect_request";
        public const string UPDATE_EFFECT_RESPONSE = "update_effect_response";
    }
}