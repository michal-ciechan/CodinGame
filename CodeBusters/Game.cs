using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBusters
{
    public class Buster : IEntity
    {
        public int EntityId { get; set; }
        public Vector Location { get; private set; }
        public bool IsCarrying { get; private set; }
        public bool IsEnemy { get; private set; }
        public int? CarryingGhostId { get; private set; }

        public Buster(Input input)
        {
            IsEnemy = input.EntityType == EntityType.Enemy;
            Update(input);
        }

        public void Update(Input input)
        {
            Location = input.Location;
            IsCarrying = input.State == 1;
            CarryingGhostId = input.Value;
        }
    }

    public interface IEntity
    {
        int EntityId { get; set; }
        void Update(Input input);
    }

    public class Ghost : IEntity
    {
        public Ghost(Input input)
        {
            Update(input);
        }

        public int EntityId { get; set; }
        public Vector Location { get; private set; }
        public int NumberOfBustersTrapping { get; private set; }
        
        public void Update(Input input)
        {
            Location = input.Location;
            NumberOfBustersTrapping = input.Value;

        }
    }

    public class Input
    {
        public int EntityId { get; set; }
        public Vector Location { get; set; }
        public EntityType EntityType { get; set; }
        public int State { get; set; }
        public int Value { get; set; }

        public Input(string[] inputs)
        {
            int entityId = int.Parse(inputs[0]); // buster id or ghost id
            int x = int.Parse(inputs[1]);
            int y = int.Parse(inputs[2]); // position of this buster / ghost
            int entityType = int.Parse(inputs[3]); // the team id if it is a buster, -1 if it is a ghost.
            int state = int.Parse(inputs[4]); // For busters: 0=idle, 1=carrying a ghost.
            int value = int.Parse(inputs[5]); // For busters: Ghost id being carried. For ghosts: number of busters attempting to trap this ghost.

            EntityId = entityId;
            Location = new Vector(x, y);
            EntityType = GetEntityType(entityType);
            State = state;
            Value = value;
        }

        private EntityType GetEntityType(int entityType)
        {
            if(entityType == -1)
                return EntityType.Ghost;

            if(entityType == Game.MyTeamId)
                return EntityType.Buster;
            
            return EntityType.Enemy;
        }
    }

    public class Game
    {
        public static int MyTeamId;
        public static Dictionary<int, IEntity> Entities = new Dictionary<int, IEntity>();
        public static List<Buster> MyTeam = new List<Buster>();
        public static List<Buster> EnemyTeam = new List<Buster>();
        public static List<Ghost> Ghosts = new List<Ghost>();



        static void Main(string[] args)
        {
            int bustersPerPlayer = int.Parse(Console.ReadLine()); // the amount of busters you control
            int ghostCount = int.Parse(Console.ReadLine()); // the amount of ghosts on the map
            MyTeamId = int.Parse(Console.ReadLine()); // if this is 0, your base is on the top left of the map, if it is one, on the bottom right

            // game loop
            while (true)
            {
                Entities.Clear();
                MyTeam.Clear();
                EnemyTeam.Clear();
                Ghosts.Clear();

                int entities = int.Parse(Console.ReadLine()); // the number of busters and ghosts visible to you
                for (int i = 0; i < entities; i++)
                {
                    string[] inputs = Console.ReadLine().Split(' ');

                    var input = new Input(inputs);

                    IEntity entity;
                    if (!Entities.TryGetValue(input.EntityId, out entity))
                    {
                        CreateEntity(input);
                        Entities.Add(entity.EntityId, entity);
                    }
                    else
                    {
                        entity.Update(input);
                    }
                }
                for (int i = 0; i < bustersPerPlayer; i++)
                {

                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    Console.WriteLine("MOVE 8000 4500"); // MOVE x y | BUST id | RELEASE
                }
            }
        }

        private static void CreateEntity(Input input)
        {
            if (input.EntityType == EntityType.Ghost)
            {
                var ghost = new Ghost(input);
                
                Ghosts.Add(ghost);
                Entities.Add(ghost.EntityId, ghost);
            }

            var buster =  new Buster(input);

            if(buster.IsEnemy)
                EnemyTeam.Add(buster);
            else
                MyTeam.Add(buster);

            Entities.Add(buster.EntityId, buster);
        }
    }
}

