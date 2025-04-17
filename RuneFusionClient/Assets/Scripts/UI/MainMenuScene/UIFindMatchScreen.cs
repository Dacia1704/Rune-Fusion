using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFindMatchScreen : UIBase
{
        [SerializeField] private Button findMatchButton;
        private TextMeshProUGUI findMatchButtonText;
        private void Awake()
        {
                findMatchButtonText = findMatchButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
                findMatchButton.interactable = true;
        }

        private void Start()
        {
                findMatchButton.onClick.AddListener(() =>
                {
                        StartCoroutine(FindMatchButtonTextAnimation());
                        SocketManager.Instance.FindMatch();
                        findMatchButton.interactable = false;
                });
        }

        private IEnumerator FindMatchButtonTextAnimation()
        {
                while (true)
                {
                        findMatchButtonText.text = "Find Match .";
                        yield return new WaitForSeconds(0.5f);
                        findMatchButtonText.text = "Find Match ..";
                        yield return new WaitForSeconds(0.5f);
                        findMatchButtonText.text = "Find Match ...";
                        yield return new WaitForSeconds(0.5f);
                }
        }
}