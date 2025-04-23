using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class UIMonsterListManager: UIBase
{
        public static UIMonsterListManager Instance { get; private set; }
        [SerializeField] private GameObject UIMonsterListSlotPrefab;
        [SerializeField] private List<MonsterPropsSO> monsterDataList;
        private Dictionary<int,UIMonsterListSlot> monsterSlotsDictionary =  new Dictionary<int, UIMonsterListSlot>();

        private int countCall;
        public Action<MonstersOwnResponseData> OnMonstersOwnDataResponse;

        private void Awake()
        {
                Instance = this;
                countCall = 0;
        }

        private void OnEnable()
        {
                if (countCall<=2)
                {
                        countCall++;
                }
                else
                {
                        Debug.Log("RequestMonsterOwnData");
                        SocketManager.Instance.RequestMonsterOwnData();
                }

                OnMonstersOwnDataResponse += GenMonsterList;
        }

        private void GenMonsterList(MonstersOwnResponseData owndata)
        {
                foreach (Transform child in transform)
                {
                        Destroy(child.gameObject);
                }
                monsterSlotsDictionary.Clear();
                foreach (var own in owndata.own_monster_list)
                {
                        MonsterSourceData mon = UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[own.id];
                        mon.MonsterProps.MonsterData.TalentPoint = own.talent_point;
                        mon.SetOwn(true);
                }
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        if (UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[(int)monsterData.MonsterData.Id].IsOwn)
                        {
                                UIMonsterListSlot monsterSlot = Instantiate(UIMonsterListSlotPrefab, transform).GetComponent<UIMonsterListSlot>();
                                monsterSlot.SetUp(monsterData,this,true);
                                monsterSlotsDictionary.Add((int)monsterData.MonsterData.Id,monsterSlot);
                        }
                }
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        if (!UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[(int)monsterData.MonsterData.Id].IsOwn)
                        {
                                UIMonsterListSlot monsterSlot = Instantiate(UIMonsterListSlotPrefab, transform).GetComponent<UIMonsterListSlot>();
                                monsterSlot.SetUp(monsterData,this,false);
                                monsterSlotsDictionary.Add((int)monsterData.MonsterData.Id,monsterSlot);
                        }
                }
        }
}