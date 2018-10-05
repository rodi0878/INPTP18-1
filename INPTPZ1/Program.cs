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
        public static readonly Color[] availableColors = new Color[]{Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta};
        public const int NumberOfIterations = 30;

        static void Main(string[] args)
        {
            Bitmap bmp = new Bitmap(300, 300);
            double xmin = -1.5;
            double xmax = 1.5;
            double ymin = -1.5;
            double ymax = 1.5;

            double xstep = (xmax - xmin) / bmp.Width;
            double ystep = (ymax - ymin) / bmp.Height;

            List<ComplexNumber> polynomialRoots = new List<ComplexNumber>();

            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            Polynomial polynomialDerivative = polynomial.Derive();

            Console.WriteLine($"Polynomial: {polynomial}");
            Console.WriteLine($"Polynomial derivative: {polynomialDerivative}");

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
                    for (int q = 0; q< NumberOfIterations; q++)
                    {
                        var diff = polynomial.Evaluate(ox).Divide(polynomialDerivative.Evaluate(ox));
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
                    Color newPixelColor = availableColors[id % availableColors.Length];
                    newPixelColor = Color.FromArgb(Math.Min(Math.Max(0, newPixelColor.R-(int)it*2), 255), Math.Min(Math.Max(0, newPixelColor.G - (int)it*2), 255), Math.Min(Math.Max(0, newPixelColor.B - (int)it*2), 255));
                    bmp.SetPixel(column, row, newPixelColor);
                }
            }
            bmp.Save("../../../newtonFractal.png");
            Console.WriteLine(" - KONEC PROGRAMU - "); // MOJE PRIDANO
            Console.ReadKey(); // MOJE PRIDANO
        }
    }
}
