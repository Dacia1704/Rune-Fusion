using UnityEngine;
using UnityEngine.UI;

public class UIHeathSkillBarManager : MonoBehaviour
{
        [SerializeField] private UIStatBar healthBar;
        [SerializeField] private UIStatBar skillBar;

        public void SetMaxHealthBar(int value)
        {
                healthBar.SetMaxValue(value);
        }
        public void SetMaxSkillBar(int value)
        {
                skillBar.SetMaxValue(value);
        }
        
        public void SetHealthBar(int value)
        {
                healthBar.SetValue(value);
        }
        public void SetSkillBar(int value)
        {
                skillBar.SetValue(value);
                if (Mathf.Approximately(skillBar.Slider.maxValue, value))
                {
                        skillBar.FillImage.color = Color.yellow;
                }
                else
                {
                        skillBar.FillImage.color = Color.white;
                }
        }
        
}