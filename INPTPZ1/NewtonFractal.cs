using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;

namespace INPTPZ1
{
    class NewtonFractal
    {
        private int maxId = 0;
        private Polynomial polynom;
        private List<Complex> roots;
        private Polynomial derivative;
        private double xstep;
        private double ystep;
        private double xMin;
        private double yMin;
        private int width;
        private int height;
        private Color[] colors;
        private double eps1 = 0.0001;
        private double eps2 = 0.001;

        public NewtonFractal(Polynomial polynom, double xMin, double xMax, double yMin, double yMax, int scale)
        {
            this.polynom = polynom;
            width = (int)Math.Round((xMax - xMin) * scale);
            height = (int)Math.Round((yMax - yMin) * scale);

            xstep = (xMax - xMin) / width;
            ystep = (yMax - yMin) / height;
            this.xMin = xMin;
            this.yMin = yMin;

            roots = new List<Complex>();

            derivative = polynom.Derive();
            colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta };
        }

        private Complex FindSolution(Complex ox)
        {
            int iteration = 0;
            Complex diff;
            do
            {
                diff = polynom.Eval(ox) / derivative.Eval(ox);
                ox = ox - diff;
                iteration++;
            } while (Complex.Abs(diff) >= eps2);
            return ox;
        }

        private int FindRootNumber(Complex ox)
        {
            bool known = false;
            int rootNumber = 0;
            for (int w = 0; w < roots.Count; w++)
            {
                if (Complex.Abs(ox - roots[w]) <= 0.1)
                {
                    known = true;
                    rootNumber = w;
                }
            }
            if (!known)
            {
                roots.Add(ox);
                rootNumber = roots.Count;
                maxId = rootNumber + 1;
            }
            return rootNumber;
        }

        private Complex GetInitialValue(int j, int i)
        {
            double x = xMin + j * xstep;
            double y = yMin + i * ystep;

            Complex initial = new Complex(x, y);

            if (initial.Real == 0)
                initial += new Complex(eps1, 0);
            if (initial.Imaginary == 0)
                initial += new Complex(0, eps1);
            return initial;
        }
        public Bitmap DrawFractal()
        {
            Complex ox;
            int id;
            Bitmap bmp = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ox = GetInitialValue(j, i);
                    ox = FindSolution(ox);
                    id = FindRootNumber(ox);
                    Color vv = colors[id % colors.Length];
                    bmp.SetPixel(j, i, vv);
                }
            }
            return bmp;
        }
    }
}
