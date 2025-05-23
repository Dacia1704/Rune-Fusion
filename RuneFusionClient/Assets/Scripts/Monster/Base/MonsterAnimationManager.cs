﻿using System;
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

        [SerializeField] private Animator skillEffect;

        public event Action OnAttack;
        public event Action OnSkill;

        protected virtual void Awake()
        {
                animator = GetComponent<Animator>();
                monster = GetComponentInParent<MonsterBase>();
                skillEffect.gameObject.SetActive(false);
        }

        public void StartSkillEffect()
        {
                skillEffect.gameObject.SetActive(true);
                skillEffect.Play("SkillStart");
        }

        public void EndSkillEffect()
        {
                skillEffect.gameObject.SetActive(false);
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

        public void PauseAnimation()
        {
                animator.speed = 0f;
        }

        public void ResumeAnimation()
        {
                animator.speed = 1f;
        }

        public void Atttack()
        {
                OnAttack?.Invoke();
        }

        public void Skill()
        {
                OnSkill?.Invoke();
        }

        public virtual void AttackInFrame()
        {
                monster.AttackInFrame();
        }

        public virtual void SkillInFrame()
        {
                monster.SkillInFrame();
        }
}