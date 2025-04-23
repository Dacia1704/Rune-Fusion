using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIRunePointManager : UIBase
{
        [SerializeField] private List<UIRunePoint> uiRunePoint;
        [field: SerializeField] public UIShieldRunePoint UIShieldRunePoint { get; private set; }

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
                                UIShieldRunePoint.SetFillImage((float)points[i]/(float)GameManager.Instance.GameManagerSO.MaxRunePoint);
                        }
                        else
                        {
                                // Debug.Log(i.ToString());
                                uiRunePoint[i].SetPointText(points[i]);
                        }
                }
        }
}