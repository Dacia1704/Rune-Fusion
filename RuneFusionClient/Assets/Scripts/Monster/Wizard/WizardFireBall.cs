﻿using DG.Tweening;
using UnityEngine;

public class WizardFireBall: FlyProjectile
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void FlyToPos(Transform target,int dam,EffectSkill effect)
    {
        if (target == null) return;
        transform.DOMove(target.position, 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.Play("Bust");
                target.GetComponent<MonsterBase>().StartHit(dam, effect);
            });
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}