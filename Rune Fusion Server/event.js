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
  },
};
export default EVENTS;
