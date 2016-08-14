using System;

namespace CodeBusters
{
    public static class Maths
    {
        public static Vector PointOnCircle(double radius, double angleInDegrees, Vector origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            var x = radius * Math.Cos(angleInDegrees * Math.PI / 180F) + origin.X;
            var y = radius * Math.Sin(angleInDegrees * Math.PI / 180F) + origin.Y;

            return new Vector(x, y);
        }

        public static double NormaliseRadians(this double angle)
        {
            return angle > 0 ? angle : angle + 2 * Math.PI;
        }

        public static double ToDegrees(this double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}