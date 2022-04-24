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

namespace Poligon
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen;
        Point mouse;
        PointF[] point = new PointF[26];
        bool ok = true;
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
            pen = new Pen(Color.RoyalBlue, 3);
        }
        int k = 0;
        private void Form1_Click(object sender, EventArgs e)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            mouse = this.PointToClient(Cursor.Position);
            point[k] = mouse;
            k++;
            if (k == 1)
            {
                g.DrawEllipse(pen, mouse.X, mouse.Y, 3, 3);
                g.DrawString(Convert.ToString(letters[k - 1]), new Font(FontFamily.GenericSansSerif, 14), new SolidBrush(Color.Black), mouse.X, mouse.Y);
            }
            if (k > 1 && ok)
            {
                g.DrawEllipse(pen, mouse.X, mouse.Y, 3, 3);
                g.DrawString(Convert.ToString(letters[k - 1]), new Font(FontFamily.GenericSansSerif, 14), new SolidBrush(Color.Black), mouse.X, mouse.Y);
                g.DrawLine(pen, point[k - 2], point[k - 1]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ok)
                g.DrawLine(pen, point[0], point[k - 1]);
            ok = false;

            int diagonale = 0;
            for (int i = 0; i < k - 2; i++)
            {
                for (int j = i + 2; j < k; j++)
                {
                    if (i == 0 && j == k - 1)
                        break;
                    if (isdiagonala(point[i], point[j]))
                    {
                        diagonale++;
                        Thread.Sleep(200);
                        g.DrawLine(pen, point[i].X, point[i].Y, point[j].X, point[j].Y);
                    }
                    if (diagonale == k - 3)
                        return;
                }
            }
        }
        private bool isdiagonala(PointF point1, PointF point2)
        {
            for (int i = 0; i < k - 1; i++)
            {
                if (determinant(point[i + 1], point[i], point1) * determinant(point[i + 1], point[i], point2) <= 0)
                {
                    if (determinant(point2, point1, point[i]) * determinant(point2, point1, point[i + 1]) <= 0)
                        return true;
                }
            }
            return false;
        }
        private float determinant(PointF point1, PointF point2, PointF point3)
        {
            float d = point1.X * (point2.Y - point3.Y) + point2.X * (point3.Y - point1.Y) + point3.X * (point1.Y - point2.Y);
            return d;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
