using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
        public static MainMenuManager Instance { get; private set; }

        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;

        }
}