
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class ErrorResponse
{
        public string error;
}

[Serializable]
public class LoginResponse
{
        public string token;
        public GameAccount user;
        public string message;
}

[Serializable]
public class CreateAccountResponse
{
        public GameAccount user;
        public string message;
}

[Serializable]
public class GameAccount
{
        public string _id;
        public string username;
}

[Serializable]
public class PlayerData
{
        public string id;
        public string playername;
        public int playerindex;
}

[Serializable]
public class MatchFoundResponse
{
        public PlayerData player1;
        public PlayerData player2;
}
[Serializable]
public class SwapRuneData
{
        public Vector2Int startRune;
        public Vector2Int endRune;
}

[Serializable]
public class TurnBaseData
{
        public string id_in_battle;
        public int speed;
        public float progress;
}

[Serializable]
public class MonsterListData
{
        public List<MonsterInBattleData> player1;
        public List<MonsterInBattleData> player2;
}

[Serializable]
public class MonsterInBattleData
{
        public int id;
        public string id_in_battle;
}

[Serializable]
public class MonsterActionRequest
{
        public string monster_id;
        public List<string> monster_target_id;
        public string skill_id;
}
[Serializable]
public class MonsterActionResponse
{
        public string monster_id;
        public List<string> monster_target_id;
        public string skill_id;
        public List<List<ActionResponse>> action_affect_list; // mỗi phn tử action là 1 action của skill
}

[Serializable]
public class ActionResponse
{
        public string id_in_battle;     
        public int dam;
        public EffectSkill effect;
}

[Serializable]
public class UpdateEffectResponse
{
        public string id_in_battle;
        public int dam;
        public List<EffectSkill> effect_list;
}