using System;
using System.Collections.Generic;
using System.Text;

namespace CodersStrikeBack
{
    public class StepRecorder
    {
        public List<double[]> Steps = new List<double[]>();

        public void Record()
        {
            Steps.Add(new []
            {
                Game.Input.Location.X,
                Game.Input.Location.Y,
                Game.Input.Checkpoint.X,
                Game.Input.Checkpoint.Y,
                Game.Input.Distance,
                Game.Input.Angle,
                Game.Input.Opponent.X,
                Game.Input.Opponent.Y,
            });
        }

        public void Log()
        {
            Logger.Write("----- Step Recorder -----");
            var sb = new StringBuilder();
            foreach (var step in Steps)
            {
                var output =  string.Join(",", step);

                Logger.Write($"new[] {{{output}}},");
            }
        }

        public static Input ToInput(int[] step)
        {
            return new Input
            {
                Location = new Vector(step[0], step[1]),
                Checkpoint = new Vector(step[2],step[3]),
                Distance = step[4],
                Angle = step[5],
                Opponent = new Vector(step[6], step[7])
            };
        }
    }
}