using System.Collections.Generic;

class Polynomial
{
    public List<ComplexNumber> Coefficient { get; set; }

    public Polynomial()
    {
        Coefficient = new List<ComplexNumber>();
    }

    public Polynomial Derive()
    {
        Polynomial derivedPolynomial = new Polynomial();
        ComplexNumber derivedCoefficient;
        for (int i = 1; i < Coefficient.Count; i++)
        {
            derivedCoefficient = Coefficient[i].Multiply(new ComplexNumber() { Re = i });
            derivedPolynomial.Coefficient.Add(derivedCoefficient);
        }

        return derivedPolynomial;
    }

    public ComplexNumber Evaluate(ComplexNumber x)
    {
        ComplexNumber result = ComplexNumber.Zero;
        for (int i = 0; i < Coefficient.Count; i++)
        {
            ComplexNumber coefficient = Coefficient[i];
            ComplexNumber powerOfX = x;
            int power = i;

            if (power > 0)
            {
                for (int j = 0; j < power - 1; j++)
                {
                    powerOfX = powerOfX.Multiply(x);
                }

                coefficient = coefficient.Multiply(powerOfX);
            }

            result = result.Add(coefficient);
        }

        return result;
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < Coefficient.Count; i++)
        {
            output += Coefficient[i];
            if (i > 0)
            {
                output += "x^" + i;

            }
            output += " + ";
        }

        return output;
    }
}