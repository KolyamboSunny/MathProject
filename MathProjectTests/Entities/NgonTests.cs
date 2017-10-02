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
    public class NgonTests
    {
        [TestMethod()]
        public void NgonTest()
        {
            double[][] edgeVectors = new double[][]
            {
                new double[]{0,4 },
                new double[]{2,-2 },
                new double[]{-2,-2 }
            };
            Ngon ngon = new Ngon(edgeVectors);
            CollectionAssert.AreEquivalent(ngon.Verticies,new Vertex[] { new Vertex(0, 4), new Vertex(2, 2), new Vertex(0, 0) });
        }
    }
}