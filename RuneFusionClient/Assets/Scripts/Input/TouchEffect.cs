using System.Collections;
using UnityEngine;

public class TouchEffect: MonoBehaviour,IPoolingObject
{
    public PoolingObjectPropsSO PoolingObjectPropsSO { get; set; }

    private void OnEnable()
    {
        StartCoroutine(AutoReturnToPool());
    }

    private IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(2);
        TouchEffectManager.Instance.ReleaseEffect(this.gameObject);
    }
    
}