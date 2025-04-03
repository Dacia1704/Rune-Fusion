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

    public Action AttackTaskComplete;
    public Action WalkTaskComplete;
    public Action SkillTaskComplete;
    public Action HitTaskComplete;
    
    public int Dam { get; protected set; }

    [HideInInspector] public bool IsAllAnimationEnd;

    protected UIHeathSkillBarManager UIHeathSkillBarManager { get; private set; }

    private TextMeshPro textMeshPro;
    
    public List<MonsterBase> TargetList { get; protected set; }
    
    public event Action<int> OnHealthChange;

    protected virtual void Awake()
    {
        MonsterAnimationManager = GetComponentInChildren<MonsterAnimationManager>();
        MonsterColliderManager = GetComponentInChildren<MonsterColliderManager>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        UIHeathSkillBarManager = GetComponentInChildren<UIHeathSkillBarManager>();
        OnHealthChange += UIHeathSkillBarManager.SetHealthBar;
        TargetList = new List<MonsterBase>();
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
    public virtual void StartHit(int dam)
    {
        Debug.Log(gameObject.name +" get Hit");
        stateMachine.ChangeState(new HurtState(this,dam));
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
    public void GetDam(int dam)
    {
        OnHealthChange?.Invoke(dam);
    }
}
