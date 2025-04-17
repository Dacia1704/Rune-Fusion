using System.Collections.Generic;
using UnityEngine;

public class WizardFrozen: MonoBehaviour
{
    private Dictionary<MonsterBase, ActionResponse> currentTurnActionResponse;
    public void BustFrozen()
    {
        foreach (KeyValuePair<MonsterBase, ActionResponse> monster in currentTurnActionResponse)
        {
            monster.Key.StartHit(monster.Value.dam, monster.Value.effect);
        }
    }

    public void SetUpFrozen(Dictionary<MonsterBase, ActionResponse> turnActionResponse)
    {
        currentTurnActionResponse = new Dictionary<MonsterBase, ActionResponse>();
        currentTurnActionResponse = turnActionResponse;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}