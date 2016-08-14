using System.Collections.Generic;
using System.Globalization;

namespace CodersStrikeBack
{
    public class Average
    {
        public static List<Average> Averages = new List<Average>();
        
        public double Total;
        public int Count;
        public double Avg;

        public Average()
        {
            Averages.Add(this);
        }

        public void Add(double d)
        {
            Total += d;
            Count++;
            Avg = Total/Count;
        }

        public override string ToString()
        {
            return Avg.ToString(CultureInfo.InvariantCulture);
        }
    }
}