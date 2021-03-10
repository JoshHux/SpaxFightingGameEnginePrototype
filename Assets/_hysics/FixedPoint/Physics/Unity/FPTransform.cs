using UnityEngine;
using System.Collections.Generic;

namespace Spax
{

    /**
    *  @brief A deterministic version of Unity's Transform component for 3D physics. 
    **/
    [ExecuteInEditMode]
    public class FPTransform : SpaxBehavior
    {

        private const float DELTA_TIME_FACTOR = 10f;

        [SerializeField]
        [HideInInspector]
        private FPVector _localPosition;

        /**
         *  @brief Property access to local position. 
         **/
        public FPVector localPosition
        {
            get
            {
                return _localPosition;
            }
            set
            {
                _localPosition = value;
            }
        }

        [SerializeField]
        [HideInInspector]
        private FPVector _position;

        private FPVector _prevPosition;
        /**
        *  @brief Property access to position. 
        *  
        *  It works as proxy to a Body's position when there is a collider attached.
        **/
        public FPVector position
        {
            get
            {
                if (FPCollider != null && FPCollider.Body != null)
                {
                    position = FPCollider.Body.FPPosition - scaledCenter;
                }

                return _position;
            }
            set
            {
                _prevPosition = _position;
                _position = value;

                if (FPCollider != null && FPCollider.Body != null)
                {
                    FPCollider.Body.FPPosition = _position + scaledCenter;
                }

                UpdateChildPosition();
            }
        }

        [SerializeField]
        [HideInInspector]
        private FPQuaternion _localRotation;

        /**
         *  @brief Property access to local rotation. 
         **/
        public FPQuaternion localRotation
        {
            get
            {
                return _localRotation;
            }
            set
            {
                _localRotation = value;
            }
        }

        [SerializeField]
        [HideInInspector]
        private FPQuaternion _rotation;

        /**
        *  @brief Property access to rotation. 
        *  
        *  It works as proxy to a Body's rotation when there is a collider attached.
        **/
        public FPQuaternion rotation
        {
            get
            {
                if (FPCollider != null && FPCollider.Body != null)
                {
                    rotation = FPQuaternion.CreateFromMatrix(FPCollider.Body.FPOrientation);
                }

                return _rotation;
            }
            set
            {
                _rotation = value;

                if (FPCollider != null && FPCollider.Body != null)
                {
                    FPCollider.Body.FPOrientation = FPMatrix.CreateFromQuaternion(_rotation);
                }

                UpdateChildRotation();
            }
        }

        [SerializeField]
        [HideInInspector]
        private FPVector _scale;

        /**
        *  @brief Property access to scale. 
        **/
        public FPVector scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }

        [SerializeField]
        [HideInInspector]
        private FPVector _localScale;

        /**
        *  @brief Property access to local scale. 
        **/
        public FPVector localScale
        {
            get
            {
                return _localScale;
            }
            set
            {
                _localScale = value;
            }
        }

        [SerializeField]
        [HideInInspector]
        private bool _serialized;

        private FPVector scaledCenter
        {
            get
            {
                if (FPCollider != null)
                {
                    return FPCollider.ScaledCenter;
                }

                return FPVector.zero;
            }
        }

        /**
        *  @brief Rotates game object to point forward vector to a target position. 
        *  
        *  @param other FPTrasform used to get target position.
        **/
        public void LookAt(FPTransform other)
        {
            LookAt(other.position);
        }

        /**
        *  @brief Rotates game object to point forward vector to a target position. 
        *  
        *  @param target Target position.
        **/
        public void LookAt(FPVector target)
        {
            this.rotation = FPQuaternion.CreateFromMatrix(FPMatrix.CreateFromLookAt(position, target));
        }

        /**
        *  @brief Moves game object based on provided axis values. 
        **/
        public void Translate(FP x, FP y, FP z)
        {
            Translate(x, y, z, Space.Self);
        }

        /**
        *  @brief Moves game object based on provided axis values and a relative space.
        *  
        *  If relative space is SELF then the game object will move based on its forward vector.
        **/
        public void Translate(FP x, FP y, FP z, Space relativeTo)
        {
            Translate(new FPVector(x, y, z), relativeTo);
        }

