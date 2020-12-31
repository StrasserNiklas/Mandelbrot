using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotLibrary
{
    public class Complex
    {
        public double A { get; set; }
        public double B { get; set; }

        public Complex(double a, double b)
        {
            this.A = a;
            this.B = b;
        }

        public void Square()
        {
            double temp = (this.A * this.A) - (this.B * this.B);
            this.B = 2.0 * this.A * this.B;
            this.A = temp;
        }

        public double Magnitude()
        {
            return Math.Sqrt((this.A * this.A) + (this.B * this.B));
        }

        public void Add(Complex c)
        {
            this.A += c.A;
            this.B += c.B;
        }


    }
}
