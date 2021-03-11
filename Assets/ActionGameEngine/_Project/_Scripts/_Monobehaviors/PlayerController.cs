using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Spax.StateMachine;
using Spax.Input;

namespace Spax
{
    public class PlayerController : SpaxBehavior
    {
        [SerializeField] private SpriteRenderer sprite;
        private FPRigidBody2D rb;
        private TestControls controls;
        [SerializeField] private FrameTimer timer;
        //controls hit and block-stop
        [SerializeField] private FrameTimer stopTimer;


        private Animator animator;

        public PlayerData data;

        public TMP_Text txt;

        //input that is parsed at the beginning on the frame
        [SerializeField] private SpaxInput input;
        //real-time controller state
        [SerializeField] private SpaxInput asyncInput;

        private Hitbox[] hitboxObj;
        [SerializeField] private FPVector2 calcVelocity;

        //used check if we need to apply at this frame
        private CharacterFrame refFrame;

        protected override void OnAwake()
        {
            controls = new TestControls();
            timer = new FrameTimer();
            //ENABLE ALL ACTIONS BEFORE USING CALLBACKS, I DON'T KNOW WHY I HAVE TO DO IT, I JUST DO
            //controls.Keyboard.Move.Enable();
            //controls.Keyboard.Jump.Enable();

            controls.Keyboard.Move.performed += ctx => CallbackDirectionInput(ctx.ReadValue<Vector2>());
            controls.Keyboard.Move.canceled += ctx => CallbackDirectionInput(ctx.ReadValue<Vector2>());

            controls.Keyboard.Jump.performed += ctx => CallbackButtonInput(Button.W);
            controls.Keyboard.Jump.canceled += ctx => CallbackButtonInput(Button.W);

            controls.Keyboard.LightAttack.performed += ctx => CallbackButtonInput(Button.I);
            controls.Keyboard.LightAttack.canceled += ctx => CallbackButtonInput(Button.I);
            controls.Keyboard.MediumAttack.performed += ctx => CallbackButtonInput(Button.J);
            controls.Keyboard.MediumAttack.canceled += ctx => CallbackButtonInput(Button.J);

        }

        // Start is called before the first frame update
        protected override void OnStart()
        {
            rb = GetComponent<FPRigidBody2D>();
            animator = GetComponent<Animator>();
            data.Initialize();
            this.ApplyNewState(data.GetState());
            //animator.SetBool("TransitionState", true);


            stopTimer.onEnd += OnHitstopEnd;

            data.moveCondition.facingRight = false;
            OnNonGrounded();


            //find the gameobject that holds all hitboxes
            foreach (Transform child in this.transform)
            {
                if (child.tag == "Hitboxes")
                {
                    hitboxObj = child.GetComponentsInChildren<Hitbox>();
                }

            }
        }

        //call ONCE before your fixed update stuff, just an organizational thing
        protected override void PreUpdate()
        {
            //tick our timers
            if (!stopTimer.TickTimer())
            {
                //tick the timer
                timer.TickTimer();
            }

            //buffer the input
            newInput = data.BufferPrev(input);

            //getting the input from outside of tick, locking in input for the current tick
            //resetting the changes made by assigning the current controller state
            input = asyncInput;



            StateFrameData newState = null;
            if (newInput)
            {
                newState = data.GetCommand();
            }
            //finding a command
            if (newState == null)
            {
                newState = data.TransitionState(!timer.IsTicking(), input);


            }


            if (newState != null)
            {
                ApplyNewState(newState);
            }

            if (!stopTimer.IsTicking())
            {
                //get the velocity from the rigidbody to manipulate
                calcVelocity = rb.velocity;
                if (data.GetState().FindFrame(timer.ElapsedTime(), ref refFrame))
                {
                    ApplyStateFrame(refFrame);

                }
            }

        }


