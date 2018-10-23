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

            string filename;

            if (args.Length == 0)
            {
                filename = "out.png";
            }
            else if (args.Length == 1)
            {
                filename = args[0];

            }
            else if (args.Length == 6)
            {
                filename = args[0];
                bool success1 = double.TryParse(args[1], out double xMinParsed);
                bool success2 = double.TryParse(args[2], out double xMaxParsed);
                bool success3 = double.TryParse(args[3], out double yMinParsed);
                bool success4 = double.TryParse(args[4], out double yMaxParsed);
                bool success5 = int.TryParse(args[5], out int scaleParsed);
                if (success1 & success2 & success3 & success4 & success5)
                {
                    xMin = xMinParsed;
                    xMax = xMaxParsed;
                    yMin = yMinParsed;
                    yMax = yMaxParsed;
                    scale = scaleParsed;
                }
                else
                {
                    Console.WriteLine("Error in entering parameters");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid numbers of arguments");
                return;
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

