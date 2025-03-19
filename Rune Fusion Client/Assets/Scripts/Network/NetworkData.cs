
using System;
using System.Collections.Generic;
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
        public List<MonsterData> player1;
        public List<MonsterData> player2;
}

[Serializable]
public class MonsterData
{
        public int id;
        public string id_in_battle;
        public int speed;
}