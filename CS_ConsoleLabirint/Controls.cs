using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    static class Controls
    {
        public static Direction GetDirection(ConsoleKey readenKey)
        {
            switch (readenKey)
            {
                case ConsoleKey.UpArrow:
                    return Direction.up;
                case ConsoleKey.RightArrow:
                    return Direction.right;
                case ConsoleKey.DownArrow:
                    return Direction.down;
                case ConsoleKey.LeftArrow:
                    return Direction.left;
                default:
                    return Direction.noDirrection;
            }
        }
    }
}
