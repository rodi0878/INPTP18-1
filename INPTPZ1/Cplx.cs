namespace INPTPZ1
{
    class Cplx
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public Cplx(double real = 0, double imag = 0)
        {
            Re = real;
            Im = imag;
        }

        public readonly static Cplx Zero = new Cplx(0, 0);

        public static Cplx operator +(Cplx a, Cplx b) => new Cplx(a.Re + b.Re, a.Im + b.Im);
        public static Cplx operator *(Cplx a, Cplx b) => new Cplx(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);
        public static Cplx operator -(Cplx a, Cplx b) => new Cplx(a.Re - b.Re, a.Im - b.Im);

        public static Cplx operator /(Cplx a, Cplx b)
        {
            Cplx tmp = a * new Cplx(b.Re, -b.Im);
            double tmp2 = b.Re * b.Re + b.Im * b.Im;
            return new Cplx(tmp.Re / tmp2, tmp.Im / tmp2);
        }

        public Cplx Power(int power)
        {
            Cplx result = new Cplx(1);
            for (int i = 0; i < power; i++)
            {
                result *= this;
            }
            return result;
        }


        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

    }
}
