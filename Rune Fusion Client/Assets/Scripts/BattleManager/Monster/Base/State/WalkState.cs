public class WalkState: State
{
    public WalkState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.WalkAnimationName);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}