        /**
        *  @brief Moves game object based on provided axis values and a relative {@link FPTransform}.
        *  
        *  The game object will move based on FPTransform's forward vector.
        **/
        public void Translate(FP x, FP y, FP z, FPTransform relativeTo)
        {
            Translate(new FPVector(x, y, z), relativeTo);
        }

        /**
        *  @brief Moves game object based on provided translation vector.
        **/
        public void Translate(FPVector translation)
        {
            Translate(translation, Space.Self);
        }

        /**
        *  @brief Moves game object based on provided translation vector and a relative space.
        *  
        *  If relative space is SELF then the game object will move based on its forward vector.
        **/
        public void Translate(FPVector translation, Space relativeTo)
        {
            if (relativeTo == Space.Self)
            {
                Translate(translation, this);
            }
            else
            {
                this.position += translation;
            }
        }

        /**
        *  @brief Moves game object based on provided translation vector and a relative {@link FPTransform}.
        *  
        *  The game object will move based on FPTransform's forward vector.
        **/
        public void Translate(FPVector translation, FPTransform relativeTo)
        {
            this.position += FPVector.Transform(translation, FPMatrix.CreateFromQuaternion(relativeTo.rotation));
        }

        /**
        *  @brief Rotates game object based on provided axis, point and angle of rotation.
        **/
        public void RotateAround(FPVector point, FPVector axis, FP angle)
        {
            FPVector vector = this.position;
            FPVector vector2 = vector - point;
            vector2 = FPVector.Transform(vector2, FPMatrix.AngleAxis(angle * FP.Deg2Rad, axis));
            vector = point + vector2;
            this.position = vector;

            Rotate(axis, angle);
        }

        /**
        *  @brief Rotates game object based on provided axis and angle of rotation.
        **/
        public void RotateAround(FPVector axis, FP angle)
        {
            Rotate(axis, angle);
        }

        /**
        *  @brief Rotates game object based on provided axis angles of rotation.
        **/
        public void Rotate(FP xAngle, FP yAngle, FP zAngle)
        {
            Rotate(new FPVector(xAngle, yAngle, zAngle), Space.Self);
        }

        /**
        *  @brief Rotates game object based on provided axis angles of rotation and a relative space.
        *  
        *  If relative space is SELF then the game object will rotate based on its forward vector.
        **/
        public void Rotate(FP xAngle, FP yAngle, FP zAngle, Space relativeTo)
        {
            Rotate(new FPVector(xAngle, yAngle, zAngle), relativeTo);
        }

        /**
        *  @brief Rotates game object based on provided axis angles of rotation.
        **/
        public void Rotate(FPVector eulerAngles)
        {
            Rotate(eulerAngles, Space.Self);
        }

        /**
        *  @brief Rotates game object based on provided axis and angle of rotation.
        **/
        public void Rotate(FPVector axis, FP angle)
        {
            Rotate(axis, angle, Space.Self);
        }

        /**
        *  @brief Rotates game object based on provided axis, angle of rotation and relative space.
        *  
        *  If relative space is SELF then the game object will rotate based on its forward vector.
        **/
        public void Rotate(FPVector axis, FP angle, Space relativeTo)
        {
            FPQuaternion result = FPQuaternion.identity;

            if (relativeTo == Space.Self)
            {
                result = this.rotation * FPQuaternion.AngleAxis(angle, axis);
            }
            else
            {
                result = FPQuaternion.AngleAxis(angle, axis) * this.rotation;
            }

            result.Normalize();
            this.rotation = result;
        }

        /**
        *  @brief Rotates game object based on provided axis angles and relative space.
        *  
        *  If relative space is SELF then the game object will rotate based on its forward vector.
        **/
        public void Rotate(FPVector eulerAngles, Space relativeTo)
        {
            FPQuaternion result = FPQuaternion.identity;

            if (relativeTo == Space.Self)
            {
                result = this.rotation * FPQuaternion.Euler(eulerAngles);
            }
            else
            {
                result = FPQuaternion.Euler(eulerAngles) * this.rotation;
            }

            result.Normalize();
            this.rotation = result;
        }

        /**
        *  @brief Current self forward vector.
        **/
        public FPVector forward
        {
            get
            {
                return FPVector.Transform(FPVector.forward, FPMatrix.CreateFromQuaternion(rotation));
            }
        }

