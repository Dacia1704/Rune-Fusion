using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MonsterAnimationManager : MonoBehaviour
{
        [HideInInspector] public string IdleAnimationName{get;private set;} = "Idle";
        [HideInInspector] public string WalkAnimationName{get;private set;} = "Walk";
        [HideInInspector] public string AttackAnimationName{get;private set;} = "Attack";
        [HideInInspector] public string SkillAnimationName{get;private set;} = "Skill";
        [HideInInspector] public string HurtAnimationnName{get;private set;} = "Hurt";
        [HideInInspector] public string DeathAnimationName{get;private set;} = "Death";

        private Animator animator;
        private MonsterBase monster;

        public event Action OnAttack; 

        protected virtual void Awake()
        {
                animator = GetComponent<Animator>();
                monster = GetComponentInParent<MonsterBase>();
        }

        public virtual void PlayAnimation(string nameAnimation, float normalizedTime,int layer = 0 )
        {
                animator.Play(nameAnimation,layer, normalizedTime);
        }
        
        public bool IsAnimationEnded(string animationName,int layerIndex=0)
        {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

                if (stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f)
                {
                        return true;
                }

                return false;
        }

        public void Atttack()
        {
                OnAttack?.Invoke();
        }

        public virtual void AttackInFrame()
        {
                monster.AttackInFrame();
        }
}