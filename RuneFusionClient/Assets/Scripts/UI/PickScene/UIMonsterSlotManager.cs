using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class UIMonsterSlotManager: MonoBehaviour
{
        [SerializeField] private GameObject UIMonsterSlotPrefab;
        [SerializeField] private List<MonsterPropsSO> monsterDataList;
        [field: SerializeField] public MonsterListSO MonsterListSO {get; private set;}
        public List<MonsterPropsSO> PickList {get; private set;}

        public Action<List<MonsterPropsSO>> OnPickChange;
        private Dictionary<int,UIMonsterSlot> monsterSlotsDictionary;
        private void Start()
        {
                monsterSlotsDictionary = new Dictionary<int, UIMonsterSlot>();
                GenMonsterList();
                PickSceneUIManager.Instance.OnStartPickTurn += StartPickTurn;
                PickList = new List<MonsterPropsSO>();
        }
        private void GenMonsterList()
        {
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        if (MonsterListSO.MonsterDictionary[(int)monsterData.MonsterData.Id].IsOwn)
                        {
                                UIMonsterSlot monsterSlot = Instantiate(UIMonsterSlotPrefab, transform).GetComponent<UIMonsterSlot>();
                                monsterSlot.SetUp(monsterData,this);
                                monsterSlotsDictionary.Add((int)monsterData.MonsterData.Id,monsterSlot);
                        }
                }
        }

        public void UpdateMonsterPick(MonsterPickPush data)
        {
                PickList.Clear();
                foreach (int id in data.player1)
                {
                        PickList.Add(monsterDataList.Find((mon) => (int)(mon.MonsterData.Id) == id));
                }
                foreach (int id in data.player2)
                {
                        PickList.Add(monsterDataList.Find((mon) => (int)(mon.MonsterData.Id) == id));
                }
                Debug.Log(JsonConvert.SerializeObject(data));
                UpdatePickedMonster(data.picked_monsters);
                OnPickChange?.Invoke(PickList);
        }

        public void StartPickTurn(int id, int quantity)
        {
                PickList.Clear();
                for (int i = 0; i < quantity; i++)
                {
                        PickList.Add(null);
                }
        }

        public void PickMonster(MonsterPropsSO monsterPropsSO)
        {
                int indexNull = PickList.FindIndex(item => item == null);
                if (indexNull != -1)
                {
                        PickList[indexNull] = monsterPropsSO;
                        if (PickSceneUIManager.Instance.PlayerIndex == 0)
                        {
                                SocketManager.Instance.PostMonsterPickData(PickSceneUIManager.Instance.MonsterSlotManager.PickList, new List<MonsterPropsSO>());
                                PickSceneUIManager.Instance.CountMonstersPick += 1;
                        }
                        else
                        {
                                SocketManager.Instance.PostMonsterPickData(new List<MonsterPropsSO>(),PickSceneUIManager.Instance.MonsterSlotManager.PickList);
                                PickSceneUIManager.Instance.CountMonstersPick += 1;
                        }
                }
                OnPickChange?.Invoke(PickList);
        }

        public void RemoveMonster(MonsterPropsSO monsterPropsSO)
        {
                int indexNull = PickList.FindIndex(item => item.MonsterData.Id == monsterPropsSO.MonsterData.Id);
                if (indexNull != -1)
                {
                        PickList[indexNull] = null;
                        if (PickSceneUIManager.Instance.PlayerIndex == 0)
                        {
                                SocketManager.Instance.PostMonsterPickData(PickSceneUIManager.Instance.MonsterSlotManager.PickList, new List<MonsterPropsSO>());
                                PickSceneUIManager.Instance.CountMonstersPick -= 1;
                        }
                        else
                        {
                                SocketManager.Instance.PostMonsterPickData(new List<MonsterPropsSO>(),PickSceneUIManager.Instance.MonsterSlotManager.PickList);
                                PickSceneUIManager.Instance.CountMonstersPick -= 1;
                        }
                }
                OnPickChange?.Invoke(PickList);
        }

        public bool CanPickMonster()
        {
                int indexNull = PickList.FindIndex(item => item == null);
                if (indexNull != -1)
                {
                        return true;
                }

                return false;
        }

        private void UpdatePickedMonster(List<int> monsterIds)
        {
                foreach (UIMonsterSlot monsterSlot in monsterSlotsDictionary.Values)
                {
                        monsterSlot.DisableCheck();
                }
                foreach (int id in monsterIds)
                {
                        if (monsterSlotsDictionary.ContainsKey(id))
                        {
                                monsterSlotsDictionary[id].EnableCheck();
                        }
                }
        }

        public void LockPickedMonster()
        {
                foreach (UIMonsterSlot monsterSlot in monsterSlotsDictionary.Values)
                {
                        if (monsterSlot.IsCheck)
                        {
                                monsterSlot.EnableLock();
                        }
                }
        }
}