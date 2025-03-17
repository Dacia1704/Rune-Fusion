public class HurtState: State
{
    public HurtState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.HurtAnimationnName);
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}