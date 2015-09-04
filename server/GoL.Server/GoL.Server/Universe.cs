using GoL.Server.App_Infrastructure;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GoL.Server
{
    public class Universe
    {
        public List<Cell> CurrentGenCells { get; set; }
        public List<Cell> NextGenCells { get; set; }
        public List<PotentialCell> PotentialCells { get; set; }
        public static bool Running = false;
        public static int ViewerCount;

        public static void Start()
        {
            var seed = Seeds.RPentomino;

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
            CurrentGenCells = seed;
            PotentialCells = new List<PotentialCell>();
            NextGenCells = new List<Cell>();
        }

        public void PopulateNextGen()
        {
            PotentialCells.Clear();
            NextGenCells.Clear();

            foreach (var cell in CurrentGenCells)
            {
                if (Rules.ShouldLive(CheckNeighbours(cell)))
                    NextGenCells.Add(cell);
            }

            var results = PotentialCells.Where(p => Rules.ShouldZombiefy(p.NeighbourCount)).Select(p => p.AsCell());

            NextGenCells.AddRange(results);
            
            GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients.All.nextUniverseStep(CurrentGenCells);

            CurrentGenCells = NextGenCells;
            NextGenCells = new List<Cell>();
        }

        private int CheckNeighbours(Cell cell)
        {
            var neighbours = GetNeighbours(cell);
            int neighbourCount = 0;

            foreach (var neighbour in neighbours)
            {
                //if (CurrentGenCells.Contains(neighbour))
                //    neighbourCount++;

                var result = (from c in CurrentGenCells
                    where c.X == neighbour.X
                    where c.Y == neighbour.Y
                    select c).ToList();
                if (result.Count != 0)
                    neighbourCount++;

                else
                {
                    var potentialCell = (from p in PotentialCells
                        where p.X == neighbour.X
                        where p.Y == neighbour.Y
                        select p).FirstOrDefault();
                    if (potentialCell == null)
                    {
                        PotentialCells.Add(new PotentialCell() {X = neighbour.X, Y = neighbour.Y, NeighbourCount = 1});
                    }
                    else
                        potentialCell.NeighbourCount++;
                }
            }
            return neighbourCount;
        }

        private List<Cell> GetNeighbours(Cell cell)
        {
            var coordinates = new List<Cell>()
            {
                new Cell() {X = cell.X-1,Y = cell.Y-1 },
                new Cell() {X = cell.X-1, Y = cell.Y},
                new Cell() {X = cell.X-1, Y = cell.Y+1},
                new Cell() {X = cell.X, Y = cell.Y-1},
                new Cell() {X = cell.X, Y = cell.Y+1},
                new Cell() {X = cell.X+1, Y = cell.Y-1},
                new Cell() {X = cell.X+1, Y = cell.Y},
                new Cell() {X = cell.X+1, Y = cell.Y+1}
            };
            
            return coordinates;
        }

        public static void Stop()
        {
            Running = false;
        }
    }
}