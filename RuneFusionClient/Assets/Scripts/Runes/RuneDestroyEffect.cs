using System;
using System.Collections;
using UnityEngine;

public class RuneDestroyEffect: MonoBehaviour, IPoolingObject
{
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    private AudioSource audioSource;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.RuneBreakSound;
        audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
    }

    private void OnEnable()
    {
        StartCoroutine(AutoReturnToPool());
        audioSource.Play();
    }

    private IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.Match.MatchBoard.RuneObjectPoolManager.ReleaseDestroyEffect(this.gameObject);
    }
}