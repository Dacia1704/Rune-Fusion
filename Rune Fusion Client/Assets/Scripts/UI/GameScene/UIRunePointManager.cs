using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRunePointManager : UIBase
{
        [SerializeField] private List<UIRunePoint> uiRunePoint;
        [SerializeField] private UIShieldRunePoint uiShieldRunePoint;

        private void Start()
        {
                UpdateUIRunePoint(new List<int>(){0,0,0,0,0});
        }

        public void UpdateUIRunePoint(List<int> runePoints)
        {
                for (int i = 0; i < runePoints.Count; i++)
                {
                        if (i == (int)RuneType.Shield)
                        {
                                uiShieldRunePoint.SetFillImage((float)runePoints[i]/(float)GameManager.Instance.GameManagerSO.MaxRunePoint);
                        }
                        else
                        {
                                Debug.Log(i.ToString());
                                uiRunePoint[i].SetPointText(runePoints[i]);
                        }
                }
        }
}