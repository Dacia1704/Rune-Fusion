using System.Collections;
using UnityEngine;

public class Wizard: MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += ShootFireBall;
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
                
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
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

        private void ShootFireBall()
        {
                Quaternion fireBallRotation = BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this)
                        ? Quaternion.identity
                        : Quaternion.Euler(0, 180, 0);
                FlyProjectile fireBall = Instantiate(((WizardPropsSO)MonsterPropsSO).WizardFireBallPrefab, transform.position, fireBallRotation).GetComponent<FlyProjectile>();
                if (BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this))
                {
                        fireBall.transform.position = new Vector3(
                                transform.position.x + ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.x,
                                transform.position.y + ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.y,
                                transform.position.z + ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.z);
                }
                else
                {
                        fireBall.transform.position = new Vector3(
                                transform.position.x - ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.x,
                                transform.position.y + ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.y,
                                transform.position.z + ((WizardPropsSO)MonsterPropsSO).WizardFireBallSummonOffset.z);
                }
                fireBall.FlyToPos(TargetList[0].transform,1);
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