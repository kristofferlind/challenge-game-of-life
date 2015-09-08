namespace GoL.MVC
{
    public class PotentialCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NeighbourCount = 0;

        public Cell AsCell()
        {
            return new Cell() {X = this.X, Y = this.Y};
        }
    }
}