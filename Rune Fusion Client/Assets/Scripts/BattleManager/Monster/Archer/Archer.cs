using UnityEngine;

public class Archer : MonsterBase
{
        protected override void Start()
        {
                base.Start();
                stateMachine.ChangeState(new IdleState(this));
        }

        public override void Attack(Transform target)
        {
                FlyProjectile arrow = Instantiate(((ArcherPropsSO)MonsterPropsSO).ArrowPrefab, transform.position, Quaternion.identity).GetComponent<FlyProjectile>();
                arrow.FlyToPos(target);
        }
}