using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spax.Input
{
    [System.Serializable]
    public class InputRecorder
    {
        public InputStorage prevInput;

        public void Initialize()
        {
            prevInput = new InputStorage();
        }

        public bool RecordInput(SpaxInput input, bool inStop = false)
        {
            return prevInput.StoreInput(input, inStop);
        }

        public int[] GetInputCodes()
        {
            return prevInput.GetReverseCodeArray();
        }

        public Button GetPressedButtons()
        {
            return prevInput.GetChangedPressed();
        }

        public InputCodeFlags GetLatestCode()
        {
            return prevInput.GetLatestCode();
        }
    }

    [System.Serializable]
    public class InputStorage
    {
        [SerializeField]
        private SpaxInput[] prevInputs;
        [SerializeField]
        private InputCode[] prevInputsCode;
        [SerializeField]
        private int arrayPos;
        [SerializeField]
        private int codeArrayPos;
        private SpaxInput changedPressed;

        private SpaxInput changedReleased;

        public InputStorage()
        {
            prevInputs = new SpaxInput[10];
            prevInputsCode = new InputCode[20];
            for (int i = 0; i < 20; i++)
            {
                prevInputsCode[i].framesHeld = 0;
                prevInputsCode[i].code = "";
            }
            arrayPos = 0;
            codeArrayPos = 0;

        }

        private int IncrementByPos(int index, bool byCodeArray)
        {

            int bound = 0;
            if (byCodeArray)
            {
                bound = prevInputsCode.Length - 1;
            }
            else
            {
                bound = prevInputs.Length - 1;
            }

            if (++index > bound)
            {
                index = 0;
            }
            return index;
        }

        private int DecrementByPos(int index, bool byCodeArray)
        {

            int bound = 0;
            if (byCodeArray)
            {
                bound = prevInputsCode.Length - 1;
            }
            else
            {
                bound = prevInputs.Length - 1;
            }

            if (--index < 0)
            {
                index = bound;
            }
            return index;
        }

        //returns true if input was successfully recorded
        public bool StoreInput(SpaxInput input, bool inStop = false)
        {
            SpaxInput curInput = prevInputs[arrayPos];
            bool ret = false;

            if (!curInput.IsEqual(input))
            {

                changedPressed.buttons = (curInput.buttons ^ input.buttons) & input.buttons;
                changedPressed.direction = (curInput.direction ^ input.direction) & input.direction;


                changedReleased.buttons = (curInput.buttons ^ input.buttons) & curInput.buttons;
                changedReleased.direction = (curInput.direction ^ input.direction) & curInput.direction;


                //Debug.Log(input.direction + " " + input.buttons);
                arrayPos = this.IncrementByPos(arrayPos, false);
                prevInputs[arrayPos] = input;
                this.SetInputCode();
                ret = true;
            }

            if (!inStop && prevInputsCode[codeArrayPos].framesHeld < 128)
            {
                prevInputsCode[codeArrayPos].framesHeld++;
            }
            return ret;
        }

        private void SetInputCode()
        {
            SpaxInput curInput = prevInputs[arrayPos];

            //records released inputs
            int change = 0;
            if (changedReleased.direction > Direction.N || changedReleased.buttons != 0)
            {

                change = (((int)changedReleased.direction & 510) << 2) | ((int)changedReleased.buttons << 11);

                //Debug.Log("release :: " + (InputCodeFlags)change);


                int checkPrev = ((int)prevInputsCode[codeArrayPos].inputCode >> 3);
                int checkCode = change >> 3;

                if (checkPrev == checkCode)
                {
                    change |= 1;
                }

                change |= 2;


                codeArrayPos = this.IncrementByPos(codeArrayPos, true);
                prevInputsCode[codeArrayPos].inputCode = change;
                prevInputsCode[codeArrayPos].framesHeld = 0;
            }

            //records pressed inputs
            change = 0;

            if (changedPressed.direction > Direction.N || changedPressed.buttons != 0)
            {

                change = (((int)changedPressed.direction & 510) << 2) | ((int)changedPressed.buttons << 11);

                //Debug.Log("press :: " + (InputCodeFlags)change);

                int checkPrev = ((int)prevInputsCode[codeArrayPos].inputCode >> 3);
                int checkCode = change >> 3;

                if ((checkPrev ^ checkCode) == 0)
                {
                    change |= 1;
                }

                change |= 4;


                codeArrayPos = this.IncrementByPos(codeArrayPos, true);
                prevInputsCode[codeArrayPos].inputCode = change;
                prevInputsCode[codeArrayPos].framesHeld = 0;
            }

        }

        public int[] GetReverseCodeArray()
        {
            List<int> ret = new List<int>();

            int index = codeArrayPos;
            int endInd = this.IncrementByPos(codeArrayPos, true);

            string debug = "";

            //add the current controller condition purely for command normals
            int toAdd = (((int)prevInputs[arrayPos].direction & 510) << 2) | ((int)prevInputs[arrayPos].buttons << 11);
            debug += (InputCodeFlags)toAdd + " | ";

            ret.Add(toAdd);


            //guarentees at least 2 elements
            //do
            while (index != endInd && prevInputsCode[index].framesHeld <= 7)

            {
                toAdd = prevInputsCode[index].inputCode;
                if (toAdd < 1)
                {
                    //Debug.Log("broken");
                    break;
                }

                debug += (InputCodeFlags)toAdd + " | ";

                ret.Add(toAdd);
                //Debug.Log("making array: " + toAdd.Length);

                index = this.DecrementByPos(index, true);
            }
            //Debug.Log(debug);
            return ret.ToArray();
        }

        public Button GetChangedPressed()
        {
            return changedPressed.buttons;
        }

        public InputCodeFlags GetLatestCode()
        {
            return (InputCodeFlags)prevInputsCode[codeArrayPos].inputCode;
        }

    }

    [System.Serializable]
    struct InputCode
    {
        public short framesHeld;
        public string code;
        public int inputCode;

        //call this to flip backwards and forwards for the inputcode
        public static int FlipBackForth(int code)
        {
            //exact integer value to mask every left/right combination
            int mask = 1752;
            //Debug.Log(mask);

            //has the left/right directional input from the command
            int maskHelper = (mask & code);

            //removes the left/right directional input
            code -= maskHelper;

            mask = (maskHelper << 1 | maskHelper >> 1) & mask;
            code |= mask;
            //Debug.Log("facing left :: " + (InputCodeFlags)mask);
            return code;
        }
    }
    //using neither RELEASED not PRESSED flag indicates command normal
    [Flags]
    public enum InputCodeFlags
    {
        NO_INTERRUPTS = 1 << 0,
        RELEASED = 1 << 1,
        PRESSED = 1 << 2,
        F = 1 << 3,//2
        B = 1 << 4,//4
        U = 1 << 5,//8
        UF = 1 << 6,//16
        UB = 1 << 7,//32
        D = 1 << 8,//64
        DF = 1 << 9,//128
        DB = 1 << 10,//256
        I = 1 << 11,
        J = 1 << 12,
        K = 1 << 13,
        L = 1 << 14,// E instead of D just in case D would get confused with D in Directions
        W = 1 << 15,
        X = 1 << 16,
        Y = 1 << 17,
        Z = 1 << 18,
        //only for transitioning states, doesn't have to do with input motions
        CURRENTLY_HELD = 1 << 19,
    }
}