public class SkillState: State
{
    public SkillState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.SkillAnimationName,0);
    }

    public override void Update()
    {
        if (monster.MonsterAnimationManager.IsAnimationEnded(monster.MonsterAnimationManager
                .SkillAnimationName))
        {
            monster.SkillTaskComplete?.Invoke();
        }
    }

    public override void Exit()
    {
        
    }
}