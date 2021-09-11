using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WindowsFormsApp1;

namespace Tests
{
    [TestClass]
    public class MazeTests
    {
        [TestMethod]
        public void TestValidCell()
        {
            var maze = new Maze();
            Assert.AreEqual(maze.GetWidth(), 10);

            Assert.IsTrue(maze.CellValid(0, 0));
            Assert.IsTrue(maze.CellValid(-1, 0));
        }
    }
}
