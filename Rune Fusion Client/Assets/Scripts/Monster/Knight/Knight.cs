using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight: MonsterBase
{
         protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
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
                //walk
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
                //attack
                stateMachine.ChangeState(new AttackState(this));
                Dam = monsterActionResponse.action_affect_list[0][0].dam;
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
                foreach (MonsterBase monter in TargetList)
                {
                        monter.StartHit(Dam);
                }
        }

        private Vector3 GetPosPerformSkill()
        {
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(TargetList[0]))
                {
                        return new Vector3(BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[TargetList[0].MonsterIndexinBattle].position.x + (MonsterPropsSO).AttackOffset
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[TargetList[0].MonsterIndexinBattle].position.y
                                , BattleManager.Instance.ArenaManager
                                        .MonsterTeam1.StartPosList[TargetList[0].MonsterIndexinBattle].position.z);
                }
                return new Vector3(BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[TargetList[0].MonsterIndexinBattle].position.x - (MonsterPropsSO).AttackOffset
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[TargetList[0].MonsterIndexinBattle].position.y
                        , BattleManager.Instance.ArenaManager
                                .MonsterTeam2.StartPosList[TargetList[0].MonsterIndexinBattle].position.z);
        }
}