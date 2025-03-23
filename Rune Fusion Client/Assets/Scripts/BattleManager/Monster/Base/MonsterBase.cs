using System;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    [field: SerializeField] public MonsterPropsSO MonsterPropsSO { get; private set; }
    [field: SerializeField] public MonsterStats MonsterStatsInBattle { get; private set; }

    [HideInInspector] public int MonsterIndexinBattle;
    public MonsterAnimationManager MonsterAnimationManager { get; private set; }
    public MonsterColliderManager MonsterColliderManager { get; private set; }
    protected StateMachine stateMachine;

    public Action AttackTaskComplete;
    public Action WalkTaskComplete;
    public Action SkillTaskComplete;
    
    public MonsterBase Target { get; protected set; }

    protected virtual void Awake()
    {
        MonsterAnimationManager = GetComponentInChildren<MonsterAnimationManager>();
        MonsterColliderManager = GetComponentInChildren<MonsterColliderManager>();
    }

    protected virtual void Start()
    {
        MonsterStatsInBattle = new MonsterStats
        {
            Attack = MonsterPropsSO.BaseStats.Attack,
            Defense = MonsterPropsSO.BaseStats.Defense,
            Health = MonsterPropsSO.BaseStats.Health,
            Speed = MonsterPropsSO.BaseStats.Speed,
            Accuracy = MonsterPropsSO.BaseStats.Accuracy,
            Resistance = MonsterPropsSO.BaseStats.Resistance,
        };
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public virtual void Attack(MonsterBase target)
    {
        
    }

    public virtual void Attack()
    {
        // Debug.Log(gameObject.name + " is attacking");
    }
}
