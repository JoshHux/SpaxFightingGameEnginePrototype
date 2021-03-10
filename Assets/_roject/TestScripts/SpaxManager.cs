using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;
using System;

namespace Spax
{
    public class SpaxManager : MonoBehaviour
    {
        public Action PreUpdate;
        public Action SpaxUpdate;
        public Action PostUpdate;
        public Action RenderUpdate;

        private IPhysicsManager physics;
        void Awake()
        {
            instance = this;

            FixedPointConfig currentConfig = ActiveConfig;
            FP lockedTimeStep = currentConfig.lockedTimeStep;


            if (currentConfig.physics2DEnabled || currentConfig.physics3DEnabled)
            {
                PhysicsManager.New(currentConfig);
                PhysicsManager.instance.LockedTimeStep = lockedTimeStep;
                PhysicsManager.instance.Init();
            }
            physics = PhysicsManager.instance;
        }

        void FixedUpdate()
        {
            PreUpdate?.Invoke();
            SpaxUpdate?.Invoke();
            physics.UpdateStep();
            PostUpdate?.Invoke();
            RenderUpdate?.Invoke();
        }

        public FixedPointConfig customConfig;

        public static FixedPointConfig FixedPointCustomConfig = null;
        private FixedPointConfig ActiveConfig
        {
            get
            {
                if (FixedPointCustomConfig != null)
                {
                    customConfig = FixedPointCustomConfig;
                    FixedPointCustomConfig = null;
                }

                if (customConfig != null)
                {
                    return customConfig;
                }

                return FixedPointGlobalConfig;
            }
        }

        public static FixedPointConfig _FixedPointGlobalConfig;

        public static FixedPointConfig FixedPointGlobalConfig
        {
            get
            {
                if (_FixedPointGlobalConfig == null)
                {
                    _FixedPointGlobalConfig = (FixedPointConfig)Resources.Load(serverSettingsAssetFile, typeof(FixedPointConfig));
                }

                return _FixedPointGlobalConfig;
            }
        }
        private const string serverSettingsAssetFile = "FixedPointGlobalConfig";

        public static FixedPointConfig Config
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }

                return instance.ActiveConfig;
            }
        }

        private static SpaxManager instance;
    }
}