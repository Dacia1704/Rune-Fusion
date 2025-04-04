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

        public override void StartAttack(MonsterActionResponse monsterActionResponse)
        {
                foreach (string id in monsterActionResponse.monster_target_id)
                {
                       TargetList.Add(BattleManager.Instance.GetMonsterByIdInBattle(id)); 
                }
                StartCoroutine(AttackCoroutine(monsterActionResponse));
        }

        public IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                // walk
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                
                // attack 3 times continuous
                stateMachine.ChangeState(new AttackState(this));
                Dam = monsterActionResponse.action_affect_list[0][0].dam;
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                stateMachine.ChangeState(new AttackState(this));
                Dam = monsterActionResponse.action_affect_list[1][0].dam;
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                stateMachine.ChangeState(new AttackState(this));
                Dam = monsterActionResponse.action_affect_list[2][0].dam;
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                stateMachine.ChangeState(new WalkState(this, 
                        BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this) ? 
                                BattleManager.Instance.ArenaManager.MonsterTeam1.StartPosList[MonsterIndexinBattle].position:
                                BattleManager.Instance.ArenaManager.MonsterTeam2.StartPosList[MonsterIndexinBattle].position));
                yield return new WaitUntil(() => walkTaskCompleted);
                ChangeNomalIdleState();
        }

        private void FireArrow()
        {
                ArcherArrow arrow = Instantiate(((ArcherPropsSO)MonsterPropsSO).ArrowPrefab, transform.position, Quaternion.identity).GetComponent<ArcherArrow>();
                arrow.FlyToPos(TargetList[0].transform,Dam);
        }

        private Vector3 GetPosPerformSkill()
        {
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(TargetList[0]))
                {
                        return BattleManager.Instance.ArenaManager.MonsterTeam2.PerformRangeSkillPosList[
                                TargetList[0].MonsterIndexinBattle].position;
                }
                return BattleManager.Instance.ArenaManager.MonsterTeam1.PerformRangeSkillPosList[
                                TargetList[0].MonsterIndexinBattle].position;
        }
}