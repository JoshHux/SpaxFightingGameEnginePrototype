using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax;
using Spax.Character;

public class PlayerController : SpaxBehavior
{
    private FPRigidBody2D rb;
    public CharacterCondition condition;
    public FPVector2 vel;

    private SpriteRenderer sprite;
    private TriggerCheckerVal wallJumpChecker;

    protected override void OnAwake()
    {
        condition.Initialize();

    }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        rb = this.GetComponent<FPRigidBody2D>();
        sprite = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.transform.GetChild(1).GetComponent<TriggerChecker>().trigger += ((para) => SetGrounded(para));
        wallJumpChecker = this.transform.GetChild(2).GetComponent<TriggerCheckerVal>();
        // rb.SetVelocity(condition.GetVelocity() * Time.fixedDeltaTime);
        //  condition.SetVelocity(rb.velocity);

    }

    // Update is called once per frame

    protected override void PreUpdate() { }
    protected override void SpaxUpdate()
    {
        ApplyGravity();


        if (FP.Abs(condition.GetVelocity().x) < condition.GetMaxSpeed())
        {
            if (Input.GetButton("Right"))
            {
                //condition.AddVelocityX(condition.GetAcceleration());
                // Debug.Log("left");
                if (FP.Abs(condition.GetVelocity().x + condition.GetAcceleration()) > condition.GetMaxSpeed())
                {
                    condition.SetVelocityX(condition.GetMaxSpeed());
                }
                else
                {
                    condition.AddVelocityX(condition.GetAcceleration());

                }

                condition.SetFacing(1);
                sprite.flipX = false;
            }
            else if (Input.GetButton("Left"))
            {
                // condition.AddVelocityX(condition.GetAcceleration() * -1);
                //Debug.Log("right");
                if (FP.Abs(condition.GetVelocity().x - condition.GetAcceleration()) > condition.GetMaxSpeed())
                {
                    condition.SetVelocityX(-condition.GetMaxSpeed());

                }
                else
                {
                    condition.AddVelocityX(condition.GetAcceleration() * -1);

                }

                condition.SetFacing(-1);
                sprite.flipX = true;
            }
            else
            {
                if (FP.Abs(condition.GetVelocity().x) <= condition.GetFriction())
                {
                    condition.SetVelocityX(FP.Zero);

                }
                else
                {
                    condition.AddVelocityX(condition.GetFriction() * -1 * condition.Getfacing());

                }
            }

            if (Input.GetButtonDown("Jump")/* && (condition.CanJump(wallJumpChecker.GetPosOfOther() - rb.position.x) > 0)*/)
            {
                //int hold = condition.CanJump((((wallJumpChecker.GetPosOfOther() - rb.position.x) * Input.GetAxis("Horizontal")) < 0) ? (wallJumpChecker.GetPosOfOther() - rb.position.x) : 0);
                int hold = condition.CanJump(this.WallJumpRef(wallJumpChecker.GetPosOfOther()));
                // condition.AddVelocityX(condition.GetAcceleration() * -1);
                if (hold > 0 && hold != 3)
                {
                    Debug.Log(hold);
                    condition.SetVelocityY(10);
                }
            }
        }

        rb.SetVelocity(condition.GetVelocity());
    }
    protected override void PostUpdate()
    {
        condition.SetVelocity(rb.velocity);
        vel = rb.velocity;
    }
    protected override void RenderUpdate() { }

    private void ApplyGravity()
    {
        condition.SetVelocityY((condition.GetMass() * -1) + condition.GetVelocity().y);
        //condition.SetVelocityY(-10);
    }

    public void SetGrounded(bool grnd)
    {
        condition.SetGrounded(grnd);
    }

    private FP WallJumpRef(FP colRef)
    {
        int holdDir = Input.GetButton("Right") ? 1 : Input.GetButton("Left") ? -1 : 0;
        return (colRef - rb.position.x) * holdDir;
    }
}
