using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class UISummonEffectManager: UIBase
{
        private List<RectTransform> summonPositionList;
        [SerializeField] private RectTransform summonOncePosition;
        [SerializeField] private GameObject summonEffectPrefab;
        
        private List<GameObject> summonEffectList = new List<GameObject>();
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
                GameObject summonObject = Instantiate(this.summonEffectPrefab, this.transform);
                summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                summonObject.GetComponent<UISummonEffect>().SetUp(summonResult,summonOncePosition);

                foreach (GameObject obj in summonEffectList)
                {
                        Destroy(obj);
                }
                summonEffectList.Clear();
                summonEffectList.Add(summonObject);
        }

        public void SummonTenTimes(List<SummonResult> summonResults,RectTransform pos)
        {
                foreach (GameObject obj in summonEffectList)
                {
                        Destroy(obj);
                }
                summonEffectList.Clear();
                for (int i=0;i<summonResults.Count;i++)
                {
                        Debug.Log(i);
                        GameObject summonObject = Instantiate(this.summonEffectPrefab, this.transform);
                        summonObject.GetComponent<RectTransform>().anchoredPosition = pos.anchoredPosition;
                        summonObject.GetComponent<UISummonEffect>().SetUp(summonResults[i],summonPositionList[i]);
                        summonEffectList.Add(summonObject);
                }
        }
        
        
}