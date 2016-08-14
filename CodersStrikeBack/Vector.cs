using System;

namespace CodersStrikeBack
{
    public struct Vector
    {
        public bool Equals(Vector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public double X;
        public double Y;
        
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public double Length => Math.Sqrt(X * X + Y * Y);
        
        public double LengthSquared => X * X + Y * Y;
        
        public void Normalize()
        {
            // Avoid overflow 
            this /= Math.Max(Math.Abs(X), Math.Abs(Y));
            this /= Length;
        }

        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }
        
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        public static double AngleBetweenDiff(Vector v1, Vector v2)
        {
            var angle = Math.Atan2(v1.Y - v2.Y, v1.X - v2.X);

            return angle.NormaliseRadians().ToDegrees();
        }

        public static double AngleBetweenDirected(Vector v1, Vector v2)
        {
            var angle = Math.Atan2(v2.Y, v2.X) - Math.Atan2(v1.Y, v1.X);

            return angle.NormaliseRadians().ToDegrees();
        }

        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector.X, -vector.Y);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return Math.Abs(v1.X - v2.X) < 0.01 && Math.Abs(v2.Y - v2.Y) < 0.01;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return Math.Abs(v1.X - v2.X) > 0.01 && Math.Abs(v2.Y - v2.Y) > 0.01;
        }

        /// <summary> 
        /// Negates the values of X and Y on this Vector
        /// </summary> 
        public void Negate()
        {
            X = -X;
            Y = -Y;
        }
        
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X,
                              vector1.Y + vector2.Y);
        }
        
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X,
                              vector1.Y + vector2.Y);
        }
        
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X,
                              vector1.Y - vector2.Y);
        }
        
        public static Vector Subtract(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X,
                              vector1.Y - vector2.Y);
        }
        
        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }
        
        public static Vector Multiply(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }
        
        public static Vector operator *(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }
        
        public static Vector Multiply(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }
        public static Vector operator /(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
        
        public static Vector Divide(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static double operator *(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static double Multiply(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static double Determinant(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }
    }
} 