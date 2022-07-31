using System;

namespace CodersStrikeBack
{
    public class Player
    {
        public Vector Location;
        public Vector PreviousLocation;
        public Vector FutureLocation;
        public double Speed;
        public double PreviousSpeed;
        public int Dist;
        public int Angle;
        public double FutureAngle;
        public Input Input;
        public double StepsLeft;

        public void Update(Input input)
        {
            Input = input;

            PreviousLocation = Location;
            PreviousSpeed = Speed;

            Location = input.Location;
            Dist = input.Distance;
            Angle = input.Angle;

            var lastMovement = (Location - PreviousLocation);
            FutureLocation = Location + lastMovement;

            FutureAngle = Vector.AngleBetween(input.Checkpoint - Location, FutureLocation - Location);
            
            Speed = lastMovement.Length;

            CalcAngle();
            CalcStepsLeft();
        }

        private void CalcStepsLeft()
        {
            StepsLeft = Dist / Speed;
        }


        private void CalcAngle()
        {
            var diffAngle = Math.Atan2(Location.Y - Input.Checkpoint.Y, Location.X - Input.Checkpoint.X);
            var directedAngle = Math.Atan2(Location.Y, Location.X) - Math.Atan2(Input.Checkpoint.Y, Input.Checkpoint.X);

            diffAngle = diffAngle.NormaliseRadians().ToDegrees();
            directedAngle = directedAngle.NormaliseRadians().ToDegrees();

            var between = Vector.AngleBetween(Location, Input.Checkpoint);

            //Logger.Write(() => Angle);
            //Logger.Write(() => between);
            //Logger.Write(() => diffAngle);
            //Logger.Write(() => directedAngle);
        }

        public void Log()
        {
            Logger.Write("---- Output Class ----");
            Logger.Write(() => PreviousSpeed);
            Logger.Write(() => Speed);
            Logger.Write(() => Dist);
            Logger.Write(() => StepsLeft);
            Logger.Write(() => Angle);
            Logger.Write(() => FutureLocation);
            Logger.Write(() => FutureAngle);
        }
    }
}