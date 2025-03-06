using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneManagerSO", menuName = "RuneManagerSO", order = 0)]
public class RuneManagerSO : ScriptableObject
{
        [field: SerializeField] public GameObject PhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject MagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject HealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject DefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject ShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject HorizontalPhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalMagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalHealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalDefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject HorizontalShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject VerticalPhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalMagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalHealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalDefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject VerticalShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject SmallBombPhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject SmallBombMagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject SmallBombHealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject SmallBombDefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject SmallBombShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject IcePhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject IceMagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject IceHealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject IceDefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject IceShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject ToxicPhysicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject ToxicMagicAttackRunePrefab { get; private set; }
        [field: SerializeField] public GameObject ToxicHealthRunePrefab { get; private set; }
        [field: SerializeField] public GameObject ToxicDefendRunePrefab { get; private set; }
        [field: SerializeField] public GameObject ToxicShieldRunePrefab { get; private set; }
        
        [field: SerializeField] public GameObject BigBombRunePrefab { get; private set; }
        
        [field: SerializeField] public List<GameObject> SingleRuneList { get; private set; }
        
        private void OnEnable()
        {
                UpdateSingleRuneList();
        }

        private void UpdateSingleRuneList()
        {
                SingleRuneList.Clear();
                List<GameObject> runes = new List<GameObject>
                {
                        PhysicAttackRunePrefab, MagicAttackRunePrefab, HealthRunePrefab, DefendRunePrefab, ShieldRunePrefab
                };
                foreach (var rune in runes)
                {
                        if (rune != null)
                        {
                                SingleRuneList.Add(rune);
                        }
                }
        }
}