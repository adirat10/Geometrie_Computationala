using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Triangulare_cu_diagonale
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen = new Pen(Color.RoyalBlue,3);
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
            p.Add(this.PointToClient(new Point(Form1.MousePosition.X, Form1.MousePosition.Y)));
            g.DrawEllipse(pen, p[n].X, p[n].Y, raza, raza);
            g.DrawString((n + 1).ToString(), new Font(FontFamily.GenericSansSerif, 10),
            new SolidBrush(Color.Navy), p[n].X + raza, p[n].Y - raza);
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
        //determina valoarea determinantului de ordin 3 cu coordonatele punctelor date si 1 p ultima coloana
        private double Sarrus(PointF p1, PointF p2, PointF p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }
        private bool intoarcere_spre_stanga(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) < 0)
                return true;
            return false;
        }
        private bool intoarcere_spre_dreapta(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) > 0)
                return true;
            return false;
        }
        private bool este_varf_convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }
        private bool este_varf_reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }
        //verifica daca doua segmente se intersecteaza
        private bool se_intersecteaza(PointF s1, PointF s2, PointF p1, PointF p2)
        {
            if (Sarrus(p2, p1, s1) * Sarrus(p2, p1, s2) <= 0 && Sarrus(s2, s1, p1) * Sarrus(s2, s1, p2) <= 0)
                return true;
            return false;
        }
        //verifica daca segmentul p_i p_j se afla in interiorul poligonului
        private bool se_afla_in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((este_varf_convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
            (este_varf_reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
        }
        //triangularea poligonului folosind diagonalele
        private void button2_Click(object sender, EventArgs e)
        {
            if (n <= 3)
                return;
            if (!poligon_inchis)
                button1_Click(sender, e); //inchide poligonul

            int nr_diagonale = 0;
            Tuple<int, int>[] diagonale = new Tuple<int, int>[n - 3];

            pen = new Pen(Color.Red,3);
            float[] dashValues = { 1, 2, 3, 4 };
            pen.DashPattern = dashValues;
            for (int i = 0; i < n - 2; i++)
                for (int j = i + 2; j < n; j++)
                {
                    if (i == 0 && j == n - 1)
                        break; // exclud ultima latura
                    bool intersectie = false;
                    //daca p_i p_j nu intersecteaza nicio latura neincidenta a poligonului
                    for (int k = 0; k < n - 1; k++)
                        if (i != k && i != (k + 1) && j != k && j != (k + 1) && se_intersecteaza(p[i], p[j], p[k], p[k + 1]))
                        {
                            intersectie = true;
                            break;
                        }
                    //verif si pt ultima latura a poligonului
                    if (i != n - 1 && i != 0 && j != n - 1 && j != 0 && se_intersecteaza(p[i], p[j], p[n - 1], p[0]))
                    {
                        intersectie = true;
                    }
                    if (!intersectie)
                    {
                        //si daca p_i p_j nu intersecteaza niciuna din diagonalele alese anterior
                        for (int k = 0; k < nr_diagonale; k++)
                            if (i != diagonale[k].Item1 && i != diagonale[k].Item2 &&
                            j != diagonale[k].Item1 && j != diagonale[k].Item2 &&
                           se_intersecteaza(p[i], p[j], p[diagonale[k].Item1], p[diagonale[k].Item2]))
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
                                Thread.Sleep(100);
                                g.DrawLine(pen, p[i], p[j]);
                                diagonale[nr_diagonale] = new Tuple<int, int>(i, j);
                                nr_diagonale++;
                            }
                        }
                    }
                    if (nr_diagonale == n - 3)
                        return;
                }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}