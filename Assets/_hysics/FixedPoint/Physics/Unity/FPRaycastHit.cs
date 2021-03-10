using System;

namespace Spax
{

    /**
    *  @brief Represents few information about a raycast hit. 
    **/
    public class FPRaycastHit
	{
		public FPRigidBody rigidbody { get; set; }
		public FPCollider collider { get; set; }
		public FPTransform transform { get; set; }
		public FPVector point { get; set; }
		public FPVector normal { get; set; }
		public FP distance { get; set; }

        public FPRaycastHit() { }

		public FPRaycastHit(FPRigidBody rigidbody, FPCollider collider, FPTransform transform, FPVector normal, FPVector origin, FPVector direction, FP fraction)
		{
			this.rigidbody = rigidbody;
			this.collider = collider;
			this.transform = transform;
			this.normal = normal;
			this.point = origin + direction * fraction;
			this.distance = fraction * direction.magnitude;
		}
	}
}

