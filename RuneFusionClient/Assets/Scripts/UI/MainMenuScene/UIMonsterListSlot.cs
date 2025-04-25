using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMonsterListSlot: UIBase, IPointerClickHandler
{
        private AudioSource audioSource;
        private UIMonsterListManager monsterListSlotManager;
        public MonsterPropsSO MonsterPropsSO {get; private set;}
        [SerializeField] private Image monsterIcon;
        [SerializeField] private GameObject blurLayer;
        private bool isHasMonster;
        private void Awake()
        {
                audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
                audioSource.clip = AudioManager.Instance.AudioPropsSO.ButtonClickSound;
                audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
                audioSource.playOnAwake = false;
        }

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
                audioSource.Play();
                if (!isHasMonster) return;
                UIMainMenuManager.Instance.ChangeToNewScreen(UIMainMenuManager.Instance.UIDetailMonster);
                UIMainMenuManager.Instance.UIDetailMonster.SetUp(MonsterPropsSO);
        }
}