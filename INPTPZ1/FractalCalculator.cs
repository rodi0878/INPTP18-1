using System;
using System.Collections.Generic;
using System.Drawing;

namespace INPTPZ1
{
    class FractalCalculator
    {
        private const double xmin = -1.5;
        private const double xmax = 1.5;
        private const double ymin = -1.5;
        private const double ymax = 1.5;
        private const int maxColorDepth = 255;
        private readonly Polynomial polynomial;
        private readonly Polynomial derivedPolynomial;
        private List<ComplexNumber> roots;
        private Color[] colours;

        public FractalCalculator()
        {
            roots = new List<ComplexNumber>();
            polynomial = new Polynomial();
            polynomial.Coeficient.Add(new ComplexNumber() { Re = 1 });
            polynomial.Coeficient.Add(ComplexNumber.Zero);
            polynomial.Coeficient.Add(ComplexNumber.Zero);
            polynomial.Coeficient.Add(new ComplexNumber() { Re = 1 });
            derivedPolynomial = polynomial.Derive();

            InitializeColours();
        }

        public double CalculateXStep(int imageHeight)
        {
            double xstep = 0.0;
            return xstep = (xmax - xmin) / imageHeight;
        }

        public double CalculateYStep(int imageWidth)
        {
            double ystep = 0.0;
            return ystep = (ymax - ymin) / imageWidth;
        }

        public Color CalculateFinalColour(int id, float iterator)
        {
            Color finalColour = colours[id % colours.Length];
            finalColour = Color.FromArgb(
                Math.Min(Math.Max(0, finalColour.R - (int)iterator * 2), maxColorDepth),
                Math.Min(Math.Max(0, finalColour.G - (int)iterator * 2), maxColorDepth),
                Math.Min(Math.Max(0, finalColour.B - (int)iterator * 2), maxColorDepth)
                );

            return finalColour;
        }

        public int FindSolutionRootNumber(ComplexNumber complexNumber)
        {
            Boolean isKnown = false;
            int id = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (Math.Pow(complexNumber.Re - roots[i].Re, 2) + Math.Pow(complexNumber.Im - roots[i].Im, 2) <= 0.01)
                {
                    isKnown = true;
                    id = i;
                }
            }
            if (!isKnown)
            {
                roots.Add(complexNumber);
                id = roots.Count;
            }
            return id;
        }

        public int IterateToFindSolution(ComplexNumber complexNumber)
        {
            int iterator = 0;
            for (int k = 0; k < 30; k++)
            {
                ComplexNumber difference = polynomial.Evaluate(complexNumber).Divide(derivedPolynomial.Evaluate(complexNumber));
                complexNumber = complexNumber.Subtract(difference);

                if (Math.Pow(difference.Re, 2) + Math.Pow(difference.Im, 2) >= 0.5)
                {
                    k--;
                }
                iterator++;
            }
            return iterator;
        }

        public void CalculateFractals(int imageHeight, int imageWidth, Bitmap result)
        {
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    ComplexNumber complexNumber = new ComplexNumber()
                    {
                        Re = xmin + j * CalculateXStep(imageHeight),
                        Im = ymin + i * CalculateYStep(imageWidth)
                    };

                    CheckRealAndImaginaryPart(complexNumber);

                    float iterator = IterateToFindSolution(complexNumber);
                    int finalSolution = FindSolutionRootNumber(complexNumber);
                    result.SetPixel(j, i, CalculateFinalColour(finalSolution, iterator));
                }
            }
            SaveImage(result);
        }

        public void InitializeColours()
        {
            colours = new Color[]
           {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
           };
        }

        public void CheckRealAndImaginaryPart(ComplexNumber complexNumber)
        {

            if (complexNumber.Re == 0)
                complexNumber.Re = 0.0001;
            if (complexNumber.Im == 0)
                complexNumber.Im = 0.0001f;
        }

        public void SaveImage(Bitmap bitmap)
        {
            bitmap.Save("../../../out.png");
        }
    }
}
