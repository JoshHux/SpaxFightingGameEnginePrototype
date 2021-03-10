
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;

namespace Spax.Character
{
    [CreateAssetMenu(fileName = "Condition Data", menuName = "Character/Condition Data", order = 0)]
    public class CharacterCondition : ScriptableObject
    {

        [SerializeField] private CharacterStats stats;
        [SerializeField] private ConditionData cond;

        //returns acceleration based on whether or not the character is grounded
        //if grounded, grounded acceleration, if airborne, air acceleration
        public FP GetAcceleration()
        {
            return (cond.isGrounded) ? stats.groundAcceleration : stats.airAcceleration;
        }

        //returns maximum allowed horizontal velocity based on whether or not the character is grounded
        //if grounded, grounded max speed, if airborne, air max speed
        //note about max speed:
        //this is the maximum allowed speed that is allowed by the character's controller
        //outside factors are allowed to influence the velocity and go beyond this threshold
        //example:
        //if the character hits a boost pad, they should be allowed to exceed their maximum velocity,
        //acceleration added should be 0, friction should bring the character's current velocity down 
        //using friction.
        public FP GetMaxSpeed()
        {
            return (cond.isGrounded) ? stats.maxGroundSpeed : stats.maxAirSpeed;
        }

        //returns friction based on whether or not the character is grounded
        //returns grounded friction if grounded, and air friction if airborne
        public FP GetFriction()
        {
            return (cond.isGrounded) ? stats.groundFriction : stats.airFriction;
        }

        //returns zero if you cannot wall jump
        // identifies which jump the player can perform
        public int CanJump(FP wallRef)
        {
            if (cond.isGrounded) { return 1; }
            else if (CheckWallJump(wallRef)) { return 2; }
            else if (CheckAirJumps()) { return 3; }

            return 0;
        }

        //checks to see if the character has used all of their air jumps
        //we don't check to see if the character is grounded because that is already checked in CanJump (the only place this should be called from)
        private bool CheckAirJumps()
        {
            return (cond.currentAirJumps < stats.maxAirJumps);
        }

        //checks to see if the character is allowed to wall jump, grounded is not checked for the same reason as the CheckAirJumps method
        //wallRef should be the relative displacement to the wall, from the character in world coordinates, the character wants to jump off of
        //allows a wall jump under 2 conditions:
        //1) the character has yet to make a wall jump
        //2) the wall the character wants to jump off of is not in the same relative orientation as the previous wall
        private bool CheckWallJump(FP wallRef)
        {
            if ((wallRef != FP.Zero) && (/*(cond.wallJumpRef == 0) ||*/ (wallRef * cond.wallJumpRef <= 0)))
            {
                cond.wallJumpRef = (wallRef < 0) ? -1 : 1;
                return true;
            }
            return false;
        }

        //the grounded parameter is whether or not you want to set the character as grounded
        //when you set the grounded bool to true, it resets the previous wall jump orientation
        public void SetGrounded(bool grounded)
        {
            if (grounded) { cond.wallJumpRef = 0; }

            cond.isGrounded = grounded;
        }


        public void SetVelocity(FPVector2 newVel)
        {
            cond.velocity = newVel;
        }
        public void SetVelocityX(FP newVel)
        {
            cond.velocity.x = newVel;
        }

        public void AddVelocityX(FP newVel)
        {
            cond.velocity.x += newVel;
        }

        public void SetVelocityY(FP newVel)
        {
            cond.velocity.y = newVel;
        }
        public FPVector2 GetVelocity()
        {
            return cond.velocity;
        }

        public FP GetMass()
        {
            return stats.mass;
        }

        public void SetFacing(int dir)
        {
            if (dir < 0)
            {
                cond.facing = -1;
            }
            else
            {
                cond.facing = 1;
            }
        }

        public int Getfacing()
        {
            return cond.facing;
        }

        public void Initialize()
        {
            cond.Initialize();
        }
    }

    [Serializable]
    internal struct ConditionData
    {
        public bool isGrounded;
        public bool isMoving;
        public FPVector2 velocity;
        public int currentAirJumps;
        //tracks last orientation of wall after a wall jump
        //only put -1,0,1
        public int wallJumpRef;
        public int facing;

        public void Initialize()
        {
            isGrounded = false;
            isMoving = false;
            velocity = new FPVector2(0, 0);
            currentAirJumps = 0;
            wallJumpRef = 0;
            facing = 1;
        }
    }
}