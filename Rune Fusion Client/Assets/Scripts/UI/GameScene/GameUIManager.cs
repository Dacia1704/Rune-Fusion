using System;
using UnityEngine;

public class GameUIManager: MonoBehaviour
{
        public static GameUIManager Instance { get; private set; }
        public UITimeCounter UITimeCounter { get; private set; }

        private void Awake()
        {
                Instance = this;
                UITimeCounter = GetComponentInChildren<UITimeCounter>();
        }
        
}