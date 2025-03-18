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
    public bool isPlayerTurn { get; private set; }

    public event Action<List<TurnBaseData>> TurnBaseQueueChanged;
    public ActionLine ActionLine {get; private set;}

    public Action OnEndTurn;

    private void Start()
    {
        playerIndex = SocketManager.Instance.PlayerData.playerindex;
        Debug.Log("Player Index: "+ playerIndex);
        TurnBaseQueue = new List<TurnBaseData>();
        ActionLine = FindFirstObjectByType<ActionLine>();
        TurnBaseQueueChanged += ActionLine.UpdateMonsterPoint;
        // TurnBaseQueueChanged += ExecuteTurn;
        OnEndTurn += SocketManager.Instance.UpdateTurnRequest;
    }
    
    public void ExecuteTurn(/*List<TurnBaseData> turnBaseData*/)
    {
        string currentTurnId = TurnBaseQueue[0].id;
        EndTurn();
        if (currentTurnId == "01" )
        {
            if (playerIndex == 0)
            {
                StartTurn();
                Debug.Log("Player 1 Turn");
            }
        }else if (currentTurnId == "02" )
        {
            if (playerIndex == 1)
            {
                StartTurn();
                Debug.Log("Player 2 Turn");
            }
        }
        else
        {
            BattleManager.Instance.GetMonsterById(currentTurnId).Attack();
            if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsKey(currentTurnId) && playerIndex == 0)
            {
                OnEndTurn?.Invoke();
            }
            if (BattleManager.Instance.MonsterTeam2Dictionary.ContainsKey(currentTurnId) && playerIndex == 1)
            {
                OnEndTurn?.Invoke();
            }
        }
    }
    public void StartTurn()
    {
        isPlayerTurn = true;
        GameManager.Instance.InputManager.SetEnableInput();
    }
    public void EndTurn()
    {
        isPlayerTurn = false;
        GameManager.Instance.InputManager.SetDisableInput();
    }
    public void UpdateTurnBaseQueue(List<TurnBaseData> turnBaseQueue)
    {
        TurnBaseQueue = turnBaseQueue;
        TurnBaseQueueChanged?.Invoke(TurnBaseQueue);
    }
}