using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UISkillMonster: UIBase,IPointerDownHandler,IPointerUpHandler
{
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        private Dictionary<string,string> keyWordsDictionary = new Dictionary<string,string>();
        
        private bool isHolding = false;
        private float holdTime = 0f;
        private bool isShowEffectDes = false;

        private float requiredHoldTime = 0.5f;
        private void Awake()
        { 
                InitKeyWordsDictionary();
        }

        private void Update()
        {
                if (isHolding)
                {
                        holdTime += Time.deltaTime;
                        if (holdTime >= requiredHoldTime)
                        {
                                isHolding = false;
                                holdTime = 0;
                                int linkIndex = TMP_TextUtilities.FindIntersectingLink(descriptionText, Input.mousePosition, Camera.main);
                                if (linkIndex != -1)
                                {
                                        var linkInfo = descriptionText.textInfo.linkInfo[linkIndex];
                                        if (linkInfo.GetLinkID() == "effect")
                                        {
                                                isShowEffectDes = true;
                                                UIMainMenuManager.Instance.UIDetailMonster.EffectDescription.Show();
                                        }
                                }
                        }
                }

                if (isShowEffectDes)
                {
                        int linkIndex = TMP_TextUtilities.FindIntersectingLink(descriptionText, Input.mousePosition, Camera.main);
                        if (linkIndex == -1)
                        {
                                UIMainMenuManager.Instance.UIDetailMonster.EffectDescription.Hide();
                                return;
                        }
                        var popup = UIMainMenuManager.Instance.UIDetailMonster.EffectDescription;
                        RectTransform popupRect = popup.GetComponent<RectTransform>();
                        Vector2 anchoredPos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                (RectTransform)popupRect.parent,     
                                Input.mousePosition , 
                                Camera.main,     
                                out anchoredPos
                        );
                        popupRect.anchoredPosition = anchoredPos;
                }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(descriptionText, Input.mousePosition, Camera.main);
                if (linkIndex != -1)
                {
                        var linkInfo = descriptionText.textInfo.linkInfo[linkIndex];
                        if (linkInfo.GetLinkID() == "effect")
                        {
                                isHolding = true;
                                holdTime = 0f;
                        }
                }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
                UIMainMenuManager.Instance.UIDetailMonster.EffectDescription.Hide();
                isShowEffectDes = false;
                isHolding = false;
                holdTime = 0f;
        }


        public void InitKeyWordsDictionary()
        {
                keyWordsDictionary.Add("Burn", "Deals damage over time. Damage fixed by 5% hp of monster.");
                keyWordsDictionary.Add("Speed Increment", "Increases SPD by 25%.");
                keyWordsDictionary.Add("Attack Increment", "Increases ATK by 50%.");
                keyWordsDictionary.Add("Defend Increment", "Increases DEF by 50%.");
                keyWordsDictionary.Add("Speed Decrement", "Decreases SPD by 25%.");
                keyWordsDictionary.Add("Attack Decrement", "Decreases ATK by 25%.");
                keyWordsDictionary.Add("Defend Decrement", "Decreases DEF by 50%.");
                keyWordsDictionary.Add("Taunt", "Forces enemies to target this monster land taunt.");
                keyWordsDictionary.Add("Frozen", "Frozen monster. That monster can't attack in this state.");
        }

        public void SetSkillText(string n, string des)
        {
                nameText.text = n;
                descriptionText.text = MarkText(des);
        }

        public string MarkText(string text)
        {
                foreach (string key in keyWordsDictionary.Keys)
                {
                        if (text.Contains(key.ToString()))
                        {
                                string tagged = $"<link=\"effect\"><color=red>{key.ToString()}</color></link>";
                                text = Regex.Replace(text, Regex.Escape(key.ToString()), tagged);
                        }
                }
                return text;
        }
        
        
}