using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace GoL.Server
{
    public class Universe
    {
        public List<Cell> CurrentGenCells { get; set; }
        public List<Cell> NextGenCells { get; set; }
        public List<PotentialCell> PotentialCells { get; set; }

        public Universe(List<Cell> seed)
        {
            CurrentGenCells = seed;
        }

        public void PopulateNextGen()
        {
            foreach (var cell in CurrentGenCells)
            {
                if (Rules.ShouldLive(CheckNeighbours(cell)))
                    NextGenCells.Add(cell);
            }

            var results = PotentialCells.Where(p => Rules.ShouldZombiefy(p.NeighbourCount)).Select(p => p.AsCell());

            NextGenCells.AddRange(results);
        }

        private int CheckNeighbours(Cell cell)
        {
            var neighbours = GetNeighbours(cell);
            int neighbourCount = 0;

            foreach (var neighbour in neighbours)
            {
                if (CurrentGenCells.Contains(neighbour))
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
    }
}