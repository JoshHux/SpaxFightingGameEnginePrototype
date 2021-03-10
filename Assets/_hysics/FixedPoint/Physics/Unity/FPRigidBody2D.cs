using UnityEngine;
using UnityEngine.Serialization;

namespace Spax
{
    /**
     *  @brief Represents a physical 2D rigid body.
     **/
    [RequireComponent(typeof(FPCollider2D))]
    [AddComponentMenu("FixedPoint/Physics/FPRigidBody2D", 11)]
    public class FPRigidBody2D : MonoBehaviour
    {

        public enum InterpolateMode { None, Interpolate, Extrapolate };

        [FormerlySerializedAs("mass")]
        [SerializeField]
        private FP _mass = 1;

        /**
         *  @brief Mass of the body. 
         **/
        public FP mass
        {
            get
            {
                if (FPCollider._body != null)
                {
                    return FPCollider._body.Mass;
                }

                return _mass;
            }

            set
            {
                _mass = value;

                if (FPCollider._body != null)
                {
                    FPCollider._body.Mass = value;
                }
            }
        }

        [SerializeField]
        private bool _useGravity = true;

        /**
         *  @brief If true it uses gravity force. 
         **/
        public bool useGravity
        {
            get
            {
                if (FPCollider.IsBodyInitialized)
                {
                    return FPCollider.Body.FPAffectedByGravity;
                }

                return _useGravity;
            }

            set
            {
                _useGravity = value;

                if (FPCollider.IsBodyInitialized)
                {
                    FPCollider.Body.FPAffectedByGravity = _useGravity;
                }
            }
        }

        [SerializeField]
        private bool _isKinematic;

        /**
         *  @brief If true it doesn't get influences from external forces. 
         **/
        public bool isKinematic
        {
            get
            {
                if (FPCollider.IsBodyInitialized)
                {
                    return FPCollider.Body.FPIsKinematic;
                }

                return _isKinematic;
            }

            set
            {
                _isKinematic = value;

                if (FPCollider.IsBodyInitialized)
                {
                    FPCollider.Body.FPIsKinematic = _isKinematic;
                }
            }
        }

        [SerializeField]
        private FP _linearDrag;

        /**
         *  @brief Linear drag coeficient.
         **/
        public FP drag
        {
            get
            {
                if (FPCollider.IsBodyInitialized)
                {
                    return FPCollider.Body.FPLinearDrag;
                }

                return _linearDrag;
            }

            set
            {
                _linearDrag = value;

                if (FPCollider.IsBodyInitialized)
                {
                    FPCollider.Body.FPLinearDrag = _linearDrag;
                }
            }
        }

        [SerializeField]
        private FP _angularDrag = 0.05f;

        /**
         *  @brief Angular drag coeficient.
         **/
        public FP angularDrag
        {
            get
            {
                if (FPCollider.IsBodyInitialized)
                {
                    return FPCollider.Body.FPAngularDrag;
                }

                return _angularDrag;
            }

            set
            {
                _angularDrag = value;

                if (FPCollider.IsBodyInitialized)
                {
                    FPCollider.Body.FPAngularDrag = _angularDrag;
                }
            }
        }

        /**
         *  @brief Interpolation mode that should be used. 
         **/
        public InterpolateMode interpolation;

        /**
         *  @brief If true it freezes Z rotation of the RigidBody (it only appears when in 2D Physics).
         **/
        public bool freezeZAxis;

        private FPCollider2D _FPCollider;

        public FPCollider2D FPCollider
        {
            get
            {
                if (_FPCollider == null)
                {
                    _FPCollider = GetComponent<FPCollider2D>();
                }

                return _FPCollider;
            }
        }

        private FPTransform2D _FPTransform;

        private FPTransform2D FPTransform
        {
            get
            {
                if (_FPTransform == null)
                {
                    _FPTransform = GetComponent<FPTransform2D>();
                }

                return _FPTransform;
            }
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector2} representing the force to be applied.
         **/
        public void AddForce(FPVector2 force)
        {
            AddForce(force, ForceMode.Force);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector2} representing the force to be applied.
         *  @param mode Indicates how the force should be applied.
         **/
        public void AddForce(FPVector2 force, ForceMode mode)
        {
            if (mode == ForceMode.Force)
            {
                FPCollider.Body.FPApplyForce(force);
            }
            else if (mode == ForceMode.Impulse)
            {
                FPCollider.Body.FPApplyImpulse(force);
            }
        }

        //Spax's addition
        public void AddImpulse(FPVector2 force)
        {
            FPCollider.Body.FPApplyImpulse(force);
        }

        public void SetVelocity(FPVector2 force)
        {
            FPCollider.Body.FPSetVelocity(force);
        }

        public void SetForce(FPVector2 force)
        {
            FPCollider.Body.FPSetForce(force);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector2} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForceAtPosition(FPVector2 force, FPVector2 position)
        {
            AddForceAtPosition(force, position, ForceMode.Impulse);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector2} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForceAtPosition(FPVector2 force, FPVector2 position, ForceMode mode)
        {
            if (mode == ForceMode.Force)
            {
                FPCollider.Body.FPApplyForce(force, position);
            }
            else if (mode == ForceMode.Impulse)
            {
                FPCollider.Body.FPApplyImpulse(force, position);
            }
        }

        /**
         *  @brief Returns the velocity of the body at some position in world space. 
         **/
        public FPVector2 GetPointVelocity(FPVector2 worldPoint)
        {
            FPVector directionPoint = (position - FPCollider.Body.FPPosition).ToFPVector();
            return FPVector.Cross(new FPVector(0, 0, FPCollider.Body.FPAngularVelocity), directionPoint).ToFPVector2() + FPCollider.Body.FPLinearVelocity;
        }

        /**
         *  @brief Simulates the provided tourque in the body. 
         *  
         *  @param torque A {@link FPVector2} representing the torque to be applied.
         **/
        public void AddTorque(FPVector2 torque)
        {
            FPCollider.Body.FPApplyTorque(torque);
        }

        /**
         *  @brief Moves the body to a new position. 
         **/
        public void MovePosition(FPVector2 position)
        {
            this.position = position;
        }

        /**
         *  @brief Rotates the body to a provided rotation. 
         **/
        public void MoveRotation(FP rot)
        {
            this.rotation = rot;
        }

        /**
        *  @brief Position of the body. 
        **/
        public FPVector2 position
        {
            get
            {
                return FPTransform.position;
            }

            set
            {
                FPTransform.position = value;
            }
        }

        /**
        *  @brief Orientation of the body. 
        **/
        public FP rotation
        {
            get
            {
                return FPTransform.rotation;
            }

            set
            {
                FPTransform.rotation = value;
            }
        }

        /**
        *  @brief LinearVelocity of the body. 
        **/
        public FPVector2 velocity
        {
            get
            {
                return FPCollider.Body.FPLinearVelocity;
            }

            set
            {
                FPCollider.Body.FPLinearVelocity = value;
            }
        }

        /**
        *  @brief AngularVelocity of the body (radians/s). 
        **/
        public FP angularVelocity
        {
            get
            {
                return FPCollider.Body.FPAngularVelocity;
            }

            set
            {
                FPCollider.Body.FPAngularVelocity = value;
            }
        }

    }

}