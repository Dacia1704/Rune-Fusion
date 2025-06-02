using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour,IPoolingObject
{
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [field: SerializeField] public RuneType Type{get;private set;}
    [field: SerializeField] public RuneForm Form{get;private set;}
    
    public bool IsChecked{get;private set;}
    public bool IsProtected { get; private set; }
    private ProtectRuneLayer protectRuneLayer;
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }
    private AudioSource audioSource;

    private SpriteRenderer spriteRune;
    private SpriteRenderer spriteProtect;
    
    
    
    [HideInInspector]public TextMeshPro TextPos;
    
    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        TextPos = GetComponentInChildren<TextMeshPro>();
        protectRuneLayer = GetComponentInChildren<ProtectRuneLayer>();

        spriteRune = GetComponent<SpriteRenderer>();
        spriteProtect = protectRuneLayer.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
    }

    private void OnEnable()
    {
        IsProtected = false;
        IsChecked = false;
        protectRuneLayer.Disappear();
    }

    public void SetRune(int row, int col)
    {
        Row = row;
        Col = col;
        TextPos.text = Row + " " + Col;
    }

    public void CheckMatches(SwapType swapType = SwapType.None)
    {
        // GameManager.Instance.RuneManager.OnRuneChangePosition?.Invoke(Tuple.Create(Row,Col),swapType);
        GameManager.Instance.MatchBoard.OnRuneChangePostionAction(Tuple.Create(Row, Col), swapType);
    }

    public void ProtectRune()
    {
        IsProtected = true;
        protectRuneLayer.Appear();
    }
    public void BreakProtectLayer()
    {
        IsProtected = false;
        GameObject destroyEffect = GameManager.Instance.MatchBoard.RuneObjectPoolManager.GetProtectDestroyEffectObject();
        destroyEffect.transform.position = transform.position;
        protectRuneLayer.Disappear();
        SetIsChecked(false);
        TextPos.text = Row + " " + Col;
    }

    public void PlayFallSound()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.RuneFallSound;
        audioSource.Play();
    }

    public void PlayMatchesSound()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.RuneMatchesSound;
        audioSource.Play();
    }

    public void SetIsChecked(bool isChecked)
    {
        IsChecked = isChecked;
    }

    public void StartHintAnimation()
    {
        transform.DOScale(1.1f, 0.5f)  
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopHintAnimation()
    {
        transform.DOKill();
        transform.DOScale(1f, 0f);
    }

    public void SetSortingOrder(int order)
    {
        spriteRune.sortingOrder = order;
        spriteProtect.sortingOrder = order+1;
    }
}

