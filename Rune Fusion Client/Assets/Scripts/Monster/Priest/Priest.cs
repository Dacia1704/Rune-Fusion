using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Priest: MonsterBase
{
        public List<GameObject> HealEffects = new List<GameObject>();
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += FireMagic;
                MonsterAnimationManager.OnSkill += HealEffect;
        }

        protected override IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                
                Debug.Log("Priest attack");
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,1);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[1])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                HealEffect();
                yield return new WaitUntil(() => HealEffects.Count == 0);
                ChangeNomalIdleState();
        }
                
        protected override IEnumerator SkillCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool skillTaskCompleted = false;
                void SkillEventHandler() => skillTaskCompleted = true;
                SkillTaskComplete += SkillEventHandler;
                
                Debug.Log("Priest skill");
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new SkillState(this));
                yield return new WaitUntil(() => skillTaskCompleted);
                skillTaskCompleted = false;
                yield return new WaitUntil(() => HealEffects.Count == 0);
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

        public void HealEffect()
        {
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
                        PriestHeal healObj = Instantiate(((PriestPropsSO)MonsterPropsSO).PriestHealPrefab).GetComponent<PriestHeal>();
                        healObj.Heal(action.Key.transform, action.Value.dam,this);
                        HealEffects.Add(healObj.gameObject);
                }  
        }
}