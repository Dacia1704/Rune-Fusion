using System;
using System.Collections.Generic;
using UnityEngine;

// props of pool 
[CreateAssetMenu(fileName = "ObjectPoolProps", menuName = "ObjectPoolProps")]
public class ObjectPoolPropsSO : ScriptableObject
{
    [field: SerializeField] public List<PoolingObjectPropsSO> PoolingObjectList { get; private set; }
}