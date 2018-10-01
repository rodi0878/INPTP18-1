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

        public FractalModel()
        {
            roots = new List<ComplexNumber>();
            colors = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };
        }

        public void GenerateNewtonFractal(Bitmap bitmap)
        {
            double xmin = -1.5;
            double xmax = 1.5;
            double ymin = -1.5;
            double ymax = 1.5;

            double xstep = (xmax - xmin) / bitmap.Width;
            double ystep = (ymax - ymin) / bitmap.Height;
            
            List<ComplexNumber> polynomeArgs = new List<ComplexNumber>
            {
                new ComplexNumber() { Re = 1 },
                ComplexNumber.Zero,
                ComplexNumber.Zero,
                new ComplexNumber() { Re = 1 }
            };

            Polynome p = new Polynome(polynomeArgs);
            Polynome pd = p.Derive();

            var maxid = 0;

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

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q < 30; q++)
                    {
                        var diff = p.Eval(ox).Divide(pd.Eval(ox));
                        ox = ox.Subtract(diff);
                        
                        if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    // find solution root number
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
                        maxid = id + 1;
                    }

                    Color calculatedColor = colors[id % colors.Length];

                    calculatedColor = Color.FromArgb(calculatedColor.R, calculatedColor.G, calculatedColor.B);
                    calculatedColor = Color.FromArgb(Math.Min(Math.Max(0, calculatedColor.R - (int)it * 2), 255), Math.Min(Math.Max(0, calculatedColor.G - (int)it * 2), 255), Math.Min(Math.Max(0, calculatedColor.B - (int)it * 2), 255));

                    bitmap.SetPixel(j, i, calculatedColor);
                }
            }

            bitmap.Save("../../../out.png");
        }
    }
}
