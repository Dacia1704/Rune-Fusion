using System;
using System.Collections;
using UnityEngine;

public class RuneDestroyEffect: MonoBehaviour, IPoolingObject
{
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }

    private void OnEnable()
    {
        StartCoroutine(AutoReturnToPool());
    }

    private IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(3);
        GameManager.Instance.RuneManager.RuneObjectPoolManager.ReleaseDestroyEffect(this.gameObject);
    }
}