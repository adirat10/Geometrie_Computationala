using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Jarvis_Convex_Hull
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.ForestGreen, 3);
            Pen pen2 = new Pen(Color.DarkRed, 3);

            Random rand = new Random();
            int n = rand.Next(5, 50);

            PointF[] m = new PointF[n];
            float ymax = float.MinValue;
            float xmin = float.MaxValue;
            float xa = 0, ya = 0;

            for (int i = 0; i < n; i++)
            {
                m[i].X = rand.Next(50, panel1.Width - 50);
                m[i].Y = rand.Next(50, panel1.Height - 50);
                g.DrawEllipse(pen2, m[i].X, m[i].Y, 1.5F, 1.5F);
                if (m[i].Y > ymax)
                {
                    ymax = m[i].Y;
                    xa = m[i].X;
                    ya = m[i].Y;
                }
                else if (m[i].Y == ymax)
                {
                    if (m[i].X < xmin)
                    {
                        xmin = m[i].X;
                        xa = m[i].X;
                        ya = m[i].Y;
                    }
                }
            }

            int k = 0;
            bool valid = true;
            float xk = xa;
            float yk = ya;

            while (valid)
            {
                float xp = m[k].X;
                float yp = m[k].Y;

                for (int i = 0; i < n; i++)
                {
                    if (determinant(xk, yk, xp, yp, m[i].X, m[i].Y) > 0)
                    {
                        xp = m[i].X;
                        yp = m[i].Y;
                    }
                }
                if (xp != xa && yp != ya)
                {
                    g.DrawLine(pen, xk, yk, xp, yp);
                    Thread.Sleep(100);
                    k++;
                    xk = xp;
                    yk = yp;
                }
                else
                {
                    g.DrawLine(pen, xk, yk, xp, yp);
                    valid = false;
                }
            }
        }
        private float determinant(float xp, float yp, float xq, float yq, float xr, float yr)
        {
            float d = xp * (yq - yr) + xq * (yr - yp) + xr * (yp - yq);
            return d;
        }
    }
}
