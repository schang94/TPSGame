using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPoint : MonoBehaviour
    {
        public Color color = Color.red;
        public float size = 1f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = NextWayPoint(i);
                Gizmos.DrawSphere(GetWayPoint(i), size);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));
            }
        }

        public int NextWayPoint(int point)
        {
            if (point + 1 == transform.childCount) return 0;
            return point + 1;
        }
        public Vector3 GetWayPoint(int point)
        {
            return transform.GetChild(point).position;
        }

        public int WayPointCount()
        {
            return transform.childCount;
        }
    }

}