        // Update is called once per frame
        protected override void SpaxUpdate()
        {


            if (!stopTimer.IsTicking())
            {



                StateConditions curCond = data.GetStateConditions();

                //for holding change in velocity for faster access
                float accel = 0f;
                //holds the x-axis input for faster access
                int xInput = input.X();

                //if there is a frame to apply some stuff, do it


                //if the state allows for gravity application
                if ((curCond & StateConditions.APPLY_GRAV) > 0)
                {
                    if ((calcVelocity.y - data.GetGravForce()) <= (-1 * data.moveStats.maxFallSpeed))
                    {
                        calcVelocity.y = -1 * data.moveStats.maxFallSpeed;
                    }
                    else
                    {
                        calcVelocity.y -= data.GetGravForce();
                    }
                }

                //read if the player wants to move
                if (xInput != 0)
                {
                    //if the state allows for movement
                    if ((curCond & StateConditions.CAN_MOVE) > 0)
                    {

                        bool isWalking = (curCond & StateConditions.WALKING) == StateConditions.WALKING;

                        //hold the acceleration for quicker access
                        accel = data.GetAcceleration(isWalking);

                        //Debug.Log(isWalking+" | "+accel);


                        if (FPMath.Abs(calcVelocity.x + accel * xInput) >= (FP)data.GetMaxSpeed(isWalking))
                        {
                            calcVelocity.x = data.GetMaxSpeed(isWalking) * xInput;
                        }
                        else
                        {
                            calcVelocity.x += accel * xInput;
                        }
                    }


                }

                //if the state applies friction
                if ((curCond & StateConditions.APPLY_FRICTION) > 0)
                {
                    //accel will be (at least) close to 0 if the player an and wants to move
                    if (Mathf.Abs(accel) < 0.00001f)
                    {
                        //hold the friction for quicker access
                        accel = data.GetFriction();

                        if ((FPMath.Abs(calcVelocity.x) - accel) <= 0f)
                        {
                            calcVelocity.x = 0f;
                        }
                        else
                        {
                            //multiply by normalized to counter the current velocity
                            calcVelocity.x -= accel * calcVelocity.normalized.x;
                        }
                    }
                }
                //assign the new calculated velocity to the rigidbody
                rb.velocity = calcVelocity;


                //txt.text = data.GetStringPrevInputs();



            }

        }

        protected override void PostUpdate()
        {

        }


        protected override void RenderUpdate()
        {
            if (!stopTimer.IsTicking())
            {

                ApplyAnimationParameters();

                //sets new animation state is a new state has been set
                if (setNewState)
                {
                    animator.PlayInFixedTime(data.GetState().stateName);
                    //Debug.Log(data.GetState().stateName);
                    setNewState = false;
                }

                animator.speed = 1.0f;
                animator.Update(Time.fixedDeltaTime);
                animator.speed = 0.0f;
            }
        }

        void OnEnable()
        {
            //I CAN APPARENTLY JUST ENABLE THE WHOLE THING? WHAT!?
            controls.Enable();
        }
        void OnDisable()
        {
            //doing this just for safety
            controls.Disable();
        }
        bool newInput = false;


        //call only once to apply the current stuff to parameters
        private void ApplyAnimationParameters()
        {/*
            animator.SetBool("WantsToMoveX", (input.X() != 0));
            animator.SetBool("IsGrounded", data.moveCondition.isGrounded);

            Vector2 forAnim = FixedPointExtensions.ToVector(rb.velocity);
            animator.SetFloat("XVelocity", forAnim.x);
            animator.SetFloat("YVelocity", forAnim.y);

            animator.SetInteger("CurrentState", (int)data.GetState().stateID);
*/
            if (newInput)
            {
                //Debug.Log("reached");

                //animator.SetBool("JumpButton", (input.buttons & Button.W) > 0);

            }
        }

        private bool setNewState = false;
        private void ApplyNewState(StateFrameData newState)
        {
            //animator.SetTrigger("StateChanged");
            //currentState = newState;
            if (newState.duration > 0)
            {
                //-1 is there to have the elapsed time equal the index
                timer.StartTimer(newState.duration - 1);
                //Debug.Log(newState.Frames.Length);
            }

            if ((newState.enterStateConditions & EnterStateConditions.KILL_Y_MOMENTUM) > 0)
            {
                calcVelocity.y = 0f;
            }
            if ((newState.enterStateConditions & EnterStateConditions.KILL_X_MOMENTUM) > 0)
            {
                calcVelocity.x = 0f;
            }
            setNewState = true;

        }



