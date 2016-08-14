using System.Collections.Generic;

namespace CodersStrikeBack
{
    public class Lap
    {
        public List<Vector> Locations = new List<Vector>();
        public int LocationIndex;
        public Vector FirstLocation;
        public Vector? NextLocation;
        public Vector Previous;
        public Vector Current;


        public void Update()
        {
            Previous = Game.Waypoint.PrevLocation;
            Current = Game.Waypoint.Location;

            if (!NextLocation.HasValue)
            {
                UpdateFirstLap();
                return;
            }

            if (Current == Previous)
                return;

            LocationIndex++;

            if (LocationIndex + 1 >= Locations.Count)
                LocationIndex = 0;

            NextLocation = Locations[LocationIndex+1];
        }

        private void UpdateFirstLap()
        {
            if (Locations.Count == 0)
            {
                FirstLocation = Current;
                Locations.Add(Current);
                return;
            }

            if (Previous == Current)
                return;

            LocationIndex++;

            if (Current == FirstLocation)
            {
                LocationIndex = 0;
                NextLocation = Locations[1];
                return;
            }

            Locations.Add(Current);
        }

        public void Log()
        {
            
            Logger.Write(() => LocationIndex);
            Logger.Write(() => Current);
            Logger.Write(() => Previous);
            Logger.Write(() => NextLocation);
            Logger.Write(() => FirstLocation);
        }
    }
}