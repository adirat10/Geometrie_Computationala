using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dreptunghi_de_arie_minima
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
            Pen p = new Pen(Color.Green, 3);
            Pen p2 = new Pen(Color.Blue, 3);

            Random r = new Random();
            int n = r.Next(2, 50);

            float xmin = int.MaxValue, ymin = int.MaxValue;
            float xmax = int.MinValue, ymax = int.MinValue;

            PointF[] m1 = new PointF[n];

            for (int i = 0; i < n; i++)
            {
                m1[i].X = r.Next(50, panel1.Width - 50);
                m1[i].Y = r.Next(50, panel1.Height - 50);
                g.DrawEllipse(p, m1[i].X, m1[i].Y, 2, 2);

                if (m1[i].X < xmin)
                    xmin = m1[i].X;
                if (m1[i].X > xmax)
                    xmax = m1[i].X;
                if (m1[i].Y < ymin)
                    ymin = m1[i].Y;
                if (m1[i].Y > ymax)
                    ymax = m1[i].Y;
            }

            g.DrawRectangle(p2, xmin, ymin, xmax - xmin, ymax - ymin);

            float L = xmax - xmin;
            float l = ymax - ymin;
            float A = L * l;
            label1.Text = Convert.ToString(A);
        }
    }
}
