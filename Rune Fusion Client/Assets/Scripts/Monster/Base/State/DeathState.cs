public class DeathState: State
{
    public DeathState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.DeathAnimationName);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}