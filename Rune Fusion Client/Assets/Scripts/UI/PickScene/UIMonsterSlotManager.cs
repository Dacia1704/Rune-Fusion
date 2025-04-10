using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterSlotManager: MonoBehaviour
{
        [SerializeField] private GameObject UIMonsterSlotPrefab;
        [SerializeField] private List<MonsterPropsSO> monsterDataList;
        public List<MonsterPropsSO> PickList {get; private set;}

        public Action<List<MonsterPropsSO>> OnPickChange;
        private void Start()
        {
                GenMonsterList();
                PickSceneUIManager.Instance.OnStartPickTurn += StartPickTurn;
                PickList = new List<MonsterPropsSO>();
        }
        private void GenMonsterList()
        {
                foreach (MonsterPropsSO monsterData in monsterDataList)
                {
                        UIMonsterSlot monsterSlot = Instantiate(UIMonsterSlotPrefab, transform).GetComponent<UIMonsterSlot>();
                        monsterSlot.SetUp(monsterData,this);
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
                        if (PickSceneUIManager.Instance.PLayerIndex == 0)
                        {
                                SocketManager.Instance.PostMonsterPickData(PickSceneUIManager.Instance.MonsterSlotManager.PickList, new List<MonsterPropsSO>());
                        }
                        else
                        {
                                SocketManager.Instance.PostMonsterPickData(new List<MonsterPropsSO>(),PickSceneUIManager.Instance.MonsterSlotManager.PickList);
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
                        if (PickSceneUIManager.Instance.PLayerIndex == 0)
                        {
                                SocketManager.Instance.PostMonsterPickData(PickSceneUIManager.Instance.MonsterSlotManager.PickList, new List<MonsterPropsSO>());
                        }
                        else
                        {
                                SocketManager.Instance.PostMonsterPickData(new List<MonsterPropsSO>(),PickSceneUIManager.Instance.MonsterSlotManager.PickList);
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

        private void EnableInput()
        {
                
        }

        private void DisableInput()
        {
                
        }
}