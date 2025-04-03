using UnityEngine;

[CreateAssetMenu(fileName = "ArcherPropsSO", menuName = "ArcherPropsSO", order = 0)]
public class ArcherPropsSO: MonsterPropsSO
{
        [field: SerializeField] public GameObject ArrowPrefab { get; private set; }
}