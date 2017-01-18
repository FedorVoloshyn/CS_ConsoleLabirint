using System;
using CS_ConsoleLabirint.Enums;

namespace CS_ConsoleLabirint
{
    class Labirint
    {
        const int wall = 0, pass = 1, hero = 2, exit = 3;
        private int[,] maze;
        private int heroX;
        private int heroY;
        private int height;
        private int width;
        public bool LevelDone { get; private set; }

        public Labirint(int height, int width)
        {
            this.heroX = 1;
            this.heroY = 1;
            this.height = height;
            this.width = width;
            this.maze = new int[this.height, this.width];
            Mazemake();
        }

        bool Deadend(int x, int y, int[,] maze) // Вспомогательная функция, определяет тупики
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
        { // Функция определяет текущий элемент и возвращает соответствующий ему символ
            // string потому что кроме самого символа вместе с ним идет разделить: пробел либо стенка
            string frameSymbol = "";
            if (maze[i, j] == wall)
            {
                bool wallUp, wallRight, wallDown, wallLeft; // Булевы значения наличия стены сверху, справа, снизу, слева
                wallUp = wallRight = wallDown = wallLeft = false;

                if (i > 0)
                    if (maze[i - 1, j] == wall)
                        wallUp = true;
                if (j < width - 1)
                    if (maze[i, j + 1] == wall)
                        wallRight = true;
                if (i < height - 1)
                    if (maze[i + 1, j] == wall)
                        wallDown = true;
                if (j > 0)
                    if (maze[i, j - 1] == wall)
                        wallLeft = true;

                // Формирование символа стены на основе соседних стен
                if (wallLeft || wallRight)
                    frameSymbol = "\u2550";
                if (wallUp || wallDown)
                    frameSymbol = "\u2551";
                if (wallLeft && wallDown)
                    frameSymbol = "\u2557";
                if (wallUp && wallLeft)
                    frameSymbol = "\u255D";
                if (wallUp && wallRight)
                    frameSymbol = "\u255A";
                if (wallDown && wallRight)
                    frameSymbol = "\u2554";
                if (wallLeft && wallRight && wallDown)
                    frameSymbol = "\u2566";
                if (wallLeft && wallRight && wallUp)
                    frameSymbol = "\u2569";
                if (wallUp && wallDown && wallRight)
                    frameSymbol = "\u2560";
                if (wallUp && wallDown && wallLeft)
                    frameSymbol = "\u2563";
                if (wallLeft && wallRight && wallUp && wallDown)
                    frameSymbol = "\u256C";

                //Добавление блока для выравнивания по ширине (фикс пунктирных горизонтальных стен)
                if (j < width - 1)
                {
                    if (maze[i, j + 1] == wall)
                        frameSymbol += '\u2550';
                    else
                        frameSymbol += ' ';
                }
            }
            else
                switch(maze[i, j])
                {
                    case pass: frameSymbol += "  "; break;
                    case hero: frameSymbol += "X "; break;
                    case exit: frameSymbol += "+ "; break;
                }

            return frameSymbol;
        }

        public void MakeStep(Direction heroDirection)
        {
            switch (heroDirection)
            {
                case Direction.up:
                    if (maze[heroX - 1, heroY] != wall)
                    {
                        if (maze[heroX - 1, heroY] == exit) LevelDone = true;
                        Swap(ref maze[heroX - 1, heroY], ref maze[heroX, heroY]);
                        heroX--;
                    }
                    break;
                case Direction.right:
                    if (maze[heroX, heroY + 1] != wall)
                    {
                        if (maze[heroX, heroY + 1] == exit) LevelDone = true;
                        Swap(ref maze[heroX, heroY + 1], ref maze[heroX, heroY]);
                        heroY++;
                    }
                    break;
                case Direction.down:
                    if (maze[heroX + 1, heroY] != wall)
                    {
                        if (maze[heroX + 1, heroY] == exit) LevelDone = true;
                        Swap(ref maze[heroX + 1, heroY], ref maze[heroX, heroY]);
                        heroX++;
                    }
                    break;
                case Direction.left:
                    if (maze[heroX, heroY - 1] != wall)
                    {
                        if (maze[heroX, heroY - 1] == exit) LevelDone = true;
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

        void Mazemake() // Собственно алгоритм
        {
            int x, y, c, a;
            LevelDone = false;
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
                    if (Deadend(x, y, maze))
                        break;
                }

                if (Deadend(x, y, maze)) // Вытаскиваем крота из тупика
                    do
                    {
                        x = 2 * (rand.Next() % ((width - 1) / 2)) + 1;
                        y = 2 * (rand.Next() % ((height - 1) / 2)) + 1;
                    }
                    while (maze[y, x] != pass);


            } // На этом и все.
            // Задаю начальную точку для героя
            maze[heroX, heroY] = 2;
            // и точку выхода
            maze[height - 2, width - 2] = 3;
            //================================
        }
    }
}
