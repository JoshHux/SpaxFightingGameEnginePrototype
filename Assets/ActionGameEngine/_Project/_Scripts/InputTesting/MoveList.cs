using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spax.Input
{
    [System.Serializable]
    public class MoveList
    {
        [SerializeField]
        private CommandMove[] moveList;

        public void Initialize()
        {
            for (int i = 0; i < moveList.Length; i++)
            {
                moveList[i].Initialize();
            }
        }

        public int FindCommand(int[] inputs, bool facingRight, CancelCondition cond)
        {
            for (int i = 0; i < moveList.Length; i++)
            {
                //checks if the move can be cancelled into
                if ((cond & moveList[i].condition) == moveList[i].condition)
                {
                    int ret = moveList[i].CheckCommand(inputs, facingRight);
                    if (ret > -1)
                    {
                        return ret;
                    }
                }
            }
            return -1;
        }
    }

    [System.Serializable]
    class CommandMove
    {
        public string moveName;
        public InputCodeFlags[] command;
        public int state;
        public CancelCondition condition;

        public void Initialize()
        {
            for (int i = 0; i < command.Length; i++)
            {
                Debug.Log(command[i]);
            }
        }

        public int CheckCommand(int[] inputs, bool facingRight)
        {
            //string print = "";
            int i = 0;
            string debug = "";
            for (int j = 0; j < inputs.Length; j++)
            {
                debug += ((InputCodeFlags)inputs[j]) + " | ";
                int fromInput = inputs[j];
                int fromCommand = (int)command[i];

                if (!facingRight)
                {
                    //exact integer value to mask every left/right combination
                    int mask = 1752;
                    //Debug.Log(mask);

                    //has the left/right directional input from the command
                    int maskHelper = (mask & fromInput);

                    //removes the left/right directional input
                    fromInput -= maskHelper;

                    mask = (maskHelper << 1 | maskHelper >> 1) & mask;
                    fromInput |= mask;
                    //Debug.Log("facing left :: " + (InputCodeFlags)mask);
                }




                if ((fromInput & fromCommand) == fromCommand)
                {
                    //Debug.Log(inputs[j] + " :: " + command[i] + " || " + i);
                    i++;
                    //this is if the motion and button are pressed on the same frame, stalls by one input
                    j--;
                    if (i == command.Length)
                    {
                        //Debug.Log(debug);
                        return state;
                    }
                }
                //makes sure that the most recent inputs match, prevents a scenario where a directional input comes after the button press and triggers the command
                else if (j == 1 && i == 0)
                {
                    //Debug.Log(debug);

                    return -1;
                }
                //print += inputs[j] + " || ";
            }
            //Debug.Log(debug);

            //Debug.Log(print);
            return -1;
        }
    }

    [Flags]
    public enum CancelCondition
    {
        //move cancelables
        NORMAL = 1 << 0,
        COMMAND_NORMAL = 1 << 1,
        SPECIAL = 1 << 2,
        EX_SPECIAL = 1 << 3,
        SUPER = 1 << 4,
        JUMP = 1 << 5,
        MOVEMENT = 1 << 6,
        GUARD = 1 << 7,
        GROUNDED = 1 << 8,
        NORM_LV1 = 1 << 0 | 1 << 9,
        NORM_LV2 = 1 << 0 | 1 << 10,
        NORM_LV3 = 1 << 0 | 1 << 11,
        NORM_LV4 = 1 << 0 | 1 << 12,
        NORM_LV5 = 1 << 0 | 1 << 13,
        NORM_LV6 = 1 << 0 | 1 << 14,
        ETC = 1 << 15,

    }
}
