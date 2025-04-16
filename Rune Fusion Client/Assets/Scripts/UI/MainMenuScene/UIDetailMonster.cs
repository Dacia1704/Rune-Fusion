using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIDetailMonster: UIBase
{
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private UIStatMonster atkStat;
        [SerializeField] private UIStatMonster defStat;
        [SerializeField] private UIStatMonster hpStat;
        [SerializeField] private UIStatMonster spdStat;
        [SerializeField] private UIStatMonster accStat;
        [SerializeField] private UIStatMonster resStat;
        [SerializeField] private UISkillMonster basicSkill;
        [SerializeField] private UISkillMonster ultimateSkill;
        
        
        public UIEffectDescription EffectDescription { get; private set; }
        private Animator monsterModelAnimator;

        public Action<StatType> OnClickMinus;
        public Action<StatType> OnClickPlus;

        private void Awake()
        {
                monsterModelAnimator = GetComponentInChildren<Animator>();
                EffectDescription = GetComponentInChildren<UIEffectDescription>();
                EffectDescription.Hide();
                atkStat.SetStatType(StatType.Attack);
                defStat.SetStatType(StatType.Defend);
                hpStat.SetStatType(StatType.Health);
                spdStat.SetStatType(StatType.Speed);
                accStat.SetStatType(StatType.Accuracy);
                resStat.SetStatType(StatType.Resistance);
        }

        public void SetUp(MonsterPropsSO monsterPropsSO)
        {
                nameText.text = monsterPropsSO.MonsterData.Name;
                atkStat.SetStatText(monsterPropsSO.MonsterData.BaseStats.Attack.ToString(), monsterPropsSO.MonsterData.TalentPoint.Attack.ToString());
                defStat.SetStatText(monsterPropsSO.MonsterData.BaseStats.Defend.ToString(), monsterPropsSO.MonsterData.TalentPoint.Defend.ToString());
                hpStat.SetStatText(monsterPropsSO.MonsterData.BaseStats.Health.ToString(), monsterPropsSO.MonsterData.TalentPoint.Health.ToString());
                spdStat.SetStatText(monsterPropsSO.MonsterData.BaseStats.Speed.ToString(), monsterPropsSO.MonsterData.TalentPoint.Speed.ToString());
                accStat.SetStatText((int)(monsterPropsSO.MonsterData.BaseStats.Accuracy * 100) + "%", monsterPropsSO.MonsterData.TalentPoint.Accuracy.ToString());
                resStat.SetStatText((int)(monsterPropsSO.MonsterData.BaseStats.Resistance * 100) + "%", monsterPropsSO.MonsterData.TalentPoint.Resistance.ToString());
                basicSkill.SetSkillText(monsterPropsSO.MonsterData.Skills[0].Name,monsterPropsSO.MonsterData.Skills[0].Description);
                ultimateSkill.SetSkillText(monsterPropsSO.MonsterData.Skills[1].Name,monsterPropsSO.MonsterData.Skills[1].Description);
        }
}