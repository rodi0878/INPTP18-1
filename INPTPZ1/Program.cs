
namespace INPTPZ1
{
    /// <summary>
    /// This program produce Newton fractals.
    /// See more at: https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            NewtonFractalCalculator calculator = new NewtonFractalCalculator();
            calculator.CalculateNewtonsFractal();
            calculator.SaveBitmapWithResult();
        }

    }

}
