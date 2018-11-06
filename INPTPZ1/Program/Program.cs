using System;
using System.Collections.Generic;
using System.Drawing;


namespace INPTPZ1.Program
{
    /// <summary>
    /// This program produces Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap picture;
            if (ValidateInputArguments(args))
            {
                int size = int.Parse(args[0]);
                picture = new Bitmap(size, size);
            } else
            {
                picture = new Bitmap(300, 300);
            }

            Helper.SetInitialValues(out double xmin, out double ymin, out double xstep, out double ystep, picture.Size.Height);

            Polynom polynom = new Polynom(new List<ComplexNumber>
            {
                new ComplexNumber() { Real = 1 }, ComplexNumber.Zero, ComplexNumber.Zero, new ComplexNumber() { Real = 1 }
            });

            Polynom polynomDerived = polynom.Derive();

            var colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            CreateFinalPicture(picture, xmin, ymin, xstep, ystep, polynom, polynomDerived, colors);

            picture.Save("../../../out.png");
        }

        private static bool ValidateInputArguments(string[] args)
        {
            if (args.Length > 1 || args.Length <= 0)
            {
                return false;
            }

            bool validation = int.TryParse(args[0], out int result);

            if (!validation)
            {
                return false;
            }
            else return true;

        }

        private static void CreateFinalPicture(Bitmap picture, double xmin, double ymin, double xstep, double ystep, Polynom polynom, Polynom polynomDerived, Color[] colors)
        {
            List<ComplexNumber> koreny = new List<ComplexNumber>();

            for (int i = 0; i < picture.Size.Height; i++)
            {
                for (int j = 0; j < picture.Size.Width; j++)
                {
                    double x = xmin + j * xstep;
                    double y = ymin + i * ystep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Real = x,
                        Imaginary = y
                    };

                    Helper.FixIfZeroAppeared(ox);

                    double it = Helper.SolveEquationByNewtonIteration(polynom, polynomDerived, ref ox);

                    int id = Helper.FindSolutionRootNumber(koreny, ox);

                    Helper.ColorizePixel(picture, colors, i, j, it, id);
                }
            }
        }
    }
}
