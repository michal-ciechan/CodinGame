using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodersStrikeBack;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WaypointTests
    {
        [Test]
        public void Speed_CheckpointChange()
        {
            var input1 = new Input()
            {
                Location = new Vector(6429, 3453),
                Checkpoint = new Vector(7225, 2159),
                Distance = 1519,
                Angle = 5,
                Opponent = new Vector(5622, 5274),
            };
            var input2 = new Input()
            {
                Location = new Vector(6582, 2896),
                Checkpoint = new Vector(7225, 2159),
                Distance = 978,
                Angle = 9,
                Opponent = new Vector(5792, 4741),
            };
            var input3 = new Input()
            {
                Location = new Vector(6778, 2348),
                Checkpoint = new Vector(3573, 5291),
                Distance = 4351,
                Angle = -173,
                Opponent = new Vector(5984, 4201),
            };

            var wp = new Waypoint();

            wp.Update(input1);
            wp.Update(input2);
            wp.Update(input3);

            wp.Speed.Should().BeLessThan(1000);
        }
    }
}
