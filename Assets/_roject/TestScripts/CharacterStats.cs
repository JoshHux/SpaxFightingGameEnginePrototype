using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;

namespace Spax.Character
{
    [Serializable]
    public struct CharacterStats
    {
        public int mass;
        public FP groundAcceleration;
        public FP airAcceleration;
        public FP maxGroundSpeed;
        public FP maxAirSpeed;
        public FP groundFriction;
        public FP airFriction;

        public int maxAirJumps;
        public FP jumpForce;
    }
}