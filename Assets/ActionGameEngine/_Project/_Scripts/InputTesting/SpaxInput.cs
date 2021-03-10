using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spax.Input
{

    [Flags]
    public enum Direction
    {

        N = 1 << 0,//1
        F = 1 << 1,//2
        B = 1 << 2,//4
        U = 1 << 3,//8
        UF = 1 << 4,//16
        UB = 1 << 5,//32
        D = 1 << 6,//64
        DF = 1 << 7,//128
        DB = 1 << 8,//256
    }

    [Flags]
    public enum Button
    {
        I = 1 << 0,
        J = 1 << 1,
        K = 1 << 2,
        L = 1 << 3,// E instead of D just in case D would get confused with D in Directions
        W = 1 << 4,
        X = 1 << 5,
        Y = 1 << 6,
        Z = 1 << 7,
    }

    [System.Serializable]
    public struct SpaxInput
    {
        public Direction direction;
        public Button buttons;

        public bool IsEqual(SpaxInput other)
        {
            return (this.direction == other.direction) && (this.buttons == other.buttons);
        }

        public int X(){
            if((direction&(Direction)146)>0){
                return 1;
            }

            
            if((direction&(Direction)292)>0){
                return -1;
            }

            return 0;
        }

    }
}
