using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Spax.Input
{
    public class InputReader : MonoBehaviour
    {
        private SpaxInput asyncInput;
        private SpaxInput syncInput;
        private TestControls controls;
        public InputProxyDisplay proxy;

        public InputRecorder inputRecorder;
        public MoveList moveList;
        public bool isFacingRight;

        void Awake()
        {
            controls = new TestControls();

            controls.Keyboard.Move.performed += ctx => CallbackDirectionInput(ctx.ReadValue<Vector2>());
            controls.Keyboard.Move.canceled += ctx => CallbackDirectionInput(ctx.ReadValue<Vector2>());

            controls.Keyboard.Jump.performed += ctx => CallbackButtonInput(Button.W);
            controls.Keyboard.Jump.canceled += ctx => CallbackButtonInput(Button.W);

            controls.Keyboard.LightAttack.performed += ctx => CallbackButtonInput(Button.I);
            controls.Keyboard.LightAttack.canceled += ctx => CallbackButtonInput(Button.I);

            inputRecorder.Initialize();
            //moveList.Initialize();
            isFacingRight = true;
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Keyboard.current[Key.LeftShift].wasPressedThisFrame)
            {
                isFacingRight = !isFacingRight;
            }
            syncInput = asyncInput;

            bool newInput = inputRecorder.RecordInput(syncInput);

            if (newInput)
            {
                SearchForCommand();
            }

            proxy.DisplayInput(syncInput);

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
        }

        public void CallbackButtonInput(Button button)
        {
            //Debug.Log("jump");
            //little delicate thatn I would like, but it works, and theoretically won't break, but still not as safe as I would like
            /*if (isPressed)
            {
                //bitwise "or" to add the desired button to the asyncInput
                asyncInput.buttons |= button;
            }
            else
            {*/
            //bitwise "xor" to remove the desired button from the asyncInput
            //this is the part I'm worried about, I needed a way to remove the enum from the bitwise, but
            asyncInput.buttons ^= button;
            //}
        }

        public void SearchForCommand()
        {
            int found = moveList.FindCommand(inputRecorder.GetInputCodes(), isFacingRight, (CancelCondition)255);
            if (found > -1)
            {
                Debug.Log(found);
            }
        }
    }
}