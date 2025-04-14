using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(FollowPathMovement))]
    public class TeleportEnemy : MonoBehaviour
    {
        public float teleportFrequency;
        public float teleportDistance;

        FollowPathMovement movement;
        private void Awake()
        {   
            movement = GetComponent<FollowPathMovement>();
            nextTpTime = Time.time + 1 / teleportFrequency;
        }

        float nextTpTime;
        private void Update()
        {
            if (nextTpTime < Time.time)
            {
                nextTpTime = Time.time + 1 / teleportFrequency;

                movement.Move(teleportDistance);
            }
        }
    }
}
