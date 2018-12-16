using System.Collections.Generic;
using System.Text;

namespace INPTPZ1
{
    class Poly
    {
        public List<Cplx> Roots { get; set; } = new List<Cplx>();

        public Poly Derive()
        {
            Poly p = new Poly();
            for (int i = 1; i < Roots.Count; i++)
                p.Roots.Add(Roots[i] * new Cplx(i, 0));

            return p;
        }

        public Cplx Eval(Cplx x)
        {
            Cplx s = new Cplx();
            for (int power = 0; power < Roots.Count; power++)
                s += Roots[power] * x.Power(power);

            return s;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Roots.Count; i++)
            {
                sb.Append(Roots[i]);
                for (int j = 0; j < i; j++)
                    sb.Append("x");

                sb.Append(" + ");
            }
            return sb.ToString();
        }
    }
}

