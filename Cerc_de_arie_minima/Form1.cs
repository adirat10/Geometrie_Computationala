using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cerc_de_arie_minima
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
            Pen p = new Pen(Color.Black, 2);
            Pen p2 = new Pen(Color.LightSkyBlue, 3);

            Random r = new Random();
            int n = r.Next(2, 50);


            PointF[] m1 = new PointF[n];
            float diametru = float.MinValue;
            float xa = 0, ya = 0, xb = 0, yb = 0;

            for (int i = 0; i < n; i++)
            {
                m1[i].X = r.Next(100, panel1.Width - 150);
                m1[i].Y = r.Next(100, panel1.Height - 150);
                g.DrawEllipse(p, m1[i].X, m1[i].Y, 1.5F, 1.5F);
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (distanta(m1[i].X, m1[i].Y, m1[j].X, m1[j].Y) > diametru)
                    {
                        diametru = distanta(m1[i].X, m1[i].Y, m1[j].X, m1[j].Y);
                        xa = m1[i].X;
                        ya = m1[i].Y;
                        xb = m1[j].X;
                        yb = m1[j].Y;
                    }
                }
            }
            float xcentru, ycentru, raza;
            xcentru = (xa + xb) / 2;
            ycentru = (ya + yb) / 2;
            //raza = distanta(xcentru, ycentru, xa, ya);
            raza = diametru / 2;

            g.DrawEllipse(p2, xcentru - raza, ycentru - raza, diametru, diametru);
            //g.DrawLine(p2, xa, ya, xb, yb);
            double Aria = Math.PI * raza * raza;
            label1.Text = Convert.ToString(Aria);
        }
        private float distanta(float x1, float y1, float x2, float y2)
        {
            float dist = (float)(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            return dist;
        }
    }
}
