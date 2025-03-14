using System;
using System.Collections;
using UnityEngine;


/*
 * playerIndex được gửi khi gửi lại room
 * Gửi map start thì gửi luôn playerIndex đi trước;
 */
public class TurnManager : MonoBehaviour
{
    private int playerIndex; 
    public bool isPlayerTurn { get; private set; }


    private void Start()
    {
        playerIndex = SocketManager.Instance.PlayerData.playerindex;
        SocketManager.Instance.OnCurrentPlayerIndexChanged += UpdateTurn; // update turn except first turn
        UpdateTurn(SocketManager.Instance.CurrentPlayerIndex); // update first turn
    }

    public void UpdateTurn(int currentPlayerIndex)
    {
        if (currentPlayerIndex != playerIndex)
        {
            EndTurn();
        }
        else
        {
            StartTurn();
        }
        Debug.Log(isPlayerTurn?"My Turn": "Opponent Turn");
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
}