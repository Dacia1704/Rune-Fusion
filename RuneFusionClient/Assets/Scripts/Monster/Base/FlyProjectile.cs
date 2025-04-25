using DG.Tweening;
using UnityEngine;

public class FlyProjectile : MonoBehaviour
{
        protected AudioSource audioSource;

        protected void Awake()
        {
                gameObject.AddComponent<AudioSource>();
                audioSource = GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
        }
        public virtual void FlyToPos(Transform target,int dam,EffectSkill effect)
        {
                if (target == null) return;
                transform.DOMove(target.position, 0.3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => Destroy(gameObject));
        }
}