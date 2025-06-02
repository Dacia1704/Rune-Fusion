using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleEndNotification: UIBase
{
       [SerializeField] private TextMeshProUGUI battleStatusText;
       [SerializeField] private TextMeshProUGUI goldText;
       [SerializeField] private GameObject victoryIcon;
       [SerializeField] private GameObject loseIcon;
       [SerializeField] private Button HomeButton;
       [SerializeField] private Button NewMatchButton;
       
       private CanvasGroup canvasGroup;
       private RectTransform rectTransform;
       private AudioSource audioSource;

       private void Awake()
       {
              canvasGroup = GetComponent<CanvasGroup>();
              rectTransform = GetComponent<RectTransform>();
              audioSource = GetComponent<AudioSource>();
              
              HomeButton.onClick.AddListener(() =>
              {
                     SceneLoadManager.Instance.LoadMainSceneWithSpecificTab(0);
              });
              NewMatchButton.onClick.AddListener(() =>
              {
                     SceneLoadManager.Instance.LoadMainSceneWithSpecificTab(2);
              });
       }

       public void SetVictory(int gold)
       {
              Show();
              victoryIcon.SetActive(true);
              loseIcon.SetActive(false);
              battleStatusText.text = "Victory";
              battleStatusText.color = Color.yellow;
              goldText.text = gold.ToString();
              canvasGroup.alpha = 0;
              rectTransform.localScale = Vector3.zero;
              PlayVictorySound();
              Sequence seq = DOTween.Sequence();
              seq.Append(canvasGroup.DOFade(1f, 0.5f));
              seq.Join(rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack)); 
       }

       public void SetLose(int gold)
       {
              Show();
              victoryIcon.SetActive(false);
              loseIcon.SetActive(true);
              battleStatusText.text = "Defeat"; 
              battleStatusText.color = Color.red;
              goldText.text = gold.ToString();
              canvasGroup.alpha = 0;
              rectTransform.localScale = Vector3.zero;
              PlayDefeatSound();
              Sequence seq = DOTween.Sequence();
              seq.Append(canvasGroup.DOFade(1f, 0.5f));
              seq.Join(rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack)); 
       }
       public void PlayVictorySound()
       {
              audioSource.clip = AudioManager.Instance.AudioPropsSO.VictorySound;
              audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
              audioSource.Play();
       }
       public void PlayDefeatSound()
       {
              audioSource.clip = AudioManager.Instance.AudioPropsSO.DefeatSound;
              audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
              audioSource.Play();
       }
}