using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Bring data on patient samples from the diagnosis machine to the laboratory with enough molecules to produce medicine!
 **/
class PlayerGame
{
    static Player me = new Player();
    static void Main(string[] args)
    {

        string[] inputs;
        int projectCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < projectCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int a = int.Parse(inputs[0]);
            int b = int.Parse(inputs[1]);
            int c = int.Parse(inputs[2]);
            int d = int.Parse(inputs[3]);
            int e = int.Parse(inputs[4]);
        }

        // game loop
        while (true)
        {
            var players = new List<Player>();

            for (int i = 0; i < 2; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                string target = inputs[0];
                int eta = int.Parse(inputs[1]);
                int score = int.Parse(inputs[2]);
                int storageA = int.Parse(inputs[3]);
                int storageB = int.Parse(inputs[4]);
                int storageC = int.Parse(inputs[5]);
                int storageD = int.Parse(inputs[6]);
                int storageE = int.Parse(inputs[7]);
                int expertiseA = int.Parse(inputs[8]);
                int expertiseB = int.Parse(inputs[9]);
                int expertiseC = int.Parse(inputs[10]);
                int expertiseD = int.Parse(inputs[11]);
                int expertiseE = int.Parse(inputs[12]);

                if (i == 0)
                {
                    me.Target = target;
                    me.Eta = eta;
                    me.Score = score;
                    me.StorageA = storageA;
                    me.StorageB = storageB;
                    me.StorageC = storageC;
                    me.StorageD = storageD;
                    me.StorageE = storageE;
                    me.ExpertiseA = expertiseA;
                    me.ExpertiseB = expertiseB;
                    me.ExpertiseC = expertiseC;
                    me.ExpertiseD = expertiseD;
                    me.ExpertiseE = expertiseE;
                }

                players.Add(new Player
                {
                    Target = target,
                    Eta = eta,
                    Score = score,
                    StorageA = storageA,
                    StorageB = storageB,
                    StorageC = storageC,
                    StorageD = storageD,
                    StorageE = storageE,
                    ExpertiseA = expertiseA,
                    ExpertiseB = expertiseB,
                    ExpertiseC = expertiseC,
                    ExpertiseD = expertiseD,
                    ExpertiseE = expertiseE,
                });

            }

            inputs = Console.ReadLine().Split(' ');
            int availableA = int.Parse(inputs[0]);
            int availableB = int.Parse(inputs[1]);
            int availableC = int.Parse(inputs[2]);
            int availableD = int.Parse(inputs[3]);
            int availableE = int.Parse(inputs[4]);
            int sampleCount = int.Parse(Console.ReadLine());

            var samples = new List<Sample>(sampleCount);
            for (int i = 0; i < sampleCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int sampleId = int.Parse(inputs[0]);
                int carriedBy = int.Parse(inputs[1]);
                int rank = int.Parse(inputs[2]);
                string expertiseGain = inputs[3];
                int health = int.Parse(inputs[4]);
                int costA = int.Parse(inputs[5]);
                int costB = int.Parse(inputs[6]);
                int costC = int.Parse(inputs[7]);
                int costD = int.Parse(inputs[8]);
                int costE = int.Parse(inputs[9]);

                samples.Add(new Sample
                {
                    SampleId = sampleId,
                    CarriedBy = carriedBy,
                    Rank = rank,
                    ExpertiseGain = expertiseGain,
                    Health = health,
                    CostA = costA,
                    CostB = costB,
                    CostC = costC,
                    CostD = costD,
                    CostE = costE,
                });
            }

            me.Samples = samples.Where(x => x.CarriedBy == 0).ToList();
            
            // Main Game Loop

            if (!me.Samples.Any())
            {

                if (me.Target != Modules.DIAGNOSIS)
                {
                    Console.WriteLine($"GOTO {Modules.DIAGNOSIS}");
                    continue;
                }
            }
            if (me.Target == Modules.DIAGNOSIS && me.Samples.Count < 3)
            {
                var remainingCapacity = 10 - me.NeededMolecules;

                var sample = samples
                    .Where(x => x.CarriedBy == -1)
                    .OrderByDescending(x => x.HealthPerCost)
                    .FirstOrDefault(x => x.TotalCost <= remainingCapacity);

                if (sample != null)
                {
                    me.Samples.Add(sample);
                    Console.WriteLine($"CONNECT {sample.SampleId}");
                    continue;
                }

            }
            if (me.Target == Modules.LABORATORY)
            {
                var sample = me.Samples.FirstOrDefault(x => !me.NeedMolecule(x));

                if (sample != null)
                {
                    me.Samples.Remove(sample);
                    Console.WriteLine($"CONNECT {sample.SampleId}");
                    continue;
                }
            }

            if (me.NeedMolecule())
            {
                if (me.Target != Modules.MOLECULES)
                {
                    Console.WriteLine($"GOTO {Modules.MOLECULES}");
                    continue;
                }
                else
                {
                    if (me.NeedsA)
                    {
                        Console.WriteLine($"CONNECT A");
                        continue;
                    }
                    if (me.NeedsB)
                    {
                        Console.WriteLine($"CONNECT B");
                        continue;
                    }
                    if (me.NeedsC)
                    {
                        Console.WriteLine($"CONNECT C");
                        continue;
                    }
                    else if (me.NeedsD)
                    {
                        Console.WriteLine($"CONNECT D");
                        continue;
                    }
                    if (me.NeedsE)
                    {
                        Console.WriteLine($"CONNECT E");
                        continue;
                    }
                }
            }
            else if (me.Target != Modules.LABORATORY)
            {
                Console.WriteLine($"GOTO {Modules.LABORATORY}");
                continue;
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


        }


    }


