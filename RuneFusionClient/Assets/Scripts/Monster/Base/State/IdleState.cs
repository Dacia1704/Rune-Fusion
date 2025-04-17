public class IdleState: State
{
    public IdleState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.IdleAnimationName,0);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}