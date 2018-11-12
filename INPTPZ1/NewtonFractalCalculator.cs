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
    class NewtonFractalCalculator
    {
        static Color[] rootColors = new Color[]{
            Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange,
            Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
        };

        const int bitmapWidth = 500;
        const int bitmapHeight = 500;
        Bitmap newtonFractalBitmap = new Bitmap(bitmapWidth, bitmapHeight);

        const double xmin = -1.5;
        const double xmax = 1.5;
        const double ymin = -1.5;
        const double ymax = 1.5;

        const double xstep = (xmax - xmin) / bitmapWidth;
        const double ystep = (ymax - ymin) / bitmapHeight;

        const int maxNumberOfNewtonIterations = 30;

        List<ComplexNumber> roots = new List<ComplexNumber>();
        Polynomial originalPolynomial = new Polynomial();
        Polynomial derivedPolynomial;

        public NewtonFractalCalculator()
        {
            originalPolynomial.Coefficient.Add(new ComplexNumber() { Re = 1 });
            originalPolynomial.Coefficient.Add(ComplexNumber.Zero);
            originalPolynomial.Coefficient.Add(ComplexNumber.Zero);
            originalPolynomial.Coefficient.Add(new ComplexNumber() { Re = 1 });

            derivedPolynomial = originalPolynomial.Derive();

            Console.WriteLine(originalPolynomial);
            Console.WriteLine(derivedPolynomial);
        }

        public void CalculateNewtonsFractal()
        {
            for (int i = 0; i < bitmapHeight; i++)
            {
                for (int j = 0; j < bitmapWidth; j++)
                {
                    ComplexNumber guesOfX = new ComplexNumber()
                    {
                        Re = xmin + j * xstep,
                        Im = ymin + i * ystep
                    };

                    if (guesOfX.Re == 0)
                        guesOfX.Re = 0.0001;
                    if (guesOfX.Im == 0)
                        guesOfX.Im = 0.0001f;

                    int numberOfIterations = 0;
                    ApplyNewtonsMethod(ref guesOfX, ref numberOfIterations);

                    int rootID = 0;
                    FindRootSolutionID(guesOfX, ref rootID);

                    ColorizePixel(j, i, numberOfIterations, rootID);
                }
            }
        }
        
        private void ApplyNewtonsMethod(ref ComplexNumber guessOfX, ref int numberOfIterations)
        {
            for (int i = 0; i < maxNumberOfNewtonIterations; i++)
            {
                ComplexNumber distance = originalPolynomial.Evaluate(guessOfX).Divide(derivedPolynomial.Evaluate(guessOfX));
                guessOfX = guessOfX.Subtract(distance);

                if (Math.Pow(distance.Re, 2) + Math.Pow(distance.Im, 2) >= 0.5)
                {
                    i--;
                }
                numberOfIterations++;
            }
        }

        private void FindRootSolutionID(ComplexNumber guessOfX, ref int rootID)
        {
            bool knownSolutionNumber = false;

            for (int i = 0; i < roots.Count; i++)
            {
                ComplexNumber distance = guessOfX.Subtract(roots[i]);
                if (Math.Pow(distance.Re, 2) + Math.Pow(distance.Im, 2) <= 0.01)
                {
                    knownSolutionNumber = true;
                    rootID = i;
                }
            }
            if (!knownSolutionNumber)
            {
                roots.Add(guessOfX);
                rootID = roots.Count;
            }
        }
        
        private void ColorizePixel(int x, int y, int numberOfIterations, int rootID)
        {
            Color pixelRootColor = rootColors[rootID % rootColors.Length];
            Color pixelShadedColor = Color.FromArgb(
                GetShadedColorChannel(pixelRootColor.R, numberOfIterations),
                GetShadedColorChannel(pixelRootColor.G, numberOfIterations),
                GetShadedColorChannel(pixelRootColor.B, numberOfIterations)
                );
            newtonFractalBitmap.SetPixel(x, y, pixelShadedColor);
        }

        private int GetShadedColorChannel(int basicColorChannel, int numberOfIterations)
        {
            return Math.Min(Math.Max(0, basicColorChannel - numberOfIterations * 2), 255);
        }

        public void SaveBitmapWithResult()
        {
            newtonFractalBitmap.Save("../../../NewtonFractalBitmap.png");
        }
    }
}
