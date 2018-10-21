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
        public static readonly Color[] availableColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta };

        public const int MaxNumberOfIterations = 30;
        public const double ToleranceOfSolutionRoot = 0.01;

        public const double xMin = -1.5;
        public const double xMax = 1.5;
        public const double yMin = -1.5;
        public const double yMax = 1.5;

        static void Main(string[] args)
        {
            Bitmap bmp = new Bitmap(300, 300);
            double xstep = CalculateStep(xMin, xMax, bmp.Width);
            double ystep = CalculateStep(yMin, yMax, bmp.Height);

            List<ComplexNumber> polynomialRoots = new List<ComplexNumber>();
            Polynomial polynomial = CreateSamplePolynomial();
            Polynomial polynomialDerivative = polynomial.Derive();

            Console.WriteLine($"Polynomial: {polynomial}");
            Console.WriteLine($"Polynomial derivative: {polynomialDerivative}");

            for (int row = 0; row < bmp.Height; row++)
            {
                for (int column = 0; column < bmp.Width; column++)
                {
                    double x = FindWorldCoordinate(xMin, column, xstep);
                    double y = FindWorldCoordinate(yMin, row, ystep);

                    ComplexNumber ox = new ComplexNumber() { Re = x, Im = y };

                    ModifyComplexNumberIfZero(ox);

                    FindSolutionWithNewtonsIteration(polynomial, polynomialDerivative, ref ox, out int iterationNumber);

                    int rootNumber = FindSolutionRootNumber(polynomialRoots, ox);

                    ColorizePixelOfBitmap(bmp, row, column, iterationNumber, rootNumber);
                }
            }
            bmp.Save("../../../newtonFractal.png");
        }

        private static void ModifyComplexNumberIfZero(ComplexNumber number)
        {
            if (number.Re == 0)
                number.Re = 0.0001;
            if (number.Im == 0)
                number.Im = 0.0001;
        }

        private static void ColorizePixelOfBitmap(Bitmap bmp, int row, int column, int iterationNumber, int rootNumber)
        {
            Color newPixelColor = availableColors[rootNumber % availableColors.Length];
            newPixelColor = Color.FromArgb(
                Math.Min(Math.Max(0, newPixelColor.R - iterationNumber * 2), 255),
                Math.Min(Math.Max(0, newPixelColor.G - iterationNumber * 2), 255),
                Math.Min(Math.Max(0, newPixelColor.B - iterationNumber * 2), 255)
                );
            bmp.SetPixel(column, row, newPixelColor);
        }

        private static double CalculateStep(double minimum, double maximum, int widthOrHeight)
        {
            return (maximum - minimum) / widthOrHeight;
        }

        private static double FindWorldCoordinate(double minimum, int rowOrColumn, double step)
        {
            return minimum + rowOrColumn * step;
        }

        private static int FindSolutionRootNumber(List<ComplexNumber> polynomialRoots, ComplexNumber ox)
        {
            bool known = false;
            int rootNumber = 0;
            for (int i = 0; i < polynomialRoots.Count; i++)
            {
                if (Math.Pow(ox.Re - polynomialRoots[i].Re, 2) + Math.Pow(ox.Im - polynomialRoots[i].Im, 2) <= ToleranceOfSolutionRoot)
                {
                    known = true;
                    rootNumber = i;
                }
            }
            if (!known)
            {
                polynomialRoots.Add(ox);
                rootNumber = polynomialRoots.Count;
            }

            return rootNumber;
        }

        private static void FindSolutionWithNewtonsIteration(Polynomial polynomial, Polynomial polynomialDerivative, ref ComplexNumber ox, out int iterationNumber)
        {
            iterationNumber = 0;
            for (int i = 0; i < MaxNumberOfIterations; i++)
            {
                var diff = polynomial.Evaluate(ox).Divide(polynomialDerivative.Evaluate(ox));
                ox = ox.Subtract(diff);

                if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                {
                    i--;
                }
                iterationNumber++;
            }
        }

        private static Polynomial CreateSamplePolynomial()
        {
            Polynomial polynomial = new Polynomial();
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { Re = 1 });
            return polynomial;
        }

    }
}
