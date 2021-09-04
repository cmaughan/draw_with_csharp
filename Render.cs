using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Render
    {
        private GameOfLife life = new GameOfLife();

        public void DrawForeground(Graphics g)
        {
            //g.FillEllipse(new SolidBrush(Color.Red), new RectangleF(10, 10, 100, 100));
            //g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(150, 150, 50, 50));
        }

        public unsafe void DrawBackground(Graphics g, Bitmap bmp)
        {
            // Draw to the bitmap
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var lockData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);

            life.InitCells(bmp.Width, bmp.Height);

            life.Update();

            int scale = life.GetScale();

            var ptr = (byte*)lockData.Scan0.ToPointer();
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pData = (UInt32*)(ptr + (y * lockData.Stride) + (x * 4));
                    int val = life.GetCell(life.GetCurrentDest(), x / scale, y / scale); 

                    if (val == 0)
                    {
                        *pData = 0xFF000000;
                    }
                    else if (val ==1 )
                    {
                        *pData = 0xFFFFFFFF;
                    }
                    else if (val ==2 )
                    {
                        *pData = 0xFFFF0000;
                    }
                }
            }

            bmp.UnlockBits(lockData);
        }
    }
}