        /**
        *  @brief Current self right vector.
        **/
        public FPVector right
        {
            get
            {
                return FPVector.Transform(FPVector.right, FPMatrix.CreateFromQuaternion(rotation));
            }
        }

        /**
        *  @brief Current self up vector.
        **/
        public FPVector up
        {
            get
            {
                return FPVector.Transform(FPVector.up, FPMatrix.CreateFromQuaternion(rotation));
            }
        }

        /**
        *  @brief Returns Euler angles in degrees.
        **/
        public FPVector eulerAngles
        {
            get
            {
                return rotation.eulerAngles;
            }
        }

        public FPMatrix4x4 localToWorldMatrix
        {
            get
            {
                FPTransform thisTransform = this;
                FPMatrix4x4 curMatrix = FPMatrix4x4.TransformToMatrix(ref thisTransform);
                FPTransform parent = tsParent;
                while (parent != null)
                {
                    curMatrix = FPMatrix4x4.TransformToMatrix(ref parent) * curMatrix;
                    parent = parent.tsParent;
                }
                return curMatrix;
            }
        }

        public FPMatrix4x4 worldToLocalMatrix
        {
            get
            {
                return FPMatrix4x4.Inverse(localToWorldMatrix);
            }
        }

        /**
         *  @brief Transform a point from local space to world space.
         **/
        public FPVector4 TransformPoint(FPVector4 point)
        {
            Debug.Assert(point.w == FP.One);
            return FPVector4.Transform(point, localToWorldMatrix);
        }

        public FPVector TransformPoint(FPVector point)
        {
            return FPVector4.Transform(point, localToWorldMatrix).ToFPVector();
        }

        /**
         *  @brief Transform a point from world space to local space.
         **/
        public FPVector4 InverseTransformPoint(FPVector4 point)
        {
            Debug.Assert(point.w == FP.One);
            return FPVector4.Transform(point, worldToLocalMatrix);
        }

        public FPVector InverseTransformPoint(FPVector point)
        {
            return FPVector4.Transform(point, worldToLocalMatrix).ToFPVector();
        }

        /**
         *  @brief Transform a direction from local space to world space.
         **/
        public FPVector4 TransformDirection(FPVector4 direction)
        {
            Debug.Assert(direction.w == FP.Zero);
            FPMatrix4x4 matrix = FPMatrix4x4.Translate(position) * FPMatrix4x4.Rotate(rotation);
            return FPVector4.Transform(direction, matrix);
        }

        public FPVector TransformDirection(FPVector direction)
        {
            return TransformDirection(new FPVector4(direction.x, direction.y, direction.z, FP.Zero)).ToFPVector();
        }

        /**
         *  @brief Transform a direction from world space to local space.
         **/
        public FPVector4 InverseTransformDirection(FPVector4 direction)
        {
            Debug.Assert(direction.w == FP.Zero);
            FPMatrix4x4 matrix = FPMatrix4x4.Translate(position) * FPMatrix4x4.Rotate(rotation);
            return FPVector4.Transform(direction, FPMatrix4x4.Inverse(matrix));
        }

        public FPVector InverseTransformDirection(FPVector direction)
        {
            return InverseTransformDirection(new FPVector4(direction.x, direction.y, direction.z, FP.Zero)).ToFPVector();
        }

        /**
         *  @brief Transform a vector from local space to world space.
         **/
        public FPVector4 TransformVector(FPVector4 vector)
        {
            Debug.Assert(vector.w == FP.Zero);
            return FPVector4.Transform(vector, localToWorldMatrix);
        }

        public FPVector TransformVector(FPVector vector)
        {
            return TransformVector(new FPVector4(vector.x, vector.y, vector.z, FP.Zero)).ToFPVector();
        }

        /**
         *  @brief Transform a vector from world space to local space.
         **/
        public FPVector4 InverseTransformVector(FPVector4 vector)
        {
            Debug.Assert(vector.w == FP.Zero);
            return FPVector4.Transform(vector, worldToLocalMatrix);
        }

        public FPVector InverseTransformVector(FPVector vector)
        {
            return InverseTransformVector(new FPVector4(vector.x, vector.y, vector.z, FP.Zero)).ToFPVector();
        }

        [HideInInspector]
        public FPCollider FPCollider;

        [HideInInspector]
        public FPTransform tsParent;

