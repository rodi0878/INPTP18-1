using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTPZ1
{
    class Complex
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public readonly static Complex Zero = new Complex(0, 0);
        public readonly static Complex One = new Complex(1, 0);
        public readonly static Complex ImaginaryOne = new Complex(0, 1);

        public Complex(double re, double im)
        {
            Real = re;
            Imaginary = im;
        }

        public static Complex Multiply(Complex a, Complex b)
        {
            double re = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double im = a.Real * b.Imaginary + a.Imaginary * b.Real;
            return new Complex(re, im);
        }


        public static Complex Add(Complex a, Complex b)
        {
            double re = a.Real + b.Real;
            double im = a.Imaginary + b.Imaginary;
            return new Complex(re, im);
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return Add(a, b);
        }
        public static Complex operator -(Complex a, Complex b)
        {
            return Subtract(a, b);
        }
        public static Complex operator *(Complex a, Complex b)
        {
            return Multiply(a, b);
        }
        public static Complex operator /(Complex a, Complex b)
        {
            return Divide(a, b);
        }

        public static Complex Subtract(Complex a, Complex b)
        {
            double re = a.Real - b.Real;
            double im = a.Imaginary - b.Imaginary;
            return new Complex(re, im);
        }

        public override string ToString()
        {
            return $"({Real} + {Imaginary}i)";
        }

        public static Complex Divide(Complex a, Complex b)
        {
            Complex tmp = Multiply(a, new Complex(b.Real, -b.Imaginary));
            double tmp2 = b.Real * b.Real + b.Imaginary * b.Imaginary;
            return new Complex(tmp.Real / tmp2, tmp.Imaginary / tmp2);
        }

        public static double Abs(Complex a)
        {
            return Math.Sqrt(a.Real * a.Real + a.Imaginary * a.Imaginary);
        }
        public static Complex Pow(Complex a, int power)
        {
            Complex bx = a;
            if (power > 0)
            {
                for (int i = 0; i < power - 1; i++)
                {
                    bx = bx * a;
                }
                return bx;
            }
            return Complex.One;
        }
    }
}
