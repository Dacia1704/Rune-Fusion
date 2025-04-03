public class HurtState: State
{
    private int dam;
    public HurtState(MonsterBase monster,int dam) : base(monster)
    {
        this.dam = dam;
    }

    public override void Enter()
    {
        monster.GetDam(dam);
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.HurtAnimationnName);
    }

    public override void Update()
    {
        if (monster.MonsterAnimationManager.IsAnimationEnded(monster.MonsterAnimationManager
                .HurtAnimationnName))
        {
            monster.HitTaskComplete?.Invoke();
        }
    }

    public override void Exit()
    {
    }
}