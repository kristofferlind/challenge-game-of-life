namespace CoolGoL
{
    public static class Rules
    {
        public static bool ShouldLive(int neighbours)
        {
             //Any live cell with fewer than two live neighbours dies, as if caused by under-population.
             //Any live cell with more than three live neighbours dies, as if by overcrowding.

             if (neighbours < 2 || neighbours > 3)
                 return false;

            //Any live cell with two or three live neighbours lives on to the next generation.
            return true;
        }

        public static bool ShouldZombiefy(int neighbours)   
        {
            //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

            if (neighbours == 3)
                return true;
            return false;
        }
    }
}