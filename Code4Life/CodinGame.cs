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
    private static MoleculeStation _molecule = new MoleculeStation();
    
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
            try
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

                    players.Add(new Player
                    {
                        Target = (Module)Enum.Parse(typeof(Module), target),
                        Eta = eta,
                        Score = score,
                        Storage =
                        {
                            A = storageA,
                            B = storageB,
                            C = storageC,
                            D = storageD,
                            E = storageE,
                        },
                        Expertise =
                        {
                            A = expertiseA,
                            B = expertiseB,
                            C = expertiseC,
                            D = expertiseD,
                            E = expertiseE,
                        },
                    });

                }

                me.Target = players[0].Target;
                me.Eta = players[0].Eta;
                me.Score = players[0].Score;
                me.Storage = players[0].Storage;
                me.Expertise = players[0].Expertise;

                inputs = Console.ReadLine().Split(' ');
                _molecule.Available.A = int.Parse(inputs[0]);
                _molecule.Available.B = int.Parse(inputs[1]);
                _molecule.Available.C = int.Parse(inputs[2]);
                _molecule.Available.D = int.Parse(inputs[3]);
                _molecule.Available.E = int.Parse(inputs[4]);

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
                        Cost =
                        {
                            A = costA,
                            B = costB,
                            C = costC,
                            D = costD,
                            E = costE,
                        },
                    });
                }


                Console.Error.WriteLine($"Me: {me}");
                Console.Error.WriteLine($"Available Molecules: {_molecule.Available}");

                foreach (var sample in me.Samples)
                {
                    Console.Error.WriteLine($"$Held Sample: {sample}");
                }

                me.Samples = samples.Where(x => x.CarriedBy == 0).ToList();

                // Main Game Loop

                if (me.Eta > 0)
                {
                    Console.WriteLine("WAIT");
                    continue;
                }

                if (!me.Samples.Any())
                {
                    if (me.Target != Module.SAMPLES)
                    {
                        Actions.GoTo(Module.SAMPLES);
                    }
                }

                if (me.Target == Module.SAMPLES)
                {
                    if (me.Samples.Count < 3)
                    {
                        Console.WriteLine($"CONNECT 2");
                        continue;
                    }

                    Actions.GoTo(Module.DIAGNOSIS);
                }

                if (me.Target == Module.DIAGNOSIS)
                {
                    var undiagnosedSample = me.Samples.FirstOrDefault(x => !x.IsDiagnosed);

                    if (undiagnosedSample != null)
                    {
                        Console.WriteLine($"CONNECT {undiagnosedSample.SampleId}");
                        continue;
                    }

                    if (me.Samples.Any() && me.Samples.All(x => !x.AreMoleculesAvailable()))
                    {
                        var sample = me.Samples.First();
                        Console.WriteLine($"CONNECT {sample.SampleId}");
                        continue;
                    }
                }

                if (me.Target == Module.LABORATORY)
                {
                    var sample = me.Samples.FirstOrDefault(x => !me.NeedMolecule(x));

                    if (sample != null)
                    {
                        me.Samples.Remove(sample);
                        Console.WriteLine($"CONNECT {sample.SampleId}");
                        continue;
                    }
                }

                if (me.NeedMolecule() && me.HasCapacity)
                {
                    if (me.Target != Module.MOLECULES)
                    {
                        Actions.GoTo(Module.MOLECULES);
                    }

                    var totalCost = 0;

                    var availableMolecules = _molecule.Available;

                    var collectableSamples = new List<Sample>();

                    foreach (var sample in me.Samples.OrderByDescending(x => x.HealthPerCost))
                    {
                        if (sample.TotalCost > 10)
                        {
                            Console.Error.WriteLine($"Ignoring SampleId {sample.SampleId} because of total cost");
                            continue;
                        }

                        {
                            if ((totalCost + sample.TotalCost) > 10)
                            {
                                Console.Error.WriteLine(
                                    $"Ignoring SampleId {sample.SampleId} because of total cost of previous samples");
                                continue;
                            }

                            if (!sample.AreMoleculesAvailable())
                            {
                                Console.Error.WriteLine(
                                    $"Ignoring SampleId {sample.SampleId} because molecules are not available");
                                continue;
                            }

                            totalCost += sample.TotalCost;
                            collectableSamples.Add(sample);
                        }
                    }

                    if (!collectableSamples.Any())
                    {
                        Console.Error.WriteLine(
                            "Uh oh, no collectable samples. Should go and discard and get new ones");
                    }

                    foreach (var sample in collectableSamples)
                    {
                        Console.Error.WriteLine($"Collecting for SampleId {sample.SampleId}");

                        if (me.NeedMolecule(sample, Molecule.A) && _molecule.Available.A > 0)
                        {
                            Actions.Connect(Molecule.A);
                        }

                        if (me.NeedMolecule(sample, Molecule.B) && _molecule.Available.B > 0)
                        {
                            Actions.Connect(Molecule.B);
                        }

                        if (me.NeedMolecule(sample, Molecule.C) && _molecule.Available.C > 0)
                        {
                            Actions.Connect(Molecule.C);
                        }

                        if (me.NeedMolecule(sample, Molecule.D) && _molecule.Available.D > 0)
                        {
                            Actions.Connect(Molecule.D);
                        }

                        if (me.NeedMolecule(sample, Molecule.E) && _molecule.Available.E > 0)
                        {
                            Actions.Connect(Molecule.E);
                        }

                        Console.Error.WriteLine($"Didn't Need any molecules for SampleId {sample.SampleId}");
                    }
                }

                if (me.Target != Module.LABORATORY)
                {
                    Actions.GoTo(Module.LABORATORY);
                }

                Console.Error.WriteLine("Uh oh reached end");
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
            }
            catch (EndTurnException e)
            {
            }
        }
    }

    public class EndTurnException : Exception
    {
    }

    private class MoleculeStation
    {
        public MoleculeQuantity Available = new MoleculeQuantity();
    }

    public static class Actions
    {
        public static void GoTo(Module module)
        {
            Console.WriteLine($"GOTO {module.ToString()}");
            throw new EndTurnException();
        }

        public static void Connect(Molecule m)
        {
            Console.WriteLine($"CONNECT {m.ToString()}");
            throw new EndTurnException();
            
        }
    }


    internal enum Module
    {
        START_POS,
        SAMPLES,
        DIAGNOSIS,
        MOLECULES,
        LABORATORY,
    }

    class Sample
    {
        public int SampleId;
        public int CarriedBy;
        public int Rank;
        public string ExpertiseGain;
        public int Health;
        public MoleculeQuantity Cost = new MoleculeQuantity();
        public MoleculeQuantity CurrentCost => Cost - me.Expertise;
        public MoleculeQuantity Need => Cost - me.Expertise - me.Storage;

        public int TotalCost => Cost.Total;
        public double HealthPerCost => (double)Health / (double)TotalCost;
        public bool IsDiagnosed => TotalCost > 0;

        public bool AreMoleculesAvailable(Molecule m) => m switch
        {
            Molecule.A => _molecule.Available.A >= Cost.A,
            Molecule.B => _molecule.Available.B >= Cost.B,
            Molecule.C => _molecule.Available.C >= Cost.C,
            Molecule.D => _molecule.Available.D >= Cost.D,
            Molecule.E => _molecule.Available.E >= Cost.E,
            _ => throw new ArgumentOutOfRangeException(nameof(m), m, null)
        };

        public bool AreMoleculesAvailable() => AreMoleculesAvailable(Molecule.A) && 
                                               AreMoleculesAvailable(Molecule.B) &&
                                               AreMoleculesAvailable(Molecule.C) && 
                                               AreMoleculesAvailable(Molecule.D) &&
                                               AreMoleculesAvailable(Molecule.E);

        public bool Needs(Molecule m) => me.NeedMolecule(this, m);

        public override string ToString()
        {
            return $"{nameof(SampleId)}: {SampleId}, {nameof(Rank)}: {Rank}, {nameof(ExpertiseGain)}: {ExpertiseGain}, {nameof(Health)}: {Health}, Cost: {Cost}, Need: {Need}, {nameof(HealthPerCost)}: {HealthPerCost}, {(IsDiagnosed ? "DIAGNOSED" : "UNDIAGNOSED")}, {nameof(AreMoleculesAvailable)}: {AreMoleculesAvailable()}";
        }
    }

    public class MoleculeQuantity
    {
        public int A;
        public int B;
        public int C;
        public int D;
        public int E;

        public int this[Molecule m] => m switch
        {
            Molecule.A => A,
            Molecule.B => B,
            Molecule.C => C,
            Molecule.D => D,
            Molecule.E => E,
            _ => throw new ArgumentOutOfRangeException(nameof(m), m, null)
        };
        public int Total => A + B + C + D + E;

        public override string ToString()
        {
            return $"{Total} [{A}, {B}, {C}, {D}, {E}]";
        }
        
        public static MoleculeQuantity operator +(MoleculeQuantity l, MoleculeQuantity r)
            => new MoleculeQuantity
            {
                A = l.A + r.A, 
                B = l.B + r.B, 
                C = l.C + r.C, 
                D = l.D + r.D, 
                E = l.E + r.E, 
            };
        
        public static MoleculeQuantity operator -(MoleculeQuantity l, MoleculeQuantity r)
            => new MoleculeQuantity
            {
                A = l.A - r.A, 
                B = l.B - r.B, 
                C = l.C - r.C, 
                D = l.D - r.D, 
                E = l.E - r.E, 
            };

    }

    class Player
    {
        public Module Target;
        public int Eta;
        public int Score;
        public MoleculeQuantity Storage = new MoleculeQuantity();
        public MoleculeQuantity Expertise = new MoleculeQuantity();


        public List<Sample> Samples = new List<Sample>();

        public bool NeedMolecule(Sample s, Molecule m) => m switch
        {
            Molecule.A => s.Cost.A > Storage.A,
            Molecule.B => s.Cost.B > Storage.B,
            Molecule.C => s.Cost.C > Storage.C,
            Molecule.D => s.Cost.D > Storage.D,
            Molecule.E => s.Cost.E > Storage.E,
            _ => throw new ArgumentOutOfRangeException(nameof(m), m, null)
        };

        public bool NeedMolecule(Sample s) => NeedMolecule(s, Molecule.A) || NeedMolecule(s, Molecule.B) || NeedMolecule(s, Molecule.C) || NeedMolecule(s, Molecule.D) || NeedMolecule(s, Molecule.E);

        public bool NeedMolecule() => Samples.Any() && Samples.Select(x => x.Need).Aggregate((l, r) => l + r).Total > 0;

        public int NeededMolecules => Samples.Sum(x => x.TotalCost);

        public bool IsCarrying(Sample s) => Samples.Any(x => x.SampleId == s.SampleId);
        public bool HasCapacity => TotalStorage < 10;
        private int TotalStorage => Storage.Total;

        public override string ToString()
        {
            return $"{nameof(Target)}: {Target}, {nameof(Eta)}: {Eta}, {nameof(Storage)}: {Storage}, {nameof(Expertise)}: {Expertise}, {nameof(HasCapacity)}: {HasCapacity}, {nameof(NeedMolecule)}: {NeedMolecule()}";
        }
    }
}

internal enum Molecule
{
    A,
    B,
    C,
    D,
    E,
}