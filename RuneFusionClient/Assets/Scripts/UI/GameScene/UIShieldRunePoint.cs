using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIShieldRunePoint: UIBase
{
        [SerializeField] private Image fillImage;
        private Button button;
        private float prevFillAmount;
        public event Action OnClickShieldButton;
        private void Start()
        {
                button = GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                        OnClickShieldButton?.Invoke();
                        ChangeUseShieldPointApperance();
                });
        }

        public void SetFillImage(float fillAmount)
        {
                prevFillAmount = fillAmount;
                this.fillImage.fillAmount = fillAmount;
        }

        public void ChangeUseShieldPointApperance()
        {
                fillImage.DOFillAmount(0f, 0.5f).SetEase(Ease.OutQuad);
        }

        public void ChangeNotUseShieldPointApperance()
        {
                fillImage.DOFillAmount(prevFillAmount, 0.5f).SetEase(Ease.OutQuad);
        }
}