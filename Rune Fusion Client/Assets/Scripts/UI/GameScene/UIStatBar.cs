using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBar: UIBase
{
        private Slider slider;

        private void Awake()
        {
                slider = GetComponent<Slider>();
        }

        public void SetMaxValue(int value)
        {
                slider.maxValue = value;
        }

        public void SetValue(int value)
        {
                this.DOKill();
                slider.DOValue(value, 0.1f).SetEase(Ease.Linear).OnComplete(() => slider.value = value);
        }
        
        
        
        
}