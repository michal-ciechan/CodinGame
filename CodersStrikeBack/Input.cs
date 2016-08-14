using System;
using System.Diagnostics.CodeAnalysis;
using Point = System.Drawing.Point;

namespace CodersStrikeBack
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class Input
    {
        public Vector Location;
        public Vector Checkpoint;
        public int Distance;
        public int Angle;
        public Vector Opponent;

        public void Next()
        {
            var inputs = Console.ReadLine().Split(' ');

            var x = int.Parse(inputs[0]);
            var y = int.Parse(inputs[1]);
            Location = new Vector(x, y);

            var checkpointX = int.Parse(inputs[2]); // x position of the next check point
            var checkpointY = int.Parse(inputs[3]); // y position of the next check point
            Checkpoint = new Vector(checkpointX, checkpointY);

            Distance = int.Parse(inputs[4]); // distance to the next checkpoint
            Angle = int.Parse(inputs[5]);
            
            inputs = Console.ReadLine().Split(' ');
            var opponentX = int.Parse(inputs[0]);
            var opponentY = int.Parse(inputs[1]);

            Opponent = new Vector(opponentX, opponentY);
        }

        public void Log()
        {
            Logger.Write("---- INPUT ----");
            Logger.Write($"Location = new Vector({Location}),");
            Logger.Write($"Checkpoint = new Vector({Checkpoint}),");
            Logger.Write($"Distance = {Distance},");
            Logger.Write($"Angle = {Angle},");
            Logger.Write($"Opponent = new Vector({Opponent}),");
        }
    }
}