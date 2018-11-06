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
            Bitmap picture = Helper.InitializeBitmap(args);

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

            Helper.CreateFinalPicture(picture, xmin, ymin, xstep, ystep, polynom, polynomDerived, colors);

            picture.Save("../../../out.png");
        }
    }
}
