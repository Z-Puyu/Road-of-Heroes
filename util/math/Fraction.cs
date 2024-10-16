using System;

namespace Game.util.math {
    public readonly struct Fraction {
        private readonly int numerator;
        private readonly int denominator;

        public Fraction(int numerator, int denominator) {
            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            if (Math.Sign(numerator) == Math.Sign(denominator)) {
                numerator = Math.Abs(numerator);
            } else {
                numerator = -Math.Abs(numerator);
            }
            denominator = Math.Abs(denominator);
            int gcd = MathUtil.Gcd(Math.Abs(numerator), denominator);
            this.numerator = numerator / gcd;
            this.denominator = denominator / gcd;
        }

        public static Fraction operator +(Fraction a) {
            return new Fraction(a.numerator, a.denominator);
        }

        public static Fraction operator -(Fraction a) {
            return new Fraction(-a.numerator, a.denominator);
        } 

        public static Fraction operator +(Fraction a, Fraction b) {
            return new Fraction(
                a.numerator * b.denominator + b.numerator * a.denominator, 
                a.denominator * b.denominator
            );
        }

        public static Fraction operator -(Fraction a, Fraction b) {
            return new Fraction(
                a.numerator * b.denominator - b.numerator * a.denominator, 
                a.denominator * b.denominator
            );
        }

        public static Fraction operator *(Fraction a, Fraction b) {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b) {
            if (b.numerator == 0) {
                throw new DivideByZeroException();
            }
            return new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static bool operator ==(Fraction a, Fraction b) {
            return a.numerator == b.numerator && a.denominator == b.denominator;
        }

        public static bool operator !=(Fraction a, Fraction b) {
            return a.numerator != b.numerator || a.denominator != b.denominator;
        }

        public static bool operator <(Fraction a, Fraction b) {
            return (a - b).numerator < 0;
        }

        public static bool operator >(Fraction a, Fraction b) {
            return (a - b).numerator > 0;
        }

        public static bool operator <=(Fraction a, Fraction b) {
            return (a - b).numerator <= 0;
        }

        public static bool operator >=(Fraction a, Fraction b) {
            return (a - b).numerator >= 0;
        }
        
        public override string ToString() {
            if (this.numerator == 0) {
                return "0";
            }
            if (this.denominator == 1) {
                return this.numerator.ToString();
            } 
            return $"{this.numerator} / {this.denominator}";
        }

        public override bool Equals(object obj) {
            return obj is Fraction f && f == this;
        }

        public override int GetHashCode() {
            return MathUtil.UniqueId(this.numerator, this.denominator).GetHashCode();
        }
    }
}