using NUnit.Framework;

namespace CoolGoL
{
    [TestFixture]
    public class RulesTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void Underpopulation(int aliveNeighbours)
        {
            var result = Rules.ShouldLive(aliveNeighbours);
            Assert.IsFalse(result);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void Survive(int aliveNeighbours)
        {
            var result = Rules.ShouldLive(aliveNeighbours);
            Assert.IsTrue(result);
        }

        [TestCase(4)]
        public void Overpopulation(int aliveNeighbours)
        {
            var result = Rules.ShouldLive(aliveNeighbours);
            Assert.IsFalse(result);
        }

        [TestCase(3)]
        public void Resurrection(int aliveNeighbours)
        {
            var result = Rules.ShouldZombiefy(aliveNeighbours);
            Assert.IsTrue(result);
        }

    }
}