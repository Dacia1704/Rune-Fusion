using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Priest: MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += FireMagic;
        }
        public override void StartAttack(MonsterActionResponse monsterActionResponse)
        {
                base.StartAttack(monsterActionResponse);
                StartCoroutine(AttackCoroutine(monsterActionResponse));
        }

        public IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                
                Debug.Log("Priest attack");
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[1])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new IdleState(this));
                Heal();
                ChangeNomalIdleState();
        }

        public void FireMagic()
        {
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
                        PriestMagic magic = Instantiate(((PriestPropsSO)MonsterPropsSO).PriestMagicPrefab, transform.position, Quaternion.identity).GetComponent<PriestMagic>();
                        if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this))
                        {
                                magic.transform.position = new Vector3(
                                        transform.position.x + ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.x,
                                        transform.position.y + ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.y,
                                        transform.position.z + ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.z);
                        }
                        else
                        {
                                magic.transform.position = new Vector3(
                                        transform.position.x - ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.x,
                                        transform.position.y + ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.y,
                                        transform.position.z + ((PriestPropsSO)MonsterPropsSO).PriestMagicSummonOffset.z);
                        }
                        magic.FlyToPos(action.Key.transform,action.Value.dam,action.Value.effect);
                }
        }

        public void Heal()
        {
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
                        action.Key.MonsterStatsInBattle.Health -= action.Value.dam;
                        OnHealthChange?.Invoke(action.Key.MonsterStatsInBattle.Health);
                }  
        }
}