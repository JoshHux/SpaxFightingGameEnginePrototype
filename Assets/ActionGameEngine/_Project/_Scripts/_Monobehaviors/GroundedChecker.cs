using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;

namespace Spax
{
    public class GroundedChecker : MonoBehaviour
    {
        private BoxCollider2D triggerCollider;
        private PlayerController player;

        //number of colliders the trigger is in, useful in OnTriggerExit2D
        private int triggeredWith;
        // Start is called before the first frame update
        void Awake()
        {
            triggerCollider = GetComponent<BoxCollider2D>();
            player = GetComponentInParent<PlayerController>();
            triggeredWith = 0;
        }

        void OnFixedTriggerEnter()
        {
            //one additional collider that is colliding with
            triggeredWith += 1;
            player.OnGrounded();
        }

        void OnFixedTriggerExit()
        {
            //a collider has exited the trigger
            triggeredWith -= 1;
            //prevents a scenario where you exit a trigger right into another trigger
            if (triggeredWith == 0)
            {
                player.OnNonGrounded();
            }
        }
    }
}