using UnityEngine;

[CreateAssetMenu(fileName = "MonsterPropsSO", menuName = "MonsterPropsSO", order = 0)]
public class MonsterPropsSO : ScriptableObject
{
        [field: SerializeField] public MonsterName Name { get; private set; }
        [field: SerializeField] public MonsterStats BaseStats { get; private set; }
        [field: SerializeField] public MonsterType Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
}