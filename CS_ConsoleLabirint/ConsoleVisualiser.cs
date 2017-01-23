using System;
using LabyrinthModel;
using LabyrinthModel.Enums;
using LabyrinthModel.Interfaces;

namespace CS_ConsoleLabirint
{
    class ConsoleVisualiser : ILabyrinthView
    {
        public event EventHandler<ButtonEventArgs> ButtonPressed;

        public ConsoleVisualiser()
        {
            Console.WindowHeight = 42;
            Console.CursorVisible = false;
        }

        public KeyboardGameControls GetDirection()
        {
            var readenKey = Console.ReadKey().Key;
            switch (readenKey)
            {
                case ConsoleKey.UpArrow:
                    return KeyboardGameControls.Up;
                case ConsoleKey.RightArrow:
                    return KeyboardGameControls.Right;
                case ConsoleKey.DownArrow:
                    return KeyboardGameControls.Down;
                case ConsoleKey.LeftArrow:
                    return KeyboardGameControls.Left;
                case ConsoleKey.Escape:
                    return KeyboardGameControls.Quit;
                case ConsoleKey.F12:
                    return KeyboardGameControls.F12;
                default:
                    return KeyboardGameControls.NoDirrection;
            }
        }

        public void ClearCanvas()
        {
            Console.Clear();
        }

        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void PrintLabyrinth(Labyrinth labirint) // Visualize labirint using console graphics
        {
            string currentLabirint = "";

            for (int i = 0; i < labirint.Height; i++)
            {
                for (int j = 0; j < labirint.Width; j++)
                    currentLabirint += GetCurrentElementSymbol(i, j, labirint);
                currentLabirint += "\n";
            }
            currentLabirint += "\n";

            Console.Write(currentLabirint);
        }

        private string GetCurrentElementSymbol(int i, int j, Labyrinth labirint) // Function defines what is curent labirint[i, j] element (wall, pass, hero or exit)
        { 
            // I used string because space ' ' or '=' adds after generating symbol
            string labirintSymbol = "";
            if (labirint.LabirintMatrix[i, j] == LabirintElements.Wall)
            {
                bool wallUp, wallRight, wallDown, wallLeft; // Булевы значения наличия стены сверху, справа, снизу, слева
                wallUp = wallRight = wallDown = wallLeft = false;

                if (i > 0)
                    if (labirint.LabirintMatrix[i - 1, j] == LabirintElements.Wall)
                        wallUp = true;
                if (j < labirint.Width - 1)
                    if (labirint.LabirintMatrix[i, j + 1] == LabirintElements.Wall)
                        wallRight = true;
                if (i < labirint.Height - 1)
                    if (labirint.LabirintMatrix[i + 1, j] == LabirintElements.Wall)
                        wallDown = true;
                if (j > 0)
                    if (labirint.LabirintMatrix[i, j - 1] == LabirintElements.Wall)
                        wallLeft = true;

                // Generete wall symbol assuming walls which are adjacent to the current
                if (wallLeft || wallRight)
                    labirintSymbol = "═";
                if (wallUp || wallDown)
                    labirintSymbol = "║";
                if (wallLeft && wallDown)
                    labirintSymbol = "╗";
                if (wallUp && wallLeft)
                    labirintSymbol = "╝";
                if (wallUp && wallRight)
                    labirintSymbol = "╚";
                if (wallDown && wallRight)
                    labirintSymbol = "╔";
                if (wallLeft && wallRight && wallDown)
                    labirintSymbol = "╦";
                if (wallLeft && wallRight && wallUp)
                    labirintSymbol = "╩";
                if (wallUp && wallDown && wallRight)
                    labirintSymbol = "╠";
                if (wallUp && wallDown && wallLeft)
                    labirintSymbol = "╣";
                if (wallLeft && wallRight && wallUp && wallDown)
                    labirintSymbol = "╬";

                // Add space or '═' to current element to fix doted (= = = =) view of horisontal wall
                if (j < labirint.Width - 1)
                {
                    if (labirint.LabirintMatrix[i, j + 1] == LabirintElements.Wall)
                        labirintSymbol += '═';
                    else
                        labirintSymbol += ' ';
                }
            }
            else
                switch (labirint.LabirintMatrix[i, j])
                {
                    case LabirintElements.Pass: labirintSymbol += "  "; break;
                    case LabirintElements.Hero: labirintSymbol += "X "; break;
                    case LabirintElements.Exit: labirintSymbol += "+ "; break;
                }

            return labirintSymbol;
        }
    }
}
