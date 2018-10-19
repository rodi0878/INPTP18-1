using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTPZ1
{
    class Polynomial
    {
        public Complex[] coefficients { get; set; }
        public int Degree { get; private set; }
        public Polynomial(int degree)
        {
            coefficients = new Complex[degree + 1];
        }
        public Polynomial(Complex[] coeff)
        {
            coefficients = coeff;

            this.Degree = coeff.Length - 1;
        }

        public Polynomial Derive()
        {
            Polynomial derivative = new Polynomial(Degree - 1);
            for (int i = 1; i < coefficients.Length; i++)
            {
                derivative.coefficients[i - 1] = this.coefficients[i] * new Complex(i, 0);
            }

            return derivative;
        }

        public Complex Eval(Complex x)
        {
            Complex sum = Complex.Zero;
            for (int i = 0; i < coefficients.Length; i++)
            {
                sum += coefficients[i] * Complex.Pow(x, i);
            }

            return sum;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < coefficients.Length; i++)
            {
                s += coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        s += "x";
                    }
                }
                s += " + ";
            }
            return s.Substring(0, s.Length - 3);
        }
    }
}