    class Modules
    {
        public static string SAMPLES = "SAMPLES";
        public static string DIAGNOSIS = "DIAGNOSIS";
        public static string MOLECULES = "MOLECULES";
        public static string LABORATORY = "LABORATORY";
    }

    class Sample
    {
        public int SampleId;
        public int CarriedBy;
        public int Rank;
        public string ExpertiseGain;
        public int Health;
        public int CostA;
        public int CostB;
        public int CostC;
        public int CostD;
        public int CostE;

        public int TotalCost => CostA + CostB + CostC + CostD + CostE;
        public double HealthPerCost => (double)Health / (double)TotalCost;
    }

    class Player
    {
        public string Target;
        public int Eta;
        public int Score;
        public int StorageA;
        public int StorageB;
        public int StorageC;
        public int StorageD;
        public int StorageE;
        public int ExpertiseA;
        public int ExpertiseB;
        public int ExpertiseC;
        public int ExpertiseD;
        public int ExpertiseE;


        public List<Sample> Samples = new List<Sample>();

        public bool NeedsA => Samples.Sum(x => x.CostA) > StorageA;
        public bool NeedsB => Samples.Sum(x => x.CostB) > StorageB;
        public bool NeedsC => Samples.Sum(x => x.CostC) > StorageC;
        public bool NeedsD => Samples.Sum(x => x.CostD) > StorageD;
        public bool NeedsE => Samples.Sum(x => x.CostE) > StorageE;

        public bool NeedMoleculeA(Sample s) => s.CostA > StorageA;
        public bool NeedMoleculeB(Sample s) => s.CostB > StorageB;
        public bool NeedMoleculeC(Sample s) => s.CostC > StorageC;
        public bool NeedMoleculeD(Sample s) => s.CostD > StorageD;
        public bool NeedMoleculeE(Sample s) => s.CostE > StorageE;

        public bool NeedMolecule(Sample s) => NeedMoleculeA(s) || NeedMoleculeB(s) || NeedMoleculeC(s) || NeedMoleculeD(s) || NeedMoleculeE(s);

        public bool NeedMolecule() => NeedsA || NeedsB || NeedsC || NeedsD || NeedsE;

        public int NeededMolecules => Samples.Sum(x => x.TotalCost);

        public bool IsCarrying(Sample s) => Samples.Any(x => x.SampleId == s.SampleId);
    }
}