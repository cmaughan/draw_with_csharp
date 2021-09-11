using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{

    public class Cell
    {
        public const int North = 0;
        public const int South = 1;
        public const int East = 2;
        public const int West = 3;
        public enum Border { Wall, Door };
        public Border[] borders = new Border[4] { Border.Wall, Border.Wall, Border.Wall, Border.Wall };
        public bool visited = false;
        public int x;
        public int y;
    }

    public class Maze
    {
        private const int width = 100;
        private const int height = 100;
        private Cell[,] cells = new Cell[width, height];
        private Random rand = new Random();

        public int GetWidth() { return width; }
        public int GetHeight() { return height; }

        private Stack<Cell> cellStack = new Stack<Cell>();

        public void Visit(int x, int y, int dir)
        {
            var cell = GetCellAtDirection(x, y, dir);
            Debug.Assert(!cell.visited);
            cell.visited = true;
            cellStack.Push(cell);
            MakeDoor(x, y, dir);
        }

        public List<Cell> GetUnvisited(int x, int y)
        {
            var ret = new List<Cell>();

            for (int dir = 0; dir < 4; dir++)
            {
                var cell = GetCellAtDirection(x, y, dir);
                if (cell == null || cell.visited == true)
                    continue;

                ret.Add(cell);
            }
            return ret;
        }

        public void Init()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Cell();
                    cells[x, y].x = x;
                    cells[x, y].y = y;

                }
            }

            /* Random Walk
            cellStack.Push(GetCell(0, 0));
            GetCell(0, 0).visited = true;
            while (cellStack.Count > 0)
            {
                var current = cellStack.Peek();

                // Walk in random dir
                var unvisited = GetUnvisited(current.x, current.y);
                if (unvisited.Count == 0)
                {
                    cellStack.Pop();
                    continue;
                }
                var entry = rand.Next(unvisited.Count);
                var cellTarget = unvisited[entry];

                int dir = 0;
                if (cellTarget.y < current.y)
                {
                    dir = Cell.North;
                }
                else if (cellTarget.y > current.y)
                {
                    dir = Cell.South;
                }
                else if (cellTarget.x > current.x)
                {
                    dir = Cell.East;
                }
                else if (cellTarget.x < current.x)
                {
                    dir = Cell.West;
                }
                Visit(current.x, current.y, dir);
            };
            */

            // Binary Tree
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Binary Tree
                    int NorthOrEast = ((rand.Next(0, 2) == 0) ? Cell.North : Cell.East);
                    if (!MakeDoor(x, y, NorthOrEast))
                    {
                        if (NorthOrEast == Cell.North)
                        {
                            MakeDoor(x, y, Cell.East);
                        }
                        else
                        {
                            MakeDoor(x, y, Cell.North);
                        }

                    }
                }
            }

            // Exits
            GetCell(0, 0).borders[Cell.North] = Cell.Border.Door;
            GetCell(width - 1, height - 1).borders[Cell.South] = Cell.Border.Door;

        }

        public bool MakeDoor(int x, int y, int dir)
        {
            Debug.Assert(CellValid(x, y));

            var startCell = GetCell(x, y);
            var targetCell = GetCellAtDirection(x, y, dir);
            if (targetCell == null)
            {
                return false;
            }

            startCell.borders[dir] = Cell.Border.Door;
            targetCell.borders[GetOppositeDiretion(dir)] = Cell.Border.Door;
            return true;

        }

        public int GetOppositeDiretion(int dir)
        {
            Debug.Assert(dir >= 0 && dir <= 3);
            switch (dir)
            {
                case Cell.North:
                    return Cell.South;
                case Cell.South:
                    return Cell.North;
                case Cell.East:
                    return Cell.West;
                case Cell.West:
                    return Cell.East;
            }
            return Cell.North;
        }
        public Cell GetCellAtDirection(int x, int y, int dir)
        {
            switch (dir)
            {
                case Cell.North:
                    y -= 1;
                    break;
                case Cell.South:
                    y += 1;
                    break;
                case Cell.East:
                    x += 1;
                    break;
                case Cell.West:
                    x -= 1;
                    break;
            }
            if (!CellValid(x, y))
            {
                return null;
            }
            return GetCell(x, y);
        }

        public int GetRandomDirection()
        {
            return rand.Next(0, 5);
        }

        public bool CellValid(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < width && y < height);
        }

        public Cell GetCell(int x, int y)
        {
            return cells[x, y];
        }
    }
}
