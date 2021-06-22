using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
    class Vector2D : ICloneable
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D (double x, double y)
        {
            X = x;
            Y = y;
        }
        public Vector2D (double x)
        {
            X = x;
            Y = 0;
        }
        public Vector2D (Vector2D v)
        {
            X = v.X;
            Y = v.Y;
        }
        public Vector2D()
        {
            X = 0;
            Y = 0;
        }

        public static Vector2D operator + (Vector2D l, Vector2D d)
        {
            return new Vector2D(l.X + d.X, l.Y + d.Y);
        }

        public static Vector2D operator -(Vector2D l, Vector2D d)
        {
            return new Vector2D(l.X - d.X, l.Y - d.Y);
        }

        public static Vector2D operator / (Vector2D l, double scalar)
        {
            if (scalar != 0)
                return new Vector2D(l.X / scalar, l.Y / scalar);
            else
                throw new Exception("Nemoguće je deliti vektor sa nulom!");
        }

        public static Vector2D operator * (Vector2D l, double scalar)
        {
            return new Vector2D(scalar * l.X, scalar * l.Y);
        }

        public static Vector2D operator *(double scalar, Vector2D d)
        {
            return new Vector2D(scalar * d.X, scalar * d.Y);
        }

        public static double operator * (Vector2D l, Vector2D d)
        {
            return l.X * d.X + l.Y * d.Y;
        }

        public static double Intensity (Vector2D v)
        {
            return v.Intensity();
        }
        public static Vector2D Normalize(Vector2D v)
        {
            double d = v.Intensity();
            if (d > 0)
            {
                return new Vector2D(v.X / d, v.Y / d);
            }
            else
                throw new Exception("Nemoguće je deliti vektor sa nulom!");
        }
        public double Intensity()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public void Normalize()
        {
            double d = Intensity();
            X /= d;
            Y /= d;
        }

        public object Clone()
        {
            return new Vector2D(X, Y);
        }

        //public override string ToString()
        //{
        //    return 
        //}
    }
}
