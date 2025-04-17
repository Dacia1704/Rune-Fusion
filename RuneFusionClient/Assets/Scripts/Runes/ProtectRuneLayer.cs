using System;
using DG.Tweening;
using UnityEngine;

public class ProtectRuneLayer : MonoBehaviour
{

        public void Disappear()
        {
                gameObject.SetActive(false);
        }
        
        public void Appear()
        {
                gameObject.SetActive(true);
        }
        
}