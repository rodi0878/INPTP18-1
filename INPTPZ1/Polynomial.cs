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
            Polynomial derivative = new Polynomial();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                derivative.Coefficients.Add(Coefficients[i].Multiply(ComplexNumber.FromRealNumber(i)));
            }

            return derivative;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;
            for (int i = 0; i < Coefficients.Count; i++)
            {
                ComplexNumber selectedCoefficient = Coefficients[i];
                ComplexNumber bx = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(x);

                    selectedCoefficient = selectedCoefficient.Multiply(bx);
                }

                result = result.Add(selectedCoefficient);
            }

            return result;
        }

        public override string ToString()
        {
            string resultString = "";
            for (int i = 0; i < Coefficients.Count; i++)
            {
                resultString += Coefficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        resultString += "x";
                    }
                }
                resultString += " + ";
            }
            return resultString;
        }
    }
}
