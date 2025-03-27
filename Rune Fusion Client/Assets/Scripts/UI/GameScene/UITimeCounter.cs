using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITimeCounter: UIBase
{
        private Slider slider;
        
        private float counter;
        private float timeTurn;

        public event Action OnTimeCounterEnd;
        
        private CanvasGroup canvasGroup;
        private void Awake()
        {
                canvasGroup = GetComponent<CanvasGroup>();
                slider = GetComponent<Slider>();
                slider.value = 1;
                Hide();
        }

        private void Update()
        {
                if (counter >= 0)
                {
                        counter -= Time.deltaTime;
                }
                if (slider.value > 0)
                {
                        slider.value = counter / timeTurn;
                }
                else
                {
                        OnTimeCounterEnd?.Invoke();
                        Hide();
                }
        }
        public void SetCountTime(float time)
        {
                Show();
                slider.value = 1;
                counter = time;
                timeTurn = time;
        }

        public override void Hide()
        {
                canvasGroup.alpha = 0;
        }

        public override void Show()
        {
                canvasGroup.alpha = 1;
        }
}