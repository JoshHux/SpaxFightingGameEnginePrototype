using UnityEngine;
using UnityEngine.Serialization;
using Spax.Physics3D;

namespace Spax {

    /**
     *  @brief Represents a physical 3D rigid body.
     **/
    [RequireComponent(typeof(FPCollider))]
    [AddComponentMenu("FixedPoint/Physics/FPRigidBody", 11)]
    public class FPRigidBody : MonoBehaviour {

        public enum InterpolateMode { None, Interpolate, Extrapolate };

        [FormerlySerializedAs("mass")]
        [SerializeField]
        private FP _mass = 1;

        /**
         *  @brief Mass of the body. 
         **/
        public FP mass {
            get {
                if (FPCollider._body != null) {
                    return FPCollider._body.Mass;
                }

                return _mass;
            }

            set {
                _mass = value;

                if (FPCollider._body != null) {
                    FPCollider._body.Mass = value;
                }
            }
        }

        [SerializeField]
        private bool _useGravity = true;

        /**
         *  @brief If true it uses gravity force. 
         **/
        public bool useGravity {
            get {
                if (FPCollider.IsBodyInitialized) {
                    return FPCollider.Body.FPAffectedByGravity;
                }

                return _useGravity;
            }

            set {
                _useGravity = value;

                if (FPCollider.IsBodyInitialized) {
                    FPCollider.Body.FPAffectedByGravity = _useGravity;
                }
            }
        }

        [SerializeField]
        private bool _isKinematic;

        /**
         *  @brief If true it doesn't get influences from external forces. 
         **/
        public bool isKinematic {
            get {
                if (FPCollider.IsBodyInitialized) {
                    return FPCollider.Body.FPIsKinematic;
                }

                return _isKinematic;
            }

            set {
                _isKinematic = value;

                if (FPCollider.IsBodyInitialized) {
                    FPCollider.Body.FPIsKinematic = _isKinematic;
                }
            }
        }

        [SerializeField]
        private FP _linearDrag;

        /**
         *  @brief Linear drag coeficient.
         **/
        public FP drag {
            get {
                if (FPCollider.IsBodyInitialized) {
                    return FPCollider.Body.FPLinearDrag;
                }

                return _linearDrag;
            }

            set {
                _linearDrag = value;

                if (FPCollider.IsBodyInitialized) {
                    FPCollider.Body.FPLinearDrag = _linearDrag;
                }
            }
        }

        [SerializeField]
        private FP _angularDrag = 0.05f;

        /**
         *  @brief Angular drag coeficient.
         **/
        public FP angularDrag {
            get {
                if (FPCollider.IsBodyInitialized) {
                    return FPCollider.Body.FPAngularDrag;
                }

                return _angularDrag;
            }

            set {
                _angularDrag = value;

                if (FPCollider.IsBodyInitialized) {
                    FPCollider.Body.FPAngularDrag = _angularDrag;
                }
            }
        }

        /**
         *  @brief Interpolation mode that should be used. 
         **/
        public InterpolateMode interpolation;

        [SerializeField]
        [HideInInspector]
        private FPRigidBodyConstraints _constraints = FPRigidBodyConstraints.None;

        /**
         *  @brief Freeze constraints applied to this body. 
         **/
        public FPRigidBodyConstraints constraints {
            get {
                if (FPCollider.IsBodyInitialized) {
                    return FPCollider._body.FreezeConstraints;
                }

                return _constraints;
            }

            set {
                _constraints = value;

                if (FPCollider.IsBodyInitialized) {
                    FPCollider._body.FreezeConstraints = value;
                }
            }
        }

        private FPCollider _FPCollider;

        /**
         *  @brief Returns the {@link FPCollider} attached.
         */
        public FPCollider FPCollider {
            get {
                if (_FPCollider == null) {
                    _FPCollider = GetComponent<FPCollider>();
                }

                return _FPCollider;
            }
        }

        private FPTransform _FPTransform;

        /**
         *  @brief Returns the {@link FPTransform} attached.
         */
        public FPTransform FPTransform {
            get {
                if (_FPTransform == null) {
                    _FPTransform = GetComponent<FPTransform>();
                }

                return _FPTransform;
            }
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector} representing the force to be applied.
         **/
        public void AddForce(FPVector force) {
            AddForce(force, ForceMode.Force);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector} representing the force to be applied.
         *  @param mode Indicates how the force should be applied.
         **/
        public void AddForce(FPVector force, ForceMode mode) {
            if (mode == ForceMode.Force) {
                FPCollider.Body.FPApplyForce(force);
            } else if (mode == ForceMode.Impulse) {
                FPCollider.Body.FPApplyImpulse(force);
            }
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForceAtPosition(FPVector force, FPVector position) {
            AddForceAtPosition(force, position, ForceMode.Force);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link FPVector} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForceAtPosition(FPVector force, FPVector position, ForceMode mode) {
            if (mode == ForceMode.Force) {
                FPCollider.Body.FPApplyForce(force, position);
            } else if (mode == ForceMode.Impulse) {
                FPCollider.Body.FPApplyImpulse(force, position);
            }
        }

        /**
         *  @brief Returns the velocity of the body at some position in world space. 
         **/
        public FPVector GetPointVelocity(FPVector worldPoint) {
            FPVector directionPoint = position - FPCollider.Body.FPPosition;
            return FPVector.Cross(FPCollider.Body.FPAngularVelocity, directionPoint) + FPCollider.Body.FPLinearVelocity;
        }

        /**
         *  @brief Simulates the provided tourque in the body. 
         *  
         *  @param torque A {@link FPVector} representing the torque to be applied.
         **/
        public void AddTorque(FPVector torque) {
            FPCollider.Body.FPApplyTorque(torque);
        }

        /**
         *  @brief Simulates the provided relative tourque in the body. 
         *  
         *  @param torque A {@link FPVector} representing the relative torque to be applied.
         **/
        public void AddRelativeTorque(FPVector torque)
        {
            FPCollider.Body.FPApplyRelativeTorque(torque);
        }

        /**
         *  @brief Changes orientation to look at target position. 
         *  
         *  @param target A {@link FPVector} representing the position to look at.
         **/
        public void LookAt(FPVector target) {
            rotation = FPQuaternion.CreateFromMatrix(FPMatrix.CreateFromLookAt(position, target));
        }

        /**
         *  @brief Moves the body to a new position. 
         **/
        public void MovePosition(FPVector position) {
            this.position = position;
        }

        /**
         *  @brief Rotates the body to a provided rotation. 
         **/
        public void MoveRotation(FPQuaternion rot) {
            this.rotation = rot;
        }

        /**
        *  @brief Position of the body. 
        **/
        public FPVector position {
            get {
                return FPTransform.position;
            }

            set {
                FPTransform.position = value;
            }
        }

        /**
        *  @brief Orientation of the body. 
        **/
        public FPQuaternion rotation {
            get {
                return FPTransform.rotation;
            }

            set {
                FPTransform.rotation = value;
            }
        }

        /**
        *  @brief LinearVelocity of the body. 
        **/
        public FPVector velocity {
            get {
                return FPCollider.Body.FPLinearVelocity;
            }

            set {
                FPCollider.Body.FPLinearVelocity = value;
            }
        }

        /**
        *  @brief AngularVelocity of the body. 
        **/
        public FPVector angularVelocity {
            get {
                return FPCollider.Body.FPAngularVelocity;
            }

            set {
                FPCollider.Body.FPAngularVelocity = value;
            }
        }

    }

}