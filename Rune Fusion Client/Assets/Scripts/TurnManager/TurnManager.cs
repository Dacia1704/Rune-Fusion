using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private int currentPlayerIndex = 0; 
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartTurn()
    {
        Debug.Log($"Player {currentPlayerIndex}'s Turn");
        isPlayerTurn = true;
    }

    public void EndTurn()
    {
        if (!isPlayerTurn) return;
        isPlayerTurn = false;
        
        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(1f);

        currentPlayerIndex = (currentPlayerIndex + 1) % 2; 
        StartTurn();
    }
}