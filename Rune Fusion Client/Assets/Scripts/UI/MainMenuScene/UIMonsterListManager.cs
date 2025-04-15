using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterListManager: UIBase
{
        [SerializeField] private GameObject UIMonsterListSlotPrefab;
        [SerializeField] private List<MonsterPropsSO> monsterDataList;
        private Dictionary<int,UIMonsterListSlot> monsterSlotsDictionary;
        private void Start()
        {
                monsterSlotsDictionary = new Dictionary<int, UIMonsterListSlot>();
                GenMonsterList();
        }
        private void GenMonsterList()
        {
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        UIMonsterListSlot monsterSlot = Instantiate(UIMonsterListSlotPrefab, transform).GetComponent<UIMonsterListSlot>();
                        monsterSlot.SetUp(monsterData,this);
                        monsterSlotsDictionary.Add((int)monsterData.MonsterData.Id,monsterSlot);
                }
        }
}