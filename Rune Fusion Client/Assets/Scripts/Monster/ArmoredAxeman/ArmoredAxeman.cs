using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmoredAxeman: MonsterBase
{
        
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
        }
        public override void StartAttack(MonsterActionResponse monsterActionResponse)
        {
                base.StartAttack(monsterActionResponse);
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
                //walk
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                //attack
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

        public override void AttackInFrame()
        {
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
                        action.Key.StartHit(action.Value.dam,action.Value.effect);
                }
        }

        private Vector3 GetPosPerformSkill()
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
                return new Vector3(BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.x - offset
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.y
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[targetList[0].Key.MonsterIndexinBattle].position.z);
        }
}