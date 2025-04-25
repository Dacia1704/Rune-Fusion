public class HurtState: State
{
    private int dam;
    private EffectSkill effect;
    public HurtState(MonsterBase monster,int dam,EffectSkill effect) : base(monster)
    {
        this.dam = dam;
        this.effect = effect;
    }

    public override void Enter()
    {
        monster.GetDam(dam,effect);
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.HurtAnimationnName,0);
        monster.PlayHurtSound();
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