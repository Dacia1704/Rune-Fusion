using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UISummonEffectManager: UIBase
{
        private List<RectTransform> summonPositionList;
        [SerializeField] private RectTransform summonOncePosition;
        [SerializeField] private GameObject summonEffectPrefab;
        [SerializeField] private Transform summonEffectParent;
        private void Awake()
        {
                summonPositionList = new List<RectTransform>();
                foreach (Transform child in this.transform)
                {
                        RectTransform rt = child as RectTransform;
                        summonPositionList.Add(rt);
                }
        }


        public IEnumerator SummonOnce(SummonResult summonResult,RectTransform pos)
        {
                foreach (Transform obj in summonEffectParent)
                {
                        obj.gameObject.SetActive(false);
                }
                GameObject summonObject = Instantiate(this.summonEffectPrefab, summonEffectParent);
                summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                summonObject.GetComponent<UISummonEffect>().SetUp(summonResult,summonOncePosition);

                yield return new WaitUntil(() =>
                {
                        return summonObject.GetComponent<UISummonEffect>().IsCompleted == true;
                });
                UISummonManager.Instance.SetSummonOnceButtonInteractable(true);
                UISummonManager.Instance.SetSummonTenTimesButtonInteractable(true);
        }

        public IEnumerator SummonTenTimes(List<SummonResult> summonResults,RectTransform pos)
        {
                foreach (Transform obj in summonEffectParent)
                {
                        obj.gameObject.SetActive(false);
                }

                List<UISummonEffect> effectList = new List<UISummonEffect>();
                for (int i=0;i<summonResults.Count;i++)
                {
                        GameObject summonObject = Instantiate(this.summonEffectPrefab, summonEffectParent);
                        summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                        summonObject.GetComponent<UISummonEffect>().SetUp(summonResults[i],summonPositionList[i]);
                        effectList.Add(summonObject.GetComponent<UISummonEffect>());
                }
                
                yield return new WaitUntil(() =>
                {
                        foreach (UISummonEffect effect in effectList)
                        {
                                if (effect.IsCompleted == false)
                                {
                                        return false;
                                }
                        }
                        return true;
                });
                UISummonManager.Instance.SetSummonOnceButtonInteractable(true);
                UISummonManager.Instance.SetSummonTenTimesButtonInteractable(true);
        }

        public void DestroyDisableObjects()
        {
                foreach (Transform obj in summonEffectParent)
                {
                        if (!obj.gameObject.activeInHierarchy)
                        {
                                Destroy(obj.gameObject);
                        }
                }
        }
        
        
}