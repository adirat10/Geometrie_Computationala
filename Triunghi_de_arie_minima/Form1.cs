using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triunghi_de_arie_minima
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
            Pen p1 = new Pen(Color.Blue, 2);
            Pen p2 = new Pen(Color.Red, 2);

            Random r = new Random();

            int n = r.Next(3, 15);
            PointF[] m = new PointF[n];

            for (int i = 0; i < n; i++)
            {
                m[i].X = r.Next(10, panel1.Width - 10);
                m[i].Y = r.Next(10, panel1.Height - 10);
                g.DrawEllipse(p1, m[i].X, m[i].Y, 2, 2);
            }

            float ariamin = float.MaxValue;
            float ax = 0, ay = 0, bx = 0, by = 0, cx = 0, cy = 0;

            for (int i = 0; i < n - 2; i++)
            {
                for (int j = i + 1; j < n - 1; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        if (aria(m[i].X, m[i].Y, m[j].X, m[j].Y, m[k].X, m[k].Y) <= ariamin && aria(m[i].X, m[i].Y, m[j].X, m[j].Y, m[k].X, m[k].Y) != 0)
                        {
                            if (triunghi(m[i].X, m[i].Y, m[j].X, m[j].Y, m[k].X, m[k].Y))
                            {
                                ariamin = aria(m[i].X, m[i].Y, m[j].X, m[j].Y, m[k].X, m[k].Y);
                                ax = m[i].X;
                                ay = m[i].Y;
                                bx = m[j].X;
                                by = m[j].Y;
                                cx = m[k].X;
                                cy = m[k].Y;
                            }
                        }
                    }
                }
            }
            g.DrawLine(p2, ax, ay, bx, by);
            g.DrawLine(p2, bx, by, cx, cy);
            g.DrawLine(p2, cx, cy, ax, ay);
            label1.Text = Convert.ToString(ariamin);
        }
        float aria(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            float a = (float)(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            float b = (float)(Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2)));
            float c = (float)(Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2)));

            float p = (a + b + c) / 2;
            float arie = (int)(Math.Sqrt(p * (p - a) * (p - b) * (p - c)));
            return arie;
        }

        private bool triunghi(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            float a = (float)(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            float b = (float)(Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2)));
            float c = (float)(Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2)));

            if (a + b > c && b + c > a && a + c > b && a != 0 && b != 0 && c != 0)
                return true;
            return false;
        }
    }
}
