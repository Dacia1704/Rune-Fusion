using System;
using UnityEngine;
using UnityEngine.UI;

public class UISummonManager: UIBase
{
        public static UISummonManager Instance { get; private set; }
        [SerializeField] private Button SummonOnceButton;
        [SerializeField] private Button SummonTenTimesButton;
        private UISummonEffectManager summonEffectManager;
        public Action<SummonResponseData> OnSummonResponseData { get; private set; }
        private void Awake()
        {
                Instance = this;
                summonEffectManager = GetComponentInChildren<UISummonEffectManager>();
                SummonOnceButton.onClick.AddListener(() =>
                {
                        SocketManager.Instance.RequestSummonData(1);
                });
                SummonTenTimesButton.onClick.AddListener(() =>
                {
                        SocketManager.Instance.RequestSummonData(10);
                });
                OnSummonResponseData += (SummonResponseData summonResponseData) =>
                {
                        if (summonResponseData.summon_results.Count > 1)
                        {
                                summonEffectManager.SummonTenTimes(summonResponseData.summon_results,SummonTenTimesButton.transform as RectTransform);
                        }
                        else
                        {
                                summonEffectManager.SummonOnce(summonResponseData.summon_results[0],SummonOnceButton.transform as RectTransform);
                        }
                };
        }
}