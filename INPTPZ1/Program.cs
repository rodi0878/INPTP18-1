using System;
using System.Collections.Generic;
using System.Drawing;

namespace INPTPZ1
{
    class Program
    {
        static void Main(string[] args)
        {
            int rootsCount = Convert.ToInt32(args[0]);

            Bitmap bmp = new Bitmap(300, 300);
            double xmin = -1;
            double xmax = 1;
            double ymin = -1;
            double ymax = 1;
            double xstep = (xmax - xmin) / 300;
            double ystep = (ymax - ymin) / 300;

            List<ComplexNumber> koreny = new List<ComplexNumber>();
            Polynome polynome = new Polynome(rootsCount > 0 && rootsCount < 10 ? rootsCount : 0);
            Polynome polynomeDerivated = polynome.Derive();

            Color[] colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            
            Process(bmp, xmin, ymin, xstep, ystep, koreny, polynome, polynomeDerivated, colors);

            bmp.Save("../../../out.png");
        }

        private static void Process(Bitmap bmp, double xmin, double ymin, double xstep, double ystep, List<ComplexNumber> roots, Polynome polynome, Polynome polynomeDerivated, Color[] colors)
        {
            int maxid = 0;

            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    ComplexNumber ox = FindCoordinates(xmin, ymin, xstep, ystep, i, j);
                    float it = FindSolution(polynome, polynomeDerivated, ref ox);
                    int id = FindSolutionRootNumber(roots, ref maxid, ox);

                    ColorizePixels(bmp, colors, i, j, it, id);
                }
            }
        }

        private static float FindSolution(Polynome polynome, Polynome polynomeDerivated, ref ComplexNumber complexNumber)
        {
            // find solution of equation using newton's iteration
            float it = 0;

            for (int q = 0; q < 30; q++)
            {
                ComplexNumber diff = polynome.Evaluate(complexNumber).Divide(polynomeDerivated.Evaluate(complexNumber));
                complexNumber = complexNumber.Subtract(diff);

                if (Math.Pow(diff.RealPart, 2) + Math.Pow(diff.ImaginaryPart, 2) >= 0.5)
                {
                    q--;
                }
                it++;
            }

            return it;
        }

        private static ComplexNumber FindCoordinates(double xmin, double ymin, double xstep, double ystep, int i, int j)
        {
            // find "world" coordinates of pixel
            double x = xmin + j * xstep;
            double y = ymin + i * ystep;

            ComplexNumber ox = new ComplexNumber()
            {
                RealPart = x,
                ImaginaryPart = (float)(y)
            };

            if (ox.RealPart == 0)
                ox.RealPart = 0.0001;
            if (ox.ImaginaryPart == 0)
                ox.ImaginaryPart = 0.0001f;

            return ox;
        }

        private static int FindSolutionRootNumber(List<ComplexNumber> roots, ref int maxId, ComplexNumber complexNumber)
        {
            bool known = false;
            int id = 0;

            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(complexNumber.RealPart - roots[i].RealPart, 2)
                    + Math.Pow(complexNumber.ImaginaryPart - roots[i].ImaginaryPart, 2) <= 0.01)
                {
                    known = true;
                    id = i;
                }
            }
            if (!known)
            {
                roots.Add(complexNumber);
                id = roots.Count;
                maxId = id + 1;
            }

            return id;
        }

        private static void ColorizePixels(Bitmap bmp, Color[] colors, int i, int j, float it, int id)
        {
            // colorize pixel according to root number
            Color vv = colors[id % colors.Length];

            vv = Color.FromArgb(vv.R, vv.G, vv.B);
            vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - (int)it * 2), 255),
                Math.Min(Math.Max(0, vv.G - (int)it * 2), 255),
                Math.Min(Math.Max(0, vv.B - (int)it * 2), 255));
            bmp.SetPixel(j, i, vv);
        }
    }
}
