using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 42;
            Console.CursorVisible = false;
            int height, width, level;
            bool gameOn = true;

            while (gameOn == true)
            {
                height = 5;
                width = 5;
                level = 1;
                Console.Clear();
                Console.WriteLine("\tWelcome to my Console Labirint!\nPress any button to start a game.");
                Console.ReadKey();
                Console.Clear();

                var timer = System.Diagnostics.Stopwatch.StartNew();

                while (width < 61)
                {
                    Labirint labirint = new Labirint(height, width);
                    Direction currentDirrection = Direction.noDirrection;

                    while (!labirint.LevelDone)
                    {
                        Console.WriteLine("Level " + level + ". Find a way to the right buttom corner!\n");

                        labirint.MakeStep(currentDirrection);
                        Console.Write(labirint.GetLabirintStringPresentation() + "\n 'Esc' to exit.");
                        ConsoleKey pressedButton = Console.ReadKey().Key;
                        switch (pressedButton)
                        {
                            case ConsoleKey.Escape:
                                Environment.Exit(0);
                                break;

                            case ConsoleKey.F12:
                                labirint.LevelDone = true;
                                break;

                            default:
                                currentDirrection = Controls.GetDirection(pressedButton);
                                break;
                        }
                        Console.Clear();
                    }
                    if (height < 37)
                        height += 2;
                    width += 2;
                    level++;
                }

                timer.Stop();
                var elapsedMs = timer.ElapsedMilliseconds;

                Console.WriteLine("\tYou made it!\n Your time: {0} seconds.\n\n Press any button to play again or 'Esc' to exit.\n", Convert.ToDouble(timer.ElapsedMilliseconds / 1000.0));
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    gameOn = false;
            }
        }
    }
}
