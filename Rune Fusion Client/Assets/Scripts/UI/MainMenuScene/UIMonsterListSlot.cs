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
        public bool IsHasMonster { get; private set; }

        public void SetUp(MonsterPropsSO data,UIMonsterListManager slotManager,bool hasMonster = true)
        {
                MonsterPropsSO = data;
                monsterIcon.sprite = data.Icon;
                monsterListSlotManager = slotManager;
                if (hasMonster)
                {
                        IsHasMonster = true;
                        blurLayer.SetActive(false);
                }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
                if (!IsHasMonster) return;
                
        }
}