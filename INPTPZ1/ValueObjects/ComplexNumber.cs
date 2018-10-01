using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTPZ1.ValueObjects
{
    /// <summary>
    /// This class represents Complex number and its basic operations.
    /// See more at: https://en.wikipedia.org/wiki/Complex_number
    /// 
    /// In operations I use 'x' and 'y' because formulas are usually expressed
    /// by these two letters. So it is easy to read as a formula.
    /// </summary>
    class ComplexNumber
    {
        public double Re { get; set; }
        public float Im { get; set; }

        public readonly static ComplexNumber Zero = new ComplexNumber()
        {
            Re = 0,
            Im = 0
        };

        public ComplexNumber Multiply(ComplexNumber y)
        {
            ComplexNumber x = this;
            return new ComplexNumber()
            {
                Re = x.Re * y.Re - x.Im * y.Im,
                Im = (float)(x.Re * y.Im + x.Im * y.Re)
            };
        }

        public ComplexNumber Add(ComplexNumber y)
        {
            ComplexNumber x = this;
            return new ComplexNumber()
            {
                Re = x.Re + y.Re,
                Im = x.Im + y.Im
            };
        }
        public ComplexNumber Subtract(ComplexNumber y)
        {
            ComplexNumber x = this;
            return new ComplexNumber()
            {
                Re = x.Re - y.Re,
                Im = x.Im - y.Im
            };
        }

        public override string ToString()
        {
            return $"({Re} + {Im}i)";
        }

        internal ComplexNumber Divide(ComplexNumber y)
        {
            var numerator = this.Multiply(new ComplexNumber() { Re = y.Re, Im = -y.Im });
            var denominator = y.Re * y.Re + y.Im * y.Im;

            return new ComplexNumber()
            {
                Re = numerator.Re / denominator,
                Im = (float)(numerator.Im / denominator)
            };
        }
    }
}
