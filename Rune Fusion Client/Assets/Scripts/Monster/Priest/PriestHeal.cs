using System;
using UnityEngine;

public class PriestHeal: MonoBehaviour
{
        private Priest priest;
        private MonsterBase monsterTarget;
        public void Destroy()
        {
                if (monsterTarget != priest)
                {
                        monsterTarget.IsAllAnimationEnd = true;
                }
                priest.HealEffects.Remove(this.gameObject);
                Destroy(this.gameObject);
        }

        public void Heal(Transform target, int healAmount, Priest pri)
        {
                priest = pri;
                transform.position = new Vector3(target.position.x, target.position.y - 0.2f, target.position.z);
                monsterTarget = target.GetComponent<MonsterBase>();
                monsterTarget.GetDam(healAmount);
        }
}