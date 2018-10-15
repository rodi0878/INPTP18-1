using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTPZ1.ValueObjects
{
    class Polynome
    {
        private List<ComplexNumber> Coeficients { get; set; }

        public Polynome()
        {
            Coeficients = new List<ComplexNumber>();
        }

        public Polynome(List<ComplexNumber> complexNumbers)
        {
            Coeficients = complexNumbers;
        }

        public Polynome Derive()
        {
            List<ComplexNumber> roots = new List<ComplexNumber>();

            for (int i = 1; i < Coeficients.Count; i++)
            {
                roots.Add(Coeficients[i].Multiply(ComplexNumber.FromReal(i)));
            }

            return new Polynome(roots);
        }

        public ComplexNumber Eval(ComplexNumber x)
        {
            ComplexNumber result = ComplexNumber.Zero;

            for (int i = 0; i < Coeficients.Count; i++)
            {
                ComplexNumber coeficient = Coeficients[i];
                ComplexNumber powered = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        powered = powered.Multiply(x);

                    coeficient = coeficient.Multiply(powered);
                }

                result = result.Add(coeficient);
            }

            return result;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Coeficients.Count; i++)
            {
                s += Coeficients[i];
                if (i > 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        s += "x";
                    }
                }
                s += " + ";
            }
            return s;
        }
    }
}
