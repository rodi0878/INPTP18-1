using System;
using System.Collections.Generic;
using System.Drawing;



namespace INPTPZ1
{
    /// <summary>
    /// This program should produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = new Settings()
            {
                Width = 300,
                Height = 300,
                Xmax = 1.5,
                Xmin = -1.5,
                Ymin = -1.5,
                Ymax = 1.5,
                PolynomialTerms = new double[] { 1, 0, 0, 1 },
                Colors = new Color[]
                {
                  Color.Red,
                  Color.Blue,
                  Color.Green,
                  Color.Yellow,
                  Color.Orange,
                  Color.Fuchsia,
                  Color.Gold,
                  Color.Cyan,
                  Color.Magenta
                },
                FileName = "out.png"
            };

            Run(settings);
        }

        private static void Run(Settings settings)
        {
            Bitmap bmp = new Bitmap(settings.Width, settings.Height);

            double xstep = (settings.Xmax - settings.Xmin) / settings.Width;
            double ystep = (settings.Ymax - settings.Ymin) / settings.Height;

            Poly p = CreatePoly(settings.PolynomialTerms);
            Poly pd = p.Derive();

            Console.WriteLine(p);
            Console.WriteLine(pd);

            List<Cplx> koreny = new List<Cplx>();
            for (int i = 0; i < settings.Width; i++)
            {
                for (int j = 0; j < settings.Height; j++)
                {
                    double x = settings.Xmin + j * xstep;
                    double y = settings.Ymin + i * ystep;

                    Cplx ox = new Cplx(x, y);
                    int it = Iterate(p, pd, ref ox);

                    bool known = false;
                    int colorIndex = 0;
                    for (int w = 0; w < koreny.Count; w++)
                    {
                        if (Math.Pow(ox.Re - koreny[w].Re, 2) + Math.Pow(ox.Im - koreny[w].Im, 2) <= 0.01)
                        {
                            known = true;
                            colorIndex = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(ox);
                        colorIndex = koreny.Count;
                    }

                    Color color = CreateColor(settings, it, colorIndex);
                    bmp.SetPixel(j, i, color);
                }
            }
            bmp.Save(settings.FileName);
        }

        private static Color CreateColor(Settings settings, int it, int colorIndex)
        {
            Color color = settings.Colors[colorIndex % settings.Colors.Length];
            return Color.FromArgb(getColor(color.R, it), getColor(color.G, it), getColor(color.B, it));
        }

        private static int Iterate(Poly p, Poly pd, ref Cplx ox)
        {
            int it = 0;
            for (int q = 0; q < 30; q++)
            {
                Cplx diff = p.Eval(ox) / pd.Eval(ox);
                ox -= diff;

                if (diff.Re * diff.Re + diff.Im * diff.Im >= 0.5)
                    q--;

                it++;
            }

            return it;
        }

        private static Poly CreatePoly(IEnumerable<double> polynomialTerms)
        {
            Poly p = new Poly();
            foreach (double term in polynomialTerms)
                p.Roots.Add(new Cplx(term));
            return p;
        }

        private static int getColor(int color, int it)
        {
            return Math.Min(Math.Max(0, color - it * 2), 255);
        }
    }

}
