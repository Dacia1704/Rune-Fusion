using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActionLine : MonoBehaviour
{
        private SpriteRenderer spriteRenderer;
        
        private Dictionary<string,GameObject> MonsterDictionary = new Dictionary<string, GameObject>();

        [field: SerializeField]public GameObject Player1 { get;private set; }
        [field: SerializeField]public GameObject Player2 {get;private set;}

        private int animationCounter = 0;
        private void Awake()
        {
                spriteRenderer = GetComponent<SpriteRenderer>();
                animationCounter = -1;
        }

        private void Start()
        {
                spriteRenderer.size = new Vector2(CameraManager.Instance.GetWidthCamera() * 0.9f, spriteRenderer.size.y);
        }

        public float GetActionLineWidth()
        {
                return spriteRenderer.bounds.size.x;
        }
        public float GetActionLineHeight()
        {
                return spriteRenderer.bounds.size.y;
        }

        public void SetActionLinePostion(Vector3 pos)
        {
                transform.position = pos;
        }

        private void CreateMonsterPoint(string id,float progress)
        {
                GameObject monsterPoint;
                if (id[0] == '0')
                {
                        if (id == "01") monsterPoint = Instantiate(Player1);
                        else monsterPoint = Instantiate(Player2);
                }
                else
                {
                        Sprite monsterSprite = Match.Instance.GetMonsterByIdInBattle(id).MonsterPropsSO.Icon;
                        monsterPoint = new GameObject();
                        monsterPoint.AddComponent<SpriteRenderer>();
                        monsterPoint.GetComponent<SpriteRenderer>().sprite = monsterSprite;
                }
                monsterPoint.transform.SetParent(transform,false);
                monsterPoint.transform.position = new Vector3( transform.position.x-GetActionLineWidth()/2 +progress * GetActionLineWidth(),transform.position.y, transform.position.z);
                MonsterDictionary.Add(id, monsterPoint);
        }

        public void SetDeathMonsterPoint(string id)
        {
                SpriteRenderer monsterPoint = MonsterDictionary[id].GetComponent<SpriteRenderer>();
                monsterPoint.color = Color.clear;
        }

        private void SetPositionMonsterPoint(string id, float progress)
        {
                GameObject monsterPoint = MonsterDictionary[id];
                Vector3 targetPosition = new Vector3( transform.position.x-GetActionLineWidth()/2 +progress * GetActionLineWidth(),transform.position.y, transform.position.z);
                if (animationCounter == -1) animationCounter = 0;
                animationCounter += 1;
                monsterPoint.transform.DOMoveX(targetPosition.x, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                        animationCounter -= 1;
                        
                });
        }

        public void UpdateMonsterPoint(List<TurnBaseData> turnBaseObjects)
        {
                foreach (TurnBaseData turnBaseData in turnBaseObjects)
                {
                        if (!MonsterDictionary.ContainsKey(turnBaseData.id_in_battle)) CreateMonsterPoint(turnBaseData.id_in_battle,turnBaseData.progress);
                        SetPositionMonsterPoint(turnBaseData.id_in_battle, turnBaseData.progress);
                }
                StartCoroutine(ExecuteTurnCoroutine());
        }

        private IEnumerator ExecuteTurnCoroutine()
        {
                // while (true)
                // {
                        yield return new WaitUntil(() => animationCounter == 0);
                        animationCounter = -1;
                        Match.Instance.TurnManager.ExecuteTurn();
                // }
        }
}