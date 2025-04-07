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
    [HideInInspector] public List<EffectSkill> EffectList;
    public Dictionary<MonsterBase, ActionResponse> CurrentTurnActionResponse;

    public Action AttackTaskComplete;
    public Action WalkTaskComplete;
    public Action SkillTaskComplete;
    public Action HitTaskComplete;


    public bool IsAllAnimationEnd;

    protected UIHeathSkillBarManager UIHeathSkillBarManager { get; private set; }

    private TextMeshPro textMeshPro;
    
    public Action<int> OnHealthChange;

    protected virtual void Awake()
    {
        MonsterAnimationManager = GetComponentInChildren<MonsterAnimationManager>();
        MonsterColliderManager = GetComponentInChildren<MonsterColliderManager>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        UIHeathSkillBarManager = GetComponentInChildren<UIHeathSkillBarManager>();
        UIEffectManager = GetComponentInChildren<UIEffectManager>();
        EffectList = new List<EffectSkill>();
        CurrentTurnActionResponse = new Dictionary<MonsterBase, ActionResponse>();
        OnHealthChange += UIHeathSkillBarManager.SetHealthBar;
    }

    protected virtual void Start()
    {
        MonsterStatsInBattle = new MonsterStats
        {
            Attack = MonsterPropsSO.MonsterData.BaseStats.Attack,
            Defense = MonsterPropsSO.MonsterData.BaseStats.Defense,
            Health = MonsterPropsSO.MonsterData.BaseStats.Health,
            Speed = MonsterPropsSO.MonsterData.BaseStats.Speed,
            Accuracy = MonsterPropsSO.MonsterData.BaseStats.Accuracy,
            Resistance = MonsterPropsSO.MonsterData.BaseStats.Resistance,
        };
        stateMachine = new StateMachine();
        UIHeathSkillBarManager.SetMaxHealthBar(MonsterStatsInBattle.Health);
        UIHeathSkillBarManager.SetHealthBar(MonsterStatsInBattle.Health);
        IsAllAnimationEnd = false;
        HitTaskComplete += ChangeNomalIdleState;
    }
    protected virtual void Update()
    {
        textMeshPro.text = MonsterStatsInBattle.ToString();
        stateMachine.Update();
    }
    public virtual void StartAttack(MonsterActionResponse monsterActionResponse)
    {
    }
    public virtual void AttackInFrame()
    {
        
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
    public void GetDam(int dam,EffectSkill effect)
    {
        MonsterStatsInBattle.Health -= dam;
        if (effect.EffectType != EffectType.None && effect.EffectType != EffectType.Heal)
        {
            if (!EffectList.Contains(effect) || effect.EffectType == EffectType.Burn)
            {
                EffectList.Add(effect);
                GameObject effectObj = UIEffectManager.GetObjectByEffect(effect.EffectType);
                effectObj.GetComponent<UIEffect>().SetDuration(effect.duration);
            }
        }
        OnHealthChange?.Invoke(MonsterStatsInBattle.Health);
    }
}
