using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax.StateMachine;
using Spax.Input;

namespace Spax
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerStuff/Data", order = 1)]
    //will contatin various things about the player, namely nonphysics conditions and stats
    public class PlayerData : ScriptableObject
    {
        //static stats of the character pertaining to movement
        public MoveStats moveStats;

        //dynamic condition of the character, based on movement
        public MoveCondition moveCondition;
        public int maxHealth;
        public int curHealth;


        [SerializeField] private StateFrameData currentState;
        public StateFrameData defaultState;

        public StateFrameData[] allStates;
        public StateFrameData[] allAttackStates;

        public InputRecorder inputRecorder;
        public MoveList commandList;
        //what we can cancel into
        [SerializeField] public CancelCondition cancelCondition;

        public void Initialize()
        {
            inputRecorder.Initialize();
            currentState = defaultState;

            int len = allStates.Length;
            for (int i = 0; i < len; i++)
            {
                allStates[i].stateID = (uint)i;
            }


            //commandList.Prepare();

            //Debug.Log(commandList.CheckTest(testInputs));


        }

        private bool inputChanged = false;
        //call to buffer an input for later
        public bool BufferPrev(SpaxInput input, bool inStop = false)
        {
            inputChanged = inputRecorder.RecordInput(input, inStop);
            return inputChanged;

        }

        public float GetGravForce()
        {
            return 1.5f * moveStats.mass;
        }



        //returns nonzero if player can jump
        //0: player cannot jump
        //1: player is grounded, can jump
        //2: player has not used maximum number of air jumps
        public int CanJump()
        {
            if (moveCondition.isGrounded)
            {
                return 1;
            }
            else
               if (moveCondition.curAirJumps < moveStats.maxAirJumps)
            {
                return 2;
            }

            return 0;
        }

        public float GetAcceleration(bool walking = false)
        {
            if (moveCondition.isGrounded)
            {
                if (walking)
                {
                    return moveStats.walkAcceleration;
                }
                return moveStats.groundAcceleration;
            }


            return moveStats.airAcceleration;
        }

        public float GetMaxSpeed(bool walking = false)
        {
            if (moveCondition.isGrounded)
            {
                if (walking)
                {
                    //Debug.Log("wants to walk");
                    return moveStats.maxWalkSpeed;
                }
                return moveStats.maxGroundSpeed;
            }



            return moveStats.maxAirSpeed;
        }
        public float GetFriction()
        {
            if (moveCondition.isGrounded)
            {
                return moveStats.groundFriction;
            }

            return moveStats.airFriction;
        }

        public StateConditions GetStateConditions()
        {
            return this.GetConditionsFromState(currentState);
        }

        private StateConditions GetConditionsFromState(StateFrameData stt)
        {
            if (stt.parentState == null)
            {
                return stt.stateConditions;
            }

            return stt.stateConditions | GetConditionsFromState(stt.parentState);
        }

        //gets string lisitng of previous inputs, for debugging purposes
        public int[] GetStringPrevInputs()
        {
            return inputRecorder.GetInputCodes();
        }

        //public method to interface from
        public StateFrameData GetCommand()
        {

            return FindCommandFromState(currentState);
        }
        //finds the command using previous inputs
        public StateFrameData FindCommandFromState(StateFrameData state)
        {

            int ret = -1;
            //Debug.Log(GetStringPrevInputs());


            ret = commandList.FindCommand(this.GetStringPrevInputs(), moveCondition.facingRight, cancelCondition);



            if (ret > -1)
            {
                AssignNewCurState(allStates[ret]);
                return allStates[ret];
            }

            return null;
        }

        public StateFrameData TransitionState(bool timerIsDone, SpaxInput input)
        {
            return CheckTransitionState(timerIsDone, input, currentState);
        }

        //this transition algorithm feels very bad, rewrite and organize
        private StateFrameData CheckTransitionState(bool timerIsDone, SpaxInput input, StateFrameData srcState)
        {
            int len = srcState._transitions.Count;

            //gets current transition conditions
            TransitionCondition curConditions = GetTransitionCondition(input);
            for (int i = 0; i < len; i++)
            {
                StateFrameData potenState = srcState._transitions[i].Target;
                TransitionCondition compare = srcState._transitions[i].GetConditions();
                InputCodeFlags inputCond = srcState._transitions[i].inputConditions;
                if ((compare & (TransitionCondition.FULL_METER | TransitionCondition.HALF_METER | TransitionCondition.QUART_METER)) == 0)
                {

                    //if there is a button pressed condition
                    if (((compare & TransitionCondition.BUTTON_PRESSED) > 0) && inputChanged && ((inputRecorder.GetPressedButtons() & (Button)compare) > 0))
                    {
                        compare ^= TransitionCondition.BUTTON_PRESSED | ((TransitionCondition)inputRecorder.GetPressedButtons() & compare);

                    }

                    //if all the conditions are met
                    if ((compare & curConditions) == compare)
                    {
                        compare = 0;
                        //Debug.Log("transition :: " + currentState._transitions[i].Target.stateName);

                    }

                    //if the timer is done
                    if (timerIsDone && (compare == TransitionCondition.ON_END))
                    {
                        //if the condiition that the state is done
                        compare ^= TransitionCondition.ON_END;

                    }

                    if (compare == 0)
                    {
                        //get the last input change
                        int fromInput = (int)inputRecorder.GetLatestCode();
                        //flip 6 and 4 direction, if needed
                        if (!moveCondition.facingRight)
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

                        //check it
                        if ((inputCond & (InputCodeFlags)fromInput) == inputCond)
                        {
                            //Debug.Log(compare);
                            return AssignNewCurState(potenState);
                        }
                    }
                }
            }

            //only really reached if state to transition to isn't found
            if (((srcState.stateConditions & StateConditions.NO_PARENT_TRANS) == 0) && (srcState.parentState != null))
            {
                return CheckTransitionState(timerIsDone, input, srcState.parentState);
            }

            return null;
        }

        //sets and returns the new state to current state
        private StateFrameData AssignNewCurState(StateFrameData newState)
        {
            currentState = newState;
            this.cancelCondition = newState.cancelCondition;
            return currentState;
        }

        public StateFrameData GetState()
        {
            return currentState;
        }

        private TransitionCondition GetTransitionCondition(SpaxInput input)
        {
            TransitionCondition ret = 0;

            if (moveCondition.isGrounded)
            {

                ret |= TransitionCondition.GROUNDED;
            }
            else
            {
                ret |= TransitionCondition.AERIAL;
            }
            ret |= (TransitionCondition)input.buttons;

            if (this.CanJump() > 0)
            {
                ret |= TransitionCondition.CANJUMP;
            }

            return ret;
        }

        //edits the cancel condition on hit
        public void AddCancelCondition(CancelCondition cond)
        {
            this.cancelCondition |= cond;
        }

    }
    [System.Serializable]
    public struct MoveStats
    {
        //mass, multiply by gravity to get acceleration to the ground
        //sort of like a gravity scale
        public float mass;

        //max speed on the ground
        public float maxGroundSpeed;
        //max speed while walking on the ground
        public float maxWalkSpeed;

        //max speed in the air
        public float maxAirSpeed;

        //sef-explanitory
        public float maxFallSpeed;

        //how fast the character accelerates when they want to move on the ground
        public float groundAcceleration;
        //how fast the character accelerates when they want to move while walking on the ground
        public float walkAcceleration;

        //how fast the character accelerates when they want to move in the air
        public float airAcceleration;

        //how fast the character decelerates when on the ground
        public float groundFriction;

        //how fast the character decelerates when in the air
        public float airFriction;

        //how fast the character accelerates when they want to move on the ground
        public float jumpForce;

        //how many air jumps the character gets
        public int maxAirJumps;
    }

    [System.Serializable]
    public struct MoveCondition
    {
        //how many air jumps the character has done
        public int curAirJumps;

        //1 if facing right, -1 if facing left
        public bool facingRight;

        //is the character grounded?
        public bool isGrounded;
    }




}