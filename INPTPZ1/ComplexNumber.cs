namespace INPTPZ1
{
    class ComplexNumber
    {
        public double Re { get; set; }
        public double Im { get; set; }
        public readonly static ComplexNumber Zero = new ComplexNumber() { Re = 0, Im = 0 };

        public ComplexNumber Multiply(ComplexNumber secondComplexNumber)
        {
            // aRe*bRe + aRe*bIm*i + aIm*bRe*i + aIm*bIm*i*i
            return new ComplexNumber()
            {
                Re = this.Re * secondComplexNumber.Re - this.Im * secondComplexNumber.Im,
                Im = this.Re * secondComplexNumber.Im + this.Im * secondComplexNumber.Re
            };
        }

        public ComplexNumber Add(ComplexNumber secondComplexNumber)
        {
            return new ComplexNumber()
            {
                Re = this.Re + secondComplexNumber.Re,
                Im = this.Im + secondComplexNumber.Im
            };
        }

        public ComplexNumber Subtract(ComplexNumber secondComplexNumber)
        {
            return new ComplexNumber()
            {
                Re = this.Re - secondComplexNumber.Re,
                Im = this.Im - secondComplexNumber.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        public ComplexNumber Divide(ComplexNumber secondComplexNumber)
        {
            // (aRe + aIm*i) / (bRe + bIm*i)
            // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
            //  bRe*bRe - bIm*bIm*i*i
            var tempComplexNumber = this.Multiply(new ComplexNumber() { Re = secondComplexNumber.Re, Im = -secondComplexNumber.Im });
            var tempSum = secondComplexNumber.Re * secondComplexNumber.Re + secondComplexNumber.Im * secondComplexNumber.Im;

            return new ComplexNumber()
            {
                Re = tempComplexNumber.Re / tempSum,
                Im = tempComplexNumber.Im / tempSum
            };
        }

        public static ComplexNumber FromRealNumber(double number) {
            return new ComplexNumber() { Re = number };
        }
    }
}
