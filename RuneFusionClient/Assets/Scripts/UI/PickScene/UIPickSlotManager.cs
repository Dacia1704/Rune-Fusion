using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickSlotManager: MonoBehaviour
{
        public List<UIPickSlot> Pick1List;
        public List<UIPickSlot> Pick2List;
        private Button ConfirmButton;

        private void Awake()
        {
                ConfirmButton = GetComponentInChildren<Button>();
        }

        private void Start()
        {
                ConfirmButton.onClick.AddListener(() =>
                {
                        ConfirmPick();
                        SocketManager.Instance.PostConfirmTurnPick();
                });
                PickSceneUIManager.Instance.MonsterSlotManager.OnPickChange += PickChange;
        }

        private void Update()
        {
                if (PickSceneUIManager.Instance.CountMonstersPick == PickSceneUIManager.Instance.AmountMonstersPick)
                {
                        ConfirmButton.interactable = true;
                }
                else
                {
                        ConfirmButton.interactable = false;
                }
        }

        public void ConfirmPick()
        {
                foreach (UIPickSlot slot in Pick1List)
                {
                        slot.SetIdle();
                }
                foreach (UIPickSlot slot in Pick2List)
                {
                        slot.SetIdle();
                }
                PickSceneUIManager.Instance.OnConfirmPick?.Invoke();
        }

        private void PickChange(List<MonsterPropsSO> pickList)
        {
                List<UIPickSlot> pickingList = new List<UIPickSlot>();
                foreach (UIPickSlot slot in Pick1List)
                {
                        if(slot.IsPicking) pickingList.Add(slot);
                }
                foreach (UIPickSlot slot in Pick2List)
                {
                        if(slot.IsPicking) pickingList.Add(slot);
                }
                for (int i = 0; i < pickingList.Count; i++)
                {
                        if (pickList[i] != null)
                        {
                                pickingList[i].EnableIcon(pickList[i].Icon);
                        }
                        else
                        {
                                pickingList[i].DisableIcon();
                        }
                }
                
        }

        public void PickTurn1()
        {
                PickSceneUIManager.Instance.AmountMonstersPick = 1;
                PickSceneUIManager.Instance.CountMonstersPick = 0;
                Pick1List[0].SetPicking();
                PickSceneUIManager.Instance.OnStartPickTurn.Invoke(0,1);
        }

        public void PickTurn2()
        {
                PickSceneUIManager.Instance.AmountMonstersPick = 2;
                PickSceneUIManager.Instance.CountMonstersPick = 0;
                Pick2List[0].SetPicking();
                Pick2List[1].SetPicking();
                PickSceneUIManager.Instance.OnStartPickTurn.Invoke(1,2);
        }

        public void PickTurn3()
        {
                PickSceneUIManager.Instance.AmountMonstersPick = 2;
                PickSceneUIManager.Instance.CountMonstersPick = 0;
                Pick1List[1].SetPicking();
                Pick1List[2].SetPicking();
                PickSceneUIManager.Instance.OnStartPickTurn.Invoke(0,2);
        }

        public void PickTurn4()
        {
                PickSceneUIManager.Instance.AmountMonstersPick = 1;
                PickSceneUIManager.Instance.CountMonstersPick = 0;
                Pick2List[2].SetPicking();
                PickSceneUIManager.Instance.OnStartPickTurn.Invoke(1,1);
        }
        


}