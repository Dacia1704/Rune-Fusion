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
        
        public int TotalUsedTalentPoints { get; private set; }
        private MonsterPropsSO monsterProps;

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

                OnClickMinus += HandleMinusTalentPointAction;
                OnClickPlus += HandlePlusTalentPointAction;
        }
        

        public void SetUp(MonsterPropsSO monsterPropsSO)
        {
                monsterProps = monsterPropsSO;
                TotalUsedTalentPoints = 0;
                nameText.text = monsterPropsSO.MonsterData.Name;
                monsterModelAnimator.runtimeAnimatorController = monsterPropsSO.ModelAnimatorController;
                if (monsterPropsSO == null)
                {
                        Debug.Log("null");
                }
                Debug.Log(monsterPropsSO.MonsterData.BaseStats.Attack);
                Debug.Log(monsterPropsSO.MonsterData.TalentPoint.Attack);
                atkStat.SetStatText(((int)Mathf.Ceil(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Attack, monsterPropsSO.MonsterData.TalentPoint.Attack))).ToString(), monsterPropsSO.MonsterData.TalentPoint.Attack.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Attack;
                defStat.SetStatText(((int)Mathf.Ceil(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Defend, monsterPropsSO.MonsterData.TalentPoint.Defend))).ToString(), monsterPropsSO.MonsterData.TalentPoint.Defend.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Defend;
                hpStat.SetStatText(((int)Mathf.Ceil(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Health, monsterPropsSO.MonsterData.TalentPoint.Health))).ToString(), monsterPropsSO.MonsterData.TalentPoint.Health.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Health;
                spdStat.SetStatText(((int)Mathf.Ceil(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Speed, monsterPropsSO.MonsterData.TalentPoint.Speed))).ToString(), monsterPropsSO.MonsterData.TalentPoint.Speed.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Speed;
                accStat.SetStatText((int)(monsterPropsSO.MonsterData.BaseStats.Accuracy * 100) + "%", monsterPropsSO.MonsterData.TalentPoint.Accuracy.ToString());
                accStat.SetStatText((int)(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Accuracy, monsterPropsSO.MonsterData.TalentPoint.Accuracy) * 100) + "%", monsterPropsSO.MonsterData.TalentPoint.Accuracy.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Accuracy;
                resStat.SetStatText((int)(CalculateStatWithTalentPoints(monsterPropsSO.MonsterData.BaseStats.Resistance, monsterPropsSO.MonsterData.TalentPoint.Resistance) * 100) + "%", monsterPropsSO.MonsterData.TalentPoint.Resistance.ToString());
                TotalUsedTalentPoints += monsterPropsSO.MonsterData.TalentPoint.Resistance;
                basicSkill.SetSkillText(monsterPropsSO.MonsterData.Skills[0].Name,monsterPropsSO.MonsterData.Skills[0].Description);
                ultimateSkill.SetSkillText(monsterPropsSO.MonsterData.Skills[1].Name,monsterPropsSO.MonsterData.Skills[1].Description);
        }

        private float CalculateStatWithTalentPoints(float statValue,int talentPoints)
        {
                float addStat = 0;
                for (int i = 1; i <= talentPoints; i++)
                {
                        if (i <= 10)
                        {
                                addStat += (statValue * 0.03f);
                        }
                        else
                        {
                                int modifier = (int)Mathf.Ceil((i - 10f) / 5f);
                                addStat += (statValue * (0.03f/Mathf.Pow(2, modifier)));
                        }
                }
                return statValue + addStat;
        }

        public void HandleMinusTalentPointAction(StatType statType)
        {
                MonsterTalentPointRequestUpdateData monsterTalentPointRequestUpdateData = new MonsterTalentPointRequestUpdateData
                {
                        id_player = SocketManager.Instance.PlayerData.id,
                        id_monster = (int)monsterProps.MonsterData.Id,
                        talent_point = monsterProps.MonsterData.TalentPoint,
                };
                if (statType == StatType.Attack && monsterProps.MonsterData.TalentPoint.Attack > 0)
                { 
                        monsterTalentPointRequestUpdateData.talent_point.Attack -= 1;
                }else if (statType == StatType.Defend && monsterProps.MonsterData.TalentPoint.Defend > 0)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Defend -= 1;
                }else if (statType == StatType.Health && monsterProps.MonsterData.TalentPoint.Health > 0)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Health -= 1;
                }else if (statType == StatType.Speed && monsterProps.MonsterData.TalentPoint.Speed > 0)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Speed -= 1;
                }else if (statType == StatType.Accuracy && monsterProps.MonsterData.TalentPoint.Accuracy > 0)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Accuracy -= 1;
                }else if (statType == StatType.Resistance && monsterProps.MonsterData.TalentPoint.Resistance > 0)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Resistance -= 1;
                }
                SocketManager.Instance.RequestUpdateTalentPoint(monsterTalentPointRequestUpdateData); 
        }

        public void HandlePlusTalentPointAction(StatType statType)
        {
                MonsterTalentPointRequestUpdateData monsterTalentPointRequestUpdateData = new MonsterTalentPointRequestUpdateData
                {
                        id_player = SocketManager.Instance.PlayerData.id,
                        id_monster = (int)monsterProps.MonsterData.Id,
                        talent_point = monsterProps.MonsterData.TalentPoint,
                };
                if (statType == StatType.Attack && TotalUsedTalentPoints <30)
                { 
                        monsterTalentPointRequestUpdateData.talent_point.Attack += 1;
                }else if (statType == StatType.Defend && TotalUsedTalentPoints <30)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Defend += 1;
                }else if (statType == StatType.Health && TotalUsedTalentPoints <30)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Health += 1;
                }else if (statType == StatType.Speed && TotalUsedTalentPoints <30)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Speed += 1;
                }else if (statType == StatType.Accuracy && TotalUsedTalentPoints <30)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Accuracy += 1;
                }else if (statType == StatType.Resistance && TotalUsedTalentPoints <30)
                {
                        monsterTalentPointRequestUpdateData.talent_point.Resistance += 1;
                }
                SocketManager.Instance.RequestUpdateTalentPoint(monsterTalentPointRequestUpdateData); 
        }
}