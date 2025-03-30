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
    private bool isTimeMonsterEnd;

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
        GameManager.Instance.InputManager.SetDisableMonsterInput();
        if (currentTurnId == "01" )
        {
            if (PlayerIndex == 0)
            {
                StartPlayerTurn();
                Debug.Log("Player 1 Turn");
            }
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }else if (currentTurnId == "02" )
        {
            if (PlayerIndex == 1)
            {
                StartPlayerTurn();
                Debug.Log("Player 2 Turn");
            }
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }
        else
        {
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
        }
        if (GameManager.Instance.BattleManager.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
        {
                       
            GameManager.Instance.InputManager.SetEnableMonsterInput();
        }
        yield return new WaitUntil(() => isTimeMonsterEnd);
        isTimeMonsterEnd = false;
        GameManager.Instance.BattleManager.GetMonsterById(currentTurnId).Attack();
        if (GameManager.Instance.BattleManager.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 0)
        {
            GameManager.Instance.BattleManager.CanChangeTurn = true;
        }
        if (GameManager.Instance.BattleManager.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && SocketManager.Instance.PlayerData.playerindex == 1)
        {
                       
            GameManager.Instance.BattleManager.CanChangeTurn = true;
        }
    }

    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitUntil(() => GameManager.Instance.BattleManager.CanChangeTurn);
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