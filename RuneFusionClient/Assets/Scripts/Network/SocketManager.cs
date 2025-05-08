using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using SocketIOClient;
using Unity.VisualScripting;
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

    public string Endpoint { get; private set; } = "http://localhost:3000";
    // public string Endpoint { get; private set; } = "https://rune-fusion.onrender.com";
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
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
        UIMainMenuManager.Instance.UIPlayerResource.SetTextName(PlayerData.playername);
    }

    public void SetToken(string token)
    {
        this.Token = token;
    }

    public void SetUpConnectSocket()
    {
        socket = new SocketIOUnity(Endpoint, new SocketIOOptions()
        {
            Query = new Dictionary<string, string>()
            {
                { "token", Token }
            }
        } );
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket connected");
            UnityThread.executeCoroutine(ChangeToTabScreen());
            GetInitMonstersData();
            ResquestResourceData();
        };
        socket.On(SocketEvents.Game.MONSTER_DATA_RESPONSE, data =>
        {
            Debug.Log("Get Monster Data Init " + data.ToString());
            List<InitMonsterData> monsterData = JsonConvert.DeserializeObject<List<InitMonsterData>>(data.ToString());
            UnityThread.executeCoroutine(InitMonstersDataCoroutine(monsterData[0]));
        });
        
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
        socket.On(SocketEvents.Game.POINT_INIT_RESPONSE, data =>
        {
            Debug.Log("Game point init" + data.ToString());
            List<PointPushData> response = JsonConvert.DeserializeObject<List<PointPushData>>(data.ToString());
            UnityThread.executeCoroutine(UpdatePointDataCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.POINT_UPDATE_PUSH, data =>
        {
            List<PointPushData> response = JsonConvert.DeserializeObject<List<PointPushData>>(data.ToString());
            UnityThread.executeCoroutine(UpdatePointDataCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.TALENT_POINT_UPDATE_RESPONSE, data =>
        {
            Debug.Log("Talent point update" + data.ToString());
            List<MonsterTalentPointRequestUpdateData> response = JsonConvert.DeserializeObject<List<MonsterTalentPointRequestUpdateData>>(data.ToString());
            UnityThread.executeCoroutine(UpdateTalentPointDataCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.SUMMON_RESPONSE, data =>
        {
            List<SummonResponseData> response = JsonConvert.DeserializeObject<List<SummonResponseData>>(data.ToString());
            UnityThread.executeCoroutine(SummonResponseCoroutine(response[0]));
        });
        socket.On(SocketEvents.Monster.MONSTER_OWN_RESPONSE, data =>
        {
            List<MonstersOwnResponseData> response = JsonConvert.DeserializeObject<List<MonstersOwnResponseData>>(data.ToString());
            UnityThread.executeCoroutine(UpdateMonsterOwnListCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.UPDATE_RESOURCE_RESPONSE, data =>
        {
            Debug.Log(data.ToString());
            List<ResourceData> response = JsonConvert.DeserializeObject<List<ResourceData>>(data.ToString());
            UnityThread.executeCoroutine(UpdateResourceCoroutine(response[0]));
        });
        socket.On(SocketEvents.Player.USE_SHIELD_RESPONSE, data =>
        {
            List<UseShieldData> response = JsonConvert.DeserializeObject<List<UseShieldData>>(data.ToString());
            UnityThread.executeCoroutine(UseShieldCoroutine(response[0]));
        });
        socket.On(SocketEvents.Game.END_GAME_RESPONSE, data =>
        {
            List<EndGameResponseData> response = JsonConvert.DeserializeObject<List<EndGameResponseData>>(data.ToString());
            UnityThread.executeCoroutine(EndGameCoroutine(response[0]));
        });
        socket.Connect();
    }
    // get
    private IEnumerator EndGameCoroutine(EndGameResponseData response)
    {
        if (response.loser_id == PlayerData.id)
        {
            GameUIManager.Instance.UIBattleEndNotification.SetLose(response.loser_gold);
        }
        else
        {
            GameUIManager.Instance.UIBattleEndNotification.SetVictory(response.winner_gold);
        }
        ResquestResourceData();
        yield return null;
    }
    private IEnumerator UseShieldCoroutine(UseShieldData useShieldData)
    {
        GameManager.Instance.BattleManager.UpdateMonsterShield(useShieldData);
        yield return null;
    }
    private IEnumerator ChangeToTabScreen()
    {
        UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance
            .UITabManager);  
        yield return null;
    }
    private IEnumerator UpdateResourceCoroutine(ResourceData resourceData)
    {
        // UIMainMenuManager.Instance.UIPlayerResource.SetResourceText(resourceData.gold, resourceData.scroll);
        UIMainMenuManager.Instance.OnResourceChange(resourceData.gold, resourceData.scroll);
        yield return null;
    }
    private IEnumerator UpdateMonsterOwnListCoroutine(MonstersOwnResponseData response)
    {
        UIMonsterListManager.Instance.OnMonstersOwnDataResponse?.Invoke(response);
        yield return null;
    }
    private IEnumerator SummonResponseCoroutine(SummonResponseData responseData)
    {
        UISummonManager.Instance.OnSummonResponseData?.Invoke(responseData);
        yield return null;
    }
    private IEnumerator UpdateTalentPointDataCoroutine(MonsterTalentPointRequestUpdateData data)
    {
        UIMainMenuManager.Instance.UpdateTalentPoint(data);
        yield return null;
    }
    private IEnumerator InitMonstersDataCoroutine(InitMonsterData initMonstersData)
    {
        UIMainMenuManager.Instance.InitializeMonsterData(initMonstersData);
        yield return null;
    }
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
        Debug.Log(JsonUtility.ToJson(response));
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
        Debug.Log("Request new Rune");
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
        if (!GameManager.Instance.BattleManager.IsBattleOver)
        {
            Debug.Log("Update Turn Request");
            socket.Emit(SocketEvents.Game.TURN_BASE_LIST_REQUEST);
        }
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
        Debug.Log("Update Monster Effect Request");
        socket.Emit(SocketEvents.Monster.UPDATE_EFFECT_REQUEST,monsterId);
    }

    public void PostMonsterPickData(List<MonsterPropsSO> p1, List<MonsterPropsSO> p2)
    {

        MonsterPickPost monsterPickPos = new MonsterPickPost();
        monsterPickPos.player1 = new List<int>();
        monsterPickPos.player2 = new List<int>();
        foreach (MonsterPropsSO p in p1)
        {
            monsterPickPos.player1.Add(p == null ? -1 : (int)p.MonsterData.Id);
        }
        foreach (MonsterPropsSO p in p2)
        {
            monsterPickPos.player2.Add(p == null ? -1 : (int)p.MonsterData.Id);
        }
        Debug.Log(JsonConvert.SerializeObject(monsterPickPos));
        socket.Emit(SocketEvents.Game.PICK_MONSTER_POST, JsonConvert.SerializeObject(monsterPickPos));
    }

    public void PostConfirmTurnPick()
    {
        socket.Emit(SocketEvents.Game.PICK_MONSTER_CONFIRM_POST);
    }
    public void RequestPointInit(){
    {
        socket.Emit(SocketEvents.Game.POINT_INIT_REQUEST);
    }}

    public void PostRunePoint(PointPushData pointPushData)
    {
        socket.Emit(SocketEvents.Game.POINT_UPDATE_POST, JsonUtility.ToJson(pointPushData));
    }

    public void GetInitMonstersData()
    {
        Debug.Log("Get Init Monsters Data");
        socket.Emit(SocketEvents.Game.MONSTER_DATA_REQUEST,JsonUtility.ToJson(PlayerData));
    }

    public void RequestUpdateTalentPoint(MonsterTalentPointRequestUpdateData monsterTalentPointRequestUpdate)
    {
        socket.Emit(SocketEvents.Game.TALENT_POINT_UPDATE_REQUEST,JsonConvert.SerializeObject(monsterTalentPointRequestUpdate));
    }

    public void RequestSummonData(int times)
    {
        SummonRequestData summonRequestData = new SummonRequestData()
        {
            player_id = PlayerData.id,
            summon_times = times,
        };
        socket.Emit(SocketEvents.Game.SUMMON_REQUEST, JsonConvert.SerializeObject(summonRequestData));
    }

    public void RequestMonsterOwnData()
    {
        // Debug.Log(JsonConvert.SerializeObject(PlayerData));
        MonstersOwnRequestData requestData = new MonstersOwnRequestData()
        {
            player_id = PlayerData.id,
        };
        Debug.Log(JsonConvert.SerializeObject(requestData));
        socket.Emit(SocketEvents.Monster.MONSTER_OWN_RESQUEST, JsonConvert.SerializeObject(requestData));
    }

    public void ResquestResourceData()
    {
        ResourceRequestData requestData = new ResourceRequestData()
        {
            player_id = PlayerData.id,
        };
        Debug.Log("Request Resource Data");
        socket.Emit(SocketEvents.Game.UPDATE_RESOURCE_REQUEST, JsonConvert.SerializeObject(requestData));
    }

    public void PushUseShieldData(string id)
    {
        UseShieldData data = new UseShieldData()
        {
            player_id = PlayerData.id,
            monster_id_in_battle = id,
        };
        Debug.Log("Push Use Shield Data");
        socket.Emit(SocketEvents.Player.USE_SHIELD_PUSH,JsonConvert.SerializeObject(data));
    }

    public void PushBuyData(int scrollAmount, int goldPrice)
    {
        Debug.Log($"Buy {scrollAmount} with {goldPrice}");
        BuyData buyData = new BuyData()
        {
            player_id = PlayerData.id,
            scroll_amount = scrollAmount,
            gold_price = goldPrice,
        };
        socket.Emit(SocketEvents.Game.BUY_DATA_PUSH, JsonConvert.SerializeObject(buyData));
    }

    public void RequestEndGame()
    {
        socket.Emit(SocketEvents.Game.END_GAME_REQUEST);
    }
}
