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
        private Bitmap bmp;
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
        private Complex ox;
        private int id;
        private double eps1 = 0.0001;

        public NewtonFractal(Polynomial polynom, double xMin, double xMax, double yMin, double yMax, int scale)
        {
            this.polynom = polynom;
            width = (int)Math.Round((xMax - xMin) * scale);
            height = (int)Math.Round((yMax - yMin) * scale);
            bmp = new Bitmap(width, height);
            xstep = (xMax - xMin) / width;
            ystep = (yMax - yMin) / height;
            this.xMin = xMin;
            this.yMin = yMin;

            roots = new List<Complex>();

            derivative = polynom.Derive();
            colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta };
        }

        private int FindSolution()
        {
            int iteration = 0;
            for (int q = 0; q < 30; q++)
            {
                Complex diff = polynom.Eval(ox) / derivative.Eval(ox);
                ox = ox - diff;

                if (Complex.Abs(diff) >= Math.Sqrt(0.5))
                {
                    q--;
                }
                iteration++;
            }
            return iteration;
        }
        private int FindRootNumber()
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
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ox = GetInitialValue(j, i);
                    int iteration = FindSolution();

                    id = FindRootNumber();

                    Color vv = colors[id % colors.Length];
                    bmp.SetPixel(j, i, vv);
                }
            }
            return bmp;
        }
    }
}
