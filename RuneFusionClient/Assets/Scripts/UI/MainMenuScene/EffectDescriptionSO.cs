using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDescriptionSO", menuName = "EffectDescriptionSO", order = 0)]
public class EffectDescriptionSO : ScriptableObject
{
        public List<EffectDesData> EffectDescriptionList;
}

[Serializable]
public class EffectDesData
{
        public EffectType EffectType;
        public string NameEffect;
        public string DescriptionEffect;
        public Sprite SpriteEffect;
}