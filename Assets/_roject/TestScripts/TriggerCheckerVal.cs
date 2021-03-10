using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;
using System;
public class TriggerCheckerVal : SpaxBehavior
{
    private FPBoxCollider2D box;
    protected override void OnStart()
    {
        box = this.GetComponent<FPBoxCollider2D>();
    }

    /*public Action<FP> trigger;

    public void OnFixedTriggerEnter(FPCollision2D other)
    {

        trigger.Invoke(other.collider._body.FPPosition.x);
    }

    public void OnFixedTriggerExit()
    {
        trigger.Invoke(0);
    }*/

    public FP GetPosOfOther()
    {
        FPCollider2D hold = FPPhysics2D.OverlapBox(box.Center + box.FPTransform.position, box.size, box.FPTransform.rotation, this.gameObject.layer);
        return (hold != null) ? hold.FPTransform.position.x : FP.Zero;
    }
}
