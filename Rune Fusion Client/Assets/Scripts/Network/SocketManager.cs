using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SocketIOClient;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance { get; private set; }
    private SocketIOUnity socket;

    private MapData mapDataPayload;

    private string Token;
    
    private PlayerData playerData;
    public string RoomId {get; private set;}

    public List<List<int>> MapStart { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void SetPlayerNetworkData(string namePlayer,string idPlayer)
    {
        this.playerData = new()
        {
            playername = namePlayer,
            id = idPlayer
        };
    }

    public void SetToken(string token)
    {
        this.Token = token;
    }

    public void SetUpConnectSocket()
    {
        Debug.Log("socket setup");
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
        
        socket.On(SocketEvents.Rune.NEW_RESPONSE, data =>
        {
            List<List<List<int>>> newRuneData = JsonConvert.DeserializeObject<List<List<List<int>>>>(data.ToString());
            UnityThread.executeCoroutine(GenNewRuneCoroutine(newRuneData[0]));
        });
        
        socket.On(SocketEvents.Player.MATCH_FOUND, data =>
        {
            List<MatchFoundResponse> response = JsonConvert.DeserializeObject<List<MatchFoundResponse>>(data.ToString());
            Debug.Log( $"Ghép cặp thành công {response[0].player1.playername} {response[0].player2.playername}" );
            RoomId = response[0].roomId;
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
        Debug.Log(MapStart.Count() +" " + mapData.Count);
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
        socket.Emit(SocketEvents.Player.FIND_MATCH, JsonUtility.ToJson(playerData));
        Debug.Log(JsonUtility.ToJson(playerData));
    }

    public void SwapRune(Vector2 start,Vector2 end)
    {
        SwapRuneData swapRuneData = new SwapRuneData
        {
            roomId = RoomId,
            startRune = new Vector2Int((int)start.x, (int)start.y),
            endRune = new Vector2Int((int)end.x, (int)end.y)
        };
        socket.Emit(SocketEvents.Rune.SWAP_RUNE, JsonUtility.ToJson(swapRuneData));
    }

    private void OnApplicationQuit()
    {
        if(socket != null) socket.Disconnect();
    }
}
