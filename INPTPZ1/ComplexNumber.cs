class ComplexNumber
{
    public double Re { get; set; }
    public double Im { get; set; }

    public readonly static ComplexNumber Zero = new ComplexNumber()
    {
        Re = 0,
        Im = 0
    };

    public ComplexNumber Multiply(ComplexNumber multiplier)
    {
        ComplexNumber multiplicand = this;
        return new ComplexNumber()
        {
            Re = multiplicand.Re * multiplier.Re - multiplicand.Im * multiplier.Im,
            Im = multiplicand.Re * multiplier.Im + multiplicand.Im * multiplier.Re
        };
    }

    public ComplexNumber Add(ComplexNumber firstAddend)
    {
        ComplexNumber secondAddend = this;
        return new ComplexNumber()
        {
            Re = firstAddend.Re + secondAddend.Re,
            Im = firstAddend.Im + secondAddend.Im
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
        // ((aRe + aIm*i) * (bRe - bIm*i)) / ((bRe + bIm*i) * (bRe - bIm*i))
        ComplexNumber dividend = this;
        ComplexNumber tmpDividend = dividend.Multiply(new ComplexNumber() { Re = divisor.Re, Im = -divisor.Im });
        double tmpDivisor = divisor.Re * divisor.Re + divisor.Im * divisor.Im;

        return new ComplexNumber()
        {
            Re = tmpDividend.Re / tmpDivisor,
            Im = tmpDividend.Im / tmpDivisor
        };
    }
}