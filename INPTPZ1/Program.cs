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
            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(300, 300);
            double xmin = -1.5;
            double xmax = 1.5;
            double ymin = -1.5;
            double ymax = 1.5;

            double xstep = (xmax - xmin) / bmp.Width;
            double ystep = (ymax - ymin) / bmp.Height;

            var availableColors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            List<ComplexNumber> polynomialRoots = new List<ComplexNumber>();

            Polynomial p = new Polynomial();
            p.Coefficients.Add(new ComplexNumber() { Re = 1 });
            p.Coefficients.Add(ComplexNumber.Zero);
            p.Coefficients.Add(ComplexNumber.Zero);
            p.Coefficients.Add(new ComplexNumber() { Re = 1 });
            Polynomial pd = p.Derive();

            Console.WriteLine($"Polynomial: {p}");
            Console.WriteLine($"Polynomial derivative: {pd}");

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int row = 0; row < bmp.Height; row++)
            {
                for (int column = 0; column < bmp.Width; column++)
                {
                    // find "world" coordinates of pixel
                    double x = xmin + column * xstep;
                    double y = ymin + row * ystep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Re = x,
                        Im = (float)(y)
                    };

                    if (ox.Re == 0)
                        ox.Re = 0.0001;
                    if (ox.Im == 0)
                        ox.Im = 0.0001f;

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q< 30; q++)
                    {
                        var diff = p.Evaluate(ox).Divide(pd.Evaluate(ox));
                        ox = ox.Subtract(diff);

                        if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    // find solution root number
                    bool known = false;
                    var id = 0;
                    for (int w = 0; w <polynomialRoots.Count;w++)
                    {
                        if (Math.Pow(ox.Re- polynomialRoots[w].Re, 2) + Math.Pow(ox.Im - polynomialRoots[w].Im, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        polynomialRoots.Add(ox);
                        id = polynomialRoots.Count; 
                    }

                    // colorize pixel according to root number
                    var vv = availableColors[id % availableColors.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R-(int)it*2), 255), Math.Min(Math.Max(0, vv.G - (int)it*2), 255), Math.Min(Math.Max(0, vv.B - (int)it*2), 255));
                    bmp.SetPixel(column, row, vv);
                }
            }
            bmp.Save("../../../newtonFractal.png");
            Console.WriteLine(" - KONEC PROGRAMU - "); // MOJE PRIDANO
            Console.ReadKey(); // MOJE PRIDANO
        }
    }

    class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }

        public Polynomial()
        {
            Coefficients = new List<ComplexNumber>();
        }

        public Polynomial Derive()
        {
            Polynomial p = new Polynomial();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                p.Coefficients.Add(Coefficients[i].Multiply(new ComplexNumber() { Re = i }));
            }

            return p;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                ComplexNumber coef = Coefficients[i];
                ComplexNumber bx = x;
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
            string resultStringRepresentation = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                resultStringRepresentation += Coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        resultStringRepresentation += "x";
                    }
                }
                resultStringRepresentation += " + ";
            }
            return resultStringRepresentation;
        }
    }

    class ComplexNumber
    {
        public double Re { get; set; }
        public float Im { get; set; }
        public readonly static ComplexNumber Zero = new ComplexNumber(){Re = 0, Im = 0};

        public ComplexNumber Multiply(ComplexNumber secondComplexNumber)
        {
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
            return new ComplexNumber()
            {
                Re = this.Re * secondComplexNumber.Re - this.Im * secondComplexNumber.Im,
                Im = (float)(this.Re * secondComplexNumber.Im + this.Im * secondComplexNumber.Re)
            };
        }

        public ComplexNumber Add(ComplexNumber secondComplexNumber)
        {
            return new ComplexNumber()
            {
                Re = this.Re + secondComplexNumber.Re,
                Im = this.Im + secondComplexNumber.Im
            };
        }

        public ComplexNumber Subtract(ComplexNumber secondComplexNumber)
        {
            return new ComplexNumber()
            {
                Re = this.Re - secondComplexNumber.Re,
                Im = this.Im - secondComplexNumber.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        internal ComplexNumber Divide(ComplexNumber secondComplexNumber)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe - bIm*bIm*i*i
            var tempComplexNumber = this.Multiply(new ComplexNumber() { Re = secondComplexNumber.Re, Im = -secondComplexNumber.Im });
            var tempSum = secondComplexNumber.Re * secondComplexNumber.Re + secondComplexNumber.Im * secondComplexNumber.Im;

            return new ComplexNumber()
            {
                Re = tempComplexNumber.Re / tempSum,
                Im = (float)(tempComplexNumber.Im / tempSum)
            };
        }
    }
}
