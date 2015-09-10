using System;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GoL.MVC;
using GoL.MVC.App_Infrastructure;
using GoL.MVC.Models;
using Microsoft.Ajax.Utilities;
using Raven.Abstractions.Extensions;

namespace GoL.MVC.Models
{
    public class Universe
    {
        public HashSet<Tuple<int,int>> CurrentGenCells { get; set; }
        public HashSet<Tuple<int,int>> NextGenCells { get; set; }
        public Dictionary<Tuple<int,int>,int> PotentialCells { get; set; }
        public static int Generation;
        private static List<Cell> StartSeed;
        private List<List<Cell>> History { get; set; }


        public static bool Running = false;
        public static int ViewerCount;

        public static void Start(List<Cell> seed = null, int generation = 0)
        {
            if (seed != null)
                StartSeed = seed;
            else
            {
                StartSeed = Seeds.RPentomino;
                StartSeed.AddRange(Seeds.Stairs);
                StartSeed.AddRange(Seeds.Toad);
                StartSeed.AddRange(Seeds.RPentomino2);
                StartSeed.AddRange(Seeds.Stairs2);
            }

            Generation = generation;

            var universe = new Universe(StartSeed);
            Running = true;

            while (Running && universe.CurrentGenCells.Count > 0)
            {
                universe.PopulateNextGen();

                if (Generation%1000==0)
                    universe.History.Add(HsToList(universe.CurrentGenCells));

                Generation++;

                Thread.Sleep(16);
            }
        }

        public Universe(List<Cell> seed)
        {
            CurrentGenCells = new HashSet<Tuple<int, int>>();
            NextGenCells = new HashSet<Tuple<int, int>>();
            PotentialCells = new Dictionary<Tuple<int, int>, int>();

            foreach (var cell in seed)
            {
                CurrentGenCells.Add(new Tuple<int, int>(cell.X, cell.Y));
            }
        }

        internal List<Generation> GetHistoryBatch(int startGeneration, int endGeneration)
        {
            var universe = new Universe(GetLatestHistory());
            int generationNumber = 0;

            List<Generation> historyBatch = new List<Generation>();

            while (generationNumber <= endGeneration)
            {
<<<<<<< HEAD
                var cells = universe.PopulateNextGen();
=======
                var cells = HsToList(universe.PopulateNextGen());
>>>>>>> origin/master
                if (generationNumber >= startGeneration)
                {
                    historyBatch.Add(new Generation() { Cells = cells, GenerationNumber = generationNumber });
                }
                generationNumber += 1;
            }

            return historyBatch;
        }

        private List<Cell> GetLatestHistory()
        {
            // First 1k is cached on the client
            if (History.Count < 2)
                return StartSeed;
            return History[History.Count - 2];
        }

        public HashSet<Tuple<int,int>> PopulateNextGen()
        {
            PotentialCells.Clear();
            NextGenCells.Clear();

            foreach (var cell in CurrentGenCells)
            {
                if (Rules.ShouldLive(CheckNeighbours(cell.Item1, cell.Item2)))
                    NextGenCells.Add(new Tuple<int, int>(cell.Item1, cell.Item2));
            }

            var results = PotentialCells.Where(p => Rules.ShouldZombiefy(p.Value)).ToHashSet();

            foreach (var result in results)
            {
                NextGenCells.Add(new Tuple<int, int>(result.Key.Item1, result.Key.Item2));
            }
            
            GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All.nextUniverseStep(HsToList(CurrentGenCells), Generation);

            CurrentGenCells = NextGenCells;
            NextGenCells = new HashSet<Tuple<int, int>>();

            return CurrentGenCells;
        }

        private static List<Cell> HsToList(HashSet<Tuple<int,int>> currentGenCells)
        {
            return currentGenCells.Select(tuple => new Cell() {X = tuple.Item1, Y = tuple.Item2}).ToList();
        }

        private int CheckNeighbours(int x, int y)
        {
            var neighbours = GetNeighbours(x, y);
            int neighbourCount = 0;

            foreach (var neighbour in neighbours)
            {
                if (CurrentGenCells.Contains(new Tuple<int, int>(neighbour.Item1, neighbour.Item2)))
                    neighbourCount++;
                else
                {
                    var currentNeighbour = new Tuple<int,int>(neighbour.Item1, neighbour.Item2);
                    int currentNeighbourCount;
                    if (PotentialCells.TryGetValue(currentNeighbour,
                        out currentNeighbourCount))
                    {
                        PotentialCells[currentNeighbour] += 1;
                    }
                    else
                    {
                        PotentialCells.Add(currentNeighbour, 1);
                    }
                }
            }
            return neighbourCount;
        }

        private HashSet<Tuple<int,int>> GetNeighbours(int x, int y)
        {
            var coordinates = new HashSet<Tuple<int,int>>();

            coordinates.Add(new Tuple<int, int>( x-1, y-1));
            coordinates.Add(new Tuple<int, int>(x-1, y));
            coordinates.Add(new Tuple<int, int>(x-1, y+1));
            coordinates.Add(new Tuple<int, int>(x, y-1));
            coordinates.Add(new Tuple<int, int>(x, y + 1));
            coordinates.Add(new Tuple<int, int>(x + 1, y - 1));
            coordinates.Add(new Tuple<int, int>(x + 1, y));
            coordinates.Add(new Tuple<int, int>(x + 1, y + 1));

            return coordinates;
        }

        public static void Stop()
        {
            Running = false;
        }
    }
}