        [HideInInspector]
        public List<FPTransform> tsChildren;

        private bool initialized = false;

        private FPRigidBody rb;

        protected override void OnStart()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            Initialize();
            rb = GetComponent<FPRigidBody>();
        }

        /**
        *  @brief Initializes internal properties based on whether there is a {@link FPCollider} attached.
        **/
        public void Initialize()
        {
            if (initialized)
            {
                return;
            }

            FPCollider = GetComponent<FPCollider>();
            if (transform.parent != null)
            {
                tsParent = transform.parent.GetComponent<FPTransform>();
            }

            foreach (Transform child in transform)
            {
                FPTransform tsChild = child.GetComponent<FPTransform>();
                if (tsChild != null)
                {
                    tsChildren.Add(tsChild);
                }

            }

            if (!_serialized)
            {
                UpdateEditMode();
            }

            if (FPCollider != null)
            {
                if (FPCollider.IsBodyInitialized)
                {
                    FPCollider.Body.FPPosition = _position + scaledCenter;
                    FPCollider.Body.FPOrientation = FPMatrix.CreateFromQuaternion(_rotation);
                }
            }
            else
            {
                // StateTracker.AddTracking(this);
            }

            initialized = true;
        }

        protected override void RenderUpdate()
        {
            if (Application.isPlaying)
            {
                if (initialized)
                {
                    UpdatePlayMode();
                }
            }
            else
            {
                UpdateEditMode();
            }
        }

        private void UpdateEditMode()
        {
            if (transform.hasChanged)
            {
                _position = transform.position.ToFPVector();
                _rotation = transform.rotation.ToFPQuaternion();
                _scale = transform.lossyScale.ToFPVector();

                _localPosition = transform.localPosition.ToFPVector();
                _localRotation = transform.localRotation.ToFPQuaternion();
                _localScale = transform.localScale.ToFPVector();

                _serialized = true;
            }
        }

        private void UpdatePlayMode()
        {

            if (tsParent != null)
            {
                _localPosition = tsParent.InverseTransformPoint(position);
                FPMatrix matrix = FPMatrix.CreateFromQuaternion(tsParent.rotation);
                _localRotation = FPQuaternion.CreateFromMatrix(FPMatrix.Inverse(matrix)) * rotation;
            }
            else
            {
                _localPosition = position;
                _localRotation = rotation;
            }

            if (rb != null)
            {
                if (rb.interpolation == FPRigidBody.InterpolateMode.Interpolate)
                {
                    transform.position = Vector3.Lerp(transform.position, position.ToVector(), Time.deltaTime * DELTA_TIME_FACTOR);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation.ToQuaternion(), Time.deltaTime * DELTA_TIME_FACTOR);
                    transform.localScale = Vector3.Lerp(transform.localScale, localScale.ToVector(), Time.deltaTime * DELTA_TIME_FACTOR);
                    return;
                }
                else if (rb.interpolation == FPRigidBody.InterpolateMode.Extrapolate)
                {
                    transform.position = (position + rb.FPCollider.Body.FPLinearVelocity * Time.deltaTime * DELTA_TIME_FACTOR).ToVector();
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation.ToQuaternion(), Time.deltaTime * DELTA_TIME_FACTOR);
                    transform.localScale = Vector3.Lerp(transform.localScale, localScale.ToVector(), Time.deltaTime * DELTA_TIME_FACTOR);
                    return;
                }
            }

            transform.position = position.ToVector();
            transform.rotation = rotation.ToQuaternion();
            transform.localScale = localScale.ToVector();
            _scale = transform.lossyScale.ToFPVector();
        }

        private void UpdateChildPosition()
        {
            foreach (FPTransform child in tsChildren)
            {
                child.Translate(_position - _prevPosition);
            }
        }

        private void UpdateChildRotation()
        {
            FPMatrix matrix = FPMatrix.CreateFromQuaternion(_rotation);
            foreach (FPTransform child in tsChildren)
            {
                child.localRotation = FPQuaternion.CreateFromMatrix(FPMatrix.Inverse(matrix)) * _rotation;
                child.localPosition = FPVector.Transform(child.localPosition, FPMatrix.CreateFromQuaternion(child.localRotation));
                child.position = TransformPoint(child.localPosition);
            }
        }
    }

}