using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISummonManager: UIBase
{
        public static UISummonManager Instance { get; private set; }
        [SerializeField] private Button SummonOnceButton;
        [SerializeField] private Button SummonTenTimesButton;
        [SerializeField] private GameObject BannerObject;
        [SerializeField] private GameObject BlockInputLayer;
        public UISummonEffectManager SummonEffectManager { get; private set; }
        public Action<SummonResponseData> OnSummonResponseData { get; private set; }
        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;

                SummonEffectManager = GetComponentInChildren<UISummonEffectManager>();
                SummonOnceButton.onClick.AddListener(() =>
                {
                        SocketManager.Instance.RequestSummonData(1);
                        SetSummonTenTimesButtonInteractable(false);
                        SetSummonOnceButtonInteractable(false);
                        BannerObject.GetComponent<CanvasGroup>().DOFade(0f, 1f);
                });
                SummonTenTimesButton.onClick.AddListener(() =>
                {
                        SocketManager.Instance.RequestSummonData(10);
                        SetSummonOnceButtonInteractable(false);
                        SetSummonTenTimesButtonInteractable(false);
                        BannerObject.GetComponent<CanvasGroup>().DOFade(0f, 1f);
                });
                OnSummonResponseData += (SummonResponseData summonResponseData) =>
                {
                        if (summonResponseData.summon_results.Count > 1)
                        {
                                StartCoroutine(SummonEffectManager.SummonTenTimes(summonResponseData.summon_results,SummonTenTimesButton.transform as RectTransform));
                        }
                        else
                        {
                                StartCoroutine(SummonEffectManager.SummonOnce(summonResponseData.summon_results[0],SummonOnceButton.transform as RectTransform));
                        }
                };
        }

        private void OnEnable()
        {
                BannerObject.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        }

        public void SetSummonOnceButtonInteractable(bool isInteractable)
        {
                SummonOnceButton.interactable = isInteractable;
                BlockInputLayer.SetActive(!isInteractable);
        }
        public void SetSummonTenTimesButtonInteractable(bool isInteractable)
        {
                SummonTenTimesButton.interactable = isInteractable;
                BlockInputLayer.SetActive(!isInteractable);
        }
        
}