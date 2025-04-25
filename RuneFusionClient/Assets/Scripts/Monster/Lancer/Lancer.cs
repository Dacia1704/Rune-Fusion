using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lancer: MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
        }
        protected override IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                //walk
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                PlaySprintSound();
                stateMachine.ChangeState(new WalkState(this, GetPosPerformAttack()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                //attack
                PlaySwordSound();
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
                MonsterAnimationManager.StartSkillEffect();
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                bool skillTaskCompleted = false;
                void SkillEventHandler() => skillTaskCompleted = true;
                SkillTaskComplete += SkillEventHandler;
                //walk
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                PlaySprintSound();
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                //skill
                PlaySwordSound();
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
        protected override Vector3 GetPosPerformAttack()
        {
                List<KeyValuePair<MonsterBase, ActionResponse>> targetList = CurrentTurnActionResponse.ToList();
                float offset = (MonsterPropsSO).AttackOffset * transform.lossyScale.x;
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(targetList[0].Key))
                {
                        return new Vector3(BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.x + offset
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.y
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.z);
                }
                return new Vector3(BattleManager.Instance.ArenaManager.MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.x - offset
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.y
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.z);
        }
        protected override Vector3 GetPosPerformSkill()
        {
                float offset = (MonsterPropsSO).AttackOffset * transform.lossyScale.x;
                if (BattleManager.Instance.MonsterTeam2Dictionary.ContainsValue(this))
                {
                        return new Vector3(BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[1].position.x + offset
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[1].position.y
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[1].position.z);
                }
                return new Vector3(BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[1].position.x - offset
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[1].position.y
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[1].position.z);
        }
}