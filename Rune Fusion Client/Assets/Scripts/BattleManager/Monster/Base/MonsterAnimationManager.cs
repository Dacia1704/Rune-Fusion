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

        private Animator Animator;

        protected virtual void Awake()
        {
                Animator = GetComponent<Animator>();
        }

        public virtual void PlayAnimation(string nameAnimation)
        {
                Animator.Play(nameAnimation);
        }
}