using System;
using LabyrinthModel.Enums;
using LabyrinthModel.Interfaces;

namespace LabyrinthModel
{
    public class ConsoleGameProcess
    {
        private readonly ILabyrinthView _view;

        public ConsoleGameProcess(ILabyrinthView _view)
        {
            this._view = _view;
        }

        public void Start()
        {
            int labyrinthHeight, labyrinthWidth, level;

            Console.WriteLine("\tWelcome to my Console Labirint!\n Use arrows to move hero (X).\n Main rule: find a way to the right buttom corner\n When you got '+' press any button to go next level!\n\n Press any button to start a game.");
            Console.ReadKey();
            Console.Clear();

            while (true)
            {
                double totalTime = 0;

                labyrinthHeight = 5;
                labyrinthWidth = 5;
                level = 1;
                
                _view.ClearCanvas();

                while (labyrinthWidth < 61)
                {
                    var timer = System.Diagnostics.Stopwatch.StartNew();
                    Labyrinth labyrinth = new Labyrinth(labyrinthHeight, labyrinthWidth);
                    KeyboardGameControls currentDirrection = KeyboardGameControls.NoDirrection;
                    while (!labyrinth.IsLevelDone)
                    {
                        _view.PrintMessage("\tLevel " + level);
                        labyrinth.MakeStep(currentDirrection);
                        _view.PrintLabyrinth(labyrinth);
                        _view.PrintMessage(" Time: "+ totalTime + " sec.\n 'Esc' to exit.");

                        KeyboardGameControls pressedButton = _view.GetDirection();
                        switch (pressedButton)
                        {
                            case KeyboardGameControls.Quit:
                                Environment.Exit(0);
                                break;

                            case KeyboardGameControls.F12:
                                labyrinth.IsLevelDone = true;
                                break;

                            default:
                                currentDirrection = pressedButton;
                                break;
                        }

                        _view.ClearCanvas();
                    }

                    if (labyrinthHeight < 37)
                    {
                        labyrinthHeight += 2;
                    }
                    labyrinthWidth += 2;
                    level++;

                    timer.Stop();
                    totalTime += timer.ElapsedMilliseconds / 1000.0;
                }
                
                _view.PrintMessage("\tYou made it!\n Your time: " + totalTime + " seconds.\n\n Press any button to play again or 'Esc' to exit.\n");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    return;
            }
        }
    }
}
