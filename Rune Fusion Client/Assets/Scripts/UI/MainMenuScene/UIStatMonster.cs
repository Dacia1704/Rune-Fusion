using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatMonster: UIBase
{
        [SerializeField] private TextMeshProUGUI statText;
        [SerializeField] private TextMeshProUGUI talentPointText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;
        
        private UIDetailMonster monsterDetail;
        public StatType StatType { get; private set; }

        public void SetStatType(StatType statType)
        {
                StatType = statType;
        }

        private void Start()
        {
                monsterDetail = GetComponentInParent<UIDetailMonster>();
                minusButton.onClick.AddListener(() =>
                {
                        monsterDetail.OnClickMinus.Invoke(StatType);
                });
                plusButton.onClick.AddListener(() =>
                {
                        monsterDetail.OnClickPlus.Invoke(StatType);
                });
        }

        public void SetStatText(string stat, string talentPoint)
        {
                statText.text = stat;
                talentPointText.text = talentPoint;
        }

        // public float CalculateStatValueWithTalentPoint()
        // {
        //         
        // }
}