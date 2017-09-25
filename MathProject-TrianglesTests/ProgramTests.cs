using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject_Triangles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject_Triangles.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void generatePointTest()
        {
            double radius = (new Random()).NextDouble() * 100;

            double[] point = Program.generatePoint(radius);
            double obtainedResult = Math.Pow(point[0], 2) + Math.Pow(point[1], 2) + Math.Pow(point[2], 2);

            Assert.AreEqual(Math.Pow(radius, 2), obtainedResult);
        }
    }
}