/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace Spax.Physics3D {

    /// <summary>
    /// A <see cref="Shape"/> representing a compoundShape consisting
    /// of several 'sub' shapes.
    /// </summary>
    public class CompoundShape : Multishape
    {
        #region public struct TransformedShape

        /// <summary>
        /// Holds a 'sub' shape and it's transformation. This TransformedShape can
        /// be added to the <see cref="CompoundShape"/>
        /// </summary>
        public struct TransformedShape
        {
            private Shape shape;
            internal FPVector position;
            internal FPMatrix orientation;
            internal FPMatrix invOrientation;
            internal FPBBox boundingBox;

            /// <summary>
            /// The 'sub' shape.
            /// </summary>
            public Shape Shape { get { return shape; } set { shape = value; } }

            /// <summary>
            /// The position of a 'sub' shape
            /// </summary>
            public FPVector Position { get { return position; } set { position = value; } }

            public FPBBox BoundingBox { get { return boundingBox; } }

            /// <summary>
            /// The inverse orientation of the 'sub' shape.
            /// </summary>
            public FPMatrix InverseOrientation
            {
                get { return invOrientation; }
            }

            /// <summary>
            /// The orienation of the 'sub' shape.
            /// </summary>
            public FPMatrix Orientation
            {
                get { return orientation; }
                set { orientation = value; FPMatrix.Transpose(ref orientation, out invOrientation); }
            }

            public void UpdateBoundingBox()
            {
                Shape.GetBoundingBox(ref orientation, out boundingBox);

                boundingBox.min += position;
                boundingBox.max += position;
            }

            /// <summary>
            /// Creates a new instance of the TransformedShape struct.
            /// </summary>
            /// <param name="shape">The shape.</param>
            /// <param name="orientation">The orientation this shape should have.</param>
            /// <param name="position">The position this shape should have.</param>
            public TransformedShape(Shape shape, FPMatrix orientation, FPVector position)
            {
                this.position = position;
                this.orientation = orientation;
                FPMatrix.Transpose(ref orientation, out invOrientation);
                this.shape = shape;
                this.boundingBox = new FPBBox();
                UpdateBoundingBox();
            }
        }
        #endregion

        private TransformedShape[] shapes;

        /// <summary>
        /// An array conaining all 'sub' shapes and their transforms.
        /// </summary>
        public TransformedShape[] Shapes { get { return this.shapes; } }

        FPVector shifted;
        public FPVector Shift { get { return -FP.One * this.shifted; } }

        private FPBBox mInternalBBox;

        /// <summary>
        /// Created a new instance of the CompountShape class.
        /// </summary>
        /// <param name="shapes">The 'sub' shapes which should be added to this 
        /// class.</param>
        public CompoundShape(List<TransformedShape> shapes)
        {
            this.shapes = new TransformedShape[shapes.Count];
            shapes.CopyTo(this.shapes);

            if (!TestValidity()) 
                throw new ArgumentException("Multishapes are not supported!");

            this.UpdateShape();
        }

        public CompoundShape(TransformedShape[] shapes)
        {
            this.shapes = new TransformedShape[shapes.Length];
            Array.Copy(shapes, this.shapes, shapes.Length);

            if (!TestValidity())
                throw new ArgumentException("Multishapes are not supported!");

            this.UpdateShape();
        }

        private bool TestValidity()
        {
            for (int i = 0; i < shapes.Length; i++)
            {
                if (shapes[i].Shape is Multishape) return false;
            }

            return true;
        }

        public override void MakeHull(ref List<FPVector> triangleList, int generationThreshold)
        {
            List<FPVector> triangles = new List<FPVector>();

            for (int i = 0; i < shapes.Length; i++)
            {
                shapes[i].Shape.MakeHull(ref triangles, 4);
                for (int e = 0; e < triangles.Count; e++)
                {
                    FPVector pos = triangles[e];
                    FPVector.Transform(ref pos,ref shapes[i].orientation,out pos);
                    FPVector.Add(ref pos, ref shapes[i].position,out pos);
                    triangleList.Add(pos);
                }
                triangles.Clear();
            }
        }

        /// <summary>
        /// Translate all subshapes in the way that the center of mass is
        /// in (0,0,0)
        /// </summary>
        private void DoShifting()
        {
            for (int i = 0; i < Shapes.Length; i++) shifted += Shapes[i].position;
            shifted *= (FP.One / shapes.Length);

            for (int i = 0; i < Shapes.Length; i++) Shapes[i].position -= shifted;
        }

        public override void CalculateMassInertia()
        {
            base.inertia = FPMatrix.Zero;
            base.mass = FP.Zero;

            for (int i = 0; i < Shapes.Length; i++)
            {
                FPMatrix currentInertia = Shapes[i].InverseOrientation * Shapes[i].Shape.Inertia * Shapes[i].Orientation;
                FPVector p = Shapes[i].Position * -FP.One;
                FP m = Shapes[i].Shape.Mass;

                currentInertia.M11 += m * (p.y * p.y + p.z * p.z);
                currentInertia.M22 += m * (p.x * p.x + p.z * p.z);
                currentInertia.M33 += m * (p.x * p.x + p.y * p.y);

                currentInertia.M12 += -p.x * p.y * m;
                currentInertia.M21 += -p.x * p.y * m;

                currentInertia.M31 += -p.x * p.z * m;
                currentInertia.M13 += -p.x * p.z * m;

                currentInertia.M32 += -p.y * p.z * m;
                currentInertia.M23 += -p.y * p.z * m;

                base.inertia += currentInertia;
                base.mass += m;
            }
        }


        internal CompoundShape()
        {
        }

        protected override Multishape CreateWorkingClone()
        {
            CompoundShape clone = new CompoundShape();
            clone.shapes = this.shapes;
            return clone;
        }


        /// <summary>
        /// SupportMapping. Finds the point in the shape furthest away from the given direction.
        /// Imagine a plane with a normal in the search direction. Now move the plane along the normal
        /// until the plane does not intersect the shape. The last intersection point is the result.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="result">The result.</param>
        public override void SupportMapping(ref FPVector direction, out FPVector result)
        {
            FPVector.Transform(ref direction, ref shapes[currentShape].invOrientation, out result);
            shapes[currentShape].Shape.SupportMapping(ref direction, out result);
            FPVector.Transform(ref result, ref shapes[currentShape].orientation, out result);
            FPVector.Add(ref result, ref shapes[currentShape].position, out result);
        }

        /// <summary>
        /// Gets the axis aligned bounding box of the orientated shape. (Inlcuding all
        /// 'sub' shapes)
        /// </summary>
        /// <param name="orientation">The orientation of the shape.</param>
        /// <param name="box">The axis aligned bounding box of the shape.</param>
        public override void GetBoundingBox(ref FPMatrix orientation, out FPBBox box)
        {
            box.min = mInternalBBox.min;
            box.max = mInternalBBox.max;

            FPVector localHalfExtents = FP.Half * (box.max - box.min);
            FPVector localCenter = FP.Half * (box.max + box.min);

            FPVector center;
            FPVector.Transform(ref localCenter, ref orientation, out center);

            FPMatrix abs; FPMath.Absolute(ref orientation, out abs);
            FPVector temp;
            FPVector.Transform(ref localHalfExtents, ref abs, out temp);

            box.max = center + temp;
            box.min = center - temp;
        }

        int currentShape = 0;
        List<int> currentSubShapes = new List<int>();

        /// <summary>
        /// Sets the current shape. First <see cref="CompoundShape.Prepare"/> has to be called.
        /// After SetCurrentShape the shape immitates another shape.
        /// </summary>
        /// <param name="index"></param>
        public override void SetCurrentShape(int index)
        {
            currentShape = currentSubShapes[index];
            shapes[currentShape].Shape.SupportCenter(out geomCen);
            geomCen += shapes[currentShape].Position;
        }

        /// <summary>
        /// Passes a axis aligned bounding box to the shape where collision
        /// could occour.
        /// </summary>
        /// <param name="box">The bounding box where collision could occur.</param>
        /// <returns>The upper index with which <see cref="SetCurrentShape"/> can be 
        /// called.</returns>
        public override int Prepare(ref FPBBox box)
        {
            currentSubShapes.Clear();

            for (int i = 0; i < shapes.Length; i++)
            {
                if (shapes[i].boundingBox.Contains(ref box) != FPBBox.ContainmentType.Disjoint)
                    currentSubShapes.Add(i);
            }

            return currentSubShapes.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rayOrigin"></param>
        /// <param name="rayEnd"></param>
        /// <returns></returns>
        public override int Prepare(ref FPVector rayOrigin, ref FPVector rayEnd)
        {
            FPBBox box = FPBBox.SmallBox;

            box.AddPoint(ref rayOrigin);
            box.AddPoint(ref rayEnd);

            return this.Prepare(ref box);
        }


        public override void UpdateShape()
        {
            DoShifting();
            UpdateInternalBoundingBox();
            base.UpdateShape();
        }

        protected void UpdateInternalBoundingBox()
        {
            mInternalBBox.min = new FPVector(FP.MaxValue);
            mInternalBBox.max = new FPVector(FP.MinValue);

            for (int i = 0; i < shapes.Length; i++)
            {
                shapes[i].UpdateBoundingBox();

                FPBBox.CreateMerged(ref mInternalBBox, ref shapes[i].boundingBox, out mInternalBBox);
            }
        }
    }
}
