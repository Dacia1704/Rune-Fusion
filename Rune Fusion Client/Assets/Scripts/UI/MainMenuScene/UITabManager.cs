
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private Sprite inactiveTabBG;
    [SerializeField] private Sprite activeTabBG;
    // [SerializeField] private Vector2 inactiveTabButtonSize;
    // [SerializeField] private Vector2 activeTabButtonSize;
    private void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int id = i;
            tabButtons[id].onClick.AddListener(()=> SwitchToTab(id));
        }
    }

    public void SwitchToTab(int tabID)
    {
        foreach (GameObject go in tabs)
        {
            go.SetActive(false);
        }

        tabs[tabID].SetActive(true);

        foreach (Button button in tabButtons)
        {
            button.GetComponent<Image>().sprite = inactiveTabBG;
        }

        tabButtons[tabID].GetComponent<Image>().sprite = activeTabBG;
    }
}