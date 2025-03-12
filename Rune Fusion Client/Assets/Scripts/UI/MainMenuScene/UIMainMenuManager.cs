using System;
using DG.Tweening;
using UnityEngine;

public class UIMainMenuManager: MonoBehaviour
{
        public static UIMainMenuManager Instance { get; private set; }
        public UILoginScreen UILoginScreen { get; private set; }
        public UIRegisterScreen UIRegisterScreen { get; private set; }
        private Vector3 UIRegisterScreenPosition;

        private void Awake()
        {
                Instance = this;
                UILoginScreen = GetComponentInChildren<UILoginScreen>();
                UIRegisterScreen = GetComponentInChildren<UIRegisterScreen>();
                UIRegisterScreenPosition = UIRegisterScreen.transform.position;
        }

        public void ChangeToLoginScreen()
        {
                DOTween.KillAll();
                UILoginScreen.Show();
                UILoginScreen.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).SetEase(Ease.InOutCubic);
                UIRegisterScreen.transform.DOMove(UIRegisterScreenPosition, 0.5f).SetEase(Ease.InOutCubic)
                        .onComplete += () => { UIRegisterScreen.Hide(); };
        }

        public void ChangeToRegisterScreen()
        {
                DOTween.KillAll();
                UIRegisterScreen.Show();
                UILoginScreen.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).SetEase(Ease.InOutCubic).onComplete += () => { UILoginScreen.Hide(); };
                UIRegisterScreen.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
        }
}