using UnityEngine;
using UnityEngine.Serialization;
using Spax.Physics3D;

namespace Spax {
    /**
     *  @brief Collider with a sphere shape. 
     **/
    [AddComponentMenu("FixedPoint/Physics/SphereCollider", 0)]
    public class FPSphereCollider : FPCollider {

        [FormerlySerializedAs("radius")]
        [SerializeField]
        private float _radius;

        /**
         *  @brief Radius of the sphere. 
         **/
        public FP radius {
            get {
                if (_body != null) {
                    return ((SphereShape)_body.Shape).Radius;
                }

                return _radius;
            }

            set {
                _radius = value.AsFloat();

                if (_body != null) {
                    ((SphereShape)_body.Shape).Radius = value;
                }
            }
        }

        /**
         *  @brief Sets initial values to {@link #radius} based on a pre-existing SphereCollider or CircleCollider2D.
         **/
        public void Reset() {
            if (GetComponent<CircleCollider2D>() != null) {
                CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();

                radius = circleCollider2D.radius;
                Center = new FPVector(circleCollider2D.offset.x, circleCollider2D.offset.y, 0);
                isTrigger = circleCollider2D.isTrigger;
            } else if (GetComponent<SphereCollider>() != null) {
                SphereCollider sphereCollider = GetComponent<SphereCollider>();

                radius = sphereCollider.radius;
                Center = sphereCollider.center.ToFPVector();
                isTrigger = sphereCollider.isTrigger;
            }
        }

        /**
         *  @brief Create the internal shape used to represent a FPSphereCollider.
         **/
        public override Shape CreateShape() {
            return new SphereShape(radius);
        }

        protected override void DrawGizmos() {
            Gizmos.DrawWireSphere(Vector3.zero, 1);
        }

        protected override Vector3 GetGizmosSize() {
            return Vector3.one * radius.AsFloat();
        }

    }

}