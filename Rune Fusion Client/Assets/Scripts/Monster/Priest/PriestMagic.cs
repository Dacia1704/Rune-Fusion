using System;
using DG.Tweening;
using UnityEngine;

public class PriestMagic: FlyProjectile
{
    private float height = 1f; // Độ cao của vòng cung
    private float duration = 0.75f; // Thời gian di chuyển

    private Vector3 prevPos;
    private Vector3 controlPoint;
    private Vector3 direction;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void FlyToPos(Transform target,int dam)
    {
        if (target == null) return;
        prevPos = transform.position;
        Vector3 startPosition = transform.position;
        controlPoint = (startPosition + target.position) / 2 + Vector3.up * height;
        if (startPosition.x > target.position.x)
        {
            controlPoint.x -= (startPosition.x - target.position.x)/6;
        }
        else
        {
            controlPoint.x += (target.position.x - startPosition.x)/6;
        }
        Vector3[] path = { controlPoint, new Vector3(target.position.x, target.position.y -0.1f, target.position.z) };

        transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.InFlash).OnUpdate(() =>
            {
                    direction = prevPos == transform.position ? (controlPoint - prevPos).normalized : (transform.position - prevPos).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle +90));
                    prevPos = transform.position;
            }).OnComplete(() =>
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                target.GetComponent<MonsterBase>().StartHit(dam);
                animator.Play("Bust");
            });
    }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }
}