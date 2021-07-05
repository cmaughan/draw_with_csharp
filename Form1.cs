using System.Windows.Forms;
using System.Drawing;
using System;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = null;
        Bitmap bmp = null;
        Render render = new Render();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyUp += Form1_KeyUp;

            StartDrawing();
        }
        
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Add)
            {

            }
            */
        }

        public void StartDrawing()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += Redraw;
            timer.Enabled = true;
        }

        private void Redraw(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        private void CheckBitmapSize()
        {
            if (bmp == null ||
                bmp.Width != this.Width ||
                bmp.Height != this.Height)
            {
                bmp = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
        }

        private unsafe void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            CheckBitmapSize();

            render.DrawBackground(g, bmp);
            DrawBitMapToScreen(g);

            render.DrawForeground(g);

        }


        void DrawBitMapToScreen(Graphics g)
        {
            g.DrawImageUnscaled(bmp, 0, 0);
        }

    }
}