        private void ApplyStateFrame(CharacterFrame currentFrame)
        {

            if ((currentFrame.Flags & FrameFlags.AUTO_JUMP) > 0)
            {
                //we want to jump, get the jump type
                int jumpId = data.CanJump();

                //may swap the next series of if statements to switch
                //check if we can jump (0 means we can't)
                if (jumpId > 0)
                {
                    //player wants to air jump
                    if (jumpId == 2)
                    {
                        //increment the number of current air jumps
                        data.moveCondition.curAirJumps += 1;
                        //Debug.Log("jumping");
                    }

                    //gets the jump velocity and sets it to the y value.
                    calcVelocity.y = data.moveStats.jumpForce;
                }
            }

            //if the state applies its own custom velocity
            if ((currentFrame.Flags & FrameFlags.APPLY_VEL) > 0)
            {
                int facing = 1;

                if (!data.moveCondition.facingRight)
                {
                    facing = -1;
                }

                if ((currentFrame.Flags & FrameFlags.SET_VEL) == FrameFlags.SET_VEL)
                {
                    calcVelocity = new FPVector2(facing * currentFrame.velocity.x, currentFrame.velocity.y);
                }
                else
                {
                    calcVelocity += new FPVector2(facing * currentFrame.velocity.x, currentFrame.velocity.y);
                }

            }

            //if the state applies its own custom velocity
            if ((currentFrame.Flags & FrameFlags.CHANGE_DIRECTION) > 0)
            {
                //Debug.Log("trying to turn around");
                //statement to change facing
                if (data.moveCondition.facingRight)
                {
                    //face left
                    sprite.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    //sprite.flipX=true;
                    data.moveCondition.facingRight = false;

                }
                else
                {
                    //face right
                    sprite.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    //sprite.flipX=false;
                    data.moveCondition.facingRight = true;
                }

            }


            //check if there are hitboxes
            if (currentFrame.HasHitboxes())
            {
                //sets the data of the bitboxes
                //get the array for easier access
                HitBoxData[] hitBoxData = currentFrame.hitboxes;
                //get the length of for the for loop
                int len = hitBoxData.Length;

                for (int i = 0; i < len; i++)
                {
                    this.hitboxObj[i].SetBoxData(hitBoxData[i], data.moveCondition.facingRight);
                }
            }

        }

        //callback for reading and recording directional inputs
        public void CallbackDirectionInput(Vector2 ctx)
        {
            //Debug.Log("called :: " + ctx);
            //ctx.x *= data.moveCondition.facing;
            int newDir = 1;

            if (ctx.y < 0)
            {
                newDir = newDir << 6;
            }
            else if (ctx.y > 0)
            {
                newDir = newDir << 3;
            }

            if (ctx.x < 0)
            {
                newDir = newDir << 2;
            }
            else if (ctx.x > 0)
            {
                newDir = newDir << 1;
            }

            asyncInput.direction = (Direction)newDir;
            //theres the possibility that the a button is tapped before the controller state is registered in syncInput
            //this scenario will result in an eaten input
            //to mitigate this, we will always add the new input to the read sync input and then re-assign the current controller state
            //after the synchronized input is parsed
            input.direction = (Direction)newDir;
        }

        public void CallbackButtonInput(Button button)
        {
            //Debug.Log("jump");
            //little delicate thatn I would like, but it works, and theoretically won't break, but still not as safe as I would like
            asyncInput.buttons ^= button;

            //theres the possibility that the a button is tapped before the controller state is registered in syncInput
            //this scenario will result in an eaten input
            //to mitigate this, we will always add the new input to the read sync input and then re-assign the current controller state
            //after the synchronized input is parsed
            input.buttons |= (button);
        }

        //call when player becomes grounded
        public void OnGrounded()
        {
            data.moveCondition.curAirJumps = 0;
            data.moveCondition.isGrounded = true;
        }

        //call when player stops being grounded
        public void OnNonGrounded()
        {
            data.moveCondition.isGrounded = false;
        }

        public void OnHit(HitBoxData hitBox)
        {
            data.AddCancelCondition(hitBox.onHitCancel);
            stopTimer.StartTimer(hitBox.hitstop);
            rb.velocity = FPVector2.zero;
            animator.SetBool("TransitionState", false);

        }

        private void OnHitstopEnd(object sender)
        {
            rb.velocity = calcVelocity;
            animator.SetBool("TransitionState", true);

            //Debug.Log(rb.velocity + " | " + calcVelocity);

        }
    }
}
