using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : ObjectPooling
{

        public GameObject GetObjectByEffect(EffectType effectType)
        {
                return GetObject(effectType.ToString());
        }
}