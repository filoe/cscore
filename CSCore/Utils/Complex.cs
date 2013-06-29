namespace CSCore.Utils
{
    [System.Diagnostics.DebuggerDisplay("r: {Real} i: {Imaginary}")]
    public struct Complex
    {
        public static readonly Complex Zero = new Complex(0, 0);

        public Complex(double real)
            : this(real, 0.0)
        {

        }

        public Complex(double real, double img)
        {
            Real = real;
            Imaginary = img;
        }

        public double Real;
        public double Imaginary;

        public static explicit operator double(Complex complex)
        {
            return complex.Value;
        }

        public double Value
        {
            get { return FastFourierTransformation.GetIntensity(this); } 
        }

        public double CalculateFFTPercentage()
        {
            return FastFourierTransformation.CalculatePercentage(this);
        }
    }
}