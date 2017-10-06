using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Entities.Tests
{
    [TestClass()]
    public class EdgeTests
    {
        [TestMethod()]
        public void EdgeTest()
        {
            double precision = 0.000001;
            Vertex vertex1 = new Vertex(0, 0);
            Vertex vertex2 = new Vertex(0,10);
            Edge edge = new Edge(vertex1, vertex2);
            Assert.AreEqual(edge.length, 10, precision);

            vertex1 = new Vertex(0, 0);
            vertex2 = new Vertex(10, 0);
            edge = new Edge(vertex1, vertex2);
            Assert.AreEqual(edge.length, 10, precision);

            vertex1 = new Vertex(0, 0);
            vertex2 = new Vertex(4, 3);
            edge = new Edge(vertex1, vertex2);
            Assert.AreEqual(edge.length, 5, precision);
        }
    }
}