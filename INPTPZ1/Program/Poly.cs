using System.Collections.Generic;

namespace INPTPZ1.Program
{
    public class Polynom
    {
        public List<ComplexNumber> Coeficients { get; set; }

        public Polynom()
        {
            Coeficients = new List<ComplexNumber>();
        }

        public Polynom(List<ComplexNumber> list)
        {
            Coeficients = list;
        }

        public Polynom Derive()
        {
            Polynom p = new Polynom();
            for (int i = 1; i < Coeficients.Count; i++)
            {
                p.Coeficients.Add(Coeficients[i].Multiply(new ComplexNumber() { Real = i }));
            }

            return p;
        }

        public ComplexNumber Eval(ComplexNumber x)
        {
            ComplexNumber s = ComplexNumber.Zero;
            for (int i = 0; i < Coeficients.Count; i++)
            {
                ComplexNumber coef = Coeficients[i];
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
