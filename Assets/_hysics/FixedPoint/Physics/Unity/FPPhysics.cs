using System;

namespace Spax
{

    /**
    *  @brief Helpers for 3D physics.
    **/
    public class FPPhysics {

        public static bool Raycast(FPVector rayOrigin, FPVector rayDirection, out FPRaycastHit hit, FP maxDistance, int layerMask = UnityEngine.Physics.DefaultRaycastLayers)
        {
            FPRay ray = new FPRay(rayOrigin, direction:rayDirection);
            hit = PhysicsWorldManager.instance.Raycast(ray, maxDistance, layerMask:layerMask);
            if (hit != null)
            {
                if (hit.distance <= maxDistance)
                    return true;
            }
            return false;
        }
    }

}