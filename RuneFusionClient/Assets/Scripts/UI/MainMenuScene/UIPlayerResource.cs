using System;
using TMPro;
using UnityEngine;

public class UIPlayerResource: UIBase
{
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textScroll;

        private int countCall;
        
        private void Awake()
        {
                countCall = 0;
        }
        
        private void OnEnable()
        {
                if (countCall <=2)
                {
                        countCall++;
                }
                else
                {
                        SocketManager.Instance.ResquestResourceData();
                }
        }

        public void SetTextName(string name)
        {
                textName.text = name;
        }

        public void SetResourceText(int gold, int scroll)
        {
                string goldStr = gold.ToString();
                if (gold >= 1000000000)
                {
                        goldStr = (gold / 1000000000).ToString() + "B";
                }
                else if(gold>=1000000)
                {
                        goldStr = (gold / 1000000).ToString() + "M";
                }else if (gold >= 1000)
                {
                        goldStr = (gold / 1000).ToString() + "K";
                }
                string scrollStr = scroll.ToString();
                if (scroll >= 1000000000)
                {
                        scrollStr = (scroll / 1000000000).ToString() + "B";
                }
                else if(scroll>=1000000)
                {
                        scrollStr = (scroll / 1000000).ToString() + "M";
                }else if (scroll >= 1000)
                {
                        scrollStr = (scroll / 1000).ToString() + "K";
                }
                textGold.text = goldStr;
                textScroll.text = scrollStr;
        }
}