using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{

    class GameOfLife
    {
        private int[] cells;
        private int currentSource = 0;
        private Random rand = new Random();
        private int currentWidth = 0;
        private int currentHeight = 0;

        public const int Alive = 1;
        public const int Dead = 0;
        public const int Old = 2;

        private const int Scale = 2;

        public int GetScale() { return Scale; }
        /// <summary>
        /// Resize the grid but don't do it if the sizes are the same as last time
        /// </summary>
        /// <param name="width">The new width</param>
        /// <param name="height">The new height</param>
        public void InitCells(int width, int height)
        {
            width = width / Scale;
            height = height / Scale;
            if (currentWidth != width || currentHeight != height)
            {
                currentSource = 0;
                currentWidth = width;
                currentHeight = height;
                cells = new int[width * height * 2];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int value;
                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                        {
                            byte[] randomNumber = new byte[4];//4 for int32
                            rng.GetBytes(randomNumber);
                            value = BitConverter.ToInt32(randomNumber, 0);
                        }
                        SetCell(GetCurrentSource(), x, y, (value % 6) <=1 ? Alive : Dead);
                    }
                }

            }

        }

        public void Update()
        {
            for (int x = 0; x < currentWidth; x++)
            {
                for (int y = 0; y < currentHeight; y++)
                {
                    int currentState = GetCell(GetCurrentSource(), x, y);

                    int aliveCount = 0;
                    for (int xx = -1; xx <= 1; xx++)
                    {
                        for (int yy = -1; yy <= 1; yy++)
                        {
                            int newX = x + xx;
                            int newY = y + yy;
                            if (newX == x && newY == y)
                            {
                                continue;
                            }
                            int surroundCell = GetCell(GetCurrentSource(), newX, newY);
                            if (surroundCell >= Alive)
                            {
                                aliveCount++;
                            }
                        }
                    }

                    Debug.Assert(aliveCount >= 0 && aliveCount <= 8);
                    // Now know how many around us.
                    if (currentState >= Alive)
                    {
                        if ((aliveCount < 2) || (aliveCount > 3))
                        {
                            SetCell(GetCurrentDest(), x, y, Dead);
                        }
                        else
                        {
                            if (currentState >= Alive)
                            {
                                SetCell(GetCurrentDest(), x, y, Old);
                            }
                            else
                            {
                                SetCell(GetCurrentDest(), x, y, Alive);
                            }
                        }
                    }
                    else // Dead
                    {
                        if (aliveCount == 3)
                        {
                            SetCell(GetCurrentDest(), x, y, Alive);
                        }
                        else
                        {
                            SetCell(GetCurrentDest(), x, y, Dead);
                        }
                    }
                    //SetCell(GetCurrentDest(), x, y, aliveCount >= 3 ?  : considerCell);
                }
            }

            currentSource = 1 - currentSource;
        }

        public void Clamp(ref int x, ref int y)
        {
            if (x == -1)
            {
                x = currentWidth - 1;
            }
            if (y == -1)
            {
                y = currentHeight - 1;
            }
            if (x >= currentWidth)
            {
                x = 0;
            }
            if (y >= currentHeight)
            {
                y = 0;
            }
        }

        public int GetCell(int source, int x, int y)
        {
            Clamp(ref x, ref y);
            return cells[(source * (currentWidth * currentHeight)) + (y * currentWidth) + x];
        }
        public void SetCell(int source, int x, int y, int aliveOrDead)
        {
            Clamp(ref x, ref y);
            cells[(source * (currentWidth * currentHeight)) + (y * currentWidth) + x] = aliveOrDead;
        }

        public int GetCurrentSource()
        {
            return currentSource;
        }
        public int GetCurrentDest()
        {
            return 1 - currentSource;
        }

    }
}
