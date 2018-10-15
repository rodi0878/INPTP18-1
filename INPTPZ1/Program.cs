using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            Bitmap bitmap = null;

            if (args.Length == 1)
            {
                bool test = int.TryParse(args[0], out int size);
                if (!test)
                {
                    Console.WriteLine("The width and height of Bitmap must be integer.");
                    return;
                }

                bitmap = new Bitmap(size, size);
            }
            else if (args.Length > 1)
            {
                Console.WriteLine("Only one parameter is allowed.");
                return;
            }
            else
            {
                bitmap = new Bitmap(300, 300);
                Console.WriteLine("Using default values. Width: 300px, Height: 300px");
            }

            FractalModel fractalModel = new FractalModel();
            fractalModel.GenerateNewtonFractal(bitmap);
        }
    }
}
