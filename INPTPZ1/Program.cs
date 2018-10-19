using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using INPTPZ1;


namespace cmplx
{
    class Program
    {
        static void Main(string[] args)
        {
            double xMin = -1.5;
            double xMax = 1.5;
            double yMin = -1.5;
            double yMax = 1.5;
            int scale = 100;

            string filename = "out.png";

            if (args.Length == 0)
            {
                filename = "out.png";
                xMin = -1.5;
                xMax = 1.5;
                yMin = -1.5;
                yMax = 1.5;
                scale = 100;

            }
            else if (args.Length == 1)
            {
                filename = args[0];
            }
            else if (args.Length == 6)
            {
                filename = args[0];
                xMin = int.Parse(args[1]);
                xMax = int.Parse(args[2]);
                yMin = int.Parse(args[3]);
                yMax = int.Parse(args[4]);
                scale = int.Parse(args[5]);
            }
            Console.WriteLine("Calculation of fractal");
            Complex[] coefficients = { Complex.One, Complex.Zero, Complex.Zero, Complex.One };
            Polynomial polynom = new Polynomial(coefficients);
            NewtonFractal fractal = new NewtonFractal(polynom, xMin, xMax, yMin, yMax, scale);
            Bitmap image = fractal.DrawFractal();
            image.Save(filename);
            Console.WriteLine("The fractal was successfully built.");
            Console.ReadKey();
        }
    }

}

