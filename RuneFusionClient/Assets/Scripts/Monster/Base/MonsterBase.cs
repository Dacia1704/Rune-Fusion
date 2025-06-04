using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MonsterBase : MonoBehaviour
{
    [field: SerializeField] public MonsterPropsSO MonsterPropsSO { get; private set; }
    [field: SerializeField] public MonsterStats MonsterStatsInBattle { get; private set; }

    [HideInInspector] public int MonsterIndexinBattle;
    [HideInInspector] public string MonsterIdInBattle;
    public MonsterAnimationManager MonsterAnimationManager { get; private set; }
    public MonsterColliderManager MonsterColliderManager { get; private set; }
    protected StateMachine stateMachine;
    protected FloatingTextObjectPooling floatingTextObjectPooling;

    protected UIEffectManager UIEffectManager;
    public Dictionary<MonsterBase, ActionResponse> CurrentTurnActionResponse;

    protected AudioSource audioSource;

    public Action AttackTaskComplete;
    public Action WalkTaskComplete;
    public Action SkillTaskComplete;
    public Action HitTaskComplete;



    [HideInInspector]public bool ShouldUseSkill;
    [HideInInspector]public bool IsAllAnimationEnd;
    [HideInInspector]public bool IsUpdateEffect;
    [HideInInspector]public bool IsFrozen;
    [HideInInspector]public bool IsAlive;
    [HideInInspector]public bool IsDead;
    [SerializeField]protected GameObject FrozenGameObject;
    [SerializeField] protected GameObject ShieldObject;
    [SerializeField] protected GameObject TurnObject;
    private int shield;

    public UIHeathSkillBarManager UIHeathSkillBarManager { get; private set; }
    
    public Action<int,bool,MonsterBase> OnHealthChange;
    protected virtual void Awake()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
        audioSource.playOnAwake = false;
        MonsterAnimationManager = GetComponentInChildren<MonsterAnimationManager>();
        MonsterColliderManager = GetComponentInChildren<MonsterColliderManager>();
        UIHeathSkillBarManager = GetComponentInChildren<UIHeathSkillBarManager>();
        UIEffectManager = GetComponentInChildren<UIEffectManager>();
        floatingTextObjectPooling = GetComponentInChildren<FloatingTextObjectPooling>();
        CurrentTurnActionResponse = new Dictionary<MonsterBase, ActionResponse>();
        OnHealthChange += UIHeathSkillBarManager.SetHealthBar;
        GameManager.Instance.Match.MatchBoard.OnRunePointsChanged += UpdateSkillBar;
        
    }

    protected virtual void Start()
    {
        shield = 0;
        stateMachine = new StateMachine();
        IsAllAnimationEnd = false;
        IsUpdateEffect = false;
        IsFrozen = false;
        HitTaskComplete += ChangeNomalIdleState;
        IsAlive = true;
        MonsterStatsInBattle = new MonsterStats
        {
            Attack = MonsterPropsSO.MonsterData.BaseStats.Attack,
            Defend = MonsterPropsSO.MonsterData.BaseStats.Defend,
            Health = MonsterPropsSO.MonsterData.BaseStats.Health,
            Speed = MonsterPropsSO.MonsterData.BaseStats.Speed,
            Accuracy = MonsterPropsSO.MonsterData.BaseStats.Accuracy,
            Resistance = MonsterPropsSO.MonsterData.BaseStats.Resistance,
            EffectList = new List<EffectSkill>(),
        };
        UIHeathSkillBarManager.SetMaxHealthBar(MonsterPropsSO.MonsterData.BaseStats.Health);
        UIHeathSkillBarManager.SetHealthBar(MonsterPropsSO.MonsterData.BaseStats.Health);
        UIHeathSkillBarManager.SetMaxSkillBar(MonsterPropsSO.MonsterData.Skills[1].PointCost);
        ShouldUseSkill = false;
        
        TurnObject.SetActive(true);
        RectTransform rectTransform = TurnObject.GetComponent<RectTransform>();
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y +6, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        TurnObject.SetActive(false);
    }

    
    
    protected virtual void Update()
    {
        stateMachine.Update();

        if (!IsAlive)
        {
            stateMachine.ChangeState(new DeathState(this));
            IsAlive = true;
            IsDead = true;
            GameManager.Instance.Match.OnMonsterDeath?.Invoke(MonsterIdInBattle);
        }
    }
    public virtual void StartAttack(MonsterActionResponse monsterActionResponse)
    {
        SetTurnObject(false);
        StartCoroutine(AttackCoroutine(monsterActionResponse));
    }

    protected virtual IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
    {
        yield return null;
    }
    public virtual void AttackInFrame()
    {
        foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
        {
            action.Key.StartHit(action.Value.dam,action.Value.effect);
        }
    }

    public virtual void StartSkill(MonsterActionResponse monsterActionResponse)
    {
        SetTurnObject(false);
        StartCoroutine(SkillCoroutine(monsterActionResponse));
    }
    protected virtual IEnumerator SkillCoroutine(MonsterActionResponse monsterActionResponse)
    {
        yield return null;
    }

    public virtual void SkillInFrame()
    {
        foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
        {
            action.Key.StartHit(action.Value.dam,action.Value.effect);
        }
    }
    public virtual void StartHit(int dam,EffectSkill effect)
    {
        if (IsDead) return;
        Debug.Log(gameObject.name +" get Hit");
        Match.Instance.TargetManager.DisableTarget();
        stateMachine.ChangeState(new HurtState(this,dam,effect));
    }

    protected void ChangeNomalIdleState()
    {
        MonsterAnimationManager.EndSkillEffect();
        stateMachine.ChangeState(new IdleState(this));
        IsAllAnimationEnd = true;
    }
    public void SetOpponent()
    {
        MonsterColliderManager.transform.tag = "Opponent";
    }
    public void SetAlly()
    {
        MonsterColliderManager.transform.tag = "Ally";
    }
    public void GetDam(int dam,EffectSkill effect=null)
    {
        // dam = 0;
        Debug.Log(gameObject.name +" has "+ MonsterStatsInBattle.Health +" "+ shield +" get dam "+ dam);
        if (dam > 0)
        {
            if (shield > dam)
            {
                shield -= dam;
                dam = 0;
            }
            else
            {
                shield = 0;
                dam -= shield;
            }
        }
        if (shield <= 0)
        {
            DisableShield();
        }
        Debug.Log(gameObject.name +" get dam: "+ dam);
        int health = MonsterStatsInBattle.Health-dam;
        
        floatingTextObjectPooling.ShowDamage(dam);
        MonsterStatsInBattle.Health = Mathf.Clamp(health,0,MonsterPropsSO.MonsterData.BaseStats.Health);
        if ( effect!=null && effect.EffectType != EffectType.None && effect.EffectType != EffectType.Heal )
        {
            if (effect.EffectType == EffectType.Burn)
            {
                MonsterStatsInBattle.EffectList.Add(effect);
                GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
                effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
            }
            else
            {
                int existingEffectIndex = MonsterStatsInBattle.EffectList.FindIndex(e => e.EffectType == effect.EffectType);
                if (existingEffectIndex != -1)
                {
                    var existingEffect = MonsterStatsInBattle.EffectList[existingEffectIndex];
                    if (existingEffect.duration < effect.duration)
                    {
                        MonsterStatsInBattle.EffectList[existingEffectIndex].duration = effect.duration;
                        GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
                        effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
                    }
                }
                else
                {
                    if(effect.EffectType == EffectType.Frozen) EnableFrozenEffect();
                    MonsterStatsInBattle.EffectList.Add(effect);
                    GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
                    effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
                }
            }
        }
        OnHealthChange?.Invoke(MonsterStatsInBattle.Health,true,this);
        if (IsFrozen)
        {
            IsAllAnimationEnd = true;
        }
    }

    public void Buff()
    {
        foreach (KeyValuePair<MonsterBase,ActionResponse> monster in CurrentTurnActionResponse)
        {
            monster.Key.GetBuff(monster.Value.effect);
            if (monster.Key != this)
            {
                monster.Key.IsAllAnimationEnd = true;
            }
        }
        
    }
    public void GetBuff(EffectSkill effect)
    {
        int existingEffectIndex = MonsterStatsInBattle.EffectList.FindIndex(e => e.EffectType == effect.EffectType);
        if (existingEffectIndex != -1)
        {
            var existingEffect = MonsterStatsInBattle.EffectList[existingEffectIndex];
            if (existingEffect.duration < effect.duration)
            {
                MonsterStatsInBattle.EffectList[existingEffectIndex].duration = effect.duration;
                GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
                effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
            }
        }
        else
        {
            MonsterStatsInBattle.EffectList.Add(effect);
            GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
            effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
        }
    }

    protected virtual Vector3 GetPosPerformAttack()
    {
        return Vector3.zero;
    }

    protected virtual Vector3 GetPosPerformSkill()
    {
        return Vector3.zero;
    }

    public virtual IEnumerator UpdateEffect(int dam,List<EffectSkill> effects)
    {
        Debug.Log("UpdateEffect " + gameObject.name);
        if (dam > 0)
        {
            StartHit(dam,null);
        }
        else
        {
            IsAllAnimationEnd = true;
        }
        yield return new WaitUntil(() => IsAllAnimationEnd);
        MonsterStatsInBattle.EffectList = effects;

        bool hasFrozen = false;
        foreach (EffectSkill effect in MonsterStatsInBattle.EffectList)
        {
            if (effect.EffectType == EffectType.Frozen)
            {
                hasFrozen = true;
                break;
            }
        }

        if (!hasFrozen && IsFrozen)
        {
            DisableFrozenEffect();
            SetTurnObject(false);
        }

        UpdateUIEffect();
        IsUpdateEffect = true;
    }

    public void EnableFrozenEffect()
    {
        IsFrozen = true;
        FrozenGameObject.SetActive(true);
        FrozenGameObject.GetComponent<Animator>().Play("Frozen");
        MonsterAnimationManager.PauseAnimation();
        IsAllAnimationEnd = true;
    }

    public void DisableFrozenEffect()
    {
        IsFrozen = false;
        FrozenGameObject.SetActive(false);
        MonsterAnimationManager.ResumeAnimation();
        ChangeNomalIdleState();
    }

    public void UpdateUIEffect()
    {
        UIEffectManager.ClearEffect();
        foreach (EffectSkill effect in MonsterStatsInBattle.EffectList)
        {
            GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
            effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
        }
    }

    private void UpdateSkillBar(PointPushData runePoints)
    {
        int point = MonsterIdInBattle[0] == '1' ? runePoints.player1[(int)MonsterPropsSO.MonsterData.Type] : runePoints.player2[(int)MonsterPropsSO.MonsterData.Type];
        point = Math.Clamp(point, 0, MonsterPropsSO.MonsterData.Skills[1].PointCost);
        // Debug.Log(gameObject.name +" UpdateSkillBar: " + point);
        UIHeathSkillBarManager.SetSkillBar(point);
    }

    public void EnableSkillMode()
    {
        if (!ShouldUseSkill)
        {
            ShouldUseSkill = true;
            ChangeSkillApperance();
        }
        else
        {
            ShouldUseSkill = false;
            ChangeNomalApperance();
        }
    }

    private void ChangeSkillApperance()
    {
        UIHeathSkillBarManager.SetSkillBar(0);
        GameManager.Instance.Match.MatchBoard.ReleaseRunePoint((RuneType)((int)MonsterPropsSO.MonsterData.Type),MonsterPropsSO.MonsterData.Skills[1].PointCost);
        MonsterAnimationManager.StartSkillEffect();
    }

    private void ChangeNomalApperance()
    {
        UIHeathSkillBarManager.SetSkillBar(MonsterPropsSO.MonsterData.Skills[1].PointCost);
        GameManager.Instance.Match.MatchBoard.AddRunePointByTpe((RuneType)((int)MonsterPropsSO.MonsterData.Type),MonsterPropsSO.MonsterData.Skills[1].PointCost);
        MonsterAnimationManager.EndSkillEffect();
    }

    public void EnableShield(int shieldAmount)
    {
        Debug.Log(gameObject.name + "EnableShield: " + shieldAmount);
        ShieldObject.SetActive(true);
        int shieldValue = (int)(MonsterPropsSO.MonsterData.BaseStats.Health * 0.0025f * shieldAmount);
        Debug.Log(gameObject.name +" enable shield: " + shieldValue);
        shield += shieldValue;
    }

    private void DisableShield()
    {
        ShieldObject.SetActive(false);
    }

    public void PlayHurtSound()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.HurtSound;
        audioSource.Play();
    }

    public void PlaySprintSound()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.SprintSound;
        audioSource.Play();
    }

    public void PlaySwordSound()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.SwordSound;
        audioSource.Play();
    }

    public void SetTurnObject(bool turn)
    {
        TurnObject.SetActive(turn);
    }
}
