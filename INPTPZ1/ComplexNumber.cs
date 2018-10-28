namespace INPTPZ1
{
    class ComplexNumber
    {
        public double RealPart { get; set; }
        public float ImaginaryPart { get; set; }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            RealPart = 0,
            ImaginaryPart = 0
        };

        public ComplexNumber Multiply(ComplexNumber násobitel)
        {
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i - aIm*bIm*i*i
            return new ComplexNumber()
            {
                RealPart = this.RealPart * násobitel.RealPart - this.ImaginaryPart * násobitel.ImaginaryPart,
                ImaginaryPart = (float)(this.RealPart * násobitel.ImaginaryPart + this.ImaginaryPart * násobitel.RealPart)
            };
        }

        public ComplexNumber Add(ComplexNumber summand)
        {
            return new ComplexNumber()
            {
                RealPart = this.RealPart + summand.RealPart,
                ImaginaryPart = this.ImaginaryPart + summand.ImaginaryPart
            };
        }
        public ComplexNumber Subtract(ComplexNumber subtrahend)
        {
            return new ComplexNumber()
            {
                RealPart = this.RealPart - subtrahend.RealPart,
                ImaginaryPart = this.ImaginaryPart - subtrahend.ImaginaryPart
            };
        }

        public override string ToString()
        {
            return $"({RealPart} + {ImaginaryPart}i)";
        }

        internal ComplexNumber Divide(ComplexNumber divisor)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe + bIm*bIm*i*i
            ComplexNumber thisNegated = this.Multiply(new ComplexNumber() { RealPart = divisor.RealPart, ImaginaryPart = -divisor.ImaginaryPart });
            double divideWith = divisor.RealPart * divisor.RealPart + divisor.ImaginaryPart * divisor.ImaginaryPart;

            return new ComplexNumber()
            {
                RealPart = thisNegated.RealPart / divideWith,
                ImaginaryPart = (float)(thisNegated.ImaginaryPart / divideWith)
            };
        }
    }
}
