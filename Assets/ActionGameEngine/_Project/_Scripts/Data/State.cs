using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax.Input;

namespace Spax.StateMachine
{

    public enum TransitionCondition : uint
    {
        //ON_END = 0,  // 0
        JUMP = 1 << 0,  // 1
        ACTION1 = 1 << 1,  // 2
        ACTION2 = 1 << 2,  // 4
        ACTION3 = 1 << 3,  // 8
        ACTION4 = 1 << 4,  // 16
        ACTION5 = 1 << 5,  // 32
        ACTION6 = 1 << 6,  // 64
        BUTTON_PRESSED = 1 << 11,
        BUTTON_RELEASED = 1 << 12,
        GROUNDED = 1 << 14,
        AERIAL = 1 << 15,
        ON_END = 1 << 16,
        QUART_METER = 1 << 17,
        HALF_METER = 1 << 18,
        FULL_METER = 1 << 19,
        CANJUMP = 1 << 20,
    }

    [Flags]
    public enum EnterStateConditions : uint
    {
        //NOTHING = 0,  // 0
        KILL_X_MOMENTUM = 1 << 0,  // 1
        KILL_Y_MOMENTUM = 1 << 1,  // 1
        NADA = 1 << 2,  // 1
    }
    [Flags]
    public enum ExitStateConditions : uint
    {
        NOTHING = 0,  // 0
        CLEAN_HITBOXES = 1 << 1,
    }

    [Flags]
    public enum StateConditions : uint
    {
        NOTHING = 0,  // 0
        CAN_MOVE = 1 << 1,  // 1
        APPLY_GRAV = 1 << 2,  // 1
        NO_PARENT_TRANS = 1 << 3,
        CAN_TURN = 1 << 4,
        APPLY_FRICTION = 1 << 5,
        WALKING = 1 | (1 << 6),

        CAN_DO = 1 << 30,  // 1
    }

    [System.Serializable]
    public struct CharacterStateTransition
    {
        public int TargetStateID;
        public uint TargetFrame;
        public uint MinFrame;
        public TransitionCondition[] Conditions;

    }
    [System.Serializable]

    public struct CharacterState
    {
        public string Name;
        public CharacterFrame[] Frames;
        public CharacterStateTransition[] Transitions;
    }

    [Flags]
    public enum FrameFlags : uint
    {
        /// <summary>
        /// If set, the player will be intangible to all attacks on all 
        /// of their hurtboxes for the frame.
        /// </summary>
        INTANGIBLE = 1 << 0,

        /// <summary>
        /// If set, the player will be intangible to all attacks on all 
        /// of their hurtboxes for the frame.
        /// </summary>
        INVINCIBLE = 1 << 1,

        /// <summary>
        /// If set, the player will be intangible to only projectile attacks.
        /// </summary>
        GRAZING = 1 << 2,

        /// <summary>
        /// If set, the player will recieve damage, but cannot be knocked back.
        /// </summary>
        SUPER_ARMOR = 1 << 3,

        /// <summary>
        /// If set, the player will face right at the start of the frame.
        /// </summary>
        FACE_RIGHT = 1 << 4,

        /// <summary>
        /// If set, the player will face left at the start of the frame.
        /// </summary>
        FACE_LEFT = 1 << 5,

        /// <summary>
        /// If set, the player will change their direction at the start 
        /// of the frame.
        /// </summary>
        CHANGE_DIRECTION = 1 << 6,
        AUTO_JUMP = 1 << 7,
        APPLY_VEL = 1 << 8,
        SET_VEL = 1 << 8 | 1 << 9,
    }

    [System.Serializable]
    public struct CharacterFrame
    {

        //public const int kMaxPlayerHitboxCount = sizeof(HitboxBitfield) * 8;
        public int atFrame;

        public FrameFlags Flags;
        public Vector2 velocity;


        public HitBoxData[] hitboxes;

        public bool Is(FrameFlags flags) => (Flags & flags) != 0;


        public bool HasHitboxes()
        {
            return (hitboxes != null) && (hitboxes.Length > 0);
        }

        public void Prepare()
        {
            if (hitboxes != null)
            {
                int len = hitboxes.Length;

                //assigns each hitbox priority
                for (int i = 0; i < len; i++)
                {
                    hitboxes[i].priority = i;
                }
            }
        }


        //public bool IsHitboxActive(int hitboxId) => (ActiveHitboxes & (1ul << hitboxId)) != 0;


    }

    [System.Serializable]
    public struct HitBoxData
    {
        public int priority;
        public int duration;
        public Vector2 offset;
        public Vector2 size;
        public float launchAngle;
        public float launchForce;
        public int damage;
        public int hitstop;
        public int hitstun;
        public int blockstun;
        public HitboxType type;
        public CancelCondition onHitCancel;

    }

    public enum HitboxType
    {
        STRIKE = 1 << 0,
        GRAB = 1 << 1,
        UNBLOCKABLE = 1 << 2,

    }
}