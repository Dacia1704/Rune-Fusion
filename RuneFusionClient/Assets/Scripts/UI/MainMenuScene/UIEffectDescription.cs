using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectDescription: UIBase
{
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private RectTransform rectTransform;


        private void Awake()
        {
                rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
                if (IsPopupOutOfBounds(rectTransform))
                {
                        rectTransform.pivot = new Vector2(1f, 0f);
                }
                else
                {
                        rectTransform.pivot = new Vector2(0f, 0f);
                }
        }


        public void SetStatDes(Sprite sprite, string description)
        {
                Show();
                iconImage.sprite = sprite;
                descriptionText.text = description;
        }
        bool IsPopupOutOfBounds(RectTransform popupRect)
        {
                RectTransform parentRect = (RectTransform)popupRect.parent;

                Vector2 pos = popupRect.anchoredPosition;
                Vector2 size = popupRect.sizeDelta;
                Vector2 parentSize = parentRect.rect.size;

                float left   = pos.x;
                float right  = pos.x + size.x;
                float bottom = pos.y;
                float top    = pos.y + size.y;

                float halfWidth = parentSize.x / 2f;
                float halfHeight = parentSize.y / 2f;

                // Check nếu bất kỳ cạnh nào của popup vượt khỏi parent
                if (left < -halfWidth || right > halfWidth || bottom < -halfHeight || top > halfHeight)
                {
                        return true;
                }

                return false;
        }


}