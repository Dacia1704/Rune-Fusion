using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wizard: MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
                MonsterAnimationManager.OnAttack += ShootFireBall;
                MonsterAnimationManager.OnSkill += CreateExplosiveFrozen;
        }
        protected override IEnumerator AttackCoroutine(MonsterActionResponse monsterActionResponse)
        {
                bool walkTaskCompleted = false;
                void WalkEventHandler() => walkTaskCompleted = true;
                WalkTaskComplete += WalkEventHandler;
                
                bool attackTaskCompleted = false;
                void AttackEventHandler() => attackTaskCompleted = true;
                AttackTaskComplete += AttackEventHandler;
                
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new WalkState(this, GetPosPerformAttack()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
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
                
                GameManager.Instance.BattleManager.SetStartTurnMonsterAnimation(monsterActionResponse,0);
                CurrentTurnActionResponse.Clear();
                foreach (ActionResponse actionResponseInEachMonster in monsterActionResponse.action_affect_list[0])
                {
                        CurrentTurnActionResponse.Add(BattleManager.Instance.GetMonsterByIdInBattle(actionResponseInEachMonster.id_in_battle), actionResponseInEachMonster);
                }
                stateMachine.ChangeState(new WalkState(this, GetPosPerformSkill()));
                yield return new WaitUntil(() => walkTaskCompleted);
                walkTaskCompleted = false;
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

        private void ShootFireBall()
        {
                Quaternion fireBallRotation = BattleManager.Instance.MonsterTeam1Dictionary.ContainsValue(this)
                        ? Quaternion.identity
                        : Quaternion.Euler(0, 180, 0);
                foreach (KeyValuePair<MonsterBase,ActionResponse> action in CurrentTurnActionResponse)
                {
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
                        fireBall.FlyToPos(action.Key.transform,action.Value.dam, action.Value.effect);
                }
        }

        private void CreateExplosiveFrozen()
        {
                WizardFrozen frozen = Instantiate(((WizardPropsSO)MonsterPropsSO).WizardFrozenPrefab).GetComponent<WizardFrozen>();
                if (MonsterIdInBattle[0] == '1')
                {
                        frozen.transform.position = GameManager.Instance.BattleManager.MonsterTeam1Dictionary["22"].transform.position;
                }
                else
                {
                        frozen.transform.position = GameManager.Instance.BattleManager.MonsterTeam1Dictionary["12"].transform.position;
                }
                frozen.SetUpFrozen(CurrentTurnActionResponse);
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
                if (BattleManager.Instance.MonsterTeam2Dictionary.ContainsValue(this))
                {
                        return BattleManager.Instance.ArenaManager.MonsterTeam2.PerformRangeSkillPosList[1].position;
                }
                return BattleManager.Instance.ArenaManager.MonsterTeam1.PerformRangeSkillPosList[1].position;
        }
}