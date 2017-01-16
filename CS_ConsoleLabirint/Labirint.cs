using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Labirint
    {
        const int wall = 0, pass = 1, hero = 2;
        int[,] maze;
        private int heroX;
        private int heroY;

        public Labirint(int height, int width)
        {
            this.heroX = 1;
            this.heroY = 1;
            this.maze = new int[height, width];
            mazemake(height, width);
        }

        bool deadend(int x, int y, int[,] maze, int height, int width) // Вспомогательная функция, определяет тупики
        {
            int a = 0;

            if (x != 1)
            {
                if (maze[y, x - 2] == pass)
                    a += 1;
            }
            else a += 1;

            if (y != 1)
            {
                if (maze[y - 2, x] == pass)
                    a += 1;
            }
            else a += 1;

            if (x != width - 2)
            {
                if (maze[y, x + 2] == pass)
                    a += 1;
            }
            else a += 1;

            if (y != height - 2)
            {
                if (maze[y + 2, x] == pass)
                    a += 1;
            }
            else a += 1;

            if (a == 4)
                return true;
            else
                return false;
        }

        public string visual(int height, int width) // Изображение результата с помощью консольной графики
        {
            string currentLabirint = "";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    switch (maze[i, j])
                    {
                        case wall: currentLabirint += "\u2551 "; break;
                        case pass: currentLabirint += "  "; break;
                        case hero: currentLabirint += "X "; break;
                    }
                currentLabirint += "\n";
            }
            currentLabirint += "\n";


            return currentLabirint;
        }

        public void makeStep(Direction heroDirection)
        {
            switch (heroDirection)
            {
                case Direction.up:
                    if (maze[heroX - 1, heroY] != wall)
                    {
                        Swap(ref maze[heroX - 1, heroY], ref maze[heroX, heroY]);
                        heroX--;
                    }
                    break;
                case Direction.right:
                    if (maze[heroX, heroY + 1] != wall)
                    {
                        Swap(ref maze[heroX, heroY + 1], ref maze[heroX, heroY]);
                        heroY++;
                    }
                    break;
                case Direction.down:
                    if (maze[heroX + 1, heroY] != wall)
                    {
                        Swap(ref maze[heroX + 1, heroY], ref maze[heroX, heroY]);
                        heroX++;
                    }
                    break;
                case Direction.left:
                    if (maze[heroX, heroY - 1] != wall)
                    {
                        Swap(ref maze[heroX, heroY - 1], ref maze[heroX, heroY]);
                        heroY--;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Swap(ref int lhs, ref int rhs)
        {
            int temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        void mazemake(int height, int width) // Собственно алгоритм
        {
            int x, y, c, a;
            bool b;
            Random rand = new Random();

            for (int i = 0; i < height; i++) // Массив заполняется землей-ноликами
                for (int j = 0; j < width; j++)
                    maze[i, j] = wall;

            x = 3; y = 3; a = 0; // Точка приземления крота и счетчик
            while (a < 10000)
            { // Да, простите, костыль, иначе есть как, но лень
                maze[y, x] = pass; a++;
                while (true)
                { // Бесконечный цикл, который прерывается только тупиком
                    c = rand.Next() % 4; // Напоминаю, что крот прорывает
                    switch (c)
                    {  // по две клетки в одном направлении за прыжок
                        case 0:
                            if (y != 1)
                                if (maze[y - 2, x] == wall)
                                { // Вверх
                                    maze[y - 1, x] = pass;
                                    maze[y - 2, x] = pass;
                                    y -= 2;
                                }
                            break;
                        case 1:
                            if (y != height - 2)
                                if (maze[y + 2, x] == wall)
                                { // Вниз
                                    maze[y + 1, x] = pass;
                                    maze[y + 2, x] = pass;
                                    y += 2;
                                }
                            break;
                        case 2:
                            if (x != 1)
                                if (maze[y, x - 2] == wall)
                                { // Налево
                                    maze[y, x - 1] = pass;
                                    maze[y, x - 2] = pass;
                                    x -= 2;
                                }
                            break;
                        case 3:
                            if (x != width - 2)
                                if (maze[y, x + 2] == wall)
                                { // Направо
                                    maze[y, x + 1] = pass;
                                    maze[y, x + 2] = pass;
                                    x += 2;
                                }
                            break;
                    }
                    if (deadend(x, y, maze, height, width))
                        break;
                }

                if (deadend(x, y, maze, height, width)) // Вытаскиваем крота из тупика
                    do
                    {
                        x = 2 * (rand.Next() % ((width - 1) / 2)) + 1;
                        y = 2 * (rand.Next() % ((height - 1) / 2)) + 1;
                    }
                    while (maze[y, x] != pass);


            } // На этом и все.
            // Задаю начальную точку для героя
            maze[heroX, heroY] = 2;
            //================================
        }
    }
}
