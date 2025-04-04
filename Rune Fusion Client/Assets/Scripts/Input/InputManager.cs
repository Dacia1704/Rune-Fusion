﻿using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
        private Vector2 startMousePosition,endMousePosition;
        private float swipeThreshold;
        private bool enablePlayerInput;
        private bool enableMonsterInput;

        public event Action<MonsterBase> OnMonsterTarget;

        private void Awake()
        {
                enablePlayerInput = false;
                enableMonsterInput = false;
                SetEnableMonsterInput();
        }

        private void Start()
        {
                swipeThreshold = GameManager.Instance.GameManagerSO.SwipeThreshold;
        }

        private void Update()
        {
                if (enablePlayerInput)
                {
                        if (Input.GetMouseButtonDown(0))
                        {
                                startMousePosition = Input.mousePosition;
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                                endMousePosition = Input.mousePosition;
                                DetectSwipe();
                        }
                }

                if (enableMonsterInput)
                {
                        if (Input.GetMouseButtonDown(0)) 
                        {
                                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                                if (hit.collider != null)
                                {
                                        Transform touchedTransform = hit.collider.transform;

                                        if (touchedTransform.CompareTag("Opponent"))
                                        {
                                                OnMonsterTarget?.Invoke(touchedTransform.parent.GetComponent<MonsterBase>());
                                        }
                                }
                        }
                }
        }
        
        private void DetectSwipe()
        {
                Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(startMousePosition.x, startMousePosition.y, 0));
                Vector3 worldEndPos = Camera.main.ScreenToWorldPoint(new Vector3(endMousePosition.x, endMousePosition.y, 0));
                float deltaX = worldEndPos.x - worldStartPos.x;
                float deltaY = worldEndPos.y - worldStartPos.y;
                if (Mathf.Abs(Mathf.Abs(deltaX) - Mathf.Abs(deltaY)) <= 0.5)
                {
                        Debug.Log("Can't swipe");
                        return;
                }
                Tuple<int,int> runeStart = GameManager.Instance.RuneManager.GetRunesByPosition(new Vector2(worldStartPos.x, worldStartPos.y));
                
                bool isVertical = !(Mathf.Abs(deltaX) > Mathf.Abs(deltaY));
                float delta = Mathf.Abs(deltaX) > Mathf.Abs(deltaY) ? deltaX : deltaY;
                
                if (Mathf.Abs(delta) > swipeThreshold)
                {
                        if (delta > 0)
                        {
                                if (isVertical)
                                {
                                        if (runeStart.Item1 < GameManager.Instance.GameManagerSO.HeightRuneMap - 1)
                                        {
                                                Debug.Log("Swiped Up");
                                                GameManager.Instance.RuneManager.SwapWithTopRune(runeStart);
                                        }
                                        else
                                        {
                                                Debug.Log("Can't swipe up");
                                        }
                                }
                                else
                                {
                                        if (runeStart.Item2 < GameManager.Instance.GameManagerSO.WidthRuneMap - 1)
                                        {
                                            Debug.Log("Swiped Right");
                                            GameManager.Instance.RuneManager.SwapWithRightRune(runeStart);    
                                        }
                                        else
                                        {
                                                Debug.Log("Can't swipe right");
                                        }
                                        
                                }
                        }
                        else
                        {
                                if (isVertical)
                                {
                                        if (runeStart.Item1 >= 1)
                                        {
                                                Debug.Log("Swiped Down"); 
                                                GameManager.Instance.RuneManager.SwapWithBottomRune(runeStart);
                                        }
                                        else
                                        {
                                                Debug.Log("Can't swipe down");
                                        }
                                        
                                }
                                else
                                {
                                        if (runeStart.Item2 >= 1)
                                        {
                                                Debug.Log("Swiped Left");
                                                GameManager.Instance.RuneManager.SwapWithLeftRune(runeStart);
                                        }
                                        else
                                        {
                                                Debug.Log("Can't swipe left");
                                        }
                                }
                                                                
                        }
                                
                }
        }

        public void SetEnablePlayerInput()
        {
                enablePlayerInput = true;
        }

        public void SetDisablePlayerInput()
        {
                enablePlayerInput = false;
        }

        public void SetEnableMonsterInput()
        {
                enableMonsterInput = true;
        }

        public void SetDisableMonsterInput()
        {
                enableMonsterInput = false;
        }
        
}