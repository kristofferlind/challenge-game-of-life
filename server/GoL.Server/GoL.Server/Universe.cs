using System;
using GoL.Server.App_Infrastructure;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Raven.Abstractions.Extensions;

namespace GoL.Server
{
    public class Universe
    {
        public HashSet<Tuple<int,int>> CurrentGenCells { get; set; }
        public HashSet<Tuple<int,int>> NextGenCells { get; set; }
        public Dictionary<Tuple<int,int>,int> PotentialCells { get; set; }

        public static bool Running = false;
        public static int ViewerCount;

        public static void Start()
        {
            var seed = Seeds.RPentomino;
            seed.AddRange(Seeds.Stairs);
            seed.AddRange(Seeds.Toad);
            seed.AddRange(Seeds.RPentomino2);
            seed.AddRange(Seeds.Stairs2);

            var universe = new Universe(seed);
            Running = true;
            int i = 0;

            while (Running && universe.CurrentGenCells.Count > 0)
            {
                universe.PopulateNextGen();
                i++;
                
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

        public void PopulateNextGen()
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
            
            GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All.nextUniverseStep(HsToList(CurrentGenCells));

            CurrentGenCells = NextGenCells;
            NextGenCells = new HashSet<Tuple<int, int>>();
        }

        private List<Cell> HsToList(HashSet<Tuple<int,int>> currentGenCells)
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