using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SocketIOClient;
using UnityEngine;
using UnityEngine.Serialization;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance { get; private set; }
    private SocketIOUnity socket;

    private MapData mapDataPayload;

    private string Token;
    
    public PlayerData PlayerData { get; private set; }
    public PlayerData OpponentData { get; private set; }
    public string RoomId {get; private set;}
    public int CurrentPlayerIndex;
    public event Action<int> OnCurrentPlayerIndexChanged;

    public List<List<int>> MapStart { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        CurrentPlayerIndex = -1;
    }

    public void SetPlayerNetworkData(string namePlayer,string idPlayer,int playerIndex)
    {
        this.PlayerData = new()
        {
            playername = namePlayer,
            id = idPlayer,
            playerindex =  playerIndex
        };
    }
    public void SetCurrentPlayerIndex(int playerIndex)
    {
        if (CurrentPlayerIndex != -1)
        {
            Debug.Log("change");
            OnCurrentPlayerIndexChanged?.Invoke(playerIndex);
        }
        this.CurrentPlayerIndex = playerIndex;
    }

    public void SetToken(string token)
    {
        this.Token = token;
    }

    public void SetUpConnectSocket()
    {
        // Debug.Log("socket setup");
        socket = new SocketIOUnity("http://localhost:3000", new SocketIOOptions()
        {
            Query = new Dictionary<string, string>()
            {
                { "token", Token }
            }
        } );
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket connected");
        };
        
        socket.On(SocketEvents.Rune.GENERATE_START_MAP, data =>
        {
            Debug.Log("Socket start map");
            List<List<List<int>>> mapData = JsonConvert.DeserializeObject<List<List<List<int>>>>(data.ToString());
            UnityThread.executeCoroutine(SaveStartMapCoroutine(mapData[0]));
        });
        socket.On(SocketEvents.Player.CURRENT_TURN, data =>
        {
            List<int> turnData = JsonConvert.DeserializeObject<List<int>>(data.ToString());
            SetCurrentPlayerIndex(turnData[0]);
        });
        socket.On(SocketEvents.Player.TURN_RESPONSE, data =>
        {
            List<int> turnData = JsonConvert.DeserializeObject<List<int>>(data.ToString());
            SetCurrentPlayerIndex(turnData[0]);
        });
        
        socket.On(SocketEvents.Rune.NEW_RESPONSE, data =>
        {
            List<List<List<int>>> newRuneData = JsonConvert.DeserializeObject<List<List<List<int>>>>(data.ToString());
            UnityThread.executeCoroutine(GenNewRuneCoroutine(newRuneData[0]));
        });
        
        socket.On(SocketEvents.Player.MATCH_FOUND, data =>
        {
            List<MatchFoundResponse> response = JsonConvert.DeserializeObject<List<MatchFoundResponse>>(data.ToString());
            Debug.Log( $"Ghép cặp thành công {response[0].player1.playername}:{response[0].player1.playerindex} {response[0].player2.playername}:{response[0].player2.playerindex}" );
            if (response[0].player1.id == PlayerData.id)
            {
                PlayerData = response[0].player1;
                OpponentData = response[0].player2;
            }
            else
            {
                PlayerData = response[0].player2;
                OpponentData = response[0].player1;
            }
            UnityThread.executeCoroutine(ChangeSceneCoroutine(SceneLoadManager.Instance.GameSceneName));
        });
        socket.On(SocketEvents.Rune.OPPONENT_SWAP_RUNE, data =>
        {
            List<SwapRuneData> response = JsonConvert.DeserializeObject<List<SwapRuneData>>(data.ToString());
            UnityThread.executeCoroutine(SwapRuneCoroutine(response[0].startRune, response[0].endRune));
        });
        socket.Connect();
    }

    private IEnumerator SwapRuneCoroutine(Vector2Int start, Vector2Int end)
    {
        Debug.Log(start.ToString() + " " + end.ToString());
        GameManager.Instance.RuneManager.SwapRunes(Tuple.Create<int, int>(start.x, start.y), Tuple.Create<int, int>(end.x, end.y) );
        yield return null;
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        SceneLoadManager.Instance.LoadSceneImmediately(sceneName);
        yield return null;
    }
    
    private IEnumerator SaveStartMapCoroutine(List<List<int>> mapData)
    {
        MapStart = mapData;
        yield return null;
    }

    private IEnumerator GenNewRuneCoroutine(List<List<int>> newRuneData)
    {
        GameManager.Instance.RuneManager.GenerateNewRune(newRuneData);
        yield return null;
    }
    

    public void RequestNewRune(string mapData)
    {
        socket.Emit(SocketEvents.Rune.NEW_REQUEST, mapData);
    }

    public void FindMatch()
    {
        socket.Emit(SocketEvents.Player.FIND_MATCH, JsonUtility.ToJson(PlayerData));
        Debug.Log(JsonUtility.ToJson(PlayerData));
    }

    public void SwapRune(Vector2 start,Vector2 end)
    {
        SwapRuneData swapRuneData = new SwapRuneData
        {
            startRune = new Vector2Int((int)start.x, (int)start.y),
            endRune = new Vector2Int((int)end.x, (int)end.y)
        };
        socket.Emit(SocketEvents.Rune.SWAP_RUNE, JsonUtility.ToJson(swapRuneData));
    }

    public void TurnRequest()
    {
        Debug.Log("Turn Request");
        socket.Emit(SocketEvents.Player.TURN_REQUEST, CurrentPlayerIndex);
    }

    private void OnApplicationQuit()
    {
        if(socket != null) socket.Disconnect();
    }
}
