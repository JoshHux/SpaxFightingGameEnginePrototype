using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spax.Input;

namespace Spax.StateMachine
{

    [CreateAssetMenu(fileName = "frameData", menuName = "PlayerStuff/FrameData", order = 1)]

    public class StateFrameData : ScriptableObject
    {

        [System.Serializable]
        public class Transition
        {
            public bool Enabled;
            public StateFrameData Target;
            public uint TargetFrame;
            public uint MinFrame;
            public uint meterReq;
            public List<TransitionCondition> Conditions;
            public InputCodeFlags inputConditions;

            public TransitionCondition GetConditions()
            {
                int len = Conditions.Count;
                TransitionCondition ret = 0;

                for (int i = 0; i < len; i++)
                {
                    ret |= Conditions[i];
                }

                return ret;

            }

            
        }


        public string stateName = "";
        public uint stateID;
        public StateFrameData parentState;

        public EnterStateConditions enterStateConditions;
        public ExitStateConditions exitStateConditions;
        public StateConditions stateConditions;
        public CancelCondition cancelCondition = (CancelCondition)255;
        public List<Transition> _transitions;

        public int duration;
        public CharacterFrame[] Frames;

        void OnValidate()
        {
            PrepFrames();

            //commandList.Prepare();
        }

        private void PrepFrames()
        {
            int len = Frames.Length;

            for (int i = 0; i < len; i++)
            {
                Frames[i].Prepare();
            }
        }

        //for finding the frame wanted
        //we do this so we minimize the number of frames that have no flags
        public bool FindFrame(int framesElapsed, ref CharacterFrame assign)
        {
            //think about chaning to dictionary using atFrame as key, if dictionary is more performant
            //get length of array
            int len = Frames.Length;
            for (int i = 0; i < len; i++)
            {
                if (Frames[i].atFrame == framesElapsed)
                {
                    assign = Frames[i];
                    return true;
                }
            }

            return false;
        }


    }

}