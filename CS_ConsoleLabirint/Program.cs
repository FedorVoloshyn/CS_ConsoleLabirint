using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            int size = 5, level = 1;
            Console.WindowHeight = 40;

            while (size < 41)
            {
                Labirint labirint = new Labirint(size, size);
                Direction currentDirrection = Direction.noDirrection;

                while (!labirint.LevelDone)
                {
                    Console.WriteLine("Level " + level + ". Find a way to the right buttom corner!\n");

                    labirint.MakeStep(currentDirrection);
                    Console.Write(labirint.Visual(size, size) + "\n Esc to exit.");
                    ConsoleKey pressedButton = Console.ReadKey().Key;
                    switch (pressedButton)
                    {
                        case ConsoleKey.Escape:
                            Environment.Exit(0);
                            break;
                        default:
                            currentDirrection = Controls.GetDirection(pressedButton);
                            break;
                    }
                    Console.Clear();
                } 
                size += 2;
                level++;
            }
        }
    }
}
