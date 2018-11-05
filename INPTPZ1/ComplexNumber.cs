namespace INPTPZ1
{
    public class ComplexNumber
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public static readonly ComplexNumber Zero = new ComplexNumber()
        {
            Re = 0,
            Im = 0
        };
        
        public static readonly ComplexNumber One = new ComplexNumber()
        {
            Re = 1,
            Im = 1
        };

        public static ComplexNumber FromRealNumber(double re)
        {
            return new ComplexNumber(){Re = re};
        }
        
        public static ComplexNumber ComplexNumberWithNegativeImaginePart(double re, double im)
        {
            return new ComplexNumber(){Re = re, Im = -im};
        }

        public ComplexNumber Multiply(ComplexNumber multiplier)
        {
            ComplexNumber multiplicand = this;
            
            return new ComplexNumber()
            {
                Re = multiplicand.Re * multiplier.Re - multiplicand.Im * multiplier.Im,
                Im = multiplicand.Re * multiplier.Im + multiplicand.Im * multiplier.Re
            };
        }

        public ComplexNumber Add(ComplexNumber addedOffset)
        {
            ComplexNumber currentComplexNumber = this;
            return new ComplexNumber()
            {
                Re = currentComplexNumber.Re + addedOffset.Re,
                Im = currentComplexNumber.Im + addedOffset.Im
            };
        }

        public ComplexNumber Subtract(ComplexNumber subtractedOffset)
        {
            ComplexNumber currentComplexNumber = this;
            
            
            
            return new ComplexNumber()
            {
                Re = currentComplexNumber.Re - subtractedOffset.Re,
                Im = currentComplexNumber.Im - subtractedOffset.Im
            };
        }

        public ComplexNumber Divide(ComplexNumber divisorComplexNumber)
        {   
            var multiplier = ComplexNumberWithNegativeImaginePart(divisorComplexNumber.Re, divisorComplexNumber.Im);
            
            var dividend = this.Multiply(multiplier);

            var divisorRePart = divisorComplexNumber.Re * divisorComplexNumber.Re;
            var divisorImPart = divisorComplexNumber.Im * divisorComplexNumber.Im;
            var divisor = divisorRePart + divisorImPart;

            return new ComplexNumber()
            {
                Re = dividend.Re / divisor,
                Im = dividend.Im / divisor
            };
        }   
        
        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }
    }
}