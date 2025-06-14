﻿using System;
using DG.Tweening;
using UnityEngine;

public class UIMainMenuManager: MonoBehaviour
{
        public static UIMainMenuManager Instance { get; private set; }
        public UILoginScreen UILoginScreen { get; private set; }
        public UIRegisterScreen UIRegisterScreen { get; private set; }
        public UITabsManager UITabManager { get; private set; }
        public UIDetailMonster UIDetailMonster { get; private set; }
        public UIPlayerResource UIPlayerResource { get; private set; }
        private UIBase currentUIScreen;
        private Vector3 UISubScreenPosition;
        
        [field: SerializeField] public MonsterListSO MonsterListSO {get; private set;}

        public Action<int,int> OnResourceChange;

        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;
                
                UILoginScreen = GetComponentInChildren<UILoginScreen>();
                UIRegisterScreen = GetComponentInChildren<UIRegisterScreen>();
                UITabManager = GetComponentInChildren<UITabsManager>();
                UIDetailMonster = GetComponentInChildren<UIDetailMonster>();
                UIPlayerResource = GetComponentInChildren<UIPlayerResource>();
                UISubScreenPosition = UIRegisterScreen.transform.position;
                UITabManager.transform.position = UISubScreenPosition;
                UIDetailMonster.transform.position = UISubScreenPosition;
                currentUIScreen = UILoginScreen;
                MonsterListSO.Initialize();
        }

        private void Start()
        {
                OnResourceChange += UIPlayerResource.SetResourceText;
                UIRegisterScreen.Hide();
                UITabManager.Hide();
                UIDetailMonster.Hide();
        }

        public void ChangeToLoginScreen()
        {
                UILoginScreen.Show();
                UILoginScreen.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).SetEase(Ease.InOutCubic);
                UIBase currentScreen = currentUIScreen;
                currentScreen.transform.DOMove(UISubScreenPosition, 0.5f).SetEase(Ease.InOutCubic)
                        .onComplete += () => { currentScreen.Hide(); };
                currentUIScreen = UILoginScreen;
        }

        public void ChangeToNewScreen(UIBase newUIScreen)
        {
                if (currentUIScreen == newUIScreen) return;
                newUIScreen.Show();
                UIBase currentScreen = currentUIScreen;
                UIBase newScreen = newUIScreen;
                newScreen.GetComponent<CanvasGroup>().DOFade(1f, 0.2f).SetEase(Ease.InOutCubic);
                Sequence swapSequence = DOTween.Sequence();
                if (currentScreen != UILoginScreen)
                {
                        swapSequence
                                .Append(currentScreen.GetComponent<CanvasGroup>().DOFade(0f, 0.2f).SetEase(Ease.InOutCubic))
                                .Append(currentScreen.transform.DOMove(UISubScreenPosition, 0f).SetEase(Ease.InOutCubic))
                                .onComplete += () => { currentScreen.Hide(); };
                }
                else
                {
                        swapSequence.Append(currentScreen.GetComponent<CanvasGroup>().DOFade(0f, 0.2f).SetEase(Ease.InOutCubic))
                                .onComplete += () => { currentScreen.Hide(); };
                }
                swapSequence.Append(newUIScreen.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic));
                currentUIScreen = newUIScreen;
                
                Debug.Log("change");
        }

        public void InitializeMonsterData(InitMonsterData initMonsterData)
        {
                foreach (var monster in initMonsterData.monsters)
                {
                        MonsterSourceData mon = MonsterListSO.MonsterDictionary[(int)monster.Id];
                        mon.MonsterProps.MonsterData = monster;
                        mon.SetOwn(false);
                }
                UIMonsterListManager.Instance.OnMonstersOwnDataResponse?.Invoke(new MonstersOwnResponseData()
                {
                        own_monster_list = initMonsterData.own_monster_list,
                });
        }

        public void UpdateTalentPoint(MonsterTalentPointRequestUpdateData talentPointData)
        {
                MonsterListSO.MonsterDictionary[(int)talentPointData.id_monster].MonsterProps.MonsterData.TalentPoint = talentPointData.talent_point;
                UIDetailMonster.SetUp(MonsterListSO.MonsterDictionary[(int)talentPointData.id_monster].MonsterProps);
        }
}