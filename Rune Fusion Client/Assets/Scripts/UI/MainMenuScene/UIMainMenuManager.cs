using System;
using DG.Tweening;
using UnityEngine;

public class UIMainMenuManager: MonoBehaviour
{
        public static UIMainMenuManager Instance { get; private set; }
        public UILoginScreen UILoginScreen { get; private set; }
        public UIRegisterScreen UIRegisterScreen { get; private set; }
        public UITabsManager UITabManager { get; private set; }
        private UIBase currentUIScreen;
        private Vector3 UISubScreenPosition;
        
        [field: SerializeField] public MonsterListSO MonsterListSO {get; private set;}

        private void Awake()
        {
                Instance = this;
                UILoginScreen = GetComponentInChildren<UILoginScreen>();
                UIRegisterScreen = GetComponentInChildren<UIRegisterScreen>();
                UITabManager = GetComponentInChildren<UITabsManager>();
                UISubScreenPosition = UIRegisterScreen.transform.position;
                UIRegisterScreen.Hide();
                UITabManager.Hide();
                currentUIScreen = UILoginScreen;
                
                MonsterListSO.Initialize();
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
                newUIScreen.Show();
                newUIScreen.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
                UIBase currentScreen = currentUIScreen;
                if (currentScreen != UILoginScreen)
                {
                        Sequence swapSequence = DOTween.Sequence();
                        swapSequence
                                .Join(currentScreen.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetEase(Ease.InOutCubic))
                                .Join(currentScreen.transform.DOMove(UISubScreenPosition, 0.1f).SetEase(Ease.InOutCubic))
                                .onComplete += () => { currentScreen.Hide(); };
                }
                else
                {
                        currentScreen.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetEase(Ease.InOutCubic).onComplete += () => { currentScreen.Hide(); };
                }
                currentUIScreen = newUIScreen;
        }

        public void InitializeMonsterData(InitMonsterData initMonsterData)
        {
                foreach (var monster in initMonsterData.monsters)
                {
                        MonsterSourceData mon = MonsterListSO.MonsterDictionary[(int)monster.Id];
                        mon.MonsterProps.MonsterData = monster;
                        mon.SetOwn(false);
                }

                foreach (var own in initMonsterData.own_monster_list)
                {
                        MonsterSourceData mon = MonsterListSO.MonsterDictionary[own];
                        mon.SetOwn(true);
                }
        }
}