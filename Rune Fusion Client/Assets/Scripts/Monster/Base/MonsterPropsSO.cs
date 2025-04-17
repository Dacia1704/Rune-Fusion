using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MonsterPropsSO", menuName = "MonsterPropsSO", order = 0)]
public class MonsterPropsSO : ScriptableObject
{
        public MonsterData MonsterData;
        public Sprite Icon;
        public AnimatorController ModelAnimatorController;
        public float AttackOffset;
}