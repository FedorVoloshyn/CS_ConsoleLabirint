using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Program
    {
        static void Main()
        {
            Console.WindowHeight = 42;
            Console.CursorVisible = false;
            int labirintHeight, labirintWidth, level;
            bool isGameOn = true;

            while (isGameOn)
            {
                labirintHeight = 5;
                labirintWidth = 5;
                level = 1;
                Console.Clear();
                Console.WriteLine("\tWelcome to my Console Labirint!\n Press any button to start a game.\n Use arrows to move hero (X).");
                Console.ReadKey();
                Console.Clear();

                var timer = System.Diagnostics.Stopwatch.StartNew();
                 
                while (labirintWidth < 61)
                {
                    Labirint labirint = new Labirint(labirintHeight, labirintWidth);
                    Direction currentDirrection = Direction.noDirrection;

                    while (!labirint.IsLevelDone)
                    {
                        Console.WriteLine("Level " + level + ". Find a way to the right buttom corner!\n");

                        labirint.MakeStep(currentDirrection);
                        Console.Write(ConsoleVisualiser.GetLabirintStringPresentation(labirint) + "'Esc' to exit.");
                        ConsoleKey pressedButton = Console.ReadKey().Key;
                        switch (pressedButton)
                        {
                            case ConsoleKey.Escape:
                                Environment.Exit(0);
                                break;

                            case ConsoleKey.F12:
                                labirint.IsLevelDone = true;
                                break;
                                
                            default:
                                currentDirrection = Controls.GetDirection(pressedButton);
                                break;
                        }
                        Console.Clear();
                    }
                    if (labirintHeight < 37)
                        labirintHeight += 2;
                    labirintWidth += 2;
                    level++;
                }

                timer.Stop();
                var elapsedMs = timer.ElapsedMilliseconds;

                Console.WriteLine("\tYou made it!\n Your time: {0} seconds.\n\n Press any button to play again or 'Esc' to exit.\n", Convert.ToDouble(timer.ElapsedMilliseconds / 1000.0));
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    isGameOn = false;
            }
        }
    }
}
