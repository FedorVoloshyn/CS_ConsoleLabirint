using System;
using LabyrinthModel.Enums;

namespace LabyrinthModel
{
    public class Labyrinth
    {
        private LabirintElements[,] labirint;
        private int heroPositionX;
        private int heroPositionY;
        private int height;
        private int width;

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public LabirintElements[,] LabirintMatrix
        {
            get { return labirint; }
        }

        public bool IsLevelDone { get; set; }

        public Labyrinth(int height, int width)
        {
            this.heroPositionX = 1;
            this.heroPositionY = 1;
            this.height = height;
            this.width = width;
            this.labirint = new LabirintElements[this.height, this.width];
            this.IsLevelDone = false;

            // Initialize labirint with only walls
            for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                labirint[i, j] = LabirintElements.Wall;

            GenerateLabirint();
        }

        public void MakeStep(KeyboardGameControls heroDirection)
        {
            switch (heroDirection)
            {
                case KeyboardGameControls.Up:
                    if (labirint[heroPositionX - 1, heroPositionY] != LabirintElements.Wall)
                    {
                        if (labirint[heroPositionX - 1, heroPositionY] == LabirintElements.Exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroPositionX - 1, heroPositionY], ref labirint[heroPositionX, heroPositionY]);
                        heroPositionX--;
                    }
                    break;
                case KeyboardGameControls.Right:
                    if (labirint[heroPositionX, heroPositionY + 1] != LabirintElements.Wall)
                    {
                        if (labirint[heroPositionX, heroPositionY + 1] == LabirintElements.Exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroPositionX, heroPositionY + 1], ref labirint[heroPositionX, heroPositionY]);
                        heroPositionY++;
                    }
                    break;
                case KeyboardGameControls.Down:
                    if (labirint[heroPositionX + 1, heroPositionY] != LabirintElements.Wall)
                    {
                        if (labirint[heroPositionX + 1, heroPositionY] == LabirintElements.Exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroPositionX + 1, heroPositionY], ref labirint[heroPositionX, heroPositionY]);
                        heroPositionX++;
                    }
                    break;
                case KeyboardGameControls.Left:
                    if (labirint[heroPositionX, heroPositionY - 1] != LabirintElements.Wall)
                    {
                        if (labirint[heroPositionX, heroPositionY - 1] == LabirintElements.Exit)
                            IsLevelDone = true;
                        Swap(ref labirint[heroPositionX, heroPositionY - 1], ref labirint[heroPositionX, heroPositionY]);
                        heroPositionY--;
                    }
                    break;
                default:
                    break;
            }
        }

        private bool IsDeadend(int x, int y) // Additional function that determinates deadends
        {
            int a = 0;

            if (x != 1)
            {
                if (labirint[y, x - 2] == LabirintElements.Pass)
                    a += 1;
            }
            else a += 1;

            if (y != 1)
            {
                if (labirint[y - 2, x] == LabirintElements.Pass)
                    a += 1;
            }
            else a += 1;

            if (x != width - 2)
            {
                if (labirint[y, x + 2] == LabirintElements.Pass)
                    a += 1;
            }
            else a += 1;

            if (y != height - 2)
            {
                if (labirint[y + 2, x] == LabirintElements.Pass)
                    a += 1;
            }
            else a += 1;

            return a == 4;
        }

        private void Swap(ref LabirintElements firstValue, ref LabirintElements secondValue)
        {
            LabirintElements temp = firstValue;
            firstValue = secondValue;
            secondValue = temp;
        }

        private void GenerateLabirint() // Labirint generation
        {
            int x, y, c, a;
            Random rand = new Random();

            x = 3;
            y = 3;
            a = 0;

            while (a < 10000)
            {
                labirint[y, x] = LabirintElements.Pass;
                a++;
                while (true)
                {
                    c = rand.Next() % 4;
                    switch (c)
                    {
                        case 0:
                            if (y != 1)
                                if (labirint[y - 2, x] == LabirintElements.Wall)
                                {
                                    labirint[y - 1, x] = LabirintElements.Pass;
                                    labirint[y - 2, x] = LabirintElements.Pass;
                                    y -= 2;
                                }
                            break;
                        case 1:
                            if (y != height - 2)
                                if (labirint[y + 2, x] == LabirintElements.Wall)
                                {
                                    labirint[y + 1, x] = LabirintElements.Pass;
                                    labirint[y + 2, x] = LabirintElements.Pass;
                                    y += 2;
                                }
                            break;
                        case 2:
                            if (x != 1)
                                if (labirint[y, x - 2] == LabirintElements.Wall)
                                {
                                    labirint[y, x - 1] = LabirintElements.Pass;
                                    labirint[y, x - 2] = LabirintElements.Pass;
                                    x -= 2;
                                }
                            break;
                        case 3:
                            if (x != width - 2)
                                if (labirint[y, x + 2] == LabirintElements.Wall)
                                {
                                    labirint[y, x + 1] = LabirintElements.Pass;
                                    labirint[y, x + 2] = LabirintElements.Pass;
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
                    } while (labirint[y, x] != LabirintElements.Pass);


            }
            // Hero's start point
            labirint[heroPositionX, heroPositionY] = LabirintElements.Hero;
            // Exit point
            labirint[height - 2, width - 2] = LabirintElements.Exit;
            //================================
        }
    }
}