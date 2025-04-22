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


        public void SummonOnce(SummonResult summonResult,RectTransform pos)
        {
                GameObject summonObject = Instantiate(this.summonEffectPrefab, summonEffectParent);
                summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                summonObject.GetComponent<UISummonEffect>().SetUp(summonResult,summonOncePosition);

                foreach (Transform obj in summonEffectParent)
                {
                        obj.gameObject.SetActive(false);
                }
        }

        public void SummonTenTimes(List<SummonResult> summonResults,RectTransform pos)
        {
                foreach (Transform obj in summonEffectParent)
                {
                        obj.gameObject.SetActive(false);
                }
                for (int i=0;i<summonResults.Count;i++)
                {
                        GameObject summonObject = Instantiate(this.summonEffectPrefab, summonEffectParent);
                        summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                        summonObject.GetComponent<UISummonEffect>().SetUp(summonResults[i],summonPositionList[i]);
                }
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