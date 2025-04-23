using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMonsterListSlot: UIBase, IPointerClickHandler
{
        private UIMonsterListManager monsterListSlotManager;
        public MonsterPropsSO MonsterPropsSO {get; private set;}
        [SerializeField] private Image monsterIcon;
        [SerializeField] private GameObject blurLayer;
        private bool isHasMonster;

        public void SetUp(MonsterPropsSO data,UIMonsterListManager slotManager,bool isOwn)
        {
                isHasMonster = isOwn;
                MonsterPropsSO = data;
                monsterIcon.sprite = data.Icon;
                monsterListSlotManager = slotManager;
                if(!isHasMonster){
                        blurLayer.SetActive(true);
                }
                else
                {
                        blurLayer.SetActive(false);
                }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
                if (!isHasMonster) return;
                UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance.UIDetailMonster);
                UIMainMenuManager.Instance.UIDetailMonster.SetUp(MonsterPropsSO);
        }
}