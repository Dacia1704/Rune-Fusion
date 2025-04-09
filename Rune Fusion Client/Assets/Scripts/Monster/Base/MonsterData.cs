
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class MonsterData
{
        [field: SerializeField] public MonsterId Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public MonsterType Type { get; private set; }
        [field: SerializeField] public MonsterStats BaseStats { get; private set; }
        [field: SerializeField] public List<MonsterSkill> Skills { get; private set; }
}

[Serializable]
public class MonsterStats
{
        [JsonProperty("attack")]
        public int Attack;
        [JsonProperty("defend")]
        public int Defend;
        [JsonProperty("health")]
        public int Health;
        [JsonProperty("speed")]
        public int Speed;
        [JsonProperty("accuracy")]
        public float Accuracy;
        [JsonProperty("resistance")]
        public float Resistance;
        [JsonProperty("effect_list")]
        public List<EffectSkill> EffectList;
        public override string ToString()
        {
                return $"Atk: {Attack}, Def: {Defend}, Hp: {Health}, Spd: {Speed}, Acc: {Accuracy}, Res: {Resistance}";
        }
}

[Serializable]
public class MonsterSkill
{
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("description")]
        public string Description;
        [JsonProperty("action_list")]
        public List<Skill> ActionList;
        [JsonIgnore]
        public Sprite Icon;
}

[Serializable]
public class Skill
{
        [JsonProperty("target_type")]
        public SkillTargetType TargetType;
        [JsonProperty("area_effect")]
        public SkillArea AreaEffect;
        [JsonProperty("random_type")]
        public SkillRandomType RandomType;
        
        // Case: can deal dam skill
        [JsonProperty("percent_attack")]
        public float PercentAttack;  // percent of attack to apply 
        [JsonProperty("penetration")]
        public float Penetration;
        // Case: can deal effect skill
        [JsonProperty("percent_health")]
        public float PercentHealth; // if heal
        [JsonProperty("effect_skill")]
        public EffectSkill EffectSkill;
        [JsonProperty("effectiveness")]
        public float Effectiveness;
        
}
[Serializable]
public class EffectSkill
{
        [JsonProperty("effect_type")]
        public EffectType EffectType;
        [JsonProperty("duration")] 
        public int duration;
}


public enum SkillTargetType
{
        Ally,
        Opponent,
        Self,
}

public enum SkillArea
{
        All,
        Single,
        Random,
}

public enum SkillRandomType
{
        None,
        HighestHp,
        LowestHp,
}

public enum EffectType
{
        None,
        Heal,
        Burn, //5%
        SpeedIncrement, //25%
        AttackIncrement, //50%
        DefendIncrement, //50%
        SpeedDecrement, //25%
        AttackDecrement, //25%
        DefendDecrement, //50%
        Taunt,
        Frozen
}
public enum MonsterId
{
        Archer=0,
        ArmoredAxeman,
        Knight,
        Lancer,
        Priest,
        Wizard,
}
public enum MonsterType
{
        PhysicAttack=0, // big dam
        MagicAttack, // medium dame, aoe attack, land debuff
        Health, //small dame, tank, sp
        Defend, //small dame, tank, sp
}