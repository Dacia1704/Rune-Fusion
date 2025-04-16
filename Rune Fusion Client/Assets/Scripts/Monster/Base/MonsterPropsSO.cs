using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterPropsSO", menuName = "MonsterPropsSO", order = 0)]
public class MonsterPropsSO : ScriptableObject
{
        public MonsterData MonsterData;
        public Sprite Icon;
        public Animator ModelAnimator;
        public float AttackOffset;
}