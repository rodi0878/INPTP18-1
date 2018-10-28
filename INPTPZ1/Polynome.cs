using System.Collections.Generic;

namespace INPTPZ1
{
    class Polynome
    {
        public List<ComplexNumber> ComplexNumbers { get; set; }

        public Polynome()
        {
            ComplexNumbers = new List<ComplexNumber>();
        }

        public Polynome(int count)
        {
            ComplexNumbers = new List<ComplexNumber>
            {
                new ComplexNumber() { RealPart = 1 }
            };

            for (int i = 0; i < count - 1; i++)
            {
                ComplexNumbers.Add(ComplexNumber.Zero);
            }
            ComplexNumbers.Add(new ComplexNumber() { RealPart = 1 });
        }

        public Polynome Derive()
        {
            Polynome p = new Polynome();

            for (int i = 1; i < ComplexNumbers.Count; i++)
            {
                p.ComplexNumbers.Add(ComplexNumbers[i].Multiply(new ComplexNumber() { RealPart = i }));
            }

            return p;
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;

            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                ComplexNumber complexNumber = ComplexNumbers[i];
                ComplexNumber complexNumberEvaluated = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        complexNumberEvaluated = complexNumberEvaluated.Multiply(x);
                    complexNumber = complexNumber.Multiply(complexNumberEvaluated);
                }
                s = s.Add(complexNumber);
            }

            return s;
        }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                output += ComplexNumbers[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                        output += "x";
                }
                output += " + ";
            }

            return output;
        }
    }
}
