using UnityEngine;

public class AttackState: State
{
    public AttackState(MonsterBase monster) : base(monster)
    {
    }

    public override void Enter()
    {
        monster.MonsterAnimationManager.PlayAnimation(monster.MonsterAnimationManager.AttackAnimationName);
    }

    public override void Update()
    {
        if (monster.MonsterAnimationManager.IsAnimationEnded(monster.MonsterAnimationManager
                .AttackAnimationName))
        {
            monster.AttackTaskComplete?.Invoke();
        }
    }

    public override void Exit()
    {
    }
}