using System;
using System.Windows;
using Point = System.Drawing.Point;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace CodersStrikeBack
{
    public class Game
    {
        public static Vector Midpoint = new Vector(8000,4500);
        public static Input Input;
        public static int Steps;

        public static int _acceleration;
        public static int _deceleration;

        public static bool straightLine = true;

        static Game()
        {
            Input = new Input();
            Output = new Output();
            Player = new Player();
            Waypoint = new Waypoint();
            StepRecorder = new StepRecorder();
            Lap = new Lap();
        }

        public static void Main(string[] args)
        {
            // game loop
            while (true)
            {
                Input.Next();
                DoStep();
            }
        }

        public static void DoStep()
        {
            Steps++;

            Waypoint.Update(Input);
            Lap.Update();
            Player.Update(Input);

            
            //Logger.Write(() => Steps);



            StepRecorder.Record();

            //StepRecorder.Log();
            //Player.Log();
            //Waypoint.Log();
            Lap.Log();
            //Input.Log();
            //Output.LogLocation();

            Output.Send();
        }

        public static Output Output;
        public static Player Player;
        public static Waypoint Waypoint;
        public static StepRecorder StepRecorder;
        public static Lap Lap;
    }
}
/**
dir -r -filter *.cs | 
	Where {$_.FullName -notlike '*\obj\*' -and $_.FullName -notlike '*\Properties\*' -and $_.FullName -notlike '*\Original.cs'} | 
	Select-String -pattern "^using" | 
	Select-Object -expand Line -unique | 
	Format-List -property Line | 
	Out-File game.out
	
dir -r -filter *.cs | 
	Where {$_.FullName -notlike '*\obj\*' -and $_.FullName -notlike '*\Properties\*' -and $_.FullName -notlike '*\Original.cs'} | 
	Select-String -pattern "^using" -NotMatch | 
	Select-Object -expand Line | 
	Format-List -property Line | 
	Out-File game.out -Append
**/

/**
 * Post Build Event:
cd $(ProjectDir)
powershell -File "$(SolutionDir)\build.ps1" "$(SolutionDir)\Game.out"
 */
