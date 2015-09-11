using System.Collections.Generic;

namespace CoolGoL
{
    public class Seeds
    {
        public Dictionary<string,List<Cell>> AllSeeds;

        public Seeds()
        {
            AllSeeds = new Dictionary<string,List<Cell>>();
            AllSeeds.Add("Stairs", Stairs);
            AllSeeds.Add("Toad", Toad);
            AllSeeds.Add("RPentomino", RPentomino);
            AllSeeds.Add("Glider Gun", GliderGun);
        }

        public static List<Cell> GliderGun = new List<Cell>()
        {
            new Cell()
            {X = 0, Y = -40 },
            new Cell()
            {X = 0, Y = -39 },
            new Cell()
            {X = -2, Y = -39 },
            new Cell()
            {X = -3, Y = -38 },
            new Cell()
            {X = -4, Y = -38 },
            new Cell()
            {X = -11, Y = -38 },
            new Cell()
            {X = -12, Y = -38 },
            new Cell()
            {X = 10, Y = -38 },
            new Cell()
            {X = 11, Y = -38 },
            new Cell()
            {X = -3, Y = -37 },
            new Cell()
            {X = -4, Y = -37 },
            new Cell()
            {X = -9, Y = -37 },
            new Cell()
            {X = -13, Y = -37 },
            new Cell()
            {X = 10, Y = -37 },
            new Cell()
            {X = 11, Y = -37 },
            new Cell()
            {X = -3, Y = -36 },
            new Cell()
            {X = -4, Y = -36 },
            new Cell()
            {X = -8, Y = -36 },
            new Cell()
            {X = -14, Y = -36 },
            new Cell()
            {X = -23, Y = -36 },
            new Cell()
            {X = -24, Y = -36 },
            new Cell()
            {X = 0, Y = -35 },
            new Cell()
            {X = -2, Y = -35 },
            new Cell()
            {X = -7, Y = -35 },
            new Cell()
            {X = -8, Y = -35 },
            new Cell()
            {X = -10, Y = -35 },
            new Cell()
            {X = -14, Y = -35 },
            new Cell()
            {X = -23, Y = -35 },
            new Cell()
            {X = -24, Y = -35 },
            new Cell()
            {X = 0, Y = -34 },
            new Cell()
            {X = -8, Y = -34 },
            new Cell()
            {X = -14, Y = -34 },
            new Cell()
            {X = -9, Y = -33 },
            new Cell()
            {X = -13, Y = -33 },
            new Cell()
            {X = -11, Y = -32 },
            new Cell()
            {X = -12, Y = -32 },

        };

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
        public static List<Cell> RPentomino = new List<Cell>()
        {
            new Cell()
            {X = 3, Y = 2},
            new Cell()
            {X = 4, Y = 2},
            new Cell()
            {X = 2, Y = 3},
            new Cell()
            {X = 3, Y = 3},
            new Cell()
            {X = 3, Y = 4}
        };
        public static List<Cell> RPentomino2 = new List<Cell>()
        {
            new Cell()
            {X = 9, Y = 2},
            new Cell()
            {X = 10, Y = 2},
            new Cell()
            {X = 8, Y = 3},
            new Cell()
            {X = 9, Y = 3},
            new Cell()
            {X = 9, Y = 4}
        };
        public static List<Cell> Stairs2 = new List<Cell>()
        {
            new Cell()
            {X = 5, Y = 10},
            new Cell()
            {X = 6, Y = 10},
            new Cell()
            {X = 6, Y = 11},
            new Cell()
            {X = 7, Y = 10},
            new Cell()
            {X = 7, Y = 11},
            new Cell()
            {X = 8, Y = 11},
        };
    };
}
