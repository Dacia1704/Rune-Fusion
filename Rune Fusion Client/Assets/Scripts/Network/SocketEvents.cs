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
    }
}