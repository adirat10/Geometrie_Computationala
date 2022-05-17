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
    }
}
