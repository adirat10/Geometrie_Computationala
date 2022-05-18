using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Partitionarea_in_poligoane_monotone
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen = new Pen(Color.RoyalBlue, 3);
        const int raza = 3;
        int n = 0; // nr de varfuri ale poligonului
        List<PointF> p = new List<PointF>(); //lista varfurilor
        bool ok = true;
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }
        private void Form1_Click(object sender, EventArgs e)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            p.Add(this.PointToClient(new Point(Form1.MousePosition.X, Form1.MousePosition.Y)));
            if (ok)
            {
                g.DrawEllipse(pen, p[n].X, p[n].Y, raza, raza);
                g.DrawString(Convert.ToString(letters[n]), new Font(FontFamily.GenericSansSerif, 14),
                new SolidBrush(Color.Black), p[n].X + raza, p[n].Y - raza);
                if (n > 0)
                    g.DrawLine(pen, p[n - 1], p[n]);
                n++;
            }
        }
        private double determinant(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }
        private bool intoarcere_spre_stanga(int p1, int p2, int p3)
        {
            if (determinant(p[p1], p[p2], p[p3]) < 0)
                return true;
            return false;
        }
        private bool varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (n < 3)
                return;
            g.DrawLine(pen, p[n - 1], p[0]);
            ok = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < n - 1; i++)
            {
                if (varf_reflex(i))
                {
                    if (p[i - 1].Y > p[i].Y || (p[i - 1].Y == p[i].Y && p[i - 1].X < p[i].X))
                    {
                        if (p[i + 1].Y > p[i].Y || (p[i + 1].Y == p[i].Y && p[i + 1].X < p[i].X))
                            g.DrawLine(pen, p[i].X, p[i].Y, p[i + 1].X, p[i + 1].Y);
                    }
                    else if (p[i - 1].Y < p[i].Y || (p[i - 1].Y == p[i].Y && p[i - 1].X > p[i].X))
                    {
                        if (p[i + 1].Y < p[i].Y || (p[i + 1].Y == p[i].Y && p[i + 1].X > p[i].X))
                            g.DrawLine(pen, p[i].X, p[i].Y, p[i + 1].X, p[i + 1].Y);
                    }
                }
            }
        }
    }
}
