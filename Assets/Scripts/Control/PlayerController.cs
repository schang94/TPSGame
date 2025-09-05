using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;
        Camera mainCamera;

        public LayerMask groundMask;
        public LayerMask enemyMask;
        private InputHandler input;
        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            mainCamera = Camera.main;
            input = GetComponent<InputHandler>();

        }

        private void Update()
        {
            if (GetComponent<Health>().Dead) return;

            if (Combat()) return;
            if (Move()) return;
        }

        private bool Combat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());

            foreach(RaycastHit hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!target.GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                if (input.IsMouse)
                {
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool Move()
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(GetRay(), out hit, Mathf.Infinity, groundMask | enemyMask);
            if (isHit)
            {
                if (input.IsMouse)
                {
                    mover.StartMove(hit.point);
                }
                return true;
            }
            return false;
        }

        private Ray GetRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
