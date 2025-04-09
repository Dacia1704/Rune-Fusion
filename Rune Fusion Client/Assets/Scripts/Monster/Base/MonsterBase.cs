using System;
using System.Collections;
using System.Collections.Generic;
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

    protected UIEffectManager UIEffectManager;
    public Dictionary<MonsterBase, ActionResponse> CurrentTurnActionResponse;

    public Action AttackTaskComplete;
    public Action WalkTaskComplete;
    public Action SkillTaskComplete;
    public Action HitTaskComplete;


    public bool IsAllAnimationEnd;
    public bool IsUpdateEffect;
    protected bool IsFrozen;
    [SerializeField]protected GameObject FrozenGameObject;

    protected UIHeathSkillBarManager UIHeathSkillBarManager { get; private set; }

    // private TextMeshPro textMeshPro;
    
    public Action<int> OnHealthChange;

    protected virtual void Awake()
    {
        MonsterAnimationManager = GetComponentInChildren<MonsterAnimationManager>();
        MonsterColliderManager = GetComponentInChildren<MonsterColliderManager>();
        // textMeshPro = GetComponentInChildren<TextMeshPro>();
        UIHeathSkillBarManager = GetComponentInChildren<UIHeathSkillBarManager>();
        UIEffectManager = GetComponentInChildren<UIEffectManager>();
        CurrentTurnActionResponse = new Dictionary<MonsterBase, ActionResponse>();
        OnHealthChange += UIHeathSkillBarManager.SetHealthBar;
    }

    protected virtual void Start()
    {
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
        stateMachine = new StateMachine();
        UIHeathSkillBarManager.SetMaxHealthBar(MonsterStatsInBattle.Health);
        UIHeathSkillBarManager.SetHealthBar(MonsterStatsInBattle.Health);
        IsAllAnimationEnd = false;
        IsUpdateEffect = false;
        IsFrozen = false;
        HitTaskComplete += ChangeNomalIdleState;
    }
    protected virtual void Update()
    {
        // textMeshPro.text = MonsterStatsInBattle.ToString();
        stateMachine.Update();
    }
    public virtual void StartAttack(MonsterActionResponse monsterActionResponse)
    {
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
        Debug.Log(gameObject.name +" get Hit");
        BattleManager.Instance.TargetManager.DisableTarget();
        stateMachine.ChangeState(new HurtState(this,dam,effect));
    }

    protected void ChangeNomalIdleState()
    {
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
        MonsterStatsInBattle.Health -= dam;
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
        OnHealthChange?.Invoke(MonsterStatsInBattle.Health);
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
        if (dam > 0)
        {
            Debug.Log(1);
            StartHit(dam,null);
        }
        else
        {
            Debug.Log(2);
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
        }

        UpdateUIEffect();
        IsUpdateEffect = true;
    }

    public void EnableFrozenEffect()
    {
        IsFrozen = true;
        FrozenGameObject.SetActive(true);
        FrozenGameObject.GetComponent<Animator>().Play("Frozen");
    }

    public void DisableFrozenEffect()
    {
        IsFrozen = false;
        FrozenGameObject.SetActive(false);
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
}
