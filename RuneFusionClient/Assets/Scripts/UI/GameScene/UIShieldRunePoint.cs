using UnityEngine;
using UnityEngine.UI;

public class UIShieldRunePoint: UIBase
{
        [SerializeField] private Image fillImage;

        public void SetFillImage(float fillAmount)
        {
                this.fillImage.fillAmount = fillAmount;
        }
}