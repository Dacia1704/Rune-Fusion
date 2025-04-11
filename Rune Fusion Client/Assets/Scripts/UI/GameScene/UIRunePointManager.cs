using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRunePointManager : UIBase
{
        [SerializeField] private List<UIRunePoint> uiRunePoint;
        [SerializeField] private UIShieldRunePoint uiShieldRunePoint;

        private void Start()
        {
                PointPushData points = new PointPushData();
                points.player1 = new List<int>() { 0, 0, 0, 0, 0 };
                points.player2 = new List<int>() { 0, 0, 0, 0, 0 };
                UpdateUIRunePoint(points);
        }

        public void UpdateUIRunePoint(PointPushData runePoints)
        {
                List<int> points = SocketManager.Instance.PlayerData.playerindex==0 ? runePoints.player1 : runePoints.player2;
                for (int i = 0; i < points.Count; i++)
                {
                        if (i == (int)RuneType.Shield)
                        {
                                uiShieldRunePoint.SetFillImage((float)points[i]/(float)GameManager.Instance.GameManagerSO.MaxRunePoint);
                        }
                        else
                        {
                                // Debug.Log(i.ToString());
                                uiRunePoint[i].SetPointText(points[i]);
                        }
                }
        }
}