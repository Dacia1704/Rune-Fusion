using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager: MonoBehaviour
{
        private Button soundButton;
        private Image image;
        
        [SerializeField] private Sprite soundOn;
        [SerializeField] private Sprite soundOff;

        private void Awake()
        {
                soundButton = GetComponent<Button>();
                image = GetComponent<Image>();
                
                image.sprite = soundOn;
        }

        private void Start()
        {
                soundButton.onClick.AddListener(() =>
                {
                        if (image.sprite == soundOn)
                        {
                                image.sprite = soundOff;
                                AudioManager.Instance.AudioPropsSO.MasterAudioMixerGroup.audioMixer.SetFloat("MasterVolumn", -80);
                        }
                        else
                        {
                                image.sprite = soundOn;
                                AudioManager.Instance.AudioPropsSO.MasterAudioMixerGroup.audioMixer.SetFloat("MasterVolumn", 20);
                        }
                });
        }
}