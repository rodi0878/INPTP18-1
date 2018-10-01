using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTPZ1.ValueObjects
{
    class Polynome
    {
        private List<ComplexNumber> ComplexNumbers { get; set; }

        public Polynome()
        {
            ComplexNumbers = new List<ComplexNumber>();
        }

        public Polynome(List<ComplexNumber> complexNumbers)
        {
            ComplexNumbers = complexNumbers;
        }

        public Polynome Derive()
        {
            List<ComplexNumber> roots = new List<ComplexNumber>();

            for (int i = 1; i < ComplexNumbers.Count; i++)
            {
                roots.Add(ComplexNumbers[i].Multiply(new ComplexNumber() { Re = i }));
            }

            return new Polynome(roots);
        }
        
        public ComplexNumber Eval(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;
            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                ComplexNumber coef = ComplexNumbers[i];
                ComplexNumber bx = x;
                int power = i;

                if (i > 0)
                {
                    for (int j = 0; j < power - 1; j++)
                        bx = bx.Multiply(x);

                    coef = coef.Multiply(bx);
                }

                s = s.Add(coef);
            }

            return s;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < ComplexNumbers.Count; i++)
            {
                s += ComplexNumbers[i];
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
