using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Labirint
    {
        private const int wall = 0, pass = 1, hero = 2, exit = 3;
        private int[,] labirint;
        private int heroX;
        private int heroY;
        private int height;
        private int width;

        public int Height { get { return height; } }
        public int Width { get { return width; } }
        public bool IsLevelDone { get; set; }

        public Labirint(int height, int width)
        {
            this.heroX = 1;
            this.heroY = 1;
            this.height = height;
            this.width = width;
            this.labirint = new int[this.height, this.width];

            // Initialize labirint with only walls
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    labirint[i, j] = wall;

            Mazemake();
        }

        private bool IsDeadend(int x, int y) // Additional function that determinates deadends
        {
            int a = 0;

            if (x != 1)
            {
                if (labirint[y, x - 2] == pass)
                    a += 1;
            }
            else a += 1;

            if (y != 1)
            {
                if (labirint[y - 2, x] == pass)
                    a += 1;
            }
            else a += 1;

            if (x != width - 2)
            {
                if (labirint[y, x + 2] == pass)
                    a += 1;
            }
            else a += 1;

            if (y != height - 2)
            {
                if (labirint[y + 2, x] == pass)
                    a += 1;
            }
            else a += 1;

            return a == 4;
        }

        public string GetLabirintStringPresentation() // Изображение результата с помощью консольной графики
        {
            string currentLabirint = "";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    currentLabirint += GetCurrentElementSymbol(i, j);
                currentLabirint += "\n";
            }
            currentLabirint += "\n";
            
            return currentLabirint;
        }

        private string GetCurrentElementSymbol(int i, int j) 
        { // Function defines what is curent labirint[i, j] (wall, pass, hero or exit)
            // I used string because 
            string labirintSymbol = "";
            if (labirint[i, j] == wall)
            {
                bool wallUp, wallRight, wallDown, wallLeft; // Булевы значения наличия стены сверху, справа, снизу, слева
                wallUp = wallRight = wallDown = wallLeft = false;

                if (i > 0)
                    if (labirint[i - 1, j] == wall)
                        wallUp = true;
                if (j < width - 1)
                    if (labirint[i, j + 1] == wall)
                        wallRight = true;
                if (i < height - 1)
                    if (labirint[i + 1, j] == wall)
                        wallDown = true;
                if (j > 0)
                    if (labirint[i, j - 1] == wall)
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

                // Add space or '═' to current element to fix dotet (= = = =) view of horisontal wall
                if (j < width - 1)
                {
                    if (labirint[i, j + 1] == wall)
                        labirintSymbol += '═';
                    else
                        labirintSymbol += ' ';
                }
            }
            else
                switch(labirint[i, j])
                {
                    case pass: labirintSymbol += "  "; break;
                    case hero: labirintSymbol += "X "; break;
                    case exit: labirintSymbol += "+ "; break;
                }

            return labirintSymbol;
        }

        public void MakeStep(Direction heroDirection)
        {
            switch (heroDirection)
            {
                case Direction.up:
                    if (labirint[heroX - 1, heroY] != wall)
                    {
                        if (labirint[heroX - 1, heroY] == exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroX - 1, heroY], ref labirint[heroX, heroY]);
                        heroX--;
                    }
                    break;
                case Direction.right:
                    if (labirint[heroX, heroY + 1] != wall)
                    {
                        if (labirint[heroX, heroY + 1] == exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroX, heroY + 1], ref labirint[heroX, heroY]);
                        heroY++;
                    }
                    break;
                case Direction.down:
                    if (labirint[heroX + 1, heroY] != wall)
                    {
                        if (labirint[heroX + 1, heroY] == exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroX + 1, heroY], ref labirint[heroX, heroY]);
                        heroX++;
                    }
                    break;
                case Direction.left:
                    if (labirint[heroX, heroY - 1] != wall)
                    {
                        if (labirint[heroX, heroY - 1] == exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroX, heroY - 1], ref labirint[heroX, heroY]);
                        heroY--;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Swap(ref int firstValue, ref int secondValue)
        {
            int temp = firstValue;
            firstValue = secondValue;
            secondValue = temp;
        }

        private void Mazemake() // Labirint generation
        {
            int x, y, c, a;
            IsLevelDone = false;
            Random rand = new Random();

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    labirint[i, j] = wall;

            x = 3;
            y = 3; 
            a = 0; 

            while (a < 10000)
            { 
                labirint[y, x] = pass;
                a++;
                while (true)
                { 
                    c = rand.Next() % 4; 
                    switch (c)
                    {  
                        case 0:
                            if (y != 1)
                                if (labirint[y - 2, x] == wall)
                                { 
                                    labirint[y - 1, x] = pass;
                                    labirint[y - 2, x] = pass;
                                    y -= 2;
                                }
                            break;
                        case 1:
                            if (y != height - 2)
                                if (labirint[y + 2, x] == wall)
                                { 
                                    labirint[y + 1, x] = pass;
                                    labirint[y + 2, x] = pass;
                                    y += 2;
                                }
                            break;
                        case 2:
                            if (x != 1)
                                if (labirint[y, x - 2] == wall)
                                { 
                                    labirint[y, x - 1] = pass;
                                    labirint[y, x - 2] = pass;
                                    x -= 2;
                                }
                            break;
                        case 3:
                            if (x != width - 2)
                                if (labirint[y, x + 2] == wall)
                                {
                                    labirint[y, x + 1] = pass;
                                    labirint[y, x + 2] = pass;
                                    x += 2;
                                }
                            break;
                    }
                    if (IsDeadend(x, y))
                        break;
                }

                if (IsDeadend(x, y)) 
                    do
                    {
                        x = 2 * (rand.Next() % ((width - 1) / 2)) + 1;
                        y = 2 * (rand.Next() % ((height - 1) / 2)) + 1;
                    }
                    while (labirint[y, x] != pass);


            }
            // Hero's start point
            labirint[heroX, heroY] = 2;
            // Exit point
            labirint[height - 2, width - 2] = 3;
            //================================
        }
    }
}
