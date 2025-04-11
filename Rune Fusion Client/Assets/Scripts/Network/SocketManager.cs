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

    public List<List<string>> MapStart { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
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

    public void SetToken(string token)
    {
        this.Token = token;
    }

    public void SetUpConnectSocket()
    {
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
            List<List<List<string>>> mapData = JsonConvert.DeserializeObject<List<List<List<string>>>>(data.ToString());
            UnityThread.executeCoroutine(SaveStartMapCoroutine(mapData[0]));
        });
        
        socket.On(SocketEvents.Rune.NEW_RESPONSE, data =>
        {
            List<List<List<string>>> newRuneData = JsonConvert.DeserializeObject<List<List<List<string>>>>(data.ToString());
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
            UnityThread.executeCoroutine(ChangeSceneCoroutine(SceneLoadManager.Instance.PickSceneName));
        });
        socket.On(SocketEvents.Rune.OPPONENT_SWAP_RUNE, data =>
        {
            List<SwapRuneData> response = JsonConvert.DeserializeObject<List<SwapRuneData>>(data.ToString());
            UnityThread.executeCoroutine(SwapRuneCoroutine(response[0].startRune, response[0].endRune));
        });
        
        socket.On(SocketEvents.Game.TURN_BASE_LIST_PUSH_DATA, data =>
        {
            //Debug.Log(data);
            List<List<TurnBaseData>> response = JsonConvert.DeserializeObject<List<List<TurnBaseData>>>(data.ToString());
            UnityThread.executeCoroutine(TurnBaseListCoroutine(response[0]));
        });
        
        socket.On(SocketEvents.Monster.MONSTER_LIST, data =>
        {
            // Debug.Log(data);
            List<MonsterListData> response = JsonConvert.DeserializeObject<List<MonsterListData>>(data.ToString());
            UnityThread.executeCoroutine(MonsterListCoroutine(response[0]));
        });
        
        socket.On(SocketEvents.Monster.MONSTER_ACTION_RESPONSE, data =>
        {
            Debug.Log("MonsterAction " + data.ToString());
            List<MonsterActionResponse> response = JsonConvert.DeserializeObject<List<MonsterActionResponse>>(data.ToString());
            UnityThread.executeCoroutine(MonsterActionCoroutine(response[0]));
        });
        
        socket.On(SocketEvents.Monster.UPDATE_EFFECT_RESPONSE, data =>
        {
            Debug.Log("MonsterEffect " + data.ToString());
            List<UpdateEffectResponse> response = JsonConvert.DeserializeObject<List<UpdateEffectResponse>>(data.ToString());
            UnityThread.executeCoroutine(UpdateEffectCoroutine(response[0]));
        });
        
        socket.On(SocketEvents.Game.PICK_MONSTER_PUSH, data =>
        {
            Debug.Log("Monster pick "+ data.ToString());
            List<MonsterPickPush> response = JsonConvert.DeserializeObject<List<MonsterPickPush>>(data.ToString());
            UnityThread.executeCoroutine(UpdateMonsterPickData(response[0]));
        });
        socket.On(SocketEvents.Game.PICK_MONSTER_CONFIRM_PUSH, data =>
        {
            UnityThread.executeCoroutine(ConfirmMonsterTurnPickCoroutine());
        });
        socket.On(SocketEvents.Game.END_PICK_MONSTER, data =>
        {
            UnityThread.executeCoroutine(ChangeSceneCoroutine(SceneLoadManager.Instance.GameSceneName));
        });
        socket.On(SocketEvents.Game.POINT_INIT, data =>
        {
            List<PointPushData> response = JsonConvert.DeserializeObject<List<PointPushData>>(data.ToString());
            UnityThread.executeCoroutine(UpdatePointDataCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.POINT_UPDATE_PUSH, data =>
        {
            List<PointPushData> response = JsonConvert.DeserializeObject<List<PointPushData>>(data.ToString());
            UnityThread.executeCoroutine(UpdatePointDataCoroutine(response[0]));
        });
        
        socket.Connect();
    }
    // get
    private IEnumerator UpdatePointDataCoroutine(PointPushData data)
    {
        GameManager.Instance.RuneManager.UpdatePoint(data);
        yield return null;
    }
    private IEnumerator ConfirmMonsterTurnPickCoroutine()
    {
        PickSceneUIManager.Instance.PickSlotManager.ConfirmPick();
        yield return null;
    }
    private IEnumerator UpdateMonsterPickData(MonsterPickPush push)
    {
        PickSceneUIManager.Instance.MonsterSlotManager.UpdateMonsterPick(push);
        yield return null;
    }
    private IEnumerator UpdateEffectCoroutine(UpdateEffectResponse response)
    {
        Debug.Log("Call");
        GameManager.Instance.BattleManager.UpdateMonsterEffect(response);
        yield return null;
    }
    private IEnumerator MonsterActionCoroutine(MonsterActionResponse monsterActionResponse)
    {
        GameManager.Instance.BattleManager.TurnManager.ExecuteMonsterAction(monsterActionResponse);
        yield return null;
    }
    
    private IEnumerator MonsterListCoroutine(MonsterListData data)
    {
        BattleManager.Instance.SetUpMonster(data);
        yield return null;
    }
    
    private IEnumerator TurnBaseListCoroutine(List<TurnBaseData> mapData)
    {
        BattleManager.Instance.TurnManager.UpdateTurnBaseQueue(mapData);
        yield return null;
    }

    private IEnumerator SwapRuneCoroutine(Vector2Int start, Vector2Int end)
    {
        Debug.Log(start.ToString() + " " + end.ToString());
        GameManager.Instance.RuneManager.SwapRunes(Tuple.Create<int, int>(start.x, start.y), Tuple.Create<int, int>(end.x, end.y), start.x == end.x ? SwapType.Horizontal : SwapType.Vertical );
        yield return null;
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        SceneLoadManager.Instance.LoadSceneImmediately(sceneName);
        yield return null;
    }
    
    private IEnumerator SaveStartMapCoroutine(List<List<string>> mapData)
    {
        MapStart = mapData;
        yield return null;
    }
    
    private IEnumerator GenNewRuneCoroutine(List<List<string>> newRuneData)
    {
        GameManager.Instance.RuneManager.GenerateNewRune(newRuneData);
        yield return null;
    }
    

    //Emit
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
    
    private void OnApplicationQuit()
    {
        if(socket != null) socket.Disconnect();
    }

    public void GameStartRequest()
    {
        socket.Emit(SocketEvents.Game.GAME_START_REQUEST);
    }

    public void UpdateTurnRequest()
    {
        Debug.Log("Update Turn Request");
        socket.Emit(SocketEvents.Game.TURN_BASE_LIST_REQUEST);
    }

    public void RequestMonsterAction(string monsterId, List<string> targetId, string skillId)
    {
        MonsterActionRequest monsterActionRequest = new MonsterActionRequest
        {
            monster_id = monsterId,
            monster_target_id = targetId,
            skill_id = skillId
        };
        socket.Emit(SocketEvents.Monster.MONSTER_ACTION_REQUEST,JsonUtility.ToJson(monsterActionRequest));
    }

    public void UpdateMonsterEffectRequest(string monsterId)
    {
        socket.Emit(SocketEvents.Monster.UPDATE_EFFECT_REQUEST,monsterId);
    }

    public void PostMonsterPickData(List<MonsterPropsSO> p1, List<MonsterPropsSO> p2)
    {

        MonsterPickPost monsterPickPos = new MonsterPickPost();
        monsterPickPos.player1 = new List<MonsterData>();
        monsterPickPos.player2 = new List<MonsterData>();
        foreach (MonsterPropsSO p in p1)
        {
            monsterPickPos.player1.Add(p == null ? null : p.MonsterData);
        }
        foreach (MonsterPropsSO p in p2)
        {
            monsterPickPos.player2.Add(p == null ? null : p.MonsterData);
        }
        Debug.Log(JsonConvert.SerializeObject(monsterPickPos));
        socket.Emit(SocketEvents.Game.PICK_MONSTER_POST, JsonConvert.SerializeObject(monsterPickPos));
    }

    public void PostConfirmTurnPick()
    {
        socket.Emit(SocketEvents.Game.PICK_MONSTER_CONFIRM_POST);
    }
}
