using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spax.Input
{
    public class InputProxyDisplay : MonoBehaviour
    {
        public GameObject stick;
        public SpriteRenderer[] buttons;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void DisplayInput(SpaxInput input)
        {
            DisplayStick(input);
            DisplayButtons(input);
        }


        public void DisplayStick(SpaxInput input)
        {
            Vector3 pos = new Vector3(0f, 0f, 0f); ;
            if ((input.direction & (Direction)56) > 0)
            {
                pos.y = 1f;
            }

            else if ((input.direction & (Direction)448) > 0)
            {
                pos.y = -1f;
            }

            if ((input.direction & (Direction)146) > 0)
            {
                pos.x = 1f;
            }

            else if ((input.direction & (Direction)292) > 0)
            {

                pos.x = -1f;
            }
            stick.transform.localPosition = pos;

        }


        public void DisplayButtons(SpaxInput input)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((input.buttons & (Button)(1 << i)) > 0)
                {
                    buttons[i].color = Color.white;
                }
                else
                {
                    buttons[i].color = Color.black;
                }
            }
        }
    }
}