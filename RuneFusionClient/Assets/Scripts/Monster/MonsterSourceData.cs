using System;
using UnityEngine;

[Serializable]
public class MonsterSourceData
{
    public MonsterId Id;
    public GameObject Prefab;
    public MonsterPropsSO MonsterProps;
    public bool IsOwn { get; private set; }

    public void SetOwn(bool isOwn)
    {
        this.IsOwn = isOwn;
    }
}