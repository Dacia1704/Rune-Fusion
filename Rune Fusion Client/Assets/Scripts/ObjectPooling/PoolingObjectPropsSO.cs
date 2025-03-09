using UnityEngine;

[CreateAssetMenu(fileName = "PoolingObjectPropsSO", menuName = "PoolingObjectPropsSO")]
// props of object to pool
public class PoolingObjectPropsSO : ScriptableObject
{
    public string KeyObject;
    public GameObject ObjectPrefab;
}