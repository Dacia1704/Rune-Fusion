using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


/*
 * playerIndex được gửi khi gửi lại room
 * Gửi map start thì gửi luôn playerIndex đi trước;
 */
public class TurnManager : MonoBehaviour
{
    public List<TurnBaseData> TurnBaseQueue { get; private set; }    
    
    public int PlayerIndex; 
    [field: SerializeField]public bool IsPlayerTurn { get; private set; }
    
    public event Action<List<TurnBaseData>> TurnBaseQueueChanged;
    public ActionLine ActionLine {get; private set;}
    [SerializeField] private bool isTimeMonsterEnd;
    
    private Coroutine currentEndTurnCoroutine;

    private void Start()
    {
        PlayerIndex = SocketManager.Instance.PlayerData.playerindex;
        Debug.Log("Player Index: "+ PlayerIndex);
        TurnBaseQueue = new List<TurnBaseData>();
        ActionLine = FindFirstObjectByType<ActionLine>();
        TurnBaseQueueChanged += ActionLine.UpdateMonsterPoint;
        isTimeMonsterEnd = false;
        GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += () =>
        {
            // Debug.Log("Monster Time Counter End");
            isTimeMonsterEnd = true;
        };
    }
    
    public void ExecuteTurn()
    {
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        // Debug.Log(currentTurnId);
        EndPlayerTurn();
        GameManager.Instance.Match.CanChangeTurn = false;
        if (currentTurnId == "01" )
        {
            Debug.Log("Player 1 Turn");
            if (PlayerIndex == 0)
            {
                StartPlayerTurn();
            }

            GameUIManager.Instance.SetTurnText("P1",SocketManager.Instance.PlayerData.playerindex == 0 ? Color.red: Color.blue);
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
            // Debug.LogError("123");
        }else if (currentTurnId == "02" )
        {
            Debug.Log("Player 2 Turn");
            if (PlayerIndex == 1)
            {
                StartPlayerTurn();
            }
            GameUIManager.Instance.SetTurnText("P2",SocketManager.Instance.PlayerData.playerindex == 1 ? Color.red: Color.blue);
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
            // Debug.LogError("123");
        }
        else
        {
            if (GameManager.Instance.Match.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 0)
            {
                GameUIManager.Instance.SetTurnText(currentTurnId, Color.red);
            }
            else if (GameManager.Instance.Match.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
            {
                GameUIManager.Instance.SetTurnText(currentTurnId, Color.red);
            }
            else
            {
                GameUIManager.Instance.SetTurnText(currentTurnId, Color.blue);
            }
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimeMonsterTurn);
            StartCoroutine(MonsterTurn());
        }
        // StartCoroutine(EndTurnCoroutine());
        
        if (currentEndTurnCoroutine != null)
        {
            StopCoroutine(currentEndTurnCoroutine);
        }
        currentEndTurnCoroutine = StartCoroutine(EndTurnCoroutine());
    }
    
