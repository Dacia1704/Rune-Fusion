using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PickSceneUIManager: MonoBehaviour
{
        public static PickSceneUIManager Instance {get; private set;}
        public UIPickSlotManager PickSlotManager {get;private set;}
        public UIMonsterSlotManager MonsterSlotManager {get;private set;}
        public UIPlayerText PlayerText {get;private set;}

        public Action OnConfirmPick;
        public Action<int,int> OnStartPickTurn;
        
        public int PlayerIdTurn { get; private set; }

        public int PLayerIndex;

        private void Awake()
        {
                Instance = this;
                PickSlotManager = GetComponentInChildren<UIPickSlotManager>();
                MonsterSlotManager = GetComponentInChildren<UIMonsterSlotManager>();
                PlayerText = GetComponentInChildren<UIPlayerText>();
        }

        private void Start()
        {
                PLayerIndex = SocketManager.Instance.PlayerData.playerindex;
                if (PLayerIndex == 0)
                {
                        PlayerText.SetNameText(SocketManager.Instance.PlayerData.playername,SocketManager.Instance.OpponentData.playername );
                }
                else
                {
                        PlayerText.SetNameText(SocketManager.Instance.OpponentData.playername, SocketManager.Instance.PlayerData.playername); 
                        // PlayerText.SetNameText("P1", "P2"); 
                }
                StartCoroutine(PickCoroutine());
        }

        private IEnumerator PickCoroutine()
        {
                bool isConfirmed = false;
                void ConfirmEventHandle() => isConfirmed = true;
                OnConfirmPick += ConfirmEventHandle;

                PlayerIdTurn = 0;
                PickSlotManager.PickTurn1();
                
                yield return new WaitUntil(()=> isConfirmed);
                isConfirmed = false;

                PlayerIdTurn = 1;
                PickSlotManager.PickTurn2();
                
                yield return new WaitUntil(()=> isConfirmed);
                isConfirmed = false;
                
                PlayerIdTurn = 0;
                PickSlotManager.PickTurn3();
                
                yield return new WaitUntil(()=> isConfirmed);
                isConfirmed = false;
                
                PlayerIdTurn = 1;
                PickSlotManager.PickTurn4();
        }
}