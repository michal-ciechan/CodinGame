using System;
using System.Runtime.InteropServices;
using CodersStrikeBack;
using FluentAssertions;

namespace Tests
{
    public class GameSimulator
    {
        public int[][] Steps { get; set; }
        public int Step;

        public GameSimulator(int[][] steps)
        {
            Steps = steps;
            Step = 1;
            DoStep();
        }

        public void GoTo(int step)
        {
            // Reset if in the past
            if(step < Step)
                DoStep(1);

            while (Step < step)
                Next();
        }

        private void Next()
        {
            Step++;
            DoStep();
        }

        public void DoStep()
        {
            DoStep(Step);
        }
        public void DoStep(int step)
        {
            if (step < 1)
                throw new Exception("GameSimulator.Steps is 1 Base");

            Game.Input = StepRecorder.ToInput(Steps[Step - 1]);
            Game.DoStep();
            Step = step;
        }
    }
}