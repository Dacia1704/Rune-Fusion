using DG.Tweening;
using UnityEngine;

public class FlyProjectile : MonoBehaviour
{
        public virtual void FlyToPos(Transform target)
        {
                if (target == null) return;
                transform.DOMove(target.position, 0.3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => Destroy(gameObject));
        }
}