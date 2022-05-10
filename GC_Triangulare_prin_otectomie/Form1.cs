using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GC_Triangulare_prin_otectomie
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen = new Pen(Color.RoyalBlue, 3);
        const int raza = 3;
        int n = 0; // nr de varfuri ale poligonului
        List<PointF> p = new List<PointF>(); //lista varfurilor
        bool poligon_inchis = false;
        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }
        //desenare poligon
        private void Form1_Click(object sender, EventArgs e)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            p.Add(this.PointToClient(new Point(Form1.MousePosition.X, Form1.MousePosition.Y)));
            g.DrawEllipse(pen, p[n].X, p[n].Y, raza, raza);
            g.DrawString(Convert.ToString(letters[n]), new Font(FontFamily.GenericSansSerif, 14),
            new SolidBrush(Color.Black), p[n].X + raza, p[n].Y - raza);
            if (n > 0)
                g.DrawLine(pen, p[n - 1], p[n]);
            n++;
        }
        //inchiderea poligonul
        private void button1_Click(object sender, EventArgs e)
        {
            if (n < 3)
                return;
            g.DrawLine(pen, p[n - 1], p[0]);
            poligon_inchis = true;
        }
        //determina valoarea determinantului
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
        private bool intoarcere_spre_dreapta(int p1, int p2, int p3)
        {
            if (determinant(p[p1], p[p2], p[p3]) > 0)
                return true;
            return false;
        }
        private bool varf_convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }
        private bool varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }
        //verifica daca doua segmente se intersecteaza
        private bool se_intersecteaza(PointF s1, PointF s2, PointF p1, PointF p2)
        {
            if (determinant(p2, p1, s1) * determinant(p2, p1, s2) <= 0 && determinant(s2, s1, p1) * determinant(s2, s1, p2) <= 0)
                return true;
            return false;
        }
        //verifica daca segmentul p_i p_j se afla in interiorul poligonului
        private bool se_afla_in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((varf_convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
            (varf_reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
        }
        //triangularea poligonului folosind diagonalele

        double aria_poligon = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if (n <= 3)
                return;
            if (!poligon_inchis)
                button1_Click(sender, e); //inchide poligonul

            pen = new Pen(Color.MediumVioletRed, 3);

            while (n > 3)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == n - 2)
                    {
                        if (isdiagonala(p[i], p[0]))
                        {
                            double aria_triunghi = Aria(p[i], p[i + 1], p[0]);
                            aria_poligon += aria_triunghi;
                            g.DrawLine(pen, p[i], p[0]);
                            Thread.Sleep(100);
                            p.Remove(p[i + 1]);
                            n--;
                            break;
                        }
                    }
                    else if (i == n - 1)
                    {
                        if (isdiagonala(p[i], p[1]))
                        {
                            double aria_triunghi = Aria(p[i], p[0], p[1]);
                            aria_poligon += aria_triunghi;
                            g.DrawLine(pen, p[i], p[1]);
                            Thread.Sleep(100);
                            p.Remove(p[0]);
                            n--;
                            break;
                        }
                    }
                    else
                    {
                        if (isdiagonala(p[i], p[i + 2]))
                        {
                            double aria_triunghi = Aria(p[i], p[i + 1], p[i + 2]);
                            aria_poligon += aria_triunghi;
                            g.DrawLine(pen, p[i], p[i + 2]);
                            Thread.Sleep(100);
                            p.Remove(p[i + 1]);
                            n--;
                            break;
                        }
                    }
                }
            }
            label3.Text = Convert.ToString(aria_poligon);
        }

        private double Aria(PointF p1, PointF p2, PointF p3)
        {
            return (double)Math.Abs(0.5 * determinant(p1, p2, p3));
        }

        private bool isdiagonala(PointF p1, PointF p2)
        {
            int nr_diagonale = 0;
            Tuple<int, int>[] diagonale = new Tuple<int, int>[n - 3];
            bool ok = false;
            for (int i = 0; i < n - 2; i++)
            {
                for (int j = i + 2; j < n; j++)
                {
                    if (i == 0 && j == n - 1)
                        break; // exclud ultima latura
                    bool intersectie = false;
                    //daca p_i p_j nu intersecteaza nicio latura neincidenta a poligonului
                    for (int k = 0; k < n - 1; k++)
                        if (i != k && i != (k + 1) && j != k && j != (k + 1) && se_intersecteaza(p1, p2, p[k], p[k + 1]))
                        {
                            intersectie = true;
                            break;
                        }
                    //verif si pt ultima latura a poligonului
                    if (i != n - 1 && i != 0 && j != n - 1 && j != 0 && se_intersecteaza(p1, p2, p[n - 1], p[0]))
                    {
                        intersectie = true;
                    }
                    if (!intersectie)
                    {
                        //si daca p_i p_j nu intersecteaza niciuna din diagonalele alese anterior
                        for (int k = 0; k < nr_diagonale; k++)
                            if (i != diagonale[k].Item1 && i != diagonale[k].Item2 &&
                            j != diagonale[k].Item1 && j != diagonale[k].Item2 &&
                           se_intersecteaza(p1, p2, p[diagonale[k].Item1], p[diagonale[k].Item2]))
                            {
                                intersectie = true;
                                break;
                            }
                        if (!intersectie)
                        {
                            //si daca p_i p_j se afla in interiorul poligonului
                            if (se_afla_in_interiorul_poligonului(i, j))
                            {
                                //se retine diagonala p_i p_j
                                //Thread.Sleep(100);
                                //g.DrawLine(pen, p[i], p[j]);
                                diagonale[nr_diagonale] = new Tuple<int, int>(i, j);
                                nr_diagonale++;
                                ok = true;
                            }
                        }
                    }
                    if (nr_diagonale == n - 3)
                        break;
                }
            }
            if (ok)
                return true;
            return false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}