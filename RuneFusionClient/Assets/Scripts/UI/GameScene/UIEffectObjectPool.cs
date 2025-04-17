using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectManager : ObjectPooling
{

        public GameObject GetObjectByEffect(EffectType effectType)
        {
                return GetObject(effectType.ToString());
        }

        public void ClearEffect()
        {
                foreach (Transform child in transform)
                {
                        ReleaseObject(child.gameObject);
                }
        }
}