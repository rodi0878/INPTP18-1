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

    class Polynome
    {
        public List<ComplexNumber> ComplexNumbers { get; set; }

        public Polynome()
        {
            ComplexNumbers = new List<ComplexNumber>();
        }

        public Polynome(int count)
        {
            ComplexNumbers = new List<ComplexNumber>
            {
                new ComplexNumber() { RealPart = 1 }
            };

            for (int i = 0; i < count - 1; i++)
            {
                ComplexNumbers.Add(ComplexNumber.Zero);
            }
            ComplexNumbers.Add(new ComplexNumber() { RealPart = 1 });
        }

        public Polynome Derive()
        {
            Polynome p = new Polynome();

            for (int i = 1; i < ComplexNumbers.Count; i++)
            {
                p.ComplexNumbers.Add(ComplexNumbers[i].Multiply(new ComplexNumber() { RealPart = i }));
            }

            return p;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;

            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                ComplexNumber complexNumber = ComplexNumbers[i];
                ComplexNumber complexNumberEvaluated = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        complexNumberEvaluated = complexNumberEvaluated.Multiply(x);
                    complexNumber = complexNumber.Multiply(complexNumberEvaluated);
                }
                s = s.Add(complexNumber);
            }

            return s;
        }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                output += ComplexNumbers[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                        output += "x";
                }
                output += " + ";
            }

            return output;
        }
    }

    class ComplexNumber
    {
        public double RealPart { get; set; }
        public float ImaginaryPart { get; set; }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            RealPart = 0,
            ImaginaryPart = 0
        };

        public ComplexNumber Multiply(ComplexNumber násobitel)
        {
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i - aIm*bIm*i*i
            return new ComplexNumber()
            {
                RealPart = this.RealPart * násobitel.RealPart - this.ImaginaryPart * násobitel.ImaginaryPart,
                ImaginaryPart = (float)(this.RealPart * násobitel.ImaginaryPart + this.ImaginaryPart * násobitel.RealPart)
            };
        }

        public ComplexNumber Add(ComplexNumber summand)
        {
            return new ComplexNumber()
            {
                RealPart = this.RealPart + summand.RealPart,
                ImaginaryPart = this.ImaginaryPart + summand.ImaginaryPart
            };
        }
        public ComplexNumber Subtract(ComplexNumber subtrahend)
        {
            return new ComplexNumber()
            {
                RealPart = this.RealPart - subtrahend.RealPart,
                ImaginaryPart = this.ImaginaryPart - subtrahend.ImaginaryPart
            };
        }

        public override string ToString()
        {
            return $"({RealPart} + {ImaginaryPart}i)";
        }

        internal ComplexNumber Divide(ComplexNumber divisor)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe + bIm*bIm*i*i
            ComplexNumber thisNegated = this.Multiply(new ComplexNumber() { RealPart = divisor.RealPart, ImaginaryPart = -divisor.ImaginaryPart });
            double divideWith = divisor.RealPart * divisor.RealPart + divisor.ImaginaryPart * divisor.ImaginaryPart;

            return new ComplexNumber()
            {
                RealPart = thisNegated.RealPart / divideWith,
                ImaginaryPart = (float)(thisNegated.ImaginaryPart / divideWith)
            };
        }
    }
}
