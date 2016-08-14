using System.Data.Common;

namespace CodersStrikeBack
{
    public class Waypoint
    {

        public Vector PrevLocation;
        public Vector Location;
        public Vector PrevPlayerLocation;
        public Vector PlayerLocation;
        public double Dist;
        public double PreviousDist;
        public double Speed;
        public Average AvgSpeed = new Average();
        public double StepsLeft;

        public void Update(Input input)
        {

            PrevLocation = Location;
            PrevPlayerLocation = PlayerLocation;

            Location = input.Checkpoint;
            PlayerLocation = input.Location;

            PreviousDist = PrevLocation == Location 
                ? Dist 
                : (Location - PrevPlayerLocation).Length;

            Dist = (Location - PlayerLocation).Length;
            Speed = PreviousDist - Dist;
            StepsLeft = Dist/Speed;

            AvgSpeed.Add(Speed);
        }

        public void Log()
        {
            Logger.Write("WP", () => PreviousDist);
            Logger.Write("WP", () => Dist);
            Logger.Write("WP", () => Speed);
            Logger.Write("WP", () => StepsLeft);
            Logger.Write("WP", () => AvgSpeed);
        }
    }
}