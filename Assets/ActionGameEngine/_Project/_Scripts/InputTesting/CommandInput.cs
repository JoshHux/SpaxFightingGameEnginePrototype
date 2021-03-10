using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spax.Input
{
    public struct CommandInput
    {
        public CommandPiece[] input;

    }
    [Flags]
    public enum InputTags
    {
        DETECT_AS_4_WAY = 1 << 0,
        MUST_BE_SAME_FRAME = 1 << 1,
        WHEN_RELEASED = 1 << 2,
        ONLY_BUTTON_PRESSED = 1 << 3,
        CHARGE_30F = 1 << 4,
    }
    public struct CommandPiece
    {
        public InputTags tags;
        public Direction direction;
        public Button button;
    }
}