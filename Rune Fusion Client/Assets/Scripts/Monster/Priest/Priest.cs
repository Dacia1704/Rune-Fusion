using System.Collections;
using UnityEngine;

public class Priest: MonsterBase
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
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                
                stateMachine.ChangeState(new AttackState(this));
                Dam = monsterActionResponse.action_affect_list[0][0].dam;
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                ChangeNomalIdleState();
        }

        public void FireMagic()
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
                magic.FlyToPos(TargetList[0].transform,1);
        }
}