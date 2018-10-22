using System.Collections.Generic;

namespace INPTPZ1
{
    public class Polynom
    {
        public List<ComplexNumber> Coefficients { get; set; }

        public Polynom()
        {
            Coefficients = new List<ComplexNumber>();
        }

        public Polynom Derive()
        {
            Polynom resultPolynom = new Polynom();
            for (int i = 1; i < Coefficients.Count; i++)
            {
                resultPolynom.Coefficients.Add(Coefficients[i].Multiply(ComplexNumber.FromRealNumber(i)));
            }

            return resultPolynom;
        }

        public ComplexNumber Eval(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;
            ComplexNumber xPower = ComplexNumber.One;

            foreach (ComplexNumber selectedCoefficient in Coefficients)
            {
                ComplexNumber resultPart = selectedCoefficient.Multiply(xPower);
                result = result.Add(resultPart);

                xPower = xPower.Multiply(x);
            }

            return result;
        }

        public override string ToString()
        {
            string result = "";
            string powerSuffix = "";

            foreach (ComplexNumber selectedCoefficient in Coefficients)
            {
                result += selectedCoefficient;
                result += powerSuffix;
                result += " + ";

                powerSuffix += "x";
            }

            return result;
        }
    }
}