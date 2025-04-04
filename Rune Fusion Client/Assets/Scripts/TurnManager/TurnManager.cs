﻿using System;
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

    private void Start()
    {
        PlayerIndex = SocketManager.Instance.PlayerData.playerindex;
        Debug.Log("Player Index: "+ PlayerIndex);
        TurnBaseQueue = new List<TurnBaseData>();
        ActionLine = FindFirstObjectByType<ActionLine>();
        TurnBaseQueueChanged += ActionLine.UpdateMonsterPoint;
        isTimeMonsterEnd = false;
        GameUIManager.Instance.UITimeCounter.OnTimeCounterEnd += () => isTimeMonsterEnd = true;
    }
    
    public void ExecuteTurn()
    {
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        EndPlayerTurn();
        if (currentTurnId == "01" )
        {
            if (PlayerIndex == 0)
            {
                StartPlayerTurn();
                Debug.Log("Player 1 Turn");
            }

            GameUIManager.Instance.SetTurnText("P1",SocketManager.Instance.PlayerData.playerindex == 0 ? Color.red: Color.blue);
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }else if (currentTurnId == "02" )
        {
            if (PlayerIndex == 1)
            {
                StartPlayerTurn();
                Debug.Log("Player 2 Turn");
            }
            GameUIManager.Instance.SetTurnText("P2",SocketManager.Instance.PlayerData.playerindex == 1 ? Color.red: Color.blue);
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }
        else
        {
            if (GameManager.Instance.BattleManager.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 0)
            {
                GameUIManager.Instance.SetTurnText(currentTurnId, Color.red);
            }
            else if (GameManager.Instance.BattleManager.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
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
        StartCoroutine(EndTurnCoroutine());
    }
    
    public IEnumerator MonsterTurn()
    {
        isTimeMonsterEnd = false;
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        if (GameManager.Instance.BattleManager.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 0)
        {
            GameManager.Instance.InputManager.SetEnableMonsterInput();
            StartCoroutine(StartMonsterAttack());
            GameManager.Instance.BattleManager.SetFalseAnimation();
            yield return new WaitUntil(() => isTimeMonsterEnd);
            isTimeMonsterEnd = false;
            yield return new WaitUntil(() => GameManager.Instance.BattleManager.CheckAnimationMonster());
            GameManager.Instance.BattleManager.CanChangeTurn = true;
        }
        else if (GameManager.Instance.BattleManager.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
        {
                       
            GameManager.Instance.InputManager.SetEnableMonsterInput();
            StartCoroutine(StartMonsterAttack());
            yield return new WaitUntil(() => isTimeMonsterEnd);
            isTimeMonsterEnd = false;
            yield return new WaitUntil(() => GameManager.Instance.BattleManager.CheckAnimationMonster());
            GameManager.Instance.BattleManager.CanChangeTurn = true;
        }
    }
    private IEnumerator StartMonsterAttack()
    {
        yield return new WaitUntil(() => GameManager.Instance.BattleManager.TargetManager.TargetedMonster != null);
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        SocketManager.Instance.RequestMonsterAction(currentTurnId,new List<string>(){GameManager.Instance.BattleManager.TargetManager.TargetedMonster.MonsterIdInBattle},"0");
        isTimeMonsterEnd = true;
        GameUIManager.Instance.UITimeCounter.EndCountTime();
        yield return null;
    }
    public void ExecuteMonsterAction(MonsterActionResponse monsterActionResponse)
    {
        GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse);
        Debug.Log(monsterActionResponse.monster_id +" attack " + monsterActionResponse.monster_target_id[0]);
        if (GameManager.Instance.BattleManager.MonsterTeam1Dictionary.ContainsKey(monsterActionResponse.monster_id))
        {
            GameManager.Instance.BattleManager.MonsterTeam1Dictionary[monsterActionResponse.monster_id]
                .StartAttack(monsterActionResponse);
        }
        else
        {
            GameManager.Instance.BattleManager.MonsterTeam2Dictionary[monsterActionResponse.monster_id]
                .StartAttack(monsterActionResponse);
        }
    }
    
    
    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitUntil(() => GameManager.Instance.BattleManager.CanChangeTurn);
        GameManager.Instance.BattleManager.CanChangeTurn = false;
        SocketManager.Instance.UpdateTurnRequest();
    }
    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        GameManager.Instance.InputManager.SetEnablePlayerInput();
        GameManager.Instance.BattleManager.CanChangeTurn = false;
    }
    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        GameManager.Instance.InputManager.SetDisablePlayerInput();
        GameManager.Instance.BattleManager.CanChangeTurn = false;
    }
    public void UpdateTurnBaseQueue(List<TurnBaseData> turnBaseQueue)
    {
        TurnBaseQueue = turnBaseQueue;
        TurnBaseQueueChanged?.Invoke(TurnBaseQueue);
    }
}