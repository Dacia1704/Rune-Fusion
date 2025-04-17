using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPickSlot: UIBase
{
        private Animator animator;
        private Image image;

        public Image Icon;

        public bool IsPicking;

        private void Awake()
        {
                animator = GetComponent<Animator>();
                image = GetComponent<Image>();
                SetIdle();
        }

        public void SetPicking()
        {
                animator.speed = 1;
                image.color = Color.white;
                IsPicking = true;
        }

        public void SetIdle()
        {
                animator.speed = 0;
                image.color = Color.black;
                IsPicking = false;
        }

        public void EnableIcon(Sprite sprite)
        {
                Icon.sprite = sprite;
                Icon.color = Color.white;
        }

        public void DisableIcon()
        {
                Icon.sprite = null;
                Icon.color = Color.clear;
        }
}