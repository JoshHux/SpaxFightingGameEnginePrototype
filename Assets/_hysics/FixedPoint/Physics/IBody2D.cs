namespace Spax
{

    /**
    * @brief Represents an interface to 2D bodies.
    **/
    public interface IBody2D : IBody
    {

        /**
        * @brief If true the body doesn't move around by collisions.
        **/
        bool FPIsStatic
        {
            get; set;
        }

        /**
        * @brief Linear drag coeficient.
        **/
        FP FPLinearDrag
        {
            get; set;
        }

        /**
        * @brief Angular drag coeficient.
        **/
        FP FPAngularDrag
        {
            get; set;
        }

        /**
         *  @brief Static friction when in contact. 
         **/
        FP FPFriction
        {
            get; set;
        }

        /**
        * @brief Coeficient of restitution.
        **/
        FP FPRestitution
        {
            get; set;
        }

        /**
        * @brief Set/get body's position.
        **/
        FPVector2 FPPosition
        {
            get; set;
        }

        /**
        * @brief Set/get body's orientation.
        **/
        FP FPOrientation
        {
            get; set;
        }

        /**
        * @brief If true the body is affected by gravity.
        **/
        bool FPAffectedByGravity
        {
            get; set;
        }

        /**
        * @brief If true the body is managed as kinematic.
        **/
        bool FPIsKinematic
        {
            get; set;
        }

        /**
        * @brief Set/get body's linear velocity.
        **/
        FPVector2 FPLinearVelocity
        {
            get; set;
        }

        /**
        * @brief Set/get body's angular velocity. (radians/s).
        **/
        FP FPAngularVelocity
        {
            get; set;
        }

        /**
        * @brief Applies a force to the body's center.
        **/
        void FPApplyForce(FPVector2 force);

        /**
        * @brief Applies a force to the body at a specific position.
        **/
        void FPApplyForce(FPVector2 force, FPVector2 position);

        /**
        * @brief Applies a impulse to the body's center.
        **/
        void FPApplyImpulse(FPVector2 force);

        //a Spax addition
        void FPSetVelocity(FPVector2 force);
        void FPSetForce(FPVector2 force);

        /**
        * @brief Applies a impulse to the body at a specific position.
        **/
        void FPApplyImpulse(FPVector2 force, FPVector2 position);

        /**
        * @brief Applies a torque force to the body.
        **/
        void FPApplyTorque(FPVector2 force);

    }

}