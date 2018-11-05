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
        private const double MinOfWordCoordinateX = -1.5;
        private const double MaxOfWordCoordinateX = 1.5;
        private const double MinOfWordCoordinateY = -1.5;
        private const double MaxOfWordCoordinateY = 1.5;

        private static double WordHeight => MaxOfWordCoordinateY - MinOfWordCoordinateY;
        private static double WordWidth => MaxOfWordCoordinateX - MinOfWordCoordinateX;

        private static int bitmapWidth = 300;
        private static int bitmapHeight = 300;

        private static double XStep => WordWidth / bitmapWidth;
        private static double YStep => WordHeight / bitmapHeight;

        private static int maximumNumberOfIterations = 30;

        private static readonly Color[] Colors = new Color[]
        {
            Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan,
            Color.Magenta
        };

        public static void Main(string[] args)
        {
            Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            List<ComplexNumber> roots = new List<ComplexNumber>();

            var poly = PreparePoly();
            Polynom derivePoly = poly.Derive();

            Console.WriteLine(poly);
            Console.WriteLine(derivePoly);

            SetColorForEachPixel(poly, derivePoly, roots, bitmap);
        }

        private static void SetColorForEachPixel(Polynom poly, Polynom derivePoly, List<ComplexNumber> roots,
            Bitmap bitmap)
        {
            for (int i = 0; i < bitmapHeight; i++)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    double wordCoordinatesX = CalculateWordCoordinatesX(j);
                    double wordCoordinatesY = CalculateWordCoordinatesY(i);

                    ComplexNumber complexNumberForSelectedPixel = new ComplexNumber()
                    {
                        Re = wordCoordinatesX,
                        Im = wordCoordinatesY
                    };

                    if (complexNumberForSelectedPixel.Re == 0)
                        complexNumberForSelectedPixel.Re = 0.0001;
                    if (complexNumberForSelectedPixel.Im == 0)
                        complexNumberForSelectedPixel.Im = 0.0001f;

                    var newtonsIteration =
                        FindSolutionOfNewtonsIteration(poly, derivePoly, ref complexNumberForSelectedPixel);
                    var rootNumber = FindSolutionOfRootNumber(roots, complexNumberForSelectedPixel);
                    ColorizePixel(j, i, bitmap, rootNumber, newtonsIteration);
                }
            }

            bitmap.Save("../../../out.png");
        }

        private static double CalculateWordCoordinatesY(int pixelIndex)
        {
            return MinOfWordCoordinateY + pixelIndex * YStep;
        }

        private static double CalculateWordCoordinatesX(int pixelIndex)
        {
            return MinOfWordCoordinateX + pixelIndex * XStep;
        }

        private static Polynom PreparePoly()
        {
            Polynom poly = new Polynom();
            poly.Coefficients.Add(ComplexNumber.FromRealNumber(1));
            poly.Coefficients.Add(ComplexNumber.Zero);
            poly.Coefficients.Add(ComplexNumber.Zero);
            poly.Coefficients.Add(ComplexNumber.FromRealNumber(1));
            return poly;
        }

        private static void ColorizePixel(int x, int y, Bitmap bitmap, int rootNumber, int iterations)
        {
            int colorIndex = rootNumber % Colors.Length;
            var selectedColor = Colors[colorIndex];

            int redComponent = CalculateComponentAmount(selectedColor.R, iterations);
            int greenComponent = CalculateComponentAmount(selectedColor.G, iterations);
            int blueComponent = CalculateComponentAmount(selectedColor.B, iterations);

            var calculatedColor = Color.FromArgb(redComponent, greenComponent, blueComponent);
            bitmap.SetPixel(x, y, calculatedColor);
        }

        private static int CalculateComponentAmount(int componentAmount, int iterations)
        {
            return Math.Min(Math.Max(0, componentAmount - iterations * 2), 255);
        }

        private static int FindSolutionOfRootNumber(List<ComplexNumber> roots, ComplexNumber complexNumber)
        {
            var known = false;
            var id = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(complexNumber.Re - roots[i].Re, 2) + Math.Pow(complexNumber.Im - roots[i].Im, 2) <= 0.01)
                {
                    known = true;
                    id = i;
                }
            }

            if (!known)
            {
                roots.Add(complexNumber);
                id = roots.Count;
            }

            return id;
        }

        private static int FindSolutionOfNewtonsIteration(Polynom poly, Polynom derivePoly,
            ref ComplexNumber complexNumber)
        {
            int iterations = 0;
            for (int i = 0; i < maximumNumberOfIterations; i++)
            {
                var diff = poly.Eval(complexNumber).Divide(derivePoly.Eval(complexNumber));
                complexNumber = complexNumber.Subtract(diff);

                if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                {
                    i--;
                }

                iterations++;
            }

            return iterations;
        }
    }
}