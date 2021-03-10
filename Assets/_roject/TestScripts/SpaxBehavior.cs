using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spax
{
    public class SpaxBehavior : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            this.OnAwake();
        }
        void Start()
        {
            SpaxManager[] managers = Object.FindObjectsOfType<SpaxManager>();
            if (managers.Length > 0)
            {
                SpaxManager manager = managers[0];
                if (manager != null)
                {
                    manager.PreUpdate += (() => PreUpdate());
                    manager.SpaxUpdate += (() => SpaxUpdate());
                    manager.PostUpdate += (() => PostUpdate());
                    manager.RenderUpdate += (() => RenderUpdate());
                }
                this.OnStart();
            }
        }

        protected virtual void OnStart() { }
        protected virtual void OnAwake() { }
        protected virtual void PreUpdate() { }
        protected virtual void SpaxUpdate() { }
        protected virtual void PostUpdate() { }
        protected virtual void RenderUpdate() { }
    }
}