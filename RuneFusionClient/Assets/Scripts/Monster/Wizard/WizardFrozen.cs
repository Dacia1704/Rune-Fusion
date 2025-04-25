using System.Collections.Generic;
using UnityEngine;

public class WizardFrozen: FlyProjectile
{
    private Dictionary<MonsterBase, ActionResponse> currentTurnActionResponse;
    public void BustFrozen()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.FrozenBustSound;
        audioSource.Play();
        foreach (KeyValuePair<MonsterBase, ActionResponse> monster in currentTurnActionResponse)
        {
            monster.Key.StartHit(monster.Value.dam, monster.Value.effect);
        }
    }

    public void SetUpFrozen(Dictionary<MonsterBase, ActionResponse> turnActionResponse)
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.FrozenCreateSound;
        audioSource.Play();
        currentTurnActionResponse = new Dictionary<MonsterBase, ActionResponse>();
        currentTurnActionResponse = turnActionResponse;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}