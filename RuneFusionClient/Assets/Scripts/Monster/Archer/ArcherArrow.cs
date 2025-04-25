using DG.Tweening;
using UnityEngine;

public class ArcherArrow: FlyProjectile
{
    public override void FlyToPos(Transform target,int dam,EffectSkill effect)
    {
        if (target == null) return;
        audioSource.clip = AudioManager.Instance.AudioPropsSO.ArrowFlySound;
        audioSource.Play();
        transform.DOMove(target.position, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                target.GetComponent<MonsterBase>().StartHit(dam,effect);
                Destroy(gameObject);
            });
    }
}