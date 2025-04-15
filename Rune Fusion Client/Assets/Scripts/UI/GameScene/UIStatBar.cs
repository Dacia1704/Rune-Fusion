using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBar: UIBase
{
        public int MaxValue { get; private set; }
        public Slider Slider { get; private set; }
        [field: SerializeField] public Image FillImage { get; private set; }

        private void Awake()
        {
                Slider = GetComponent<Slider>();
        }

        public void SetMaxValue(int value)
        {
                MaxValue = value;
                Slider.maxValue = value;
        }

        public void SetValue(int value, bool IsHpBar = false, MonsterBase monsterBase = null)
        {
                if (value < 200)
                {
                        Debug.Log("Update skill bar2: " + value);
                }

                this.DOKill();
                Slider.DOValue(value, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                        if (IsHpBar && monsterBase && value ==0)
                        {
                                monsterBase.IsAlive = false;
                        }       
                        Slider.value = value;
                });
        }




}