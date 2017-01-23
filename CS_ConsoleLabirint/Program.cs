using System;
using LabyrinthModel;

namespace CS_ConsoleLabirint
{
    class Program
    {
        static void Main()
        {
            ConsoleGameProcess game = new ConsoleGameProcess(new ConsoleVisualiser());
            game.Start();
        }
    }
}
