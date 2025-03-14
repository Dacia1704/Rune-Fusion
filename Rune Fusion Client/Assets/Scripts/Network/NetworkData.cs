
using System;
using UnityEngine;


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
}

[Serializable]
public class MatchFoundResponse
{
        public string roomId;
        public PlayerData player1;
        public PlayerData player2;
}
[Serializable]
public class SwapRuneData
{
        public string roomId;
        public Vector2Int startRune;
        public Vector2Int endRune;
}