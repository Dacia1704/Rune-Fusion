public class DeathState: State
{
    public DeathState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.DeathAnimationName,0);
        monster.UIHeathSkillBarManager.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (monster.MonsterAnimationManager.IsAnimationEnded(monster.MonsterAnimationManager
                .DeathAnimationName))
        {
            monster.MonsterColliderManager.gameObject.SetActive(false);
            monster.IsAllAnimationEnd = true;
        }
    }

    public override void Exit()
    {
    }
}