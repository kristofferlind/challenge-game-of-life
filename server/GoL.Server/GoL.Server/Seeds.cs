using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoL.Server
{
    public class Seeds
    {
        public static List<Cell> Stairs = new List<Cell>()
        {
            new Cell()
            {X = 2, Y = 10},
            new Cell()
            {X = 3, Y = 10},
            new Cell()
            {X = 3, Y = 11},
            new Cell()
            {X = 4, Y = 10},
            new Cell()
            {X = 4, Y = 11},
            new Cell()
            {X = 5, Y = 11},
        };

        public static List<Cell> Toad = new List<Cell>()
        {
            new Cell()
            {X = 0, Y = 0},
            new Cell()
            {X = 0, Y = 1},
            new Cell()
            {X = 0, Y = 2},
            new Cell()
            {X = 1, Y = 1},
            new Cell()
            {X = 1, Y = 2},
            new Cell()
            {X = 1, Y = 3},
        };
    };
}
