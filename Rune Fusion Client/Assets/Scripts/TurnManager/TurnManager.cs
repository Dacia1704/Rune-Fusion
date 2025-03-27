using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * playerIndex được gửi khi gửi lại room
 * Gửi map start thì gửi luôn playerIndex đi trước;
 */
public class TurnManager : MonoBehaviour
{
    public List<TurnBaseData> TurnBaseQueue { get; private set; }    
    
    private int playerIndex; 
    [field: SerializeField]public bool IsPlayerTurn { get; private set; }
    
    public event Action<List<TurnBaseData>> TurnBaseQueueChanged;
    public ActionLine ActionLine {get; private set;}

    private void Start()
    {
        playerIndex = SocketManager.Instance.PlayerData.playerindex;
        Debug.Log("Player Index: "+ playerIndex);
        TurnBaseQueue = new List<TurnBaseData>();
        ActionLine = FindFirstObjectByType<ActionLine>();
        TurnBaseQueueChanged += ActionLine.UpdateMonsterPoint;
    }
    
    public void ExecuteTurn()
    {
        string currentTurnId = TurnBaseQueue[0].id_in_battle;
        EndTurn();
        if (currentTurnId == "01" )
        {
            if (playerIndex == 0)
            {
                StartTurn();
                Debug.Log("Player 1 Turn");
            }
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }else if (currentTurnId == "02" )
        {
            if (playerIndex == 1)
            {
                StartTurn();
                Debug.Log("Player 2 Turn");
            }
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimePlayerTurn);
        }
        else
        {
            GameUIManager.Instance.UITimeCounter.SetCountTime(GameManager.Instance.GameManagerSO.TimeMonsterTurn);
            GameManager.Instance.BattleManager.MonsterTurn();
        }
        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        yield return new WaitUntil(() => GameManager.Instance.BattleManager.CanChangeTurn);
        SocketManager.Instance.UpdateTurnRequest();
    }
    public void StartTurn()
    {
        IsPlayerTurn = true;
        GameManager.Instance.InputManager.SetEnableInput();
        GameManager.Instance.BattleManager.CanChangeTurn = false;
    }
    public void EndTurn()
    {
        IsPlayerTurn = false;
        GameManager.Instance.InputManager.SetDisableInput();
        GameManager.Instance.BattleManager.CanChangeTurn = false;
    }
    public void UpdateTurnBaseQueue(List<TurnBaseData> turnBaseQueue)
    {
        TurnBaseQueue = turnBaseQueue;
        TurnBaseQueueChanged?.Invoke(TurnBaseQueue);
    }
}