using System.Collections;
using UnityEngine;

public class Archer : MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += FireArrow;
        }

        public override void Attack(MonsterBase target)
        {
                Target = target;
                StartCoroutine(AttackCoroutine(target));
        }

        public IEnumerator AttackCoroutine(MonsterBase target)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                stateMachine.ChangeState(new WalkState(this, 
                        BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this) ? 
                                BattleManager.Instance.ArenaManager.MonsterTeam1.StartPosList[MonsterIndexinBattle].position:
                                BattleManager.Instance.ArenaManager.MonsterTeam2.StartPosList[MonsterIndexinBattle].position));
                yield return new WaitUntil(() => walkTaskCompleted);
                stateMachine.ChangeState(new IdleState(this));
        }

        private void FireArrow()
        {
                FlyProjectile arrow = Instantiate(((ArcherPropsSO)MonsterPropsSO).ArrowPrefab, transform.position, Quaternion.identity).GetComponent<FlyProjectile>();
                arrow.FlyToPos(Target.transform);
        }

        private Vector3 GetPosPerformSkill()
        {
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(Target))
                {
                        return BattleManager.Instance.ArenaManager.MonsterTeam2.PerformRangeSkillPosList[
                                Target.MonsterIndexinBattle].position;
                }
                return BattleManager.Instance.ArenaManager.MonsterTeam1.PerformRangeSkillPosList[
                                Target.MonsterIndexinBattle].position;
        }
}