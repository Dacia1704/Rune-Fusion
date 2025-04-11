using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBar: UIBase
{
        public Slider Slider { get; private set; }
        [field: SerializeField] public Image FillImage { get; private set; }

        private void Awake()
        {
                Slider = GetComponent<Slider>();
        }

        public void SetMaxValue(int value)
        {
                Slider.maxValue = value;
        }

        public void SetValue(int value)
        {
                if(value > Slider.maxValue) value = (int)Slider.maxValue;
                this.DOKill();
                Slider.DOValue(value, 0.1f).SetEase(Ease.Linear).OnComplete(() => Slider.value = value);
        }
        
        
        
        
}