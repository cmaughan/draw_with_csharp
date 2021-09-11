using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Render
    {
        //private GameOfLife life = new GameOfLife();
        private Maze maze = new Maze();

        public Render()
        {
            maze.Init();
        }

        public void DrawForeground(Graphics g)
        {
            const int margin = 2;
            int screenWidth = (int)(g.VisibleClipBounds.Width) - margin * 2;
            int screenHeight = (int)(g.VisibleClipBounds.Height) - margin * 2;

            int width = maze.GetWidth();
            int height = maze.GetHeight();

            int pixelsPerCellX = (int)(screenWidth) / width;
            int pixelsPerCellY = (int)(screenHeight) / height;

            if (pixelsPerCellX < pixelsPerCellY)
            {
                pixelsPerCellY = pixelsPerCellX;
            }
            else
            {
                pixelsPerCellX = pixelsPerCellY;
            }

            Pen p = new Pen(Brushes.White);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int xx = x * pixelsPerCellX + margin;
                    int yy = y * pixelsPerCellY + margin;

                    var cell = maze.GetCell(x, y);

                    if (cell.borders[Cell.North] == Cell.Border.Wall)
                    {
                        g.DrawLine(p, xx, yy, xx + pixelsPerCellX, yy);
                    }
                    if (cell.borders[Cell.West] == Cell.Border.Wall)
                    {
                        g.DrawLine(p, xx, yy, xx, yy + pixelsPerCellY);
                    }
                    if (cell.borders[Cell.East] == Cell.Border.Wall)
                    {
                        g.DrawLine(p, xx + pixelsPerCellX, yy, xx + pixelsPerCellX, yy + pixelsPerCellY);
                    }
                    if (cell.borders[Cell.South] == Cell.Border.Wall)
                    {
                        g.DrawLine(p, xx, yy + pixelsPerCellY, xx + pixelsPerCellX, yy + pixelsPerCellY);
                    }
                }
            }
        }

        public unsafe void DrawBackground(Graphics g, Bitmap bmp)
        {
        }
    }
}
