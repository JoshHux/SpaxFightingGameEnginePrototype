using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax.StateMachine;
using Spax;

namespace Spax
{
    public class Hitbox : SpaxBehavior
    {
        private BoxCollider2D boxCollider;
        private ContactFilter2D filter;

        [SerializeField] private bool isActive;
        //currently colliding with
        [SerializeField] private List<Collider2D> curColliding;
        //previously collided with
        [SerializeField] private List<Collider2D> wasColliding;
        //what gets queried, difference between the two previous lists
        [SerializeField] private List<Collider2D> diffColliders;

        [SerializeField] private FrameTimer timer;

        //if negative, then the box can hurt people on its team
        //the value is the team it is on
        private int allignment;


        private HitBoxData data;
        private PlayerController player;

        // Start is called before the first frame update
        protected override void OnStart()
        {
            boxCollider = this.GetComponent<BoxCollider2D>();
            player = transform.parent.GetComponentInParent<PlayerController>();

            filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.useTriggers = true;

            filter.SetLayerMask(3 << 8);

        }

        // Update is called once per frame
        protected override void PostUpdate()
        {
            //the time is ticking if the hitbox is active
            if (timer.TickTimer())
            {
                //query collisions if the hitbox is supposed to be active
                boxCollider.OverlapCollider(filter, curColliding);

                diffColliders = curColliding.Except(wasColliding).ToList();

                wasColliding = curColliding.ToList();


                int len = diffColliders.Count;
                if (len > 0)
                {
                    for (int i = 0; i < len; i++)
                    {
                        Hurtbox box = curColliding[i].GetComponent<Hurtbox>();
                        if (box.GetAllignment() != allignment)
                        {
                            box.GetHit();
                            player.OnHit(data);
                        }
                    }
                }
            }
        }

        public void SetBoxData(HitBoxData boxData, bool facingRight)
        {
            data = boxData;
            int facing = 1;
            if (!facingRight)
            {
                facing = -1;
            }

            boxCollider.offset = new Vector2(data.offset.x * facing, data.offset.y);
            boxCollider.size = data.size;

            timer.StartTimer(data.duration);

            //refreshes list to prepare for collision queries
            curColliding.Clear();
            wasColliding.Clear();
            diffColliders.Clear();
        }

        public void Deactivate()
        {
            timer.ForceTimerEnd();
        }

        public bool IsActive()
        {
            return timer.IsTicking();
        }

        /*void OnDrawGizmos()
        {
            Gizmos.DrawCube(new Vector3(data.offset.x, data.offset.y, 0f), new Vector3(data.size.x, data.size.y, 0f));
        }*/
    }
}