using UnityEngine;

[CreateAssetMenu(fileName = "PriestPropsSO", menuName = "PriestPropsSO", order = 0)]
public class PriestPropsSO: MonsterPropsSO
{
    [field: SerializeField] public GameObject PriestMagicPrefab { get; private set; }
    [field: SerializeField] public Vector3 PriestMagicSummonOffset { get; private set; }
}