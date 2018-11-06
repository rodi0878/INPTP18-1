﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace INPTPZ1.Program
{
    public static class Helper
    {
        public static void FixIfZeroAppeared(ComplexNumber ox)
        {
            if (ox.Real == 0)
                ox.Real = 0.0001;
            if (ox.Imaginary == 0)
                ox.Imaginary = 0.0001f;
        }

        public static void SetInitialValues(out double xmin, out double ymin, out double xstep, out double ystep, int size)
        {
            xmin = -1.5;
            double xmax = 1.5;
            ymin = -1.5;
            double ymax = 1.5;

            xstep = (xmax - xmin) / size;
            ystep = (ymax - ymin) / size;
        }

        public static float SolveEquationByNewtonIteration(Polynom polynom, Polynom polynomDerived, ref ComplexNumber ox)
        {
            float it = 0;
            for (int q = 0; q < 30; q++)
            {
                var diff = polynom.Eval(ox).Divide(polynomDerived.Eval(ox));
                ox = ox.Subtract(diff);

                if (Math.Pow(diff.Real, 2) + Math.Pow(diff.Imaginary, 2) >= 0.5)
                {
                    q--;
                }
                it++;
            }
            return it;
        }

        public static int FindSolutionRootNumber(List<ComplexNumber> roots, ComplexNumber ox)
        {
            var known = false;
            var id = 0;
            for (int w = 0; w < roots.Count; w++)
            {
                if (Math.Pow(ox.Real - roots[w].Real, 2) + Math.Pow(ox.Imaginary - roots[w].Imaginary, 2) <= 0.01)
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

        public static void ColorizePixel(Bitmap bmp, Color[] clrs, int i, int j, double it, int id)
        {
            var vv = clrs[id % clrs.Length];
            vv = Color.FromArgb(vv.R, vv.G, vv.B);
            vv = Color.FromArgb(Math.Min(Math.Max(0, vv.R - (int)it * 2), 255), Math.Min(Math.Max(0, vv.G - (int)it * 2), 255), Math.Min(Math.Max(0, vv.B - (int)it * 2), 255));
            bmp.SetPixel(j, i, vv);
        }


        public static Bitmap InitializeBitmap(string[] args)
        {
            Bitmap picture;
            if (ValidateInputArguments(args))
            {
                int size = int.Parse(args[0]);
                picture = new Bitmap(size, size);
            }
            else
            {
                picture = new Bitmap(300, 300);
            }

            return picture;
        }

        private static bool ValidateInputArguments(string[] args)
        {
            if (args.Length > 1 || args.Length <= 0)
            {
                return false;
            }

            bool validation = int.TryParse(args[0], out int result);

            if (!validation)
            {
                return false;
            }
            else return true;
        }

        public static void CreateFinalPicture(Bitmap picture, double xmin, double ymin, double xstep, double ystep, Polynom polynom, Polynom polynomDerived, Color[] colors)
        {
            List<ComplexNumber> koreny = new List<ComplexNumber>();

            for (int i = 0; i < picture.Size.Height; i++)
            {
                for (int j = 0; j < picture.Size.Width; j++)
                {
                    double x = xmin + j * xstep;
                    double y = ymin + i * ystep;

                    ComplexNumber ox = new ComplexNumber()
                    {
                        Real = x,
                        Imaginary = y
                    };

                    FixIfZeroAppeared(ox);

                    double it = SolveEquationByNewtonIteration(polynom, polynomDerived, ref ox);

                    int id = FindSolutionRootNumber(koreny, ox);

                    ColorizePixel(picture, colors, i, j, it, id);
                }
            }
        }
    }
}
