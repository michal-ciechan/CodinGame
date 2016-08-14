using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CodersStrikeBack;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MathsTests
    {
        static MathsTests()
        {
            Logger.WriteAction = Console.WriteLine;
        }
        [Test]
        public void PointOnCircle()
        {
            var origin = new Vector(0,0);
            var top = Maths.PointOnCircle(10, 0, origin);
            var right = Maths.PointOnCircle(10, 90, origin);
            var mid = Maths.PointOnCircle(10, 45, origin);
            var third = Maths.PointOnCircle(10, 30, origin);
            var third2 = Maths.PointOnCircle(10, 60, origin);

            Logger.Write(() => top);
            Logger.Write(() => right);
            Logger.Write(() => mid);
            Logger.Write(() => third);
            Logger.Write(() => third2);
        }
    }
}
