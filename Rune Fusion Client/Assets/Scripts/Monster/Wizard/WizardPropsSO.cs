using UnityEngine;

[CreateAssetMenu(fileName = "WizardPropsSO", menuName = "WizardPropsSO", order = 0)]
public class WizardPropsSO: MonsterPropsSO
{
    [field: SerializeField] public GameObject WizardFireBallPrefab { get; private set; }
    [field: SerializeField] public Vector3 WizardFireBallSummonOffset { get; private set; }
    [field: SerializeField] public GameObject WizardFrozenPrefab { get; private set; }
}