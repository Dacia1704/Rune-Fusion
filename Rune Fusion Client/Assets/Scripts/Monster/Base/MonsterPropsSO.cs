using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterPropsSO", menuName = "MonsterPropsSO", order = 0)]
public class MonsterPropsSO : ScriptableObject
{
        [field: SerializeField] public MonsterData MonsterData { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public float AttackOffset { get; private set; }
}