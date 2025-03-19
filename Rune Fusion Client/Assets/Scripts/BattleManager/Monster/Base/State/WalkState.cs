using DG.Tweening;
using UnityEngine;

public class WalkState: State
{
    protected Transform target;
    public WalkState(MonsterBase monster,Transform target) : base(monster)
    {
        this.target = target;
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.WalkAnimationName);
        monster.transform.DOMove(this.target.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            monster.WalkTaskComplete?.Invoke();
        });
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}