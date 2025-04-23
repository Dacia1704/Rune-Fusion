using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISummonEffect: UIBase
{
        [SerializeField] private GameObject startEffect;
        [SerializeField] private GameObject summonEffect;
        [SerializeField] private GameObject summonResult;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Image goldImage;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject bumpEffect;

        private bool isDuplicate;
        public void SetUp(SummonResult result,RectTransform pos)
        {
                summonEffect.SetActive(false);
                if (result.monster_id >= 0)
                {
                        characterImage.sprite = UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[result.monster_id].MonsterProps.Icon;
                        if (UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[result.monster_id].IsOwn)
                        {
                                isDuplicate = true;
                        }
                        else
                        {
                                isDuplicate = false;
                                UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[result.monster_id].SetOwn(true);
                        }
                        characterImage.gameObject.SetActive(true);
                        goldImage.gameObject.SetActive(false);
                        resultText.text = UIMainMenuManager.Instance.MonsterListSO.MonsterDictionary[result.monster_id].MonsterProps.MonsterData.Name;
                }
                else
                {
                        characterImage.gameObject.SetActive(false);
                        goldImage.gameObject.SetActive(true);
                        resultText.text = result.gold.ToString();
                }
                summonResult.SetActive(false);
                
                startEffect.SetActive(true);
                gameObject.GetComponent<RectTransform>()
                        .DOAnchorPos(pos.anchoredPosition, 1f)
                        .SetEase(Ease.OutQuad).OnComplete(() =>
                        {
                                startEffect.SetActive(false);
                                summonEffect.SetActive(true);
                        });
        }

        private void Update()
        {
                if (summonEffect.activeSelf)
                {
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                        if (stateInfo.normalizedTime >= 0.9f)
                        {
                                startEffect.SetActive(false);
                                summonEffect.SetActive(false);
                                summonResult.SetActive(true);
                                if (isDuplicate)
                                {
                                        characterImage.GetComponent<RectTransform>().DOShakeAnchorPos(1.5f, new Vector2(5f, 5f), 50, 90, false, false)
                                                .SetDelay(0.7f)
                                                .OnComplete(() =>
                                                {
                                                        StartCoroutine(DuplicateParticalCoroutine());
                                                });
                                }
                                else
                                {
                                        summonResult.GetComponent<RectTransform>().DOShakeAnchorPos(1.5f, new Vector2(10f, 10f), 1, 90, false, false)
                                                .SetLoops(-1, LoopType.Restart);
                                }
                        }
                }
        }

        private IEnumerator DuplicateParticalCoroutine()
        {
                GameObject bumpObject = Instantiate(bumpEffect);
                Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(
                        screenPos.x,
                        screenPos.y,
                        10
                ));
                bumpObject.transform.position = worldPos;
                
                float waitTime = bumpObject.GetComponent<ParticleSystem>().main.duration * 0.3f; 
                bumpObject.GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(waitTime);
                characterImage.gameObject.SetActive(false);
                goldImage.gameObject.SetActive(true);
                resultText.text = "2000";
                summonResult.GetComponent<RectTransform>().DOShakeAnchorPos(1.5f, new Vector2(10f, 10f), 1, 90, false, false)
                        .SetLoops(-1, LoopType.Restart);
                
                UISummonManager.Instance.SetSummonOnceButtonInteractable(true);
                UISummonManager.Instance.SetSummonTenTimesButtonInteractable(true);
                UISummonManager.Instance.SummonEffectManager.DestroyDisableObjects();
        }

        private void OnDestroy()
        {
                DOTween.KillAll(startEffect);
                DOTween.KillAll(summonEffect);
                DOTween.KillAll(summonResult);
                DOTween.KillAll(gameObject);
        }
}