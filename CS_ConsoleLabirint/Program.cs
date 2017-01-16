using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;
            Console.WindowHeight = 40;

            Console.WriteLine("Enter length: ");
            int height = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter width: ");
            int width = Int32.Parse(Console.ReadLine());

            Console.Clear();

            Labirint labirint = new Labirint(height, width);
            Direction currentDirrection = Direction.noDirrection;

            while (!exit)
            {
                labirint.makeStep(currentDirrection);
                Console.Write(labirint.visual(height, width) + "\n Esc to exit.");
                ConsoleKey pressedButton = Console.ReadKey().Key;
                switch(pressedButton)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    default:
                        currentDirrection = Controls.GetDirection(pressedButton);
                        break;
                }
                Console.Clear();
            }

        }
    }
}
