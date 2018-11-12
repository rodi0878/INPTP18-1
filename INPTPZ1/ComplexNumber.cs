

namespace INPTPZ1
{
    class ComplexNumber
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            Re = 0,
            Im = 0
        };

        public ComplexNumber Multiply(ComplexNumber multiplicant)
        {
            ComplexNumber multiplier = this;
            return new ComplexNumber()
            {
                Re = multiplier.Re * multiplicant.Re - multiplier.Im * multiplicant.Im,
                Im = multiplier.Re * multiplicant.Im + multiplier.Im * multiplicant.Re
            };
        }

        public ComplexNumber Add(ComplexNumber summand)
        {
            ComplexNumber addition = this;
            return new ComplexNumber()
            {
                Re = addition.Re + summand.Re,
                Im = addition.Im + summand.Im
            };
        }
        public ComplexNumber Subtract(ComplexNumber subtrahend)
        {
            ComplexNumber minuend = this;
            return new ComplexNumber()
            {
                Re = minuend.Re - subtrahend.Re,
                Im = minuend.Im - subtrahend.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        public ComplexNumber Divide(ComplexNumber divisor)
        {
            var divident = this.Multiply(new ComplexNumber() { Re = divisor.Re, Im = -divisor.Im });
            var quotient = divisor.Re * divisor.Re + divisor.Im * divisor.Im;

            return new ComplexNumber()
            {
                Re = divident.Re / quotient,
                Im = divident.Im / quotient
            };
        }
    }
}
