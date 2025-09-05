using RPG.Combat;
using RPG.Core;
using RPG.Movement;

using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        Transform player;
        Health health;
        Fighter fighter;

        float playerDistance = 5f;
        float suspicionTime = 3f;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float wayPointDis = 1f;
        float arrivedTime = 3f;

        [SerializeField] PatrolPoint patrolPoint;
        public int curWayPointIdx = 0;
        void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            patrolPoint = GameObject.Find("PatrolPoints").GetComponent<PatrolPoint>();
            

        }

        void Update()
        {
            if (health.Dead) return;

            if (GetToPlayerRange() && fighter.CanAttack(player.gameObject))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTime();
        }

        void UpdateTime()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }
        bool GetToPlayerRange()
        {
            return Vector3.Distance(transform.position, player.position) < playerDistance;
        }

        void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player.gameObject);
        }

        void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        void PatrolBehavior()
        {
            if (patrolPoint == null) return;

            if(AtPoint())
            {
                timeSinceArrivedAtWaypoint = 0f;
                curWayPointIdx = patrolPoint.NextWayPoint(curWayPointIdx); 
            }
            if (timeSinceArrivedAtWaypoint > arrivedTime)
                GetComponent<Mover>().StartMove(patrolPoint.GetWayPoint(curWayPointIdx));
        }

        bool AtPoint()
        {
            return Vector3.Distance(transform.position, patrolPoint.GetWayPoint(curWayPointIdx)) < wayPointDis;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, playerDistance);
        }
    }
}