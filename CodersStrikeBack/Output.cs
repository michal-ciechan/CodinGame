using System;
using System.Drawing;
using System.Windows;

namespace CodersStrikeBack
{
    public class Output
    {
        public int Thrust = 100;
        public bool Boost;
        public Vector Location;
        private double _stepsLeft;
        private Vector? _nextLocation;

        public void Send()
        {
            //PlayerLocation = new Vector(0,0);
            //if (Game.Steps > 20) Thrust = 0;

            _stepsLeft = Game.Waypoint.StepsLeft;
            _nextLocation = Game.Lap.NextLocation;

            Location = CalcLocation();
            
            var thrust = GetThrust();
            
            // You have to output the target position
            // followed by the power (0 <= thrust <= 100)
            // i.e.: "x y thrust"
            var msg = $"{(int)Location.X} {(int)Location.Y} {thrust}";

            Console.WriteLine(msg);
        }

        public Vector CalcLocation()
        {
            if (Game.Steps == 1)
                    return Game.Waypoint.Location;
            if (Game.Player.Angle > 60)
                return Game.Waypoint.Location;

            if (_stepsLeft < 0)
            {
                return Game.Waypoint.Location;
            }
            if (_stepsLeft <= 2)
            {
                return _nextLocation ?? Game.Midpoint;
            }
            if (_stepsLeft <= 4)
            {
                return _nextLocation ?? Game.Waypoint.Location;
            }



            var future = Game.Player.FutureLocation;
            var waypoint = Game.Waypoint.Location;
            var location = Game.Player.Location;

            var waypointNorm = waypoint - location;
            var futureNorm = future- location;
            var radius = waypointNorm.Length;

            var playerAngle = Vector.AngleBetweenDirected(new Vector(radius, 0), futureNorm);
            var waypointAngle = Vector.AngleBetweenDirected(new Vector(radius, 0), waypointNorm);
            
            var diffAngle = waypointAngle - playerAngle;

            var newAngle = diffAngle + diffAngle + playerAngle;
            
            var aim = Maths.PointOnCircle(Math.Abs(radius), newAngle, location);
            
            return aim;
        }

        private string GetThrust()
        {
            if (Boost)
                return "BOOST";


            return ((int)CalcThrust()).ToString();
        }

        private double CalcThrust()
        {

            if (_stepsLeft <= 0)
            {
                return CalcTurnThrust();
            }

            if (_stepsLeft > 5)
                return 100;

            if (_stepsLeft > 3)
                return 50;

            if (_stepsLeft > 1)
                return 30;

            return 0;

            // var angle = Math.Abs(_cpAngle);

            //var fullAngle = 45;
            //var stopAngle = 145;
            //var angleMaxSteps = stopAngle - fullAngle;
            //var thrustPerAngle = 100 / (double)angleMaxSteps;
            //var diffAngle = angle - fullAngle;

            //if (angle > stopAngle)
            //    thrust = 0;
            //else if (stepsLeft <= 4)
            //    thrust = 0;
            //else if (angle < fullAngle)
            //    thrust = 100;
            //else
            //    thrust = 100 - (int)(diffAngle * thrustPerAngle);

            //if (angle < 8 && _dist > 6000)
            //    thrustOut = "BOOST";
        }

        private double CalcTurnThrust()
        {
            var angle = Math.Abs(Game.Player.Angle);

            if (angle > 100)
                return 50;
            if (angle > 45)
                return 100;

            return 100;
        }

        public void LogThrust()
        {
            Logger.Write("---- Output Class ----");
            Logger.Write(() => Game.Waypoint.StepsLeft);
            Logger.Write(() => Game.Player.Angle);
        }

        public void LogLocation()
        {
            Logger.Write("---- Output Class ----");
            Logger.Write(() => Game.Waypoint.StepsLeft);
            Logger.Write(() => Game.Player.Angle);
            Logger.Write(() => Game.Waypoint.Location);
            Logger.Write(() => Game.Lap.NextLocation);
            Logger.Write("Output", () => Game.Output.Location);
        }

        public void Log()
        {
            Logger.Write("---- Output Class ----");
            Logger.Write(() => Boost);
        }
    }
}