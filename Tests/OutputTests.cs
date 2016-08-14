using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CodersStrikeBack;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OutputTests
    {
        private const double Precision = 0.15;

        static OutputTests()
        {
            Logger.WriteAction = Console.WriteLine;
        }

        [Test]
        public void CalcLocationTests_Origin_0_0()
        {
            Game.Player.Location = new Vector(0,0);
            Game.Player.FutureLocation = new Vector(10,0);
            Game.Waypoint.Location = new Vector(7,7);
            
            CalcLocation_ShouldBe(new Vector(0, 10));
        }

        [Test]
        public void CalcLocationTests_Origin_10_10()
        {
            Game.Player.Location = new Vector(10, 10);
            Game.Player.FutureLocation = new Vector(20, 10);
            Game.Waypoint.Location = new Vector(17, 17);

            CalcLocation_ShouldBe(new Vector(10,20));
        }

        [Test]
        public void CalcLocationTests_Origin_0_0_Future_45Angle()
        {
            Game.Player.Location = new Vector(0, 0);
            Game.Player.FutureLocation = new Vector(7, 7);
            Game.Waypoint.Location = new Vector(10, 0);

            CalcLocation_ShouldBe(new Vector(7, -7));
        }

        [Test]
        public void CalcLocationTests_2nd_Step()
        {
            var sim = new GameSimulator(new[]
            {
                new[] {4465, 6562, 4099, 7401, 915, 0, 4007, 8909},
                new[] {4238, 7083, 13530, 2325, 10439, -140, 4105, 9298},
            });

            sim.GoTo(2);

            Game.Output.Location.ShouldBeApproximately(Game.Waypoint.Location, 1000);
        }

        private static void CalcLocation_ShouldBe(Vector expected)
        {
            var res = Game.Output.CalcLocation();

            Logger.Write(() => res);

            res.ShouldBeApproximately(expected, Precision);
        }
    }
}
