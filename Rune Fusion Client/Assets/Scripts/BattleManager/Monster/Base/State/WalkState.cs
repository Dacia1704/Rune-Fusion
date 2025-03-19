using DG.Tweening;
using UnityEngine;

public class WalkState: State
{
    protected Vector3 target;
    public WalkState(MonsterBase monster,Vector3 target) : base(monster)
    {
        this.target = target;
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.WalkAnimationName);
        monster.transform.DOMove(this.target, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() =>
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