using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Archer : MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += FireArrow;
                MonsterAnimationManager.OnSkill += FireArrow;
        }
        protected override IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                // walk
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new WalkState(this, GetPosPerformAttack()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                // attack 3 times continuous
                stateMachine.ChangeState(new AttackState(this));
                
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,1);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[1])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,2);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[2])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                stateMachine.ChangeState(new WalkState(this, 
                        BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this) ? 
                                BattleManager.Instance.ArenaManager.MonsterTeam1.StartPosList[MonsterIndexinBattle].position:
                                BattleManager.Instance.ArenaManager.MonsterTeam2.StartPosList[MonsterIndexinBattle].position));
                yield return new WaitUntil(() => walkTaskCompleted);
                ChangeNomalIdleState();
        }
        
        protected override IEnumerator SkillCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                
                bool skillTaskCompleted = false;
                void SkillEventHandler() => skillTaskCompleted = true;
                SkillTaskComplete += SkillEventHandler;
                // walk
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                // skill
                stateMachine.ChangeState(new SkillState(this));
                
                yield return new WaitUntil(() => skillTaskCompleted);
                skillTaskCompleted = false;
                
                stateMachine.ChangeState(new WalkState(this, 
                        BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this) ? 
                                BattleManager.Instance.ArenaManager.MonsterTeam1.StartPosList[MonsterIndexinBattle].position:
                                BattleManager.Instance.ArenaManager.MonsterTeam2.StartPosList[MonsterIndexinBattle].position));
                yield return new WaitUntil(() => walkTaskCompleted);
                ChangeNomalIdleState();
        }

        private void FireArrow()
        {
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
                        ArcherArrow arrow = Instantiate(((ArcherPropsSO)MonsterPropsSO).ArrowPrefab, transform.position, Quaternion.identity).GetComponent<ArcherArrow>();
                        arrow.FlyToPos(action.Key.transform,action.Value.dam,action.Value.effect);
                }
        }

        protected override Vector3 GetPosPerformAttack()
        {
                List<KeyValuePair<MonsterBase, ActionResponse>> targetList = CurrentTurnActionResponse.ToList();
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(targetList[0].Key))
                {
                        return BattleManager.Instance.ArenaManager.MonsterTeam2.PerformRangeSkillPosList[
                                targetList[0].Key.MonsterIndexinBattle].position;
                }
                return BattleManager.Instance.ArenaManager.MonsterTeam1.PerformRangeSkillPosList[
                                targetList[0].Key.MonsterIndexinBattle].position;
        }
        
        protected override Vector3 GetPosPerformSkill()
        {
                List<KeyValuePair<MonsterBase, ActionResponse>> targetList = CurrentTurnActionResponse.ToList();
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(targetList[0].Key))
                {
                        return BattleManager.Instance.ArenaManager.MonsterTeam2.PerformRangeSkillPosList[
                                targetList[0].Key.MonsterIndexinBattle].position;
                }
                return BattleManager.Instance.ArenaManager.MonsterTeam1.PerformRangeSkillPosList[
                        targetList[0].Key.MonsterIndexinBattle].position;
        }
}