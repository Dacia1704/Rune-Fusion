public class DeathState: State
{
    public DeathState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.DeathAnimationName,0);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}