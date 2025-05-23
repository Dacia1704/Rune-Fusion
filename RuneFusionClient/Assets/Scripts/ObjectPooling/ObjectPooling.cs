﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;


public class ObjectPooling: MonoBehaviour
{
    protected Dictionary<string, ObjectPool<GameObject>> pools;

    [SerializeField] protected ObjectPoolPropsSO objectPoolProps;

    protected virtual void Awake()
    {
        pools = new Dictionary<string,ObjectPool<GameObject>>();
    }

    public GameObject GetObject(string keyObject)
    {
        if (pools.ContainsKey(keyObject))
        {
            return pools[keyObject].Get();
        }
        pools[keyObject] = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                PoolingObjectPropsSO objProps = objectPoolProps.PoolingObjectList.Find(obj => obj.KeyObject == keyObject);
                GameObject poolObject = Instantiate(objProps.ObjectPrefab, transform);
                poolObject.GetComponent<IPoolingObject>().PoolingObjectPropsSO = objProps;
                return poolObject;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: DestroyImmediate,
            collectionCheck: false,
            defaultCapacity: 30,
            maxSize: 50
        );
        return pools[keyObject].Get();
    }

    public void ReleaseObject(GameObject objectToRealse)
    {
        string keyObjectToRealse = objectToRealse.GetComponent<IPoolingObject>().PoolingObjectPropsSO.KeyObject;
        string keyObject = objectPoolProps.PoolingObjectList.Find(obj => obj.KeyObject == keyObjectToRealse).KeyObject;
        objectToRealse.transform.SetParent(transform);
        pools[keyObject].Release(objectToRealse);
    }
}