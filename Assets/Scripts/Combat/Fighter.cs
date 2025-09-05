using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private readonly int hashAttack = Animator.StringToHash("Attack");
        private readonly int hashStopAttack = Animator.StringToHash("stopAttack");

        Mover mover;
        Health target;
        ActionScheduler actionScheduler;
        Animator animator;
        Health health;
        private float weaponRange = 2f;
        private float runSpeed = 5.5f;

        private float lastAttackTime = 1f;
        private float attackCoolTime = 1f;


        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }
        private void Update()
        {
            lastAttackTime += Time.deltaTime;
            if (target == null) return;
            if (target.Dead)
            {
                Cancel();
                return;
            }
            if (health.Dead) return;

            if (!GetRange())
            {
                mover.MoveTo(target.transform.position, runSpeed);
            }
            else
            {
                mover.Stop();
                AttackBehavior();
            }
        }

        private bool GetRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }
        public void Attack(GameObject combat)
        {
            actionScheduler.StartAction(this);
            target = combat.GetComponent<Health>();
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            var dir = (target.transform.position - transform.position);
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f);

            if (lastAttackTime > attackCoolTime)
            {
                lastAttackTime = 0f;
                TriggerAttack();
            }
        }

        public void Hit()
        {
            if (target == null) return;
            target.TakeDamage(10f);
        }
        public void TriggerAttack()
        {
            animator.SetTrigger(hashAttack);
            animator.ResetTrigger(hashStopAttack);
        }
        public void StopAttack()
        {
            animator.ResetTrigger(hashAttack);
            animator.SetTrigger(hashStopAttack);
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        public bool CanAttack(GameObject combat)
        {
            if (combat == null) return false;
            var target = combat.GetComponent<Health>();
            return target != null && !target.Dead;
        }
    }

}

