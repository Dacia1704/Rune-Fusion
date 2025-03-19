using UnityEngine;

public class ArcherAttackState: AttackState
{
    public ArcherAttackState(Archer monster) : base(monster)
    {
    }

    public override void Update()
    {
        base.Update();
        if (((Archer)monster).MonsterAnimationManager.IsAnimationEnded(monster.MonsterAnimationManager
                .AttackAnimationName))
        {
            ((Archer)monster).AttackTaskComplete?.Invoke();
        }
    }
}