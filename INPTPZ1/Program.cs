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
            const int imageWidth = 300;
            const int imageHeight = 300;

            Bitmap bitMap = new Bitmap(imageHeight, imageWidth);
            FractalCalculator calculator = new FractalCalculator();
            calculator.CalculateFractals(imageWidth, imageHeight, bitMap);
        }

    }
}
