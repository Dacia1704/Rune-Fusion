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
        
        public void SetHealthBar(int value,bool isHpBar = false, MonsterBase monsterBase = null)
        {
                healthBar.SetValue(value,isHpBar,monsterBase);
        }
        public void SetSkillBar(int value)
        {
                Debug.Log("Update skill bar1: "+ value + " " + skillBar.MaxValue);
                skillBar.SetValue(value);
                skillBar.FillImage.color = skillBar.MaxValue == value ? Color.yellow : Color.blue;
        }
        
}