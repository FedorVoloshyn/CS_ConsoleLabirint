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
            mazemake();
        }

        bool deadend(int x, int y, int[,] maze) // Вспомогательная функция, определяет тупики
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

        public string Visual() // Изображение результата с помощью консольной графики
        {
            string currentLabirint = "";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                    currentLabirint += getCurrentElementSymbol(i, j);
                currentLabirint += "\n";
            }
            currentLabirint += "\n";
            
            return currentLabirint;
        }

        private string getCurrentElementSymbol(int i, int j)
        {  //Функцию следует использовать только в том случае, если maze[i, j] является wall
            string frameSymbol = "";
            if (maze[i, j] == wall)
            {
                bool dirUp, dirRight, dirDown, dirLeft; // направления
                dirUp = dirRight = dirDown = dirLeft = false;

                if (i > 0)
                    if (maze[i - 1, j] == wall)
                        dirUp = true;
                if (j < width - 1)
                    if (maze[i, j + 1] == wall)
                        dirRight = true;
                if (i < height - 1)
                    if (maze[i + 1, j] == wall)
                        dirDown = true;
                if (j > 0)
                    if (maze[i, j - 1] == wall)
                        dirLeft = true;

                // Формирование символа на основе того, какие блоки-соседи у текущего
                if (dirLeft || dirRight)
                    frameSymbol = "\u2550";
                if (dirUp || dirDown)
                    frameSymbol = "\u2551";
                if (dirLeft && dirDown)
                    frameSymbol = "\u2557";
                if (dirUp && dirLeft)
                    frameSymbol = "\u255D";
                if (dirUp && dirRight)
                    frameSymbol = "\u255A";
                if (dirDown && dirRight)
                    frameSymbol = "\u2554";
                if (dirLeft && dirRight && dirDown)
                    frameSymbol = "\u2566";
                if (dirLeft && dirRight && dirUp)
                    frameSymbol = "\u2569";
                if (dirUp && dirDown && dirRight)
                    frameSymbol = "\u2560";
                if (dirUp && dirDown && dirLeft)
                    frameSymbol = "\u2563";
                if (dirLeft && dirRight && dirUp && dirDown)
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

        void mazemake() // Собственно алгоритм
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
                    if (deadend(x, y, maze))
                        break;
                }

                if (deadend(x, y, maze)) // Вытаскиваем крота из тупика
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
