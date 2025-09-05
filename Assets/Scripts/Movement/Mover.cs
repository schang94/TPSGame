using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private readonly int hashMove = Animator.StringToHash("ForwardSpeed");

        Animator animator;
        Fighter fighter;
        ActionScheduler actionScheduler;
        NavMeshAgent agent;

        private float movedampTime = 0.01f;
        private float moveSpeed = 3.5f;

        private void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void StartMove(Vector3 pos)
        {
            actionScheduler.StartAction(this);
            MoveTo(pos, moveSpeed);
        }
        public void MoveTo(Vector3 pos, float speed)
        {
            agent.speed = speed;
            agent.destination = pos;
            agent.isStopped = false;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
        private void Update()
        {
            Anim();
        }

        void Anim()
        {
            var worldVelocity = agent.velocity;
            var localVelocity = transform.InverseTransformDirection(worldVelocity);
            float moveVelocity = localVelocity.z;
            animator.SetFloat(hashMove, moveVelocity, movedampTime, Time.deltaTime);
        }

        public void Cancel()
        {
            fighter.Cancel();
        }
    }

}

  

