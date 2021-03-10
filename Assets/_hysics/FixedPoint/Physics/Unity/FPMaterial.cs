using UnityEngine;

namespace Spax {

    /**
     *  @brief Simulates physical properties of a body.
     **/
    [AddComponentMenu("FixedPoint/Physics/FPMaterial", 22)]
    public class FPMaterial : MonoBehaviour {

        /**
         *  @brief Static friction when in contact. 
         **/
         [Header("接触时的静摩擦")]
        public FP friction = FP.Zero;

        /**
         *  @brief Coeficient of restitution. 
         **/
        [Header("恢复系数")]
        public FP restitution = FP.Zero;

    }

}