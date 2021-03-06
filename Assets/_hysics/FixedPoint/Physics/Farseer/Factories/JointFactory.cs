
namespace Spax.Physics2D
{
    /// <summary>
    /// An easy to use factory for using joints.
    /// </summary>
    public static class JointFactory
    {
        #region Motor Joint

        public static MotorJoint CreateMotorJoint(World world, Body bodyA, Body bodyB, bool useWorldCoordinates = false)
        {
            MotorJoint joint = new MotorJoint(bodyA, bodyB, useWorldCoordinates);
            world.AddJoint(joint);
            return joint;
        }

        #endregion

        #region Revolute Joint

        public static RevoluteJoint CreateRevoluteJoint(World world, Body bodyA, Body bodyB, FPVector2 anchorA, FPVector2 anchorB, bool useWorldCoordinates = false)
        {
            RevoluteJoint joint = new RevoluteJoint(bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
            world.AddJoint(joint);
            return joint;
        }

        public static RevoluteJoint CreateRevoluteJoint(World world, Body bodyA, Body bodyB, FPVector2 anchor)
        {
            FPVector2 localanchorA = bodyA.GetLocalPoint(bodyB.GetWorldPoint(anchor));
            RevoluteJoint joint = new RevoluteJoint(bodyA, bodyB, localanchorA, anchor);
            world.AddJoint(joint);
            return joint;
        }


        #endregion

        #region Rope Joint

        public static RopeJoint CreateRopeJoint(World world, Body bodyA, Body bodyB, FPVector2 anchorA, FPVector2 anchorB, bool useWorldCoordinates = false)
        {
            RopeJoint ropeJoint = new RopeJoint(bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
            world.AddJoint(ropeJoint);
            return ropeJoint;
        }

        #endregion

        #region Weld Joint

        public static WeldJoint CreateWeldJoint(World world, Body bodyA, Body bodyB, FPVector2 anchorA, FPVector2 anchorB, bool useWorldCoordinates = false)
        {
            WeldJoint weldJoint = new WeldJoint(bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
            world.AddJoint(weldJoint);
            return weldJoint;
        }

        #endregion

        #region Prismatic Joint

        public static PrismaticJoint CreatePrismaticJoint(World world, Body bodyA, Body bodyB, FPVector2 anchor, FPVector2 axis, bool useWorldCoordinates = false)
        {
            PrismaticJoint joint = new PrismaticJoint(bodyA, bodyB, anchor, axis, useWorldCoordinates);
            world.AddJoint(joint);
            return joint;
        }

        #endregion

        #region Wheel Joint

        public static WheelJoint CreateWheelJoint(World world, Body bodyA, Body bodyB, FPVector2 anchor, FPVector2 axis, bool useWorldCoordinates = false)
        {
            WheelJoint joint = new WheelJoint(bodyA, bodyB, anchor, axis, useWorldCoordinates);
            world.AddJoint(joint);
            return joint;
        }

        public static WheelJoint CreateWheelJoint(World world, Body bodyA, Body bodyB, FPVector2 axis)
        {
            return CreateWheelJoint(world, bodyA, bodyB, FPVector2.zero, axis);
        }

        #endregion

        #region Angle Joint

        public static AngleJoint CreateAngleJoint(World world, Body bodyA, Body bodyB)
        {
            AngleJoint angleJoint = new AngleJoint(bodyA, bodyB);
            world.AddJoint(angleJoint);
            return angleJoint;
        }

        #endregion

        #region Distance Joint

        public static DistanceJoint CreateDistanceJoint(World world, Body bodyA, Body bodyB, FPVector2 anchorA, FPVector2 anchorB, bool useWorldCoordinates = false)
        {
            DistanceJoint distanceJoint = new DistanceJoint(bodyA, bodyB, anchorA, anchorB, useWorldCoordinates);
            world.AddJoint(distanceJoint);
            return distanceJoint;
        }

        public static DistanceJoint CreateDistanceJoint(World world, Body bodyA, Body bodyB)
        {
            return CreateDistanceJoint(world, bodyA, bodyB, FPVector2.zero, FPVector2.zero);
        }

        #endregion

        #region Friction Joint

        public static FrictionJoint CreateFrictionJoint(World world, Body bodyA, Body bodyB, FPVector2 anchor, bool useWorldCoordinates = false)
        {
            FrictionJoint frictionJoint = new FrictionJoint(bodyA, bodyB, anchor, useWorldCoordinates);
            world.AddJoint(frictionJoint);
            return frictionJoint;
        }

        public static FrictionJoint CreateFrictionJoint(World world, Body bodyA, Body bodyB)
        {
            return CreateFrictionJoint(world, bodyA, bodyB, FPVector2.zero);
        }

        #endregion

        #region Gear Joint

        public static GearJoint CreateGearJoint(World world, Body bodyA, Body bodyB, Joint2D jointA, Joint2D jointB, FP ratio)
        {
            GearJoint gearJoint = new GearJoint(bodyA, bodyB, jointA, jointB, ratio);
            world.AddJoint(gearJoint);
            return gearJoint;
        }

        #endregion

        #region Pulley Joint

        public static PulleyJoint CreatePulleyJoint(World world, Body bodyA, Body bodyB, FPVector2 anchorA, FPVector2 anchorB, FPVector2 worldAnchorA, FPVector2 worldAnchorB, FP ratio, bool useWorldCoordinates = false)
        {
            PulleyJoint pulleyJoint = new PulleyJoint(bodyA, bodyB, anchorA, anchorB, worldAnchorA, worldAnchorB, ratio, useWorldCoordinates);
            world.AddJoint(pulleyJoint);
            return pulleyJoint;
        }

        #endregion

        #region MouseJoint

        public static FixedMouseJoint CreateFixedMouseJoint(World world, Body body, FPVector2 worldAnchor)
        {
            FixedMouseJoint joint = new FixedMouseJoint(body, worldAnchor);
            world.AddJoint(joint);
            return joint;
        }

        #endregion
    }
}