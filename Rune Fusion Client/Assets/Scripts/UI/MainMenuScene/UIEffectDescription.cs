using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectDescription: UIBase
{
        private Image iconImage;
        private TextMeshProUGUI descriptionText;


        private void Awake()
        {
                iconImage = GetComponentInChildren<Image>();
                descriptionText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        

        public void SetStatDes(Sprite sprite, string description)
        {
                Show();
                iconImage.sprite = sprite;
                descriptionText.text = description;
        }
}