using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMonsterSlot: UIBase,IPointerClickHandler
{
        private UIMonsterSlotManager monsterSlotManager;
        public MonsterPropsSO MonsterPropsSO {get; private set;}
        [SerializeField] private Image MonsterIcon;
        [SerializeField] private CanvasGroup CheckObject;

        private bool IsCheck;

        public void SetUp(MonsterPropsSO data,UIMonsterSlotManager slotManager)
        {
                MonsterPropsSO = data;
                MonsterIcon.sprite = data.Icon;
                monsterSlotManager = slotManager;
                DisableCheck();
        }

        public void EnableCheck()
        {
                CheckObject.alpha = 1;
                IsCheck = true;
        }

        public void DisableCheck()
        {
                CheckObject.alpha = 0;
                IsCheck = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
                if (PickSceneUIManager.Instance.PlayerIndex != PickSceneUIManager.Instance.PlayerIdTurn) return;
                if (IsCheck)
                {
                        DisableCheck();
                        monsterSlotManager.RemoveMonster(MonsterPropsSO);
                        Debug.Log("Disable pick");
                }
                else
                {
                        if (monsterSlotManager.CanPickMonster())
                        {
                                EnableCheck();
                                monsterSlotManager.PickMonster(MonsterPropsSO);
                                Debug.Log("Enable pick");
                        }
                }
        }
}