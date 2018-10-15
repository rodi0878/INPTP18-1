using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using INPTPZ1.ValueObjects;

namespace INPTPZ1
{
    class FractalModel
    {
        private List<ComplexNumber> roots;

        private readonly Color[] colors;

        private readonly double xmin = -1.5;
        private readonly double xmax = 1.5;
        private readonly double ymin = -1.5;
        private readonly double ymax = 1.5;

        private readonly Polynome basePolynome;
        private readonly Polynome derivedPolynome;

        public FractalModel()
        {
            roots = new List<ComplexNumber>();
            colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            basePolynome = new Polynome(new List<ComplexNumber>{
                new ComplexNumber() { Re = 1 },
                ComplexNumber.Zero,
                ComplexNumber.Zero,
                new ComplexNumber() { Re = 1 }
            });
            derivedPolynome = basePolynome.Derive();
        }

        public void GenerateNewtonFractal(Bitmap bitmap)
        {
            double xstep = GetXStep(bitmap);
            double ystep = GetYStep(bitmap);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    double x = xmin + j * xstep;
                    double y = ymin + i * ystep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Re = x,
                        Im = (float)(y)
                    };

                    if (ox.Re == 0)
                        ox.Re = 0.0001;
                    if (ox.Im == 0)
                        ox.Im = 0.0001f;

                    float iteration = NewtonsIteration(ref ox);

                    var id = FindRootNumber(ref ox);

                    bitmap.SetPixel(j, i, CalculateColor(id, iteration));
                }
            }

            bitmap.Save("../../../out.png");
        }

        private int FindRootNumber(ref ComplexNumber ox)
        {
            var known = false;
            var id = 0;
            for (int w = 0; w < roots.Count; w++)
            {
                if (Math.Pow(ox.Re - roots[w].Re, 2) + Math.Pow(ox.Im - roots[w].Im, 2) <= 0.01)
                {
                    known = true;
                    id = w;
                }
            }
            if (!known)
            {
                roots.Add(ox);
                id = roots.Count;
            }

            return id;
        }

        private float NewtonsIteration(ref ComplexNumber ox)
        {
            float iteration = 0;
            for (int q = 0; q < 30; q++)
            {
                var diff = basePolynome.Eval(ox).Divide(derivedPolynome.Eval(ox));
                ox = ox.Subtract(diff);

                if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                {
                    q--;
                }
                iteration++;
            }

            return iteration;
        }

        private Color CalculateColor(int id, float iteration)
        {
            Color calculatedColor = colors[id % colors.Length];

            calculatedColor = Color.FromArgb(Math.Min(Math.Max(0, calculatedColor.R - (int)iteration * 2), 255), Math.Min(Math.Max(0, calculatedColor.G - (int)iteration * 2), 255), Math.Min(Math.Max(0, calculatedColor.B - (int)iteration * 2), 255));

            return calculatedColor;
        }

        private double GetXStep(Bitmap bitmap)
        {
            return (xmax - xmin) / bitmap.Width;
        }

        private double GetYStep(Bitmap bitmap)
        {
            return (ymax - ymin) / bitmap.Height;
        }
    }
}