    public IEnumerator MonsterTurn()
    {
        isTimeMonsterEnd = false;
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        MonsterBase monsterBase = GameManager.Instance.Match.GetMonsterByIdInBattle(currentTurnId);
        monsterBase.SetTurnObject(true);
        if (GameManager.Instance.Match.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 0)
        {
            if (!monsterBase.IsDead)
            {
                if (!monsterBase.IsFrozen)
                {
                    
                    GameManager.Instance.InputManager.SetEnableMonsterInput();
                    StartCoroutine(StartMonsterAttack());
                    GameManager.Instance.Match.SetFalseAnimation();
                    // yield return new WaitUntil(() => isTimeMonsterEnd);
                    // isTimeMonsterEnd = false;
                    yield return new WaitUntil(() => GameManager.Instance.Match.CheckAnimationMonster());
                }
                GameManager.Instance.InputManager.SetDisableMonsterInput();
                monsterBase.IsUpdateEffect = false;
                SocketManager.Instance.UpdateMonsterEffectRequest(currentTurnId);
                yield return new WaitUntil(() => monsterBase.IsUpdateEffect);
            }
            GameManager.Instance.Match.CanChangeTurn = true;
        }
        else if (GameManager.Instance.Match.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
        {
            if (!monsterBase.IsDead)
            {
                if (!monsterBase.IsFrozen)
                {
                    GameManager.Instance.InputManager.SetEnableMonsterInput();
                    StartCoroutine(StartMonsterAttack());
                    GameManager.Instance.Match.SetFalseAnimation();
                    // yield return new WaitUntil(() => isTimeMonsterEnd);
                    // isTimeMonsterEnd = false;
                    yield return new WaitUntil(() => GameManager.Instance.Match.CheckAnimationMonster());
                }
                GameManager.Instance.InputManager.SetDisableMonsterInput();
                monsterBase.IsUpdateEffect = false;
                SocketManager.Instance.UpdateMonsterEffectRequest(currentTurnId);
                yield return new WaitUntil(() => monsterBase.IsUpdateEffect);
            }
            GameManager.Instance.Match.CanChangeTurn = true;
        }
        
        // string currentTurnId = TurnBaseQueue[0].id_in_battle;
        // if (currentTurnId[0] == '1' && PlayerIndex == 0)
        // {
        //     GameManager.Instance.BattleManager.CanChangeTurn = true;
        // }
        // else if (currentTurnId[0] == '2' && PlayerIndex == 1)
        // {
        //     GameManager.Instance.BattleManager.CanChangeTurn = true;
        // }
        // yield return null;
    }
    private IEnumerator StartMonsterAttack()
    {
        yield return new WaitUntil(() => (GameManager.Instance.Match.TargetManager.TargetedMonster != null || isTimeMonsterEnd));
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        MonsterBase monsterBase = GameManager.Instance.Match.GetMonsterByIdInBattle(currentTurnId);
        if (isTimeMonsterEnd)
        {
            Match.Instance.SkillEnableCheck(monsterBase);
            Debug.Log("Time monster end");
            MonsterBase autoChoose = GameManager.Instance.Match.AutoChooseTargetMonster(currentTurnId);
            SocketManager.Instance.RequestMonsterAction(currentTurnId,new List<string>(){autoChoose.MonsterIdInBattle},monsterBase.ShouldUseSkill ?"1": "0");
        }
        else
        {
            SocketManager.Instance.RequestMonsterAction(currentTurnId,new List<string>(){GameManager.Instance.Match.TargetManager.TargetedMonster.MonsterIdInBattle},monsterBase.ShouldUseSkill ?"1": "0");
        }
        GameManager.Instance.Match.MatchBoard.PostPointData();
        monsterBase.ShouldUseSkill = false;        
        isTimeMonsterEnd = false;
        GameUIManager.Instance.UITimeCounter.EndCountTime();
        yield return null;
    }
    public void ExecuteMonsterAction(MonsterActionResponse monsterActionResponse)
    {
        if (monsterActionResponse.skill_id == "0")
        {
            Debug.Log(monsterActionResponse.monster_id +" attack " + monsterActionResponse.monster_target_id[0]);
            if (GameManager.Instance.Match.MonsterTeam1Dictionary.ContainsKey(monsterActionResponse.monster_id))
            {
                GameManager.Instance.Match.MonsterTeam1Dictionary[monsterActionResponse.monster_id]
                    .StartAttack(monsterActionResponse);
            }
            else
            {
                GameManager.Instance.Match.MonsterTeam2Dictionary[monsterActionResponse.monster_id]
                    .StartAttack(monsterActionResponse);
            }
        }
        else
        {
            Debug.Log(monsterActionResponse.monster_id +" use ultimate skill to " + monsterActionResponse.monster_target_id[0]);
            if (GameManager.Instance.Match.MonsterTeam1Dictionary.ContainsKey(monsterActionResponse.monster_id))
            {
                GameManager.Instance.Match.MonsterTeam1Dictionary[monsterActionResponse.monster_id]
                    .StartSkill(monsterActionResponse);
            }
            else
            {
                GameManager.Instance.Match.MonsterTeam2Dictionary[monsterActionResponse.monster_id]
                    .StartSkill(monsterActionResponse);
            }
        }
    }
    
    
    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitUntil(() => GameManager.Instance.Match.CanChangeTurn);
        Match.Instance.TargetManager.DisableTarget();
        GameManager.Instance.Match.CanChangeTurn = false;
        SocketManager.Instance.UpdateTurnRequest();
    }
    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        GameManager.Instance.InputManager.SetEnablePlayerInput();
        GameManager.Instance.Match.CanChangeTurn = false;
    }
    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        GameManager.Instance.InputManager.SetDisablePlayerInput();
        GameManager.Instance.Match.CanChangeTurn = false;
    }
    public void UpdateTurnBaseQueue(List<TurnBaseData> turnBaseQueue)
    {
        TurnBaseQueue = turnBaseQueue;
        TurnBaseQueueChanged?.Invoke(TurnBaseQueue);
    }
}