using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodersStrikeBack;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class AngleTests
    {
        [Test]
        public void VectorTests()
        {
            var v1X = 0;
            var v1Y = 10;

            WriteAngles(v1X,v1Y, 0, 10);
            WriteAngles(v1X,v1Y, 5, 10);
            WriteAngles(v1X,v1Y, 10, 10);
            WriteAngles(v1X,v1Y, 10, 5);
            WriteAngles(v1X,v1Y, 10, 0);
            WriteAngles(v1X,v1Y, 10, -5);
            WriteAngles(v1X,v1Y, 10, -10);
            WriteAngles(v1X,v1Y, 5, -10);
            WriteAngles(v1X,v1Y, 0, -10);
            WriteAngles(v1X,v1Y, -5, -10);
            WriteAngles(v1X,v1Y, -10, -10);
            WriteAngles(v1X,v1Y, -10, -5);
            WriteAngles(v1X,v1Y, -10, 0);
            WriteAngles(v1X,v1Y, -5, 0);
        }

        private static void WriteAngles(double v1X, double v1Y, double v2X, double v2Y)
        {
            WriteAngles(new Vector(v1X, v1Y), new Vector(v2X, v2Y));
        }

        private static void WriteAngles(Vector v1, Vector v2)
        {
            Console.WriteLine($"{v1} -> {v2}");
            Console.WriteLine("Diff =             " + Vector.AngleBetweenDiff(v1, v2));
            Console.WriteLine("Diff Reverse =     " + Vector.AngleBetweenDiff(v2, v1));
            Console.WriteLine();
            Console.WriteLine("Directed =         " + Vector.AngleBetweenDirected(v1, v2));
            Console.WriteLine("Directed Reverse = " + Vector.AngleBetweenDirected(v2, v1));
            Console.WriteLine();
            Console.WriteLine("Between =          " + Vector.AngleBetween(v1, v2));
            Console.WriteLine("Between Reverse =  " + Vector.AngleBetween(v2, v1));
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
