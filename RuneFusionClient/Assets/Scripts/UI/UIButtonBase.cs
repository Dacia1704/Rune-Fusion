using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIButtonBase: UIBase
{
        private AudioSource audioSource;
        private void Awake()
        {
                audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
                audioSource.clip = AudioManager.Instance.AudioPropsSO.ButtonClickSound;
                audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
                audioSource.playOnAwake = false;
                GetComponent<Button>().onClick.AddListener(() =>
                {
                        audioSource.Play();
                });
        }
}