using System;
using TMPro;
using UnityEngine;

public class GameUIManager: MonoBehaviour
{
        public static GameUIManager Instance { get; private set; }
        public UITimeCounter UITimeCounter { get; private set; }
        public UIRunePointManager UIRunePointManager { get; private set; }

        public TextMeshProUGUI TurnText;

        private void Awake()
        {
                Instance = this;
                UITimeCounter = GetComponentInChildren<UITimeCounter>();
                UIRunePointManager = FindFirstObjectByType<UIRunePointManager>();
        }

        public void SetTurnText(string turnText, Color textColor)
        {
                TurnText.text = turnText;
                TurnText.color = textColor;
        }
}