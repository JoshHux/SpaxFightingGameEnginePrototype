﻿using UnityEngine;
using UnityEngine.Serialization;
using Spax.Physics3D;

namespace Spax {
    /**
     *  @brief Collider with a capsule shape. 
     **/
    [AddComponentMenu("FixedPoint/Physics/CapsuleCollider", 0)]
    public class FPCapsuleCollider : FPCollider {

        [FormerlySerializedAs("radius")]
        [SerializeField]
        private FP _radius;

        /**
         *  @brief Radius of the capsule. 
         **/
        public FP radius {
            get {
                if (_body != null) {
                    return ((CapsuleShape)_body.Shape).Radius;
                }

                return _radius;
            }
            set {
                _radius = value;

                if (_body != null) {
                    ((CapsuleShape)_body.Shape).Radius = _radius;
                }
            }
        }

        [FormerlySerializedAs("length")]
        [SerializeField]
        private FP _length;

        /**
         *  @brief Length of the capsule. 
         **/
        public FP length {
            get {
                if (_body != null) {
                    return ((CapsuleShape)_body.Shape).Length;
                }

                return _length;
            }
            set {
                _length = value;

                if (_body != null) {
                    ((CapsuleShape)_body.Shape).Length = _length;
                }
            }
        }

        /**
         *  @brief Create the internal shape used to represent a FPCapsuleCollider.
         **/
        public override Shape CreateShape() {
            return new CapsuleShape(length, radius);
        }

        protected override void DrawGizmos() {
            Gizmos.DrawWireSphere(Vector3.zero, 1);
            Gizmos.DrawWireSphere(new FPVector(0, length / radius - 2 * radius, 0).ToVector(), 1);
            Gizmos.DrawWireSphere(new FPVector(0, -length / radius + 2 * radius, 0).ToVector(), 1);
        }

        protected override Vector3 GetGizmosSize() {
            return Vector3.one * radius.AsFloat();
        }

    }
}