using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : ObjectPooling
{
        private AudioSource audioSource;
        protected override void Awake()
        {
                base.Awake();
                gameObject.AddComponent<AudioSource>();
                audioSource = GetComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
                audioSource.playOnAwake = false;
        }

        public void PlayBuffSound()
        {
                audioSource.clip = AudioManager.Instance.AudioPropsSO.BuffSound;
                audioSource.Play();
        }

        public void PlayDebuffSound()
        {
                audioSource.clip = AudioManager.Instance.AudioPropsSO.DebuffSound;
                audioSource.Play();
        }

        public GameObject GetObjectByEffect(EffectType effectType)
        {
                if (effectType is EffectType.AttackIncrement or EffectType.DefendIncrement or EffectType.SpeedIncrement)
                {
                        PlayBuffSound();
                }
                else
                {
                        PlayDebuffSound();
                }
                return GetObject(effectType.ToString());
        }

        public void ClearEffect()
        {
                foreach (Transform child in transform)
                {
                        ReleaseObject(child.gameObject);
                }
        }
}