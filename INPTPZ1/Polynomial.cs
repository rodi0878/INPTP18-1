using System.Collections.Generic;

namespace INPTPZ1
{
    class Polynomial
    {
        public List<ComplexNumber> Coefficients { get; set; }

        public Polynomial()
        {
            Coefficients = new List<ComplexNumber>();
        }

        public Polynomial Derive()
        {
            Polynomial p = new Polynomial();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                p.Coefficients.Add(Coefficients[i].Multiply(ComplexNumber.FromRealNumber(i)));
            }

            return p;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                ComplexNumber coef = Coefficients[i];
                ComplexNumber bx = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(x);

                    coef = coef.Multiply(bx);
                }

                result = result.Add(coef);
            }

            return result;
        }

        public override string ToString()
        {
            string resultStringRepresentation = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                resultStringRepresentation += Coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        resultStringRepresentation += "x";
                    }
                }
                resultStringRepresentation += " + ";
            }
            return resultStringRepresentation;
        }
    }
}
