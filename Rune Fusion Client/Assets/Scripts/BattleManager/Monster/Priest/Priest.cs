using System.Collections;
using UnityEngine;

public class Priest: MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
        }
        public override void Attack(MonsterBase target)
        {
                Target = target;
                StartCoroutine(AttackCoroutine(target));
                MonsterAnimationManager.OnAttack += FireMagic;
        }

        public IEnumerator AttackCoroutine(MonsterBase target)
        {
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                stateMachine.ChangeState(new AttackState(this));
                yield return new WaitUntil(() => attackTaskCompleted);
                attackTaskCompleted = false;
                stateMachine.ChangeState(new IdleState(this));
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
                magic.FlyToPos(Target.transform);
        }
}