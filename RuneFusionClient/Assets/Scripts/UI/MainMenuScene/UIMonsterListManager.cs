using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterListManager: UIBase
{
        public static UIMonsterListManager Instance { get;private set; }
        [SerializeField] private GameObject UIMonsterListSlotPrefab;
        [SerializeField] private List<MonsterPropsSO> monsterDataList;
        public Dictionary<int,UIMonsterListSlot> MonsterSlotsDictionary;

        private void Awake()
        {
                Instance = this;
        }

        private void Start()
        {
                MonsterSlotsDictionary = new Dictionary<int, UIMonsterListSlot>();
                GenMonsterList();
        }
        private void GenMonsterList()
        {
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        UIMonsterListSlot monsterSlot = Instantiate(UIMonsterListSlotPrefab, transform).GetComponent<UIMonsterListSlot>();
                        monsterSlot.SetUp(monsterData,this);
                        MonsterSlotsDictionary.Add((int)monsterData.MonsterData.Id,monsterSlot);
                }
        }
}