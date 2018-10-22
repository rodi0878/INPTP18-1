using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;


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
            // TODO: add parameters from args?
            Bitmap bmp = new Bitmap(300, 300);
            double xmin = -1.5;
            double xmax = 1.5;
            double ymin = -1.5;
            double ymax = 1.5;

            double xstep = (xmax - xmin) / 300;
            double ystep = (ymax - ymin) / 300;

            List<ComplexNumber> koreny = new List<ComplexNumber>();
            // TODO: poly should be parameterised?
            Polynom p = new Polynom();
            p.Coefficients.Add(new ComplexNumber() { Re = 1 });
            p.Coefficients.Add(ComplexNumber.Zero);
            p.Coefficients.Add(ComplexNumber.Zero);
            //p.Coefficients.Add(ComplexNumber.Zero);
            p.Coefficients.Add(new ComplexNumber() { Re = 1 });
            Polynom pd = p.Derive();

            Console.WriteLine(p);
            Console.WriteLine(pd);

            var clrs = new Color[]
            {
                Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
            };

            var maxid = 0;

            // TODO: cleanup!!!
            // for every pixel in image...
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    // find "world" coordinates of pixel
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

                    //Console.WriteLine(ox);

                    // find solution of equation using newton's iteration
                    float it = 0;
                    for (int q = 0; q< 30; q++)
                    {
                        var diff = p.Eval(ox).Divide(pd.Eval(ox));
                        ox = ox.Subtract(diff);

                        //Console.WriteLine($"{q} {ox} -({diff})");
                        if (Math.Pow(diff.Re, 2) + Math.Pow(diff.Im, 2) >= 0.5)
                        {
                            q--;
                        }
                        it++;
                    }

                    //Console.ReadKey();

                    // find solution root number
                    var known = false;
                    var id = 0;
                    for (int w = 0; w <koreny.Count;w++)
                    {
                        if (Math.Pow(ox.Re- koreny[w].Re, 2) + Math.Pow(ox.Im - koreny[w].Im, 2) <= 0.01)
                        {
                            known = true;
                            id = w;
                        }
                    }
                    if (!known)
                    {
                        koreny.Add(ox);
                        id = koreny.Count;
                        maxid = id + 1; 
                    }

                    // colorize pixel according to root number
                    //int vv = id;
                    //int vv = id * 50 + (int)it*5;
                    var vv = clrs[id % clrs.Length];
                    vv = Color.FromArgb(vv.R, vv.G, vv.B);
                    vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R-(int)it*2), 255), Math.Min(Math.Max(0, vv.G - (int)it*2), 255), Math.Min(Math.Max(0, vv.B - (int)it*2), 255));
                    //vv = Math.Min(Math.Max(0, vv), 255);
                    bmp.SetPixel(j, i, vv);
                    //bmp.SetPixel(j, i, Color.FromArgb(vv, vv, vv));
                }
            }

            // TODO: delete I suppose...
            //for (int i = 0; i < 300; i++)
            //{
            //    for (int j = 0; j < 300; j++)
            //    {
            //        Color c = bmp.GetPixel(j, i);
            //        int nv = (int)Math.Floor(c.R * (255.0 / maxid));
            //        bmp.SetPixel(j, i, Color.FromArgb(nv, nv, nv));
            //    }
            //}

                    bmp.Save("../../../out.png");
            //Console.ReadKey();
        }
    }
}
