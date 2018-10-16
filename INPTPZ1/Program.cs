using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;


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
            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(300, 300);
            double xmin = -1;
            double xmax = 1;
            double ymin = -1;
            double ymax = 1;

            double xstep = (xmax - xmin) / 300;
            double ystep = (ymax - ymin) / 300;

            List<Cplx> koreny = new List<Cplx>();
            Poly p = new Poly(3);
            Poly pd = p.Derive();

            var clrs = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var maxid = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    Cplx ox = FindCoordinates(xmin, ymin, xstep, ystep, i, j);

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q < 30; q++)
                    {
                        var diff = p.Eval(ox).Divide(pd.Eval(ox));
                        ox = ox.Subtract(diff);

                        //Console.WriteLine($"{q} {ox} -({diff})");
                        if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    int id = FindSolutionRootNumber(koreny, ref maxid, ox);

                    ColorizePixels(bmp, clrs, i, j, it, id);
                }
            }

            bmp.Save("../../../out.png");
        }

        private static Cplx FindCoordinates(double xmin, double ymin, double xstep, double ystep, int i, int j)
        {
            // find "world" coordinates of pixel
            double x = xmin + j * xstep;
            double y = ymin + i * ystep;

            Cplx ox = new Cplx()
            {
                Re = x,
                Im = (float)(y)
            };

            if (ox.Re == 0)
                ox.Re = 0.0001;
            if (ox.Im == 0)
                ox.Im = 0.0001f;
            return ox;
        }

        private static int FindSolutionRootNumber(List<Cplx> koreny, ref int maxid, Cplx ox)
        {
            var known = false;
            var id = 0;
            for (int w = 0; w < koreny.Count; w++)
            {
                if (Math.Pow(ox.Re - koreny[w].Re, 2) + Math.Pow(ox.Im - koreny[w].Im, 2) <= 0.01)
                {
                    known = true;
                    id = w;
                }
            }
            if (!known)
            {
                koreny.Add(ox);
                id = koreny.Count;
                maxid = id + 1;
            }

            return id;
        }

        private static void ColorizePixels(Bitmap bmp, Color[] clrs, int i, int j, float it, int id)
        {
            // colorize pixel according to root number
            var vv = clrs[id % clrs.Length];
            vv = Color.FromArgb(vv.R, vv.G, vv.B);
            vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - (int)it * 2), 255), Math.Min(Math.Max(0, vv.G - (int)it * 2), 255), Math.Min(Math.Max(0, vv.B - (int)it * 2), 255));
            bmp.SetPixel(j, i, vv);
        }
    }

    class Poly
    {
        public List<Cplx> Coe { get; set; }

        public Poly()
        {
            Coe = new List<Cplx>();
        }

        public Poly(int n)
        {
            Coe = new List<Cplx>();
            
            Coe.Add(new Cplx() { Re = 1 });
            for (int i = 0; i < n - 1; i++)
            {
                Coe.Add(Cplx.Zero);
            }
            Coe.Add(new Cplx() { Re = 1 });
        }

        public Poly Derive()
        {
            Poly p = new Poly();
            for (int i = 1; i < Coe.Count; i++)
            {
                p.Coe.Add(Coe[i].Multiply(new Cplx() { Re = i }));
            }

            return p;
        }

        public Cplx Eval(Cplx x)
        {
            Cplx s = Cplx.Zero;
            for (int i = 0; i < Coe.Count; i++)
            {
                Cplx coef = Coe[i];
                Cplx bx = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(x);

                    coef = coef.Multiply(bx);
                }

                s = s.Add(coef);
            }

            return s;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Coe.Count; i++)
            {
                s += Coe[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        s += "x";
                    }
                }
                s += " + ";
            }
            return s;
        }
    }

    class Cplx
    {
        public double Re { get; set; }
        public float Im { get; set; }

        public readonly static Cplx Zero = new Cplx()
        {
            Re = 0,
            Im = 0
        };

        public Cplx Multiply(Cplx b)
        {
            Cplx a = this;
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
            return new Cplx()
            {
                Re = a.Re * b.Re - a.Im * b.Im,
                Im = (float)(a.Re * b.Im + a.Im * b.Re)
            };
        }

        public Cplx Add(Cplx b)
        {
            Cplx a = this;
            return new Cplx()
            {
                Re = a.Re + b.Re,
                Im = a.Im + b.Im
            };
        }
        public Cplx Subtract(Cplx b)
        {
            Cplx a = this;
            return new Cplx()
            {
                Re = a.Re - b.Re,
                Im = a.Im - b.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        internal Cplx Divide(Cplx b)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe - bIm*bIm*i*i
            var tmp = this.Multiply(new Cplx() { Re = b.Re, Im = -b.Im });
            var tmp2 = b.Re * b.Re + b.Im * b.Im;

            return new Cplx()
            {
                Re = tmp.Re / tmp2,
                Im = (float)(tmp.Im / tmp2)
            };
        }
    }
}
