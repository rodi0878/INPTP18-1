using System.Collections.Generic;
namespace INPTPZ1
{
    class Polynomial
    {
        public List<ComplexNumber> Coeficient { get; set; }

        public Polynomial()
        {
            Coeficient = new List<ComplexNumber>();
        }

        public Polynomial Derive()
        {
            Polynomial polynome = new Polynomial();
            for (int i = 1; i < Coeficient.Count; i++)
            {
                polynome.Coeficient.Add(Coeficient[i].Multiply(new ComplexNumber() { Re = i }));
            }
            return polynome;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber complexNumber = ComplexNumber.Zero;
            for (int i = 0; i < Coeficient.Count; i++)
            {
                ComplexNumber coeficient = Coeficient[i];
                ComplexNumber multiplied = x;
                if (i > 0)
                {
                    for (int j = 0; j < i - 1; j++)
                        multiplied = multiplied.Multiply(x);

                    coeficient = coeficient.Multiply(multiplied);
                }
                complexNumber = complexNumber.Add(coeficient);
            }
            return complexNumber;
        }

        public override string ToString()
        {
            string outputValue = "";
            for (int i = 0; i < Coeficient.Count; i++)
            {
                outputValue += Coeficient[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        outputValue += "x";
                    }
                }
                outputValue += " + ";
            }
            return outputValue;
        }
    }
}
