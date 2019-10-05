using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAISolver
{
    class FourierTransformation
    {
    }
    class Complex
    {
        public float real = 0.0f;
        public float imag = 0.0f;

        //Empty constructor 
        public Complex()
        {
        }
        public Complex(double real, double im)
        {
            this.real = (float)real;
            this.imag = (float)imag;
        }
        public Complex(float real, float im)
        {
            this.real = real;
            this.imag = imag;
        }

        public string ToString()
        {
            string data = real.ToString() + " " + imag.ToString() + "i";
            return data;
        }

        //Convert from polar to rectangular 
        public static Complex from_polar(double r, double radians)
        {
            Complex data = new Complex( (float)(r * Math.Cos(radians)), (float)( r * Math.Sin(radians)));
            return data;
        }

        //Override addition operator 
        public static Complex operator +(Complex a, Complex b)
        {
            Complex data = new Complex(a.real + b.real, a.imag + b.imag);
            return data;
        }

        //Override subtraction operator 
        public static Complex operator -(Complex a, Complex b)
        {
            Complex data = new Complex(a.real - b.real, a.imag - b.imag);
            return data;
        }

        //Override multiplication operator 
        public static Complex operator *(Complex a, Complex b)
        {
            Complex data = new Complex((a.real * b.real) - (a.imag * b.imag), (a.real * b.imag + (a.imag * b.real)));
            return data;
        }

        //Return magnitude of complex number 
        public float magnitude
        {
            get
            {
                return (float) Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imag, 2));
            }
        }

        public float phase
        {
            get
            {
                return (float) Math.Atan(imag / real);
            }
        }
    }


    class fourier
    {
        public static Complex[] DFT(Complex[] x)
        {
            int N = x.Length;
            Complex[] X = new Complex[N];

            for (int k = 0; k < N; k++)
            {
                X[k] = new Complex(0, 0);

                for (int n = 0; n < N; n++)
                {
                    Complex temp = Complex.from_polar(1, -2 * Math.PI * n * k / N);
                    temp *= x[n];
                    X[k] += temp;
                }
            }

            return X;
        }

        public static Complex[] FFT(Complex[] x)
        {
            int N = x.Length;
            Complex[] X = new Complex[N];

            Complex[] d, D, e, E;

            if (N == 1)
            {
                X[0] = x[0];
                return X;
            }

            int k;

            e = new Complex[N / 2];
            d = new Complex[N / 2];

            for (k = 0; k < N / 2; k++)
            {
                e[k] = x[2 * k];
                d[k] = x[2 * k + 1];
            }

            D = FFT(d);
            E = FFT(e);

            for (k = 0; k < N / 2; k++)
            {
                Complex temp = Complex.from_polar(1, -2 * Math.PI * k / N);
                D[k] *= temp;
            }

            for (k = 0; k < N / 2; k++)
            {
                X[k] = E[k] + D[k];
                X[k + N / 2] = E[k] - D[k];
            }

            return X;
        }

    }

}
