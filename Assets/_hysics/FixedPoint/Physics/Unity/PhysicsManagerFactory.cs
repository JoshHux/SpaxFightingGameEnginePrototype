namespace Spax {

    /**
     *  @brief Manages physics simulation.
     **/
    public class PhysicsManager {

        /**
         *  @brief Indicates the type of physics simulations: 2D or 3D.
         **/
        public enum PhysicsType {W_2D, W_3D};

        /**
         *  @brief Returns a proper implementation of {@link IPhysicsManager}.
         **/
        public static IPhysicsManager instance;

        /**
         *  @brief Instantiates a new {@link IPhysicsManager}.
         *  
         *  @param FixedPointConfig Indicates if is a 2D or 3D world.
         **/
        public static IPhysicsManager New(FixedPointConfig FixedPointConfig) {
            if (FixedPointConfig.physics3DEnabled) {
                instance = new PhysicsWorldManager();
                instance.Gravity = FixedPointConfig.gravity3D;
                instance.SpeculativeContacts = FixedPointConfig.speculativeContacts3D;
            } else if (FixedPointConfig.physics2DEnabled) {
                instance = new Physics2DWorldManager();
                instance.Gravity = new FPVector(FixedPointConfig.gravity2D.x, FixedPointConfig.gravity2D.y, 0);
                instance.SpeculativeContacts = FixedPointConfig.speculativeContacts2D;
            }

            return instance;
        }

        /**
         *  @brief Instantiates a 3D physics for tests purpose.
         **/
        internal static void InitTest3D() {
            instance = new PhysicsWorldManager();
            instance.Gravity = new FPVector(0, -10, 0);
            instance.LockedTimeStep = 0.02f;
            instance.Init();
        }

        /**
         *  @brief Instantiates a 2D physics for tests purpose.
         **/
        internal static void InitTest2D() {
            instance = new Physics2DWorldManager();
            instance.Gravity = new FPVector(0, -10, 0);
            instance.LockedTimeStep = 0.02f;
            instance.Init();
        }

    }

}