using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathProject.Tools;

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
                new double[]{4,0 },
                new double[]{0,-4 },
                new double[]{-4,0 },
            };
            Ngon ngon = new Ngon(edgeVectors);
            CollectionAssert.AreEquivalent(ngon.Verticies, new Vertex[] { new Vertex(0, 4), new Vertex(4, 4), new Vertex(4, 0), new Vertex(0, 0) });
            foreach (Edge edge in ngon.Edges)
                Assert.AreEqual(edge.length, 4, 0.001);
        }

        [TestMethod()]
        public void angleBetweenEdgesDegreesTest()
        {
            Edge a = new Edge(new Vertex(0, 0), new double[] { 0, 1 });
            Edge b = new Edge(new Vertex(0, 1), new double[] { 1, 0 });
            Ngon ngon = new Ngon(new double[][] { new double[] { 0, 1 }, new double[] { 0, -1 }, });
            Assert.AreEqual(ngon.angleBetweenEdgesDegrees(a, b), 90, 0.0001);

            a = new Edge(new Vertex(0, 0), new double[] { 1, 1 });
            b = new Edge(new Vertex(1, 1), new double[] { 2, 2 });
            Assert.AreEqual(ngon.angleBetweenEdgesDegrees(a, b), 180, 0.0001);

            a = new Edge(new Vertex(0, 0), new double[] { 0, 3 });
            b = new Edge(new Vertex(0, 3), new double[] { 3, -3 });
            Assert.AreEqual(ngon.angleBetweenEdgesDegrees(a, b), 45, 0.0001);
        }

        [TestMethod()]
        public void getTypeTest()
        {
            double[][] edgevectors =
            {
                new double[]{1,0},
                new double[]{0,1},
                new double[]{-1,0},
                new double[]{0,-1}
            };
            Ngon ngon = new Ngon(edgevectors);
            Assert.AreEqual(ngon.Type, NgonType.Convex);

            edgevectors = new double[][]
            {
                new double[]{0,2},
                new double[]{4,-2},
                new double[]{0,2},
                new double[]{-4,-2}
            };
            ngon = new Ngon(edgevectors);
            Assert.AreEqual(ngon.Type, NgonType.Self_Intersecting);

            edgevectors = new double[][]{
                new double[]{1,0},
                new double[]{(double)-3/4,(double)1 / 4},
                new double[]{(double)-1/4,(double)3 / 4},
                new double[]{0,-1}
            };
            ngon = new Ngon(edgevectors);
            Assert.AreEqual(ngon.Type, NgonType.Reflex);
        }

        [TestMethod()]
        public void edgesIntersectTest()
        {
            Ngon n = new Ngon(new double[][] { new double[] { 0, 1 }, new double[] { 0, -1 } });

            //intersect
            Edge e1 = new Edge(new Vertex(0, 0), new Vertex(1, 1));
            Edge e2 = new Edge(new Vertex(1, 0), new Vertex(0, 1));
            Assert.IsTrue(n.edgesIntersect(e1, e2));

            //parallel
            e1 = new Edge(new Vertex(0, 1), new Vertex(3, 1));
            e2 = new Edge(new Vertex(0, 0), new Vertex(10, 0));
            Assert.IsFalse(n.edgesIntersect(e1, e2));

            //nonparallel, not intersect
            e1 = new Edge(new Vertex(10, 0), new Vertex(8, 1));
            e2 = new Edge(new Vertex(0, 0), new Vertex(3, 20));
            Assert.IsFalse(n.edgesIntersect(e1, e2));
        }

        [TestMethod()]
        public void getEdgeVectorsTest()
        {
            double[][] edgevectors = new double[][]{
                new double[]{0,2},
                new double[]{4,-2},
                new double[]{0,2},
                new double[]{-4,-2}
            };
            Ngon ngon = new Ngon(edgevectors);
            compareArrays(ngon.getEdgeVectors(), edgevectors);
        }
        private void compareArrays(double[][] a, double[][] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                CollectionAssert.AreEquivalent(a[i], b[i]);
            }
        }

        [TestMethod()]
        public void isConvexXorTest()
        {
            long sampleSize = 1000000;
            for (int i = 0; i < sampleSize; i++)
            {
                var ngon = Program.generateRandomNgon(5);
                PluckerMatrix plucker = new PluckerMatrix(ngon);
                SignMatrix signMatrix = new SignMatrix(plucker);
                ngon.PluckerSignMatrix = signMatrix;
                bool xorOutput = ngon.isConvexXor();
                bool result = ngon.Type == NgonType.Convex;
                if (result != xorOutput)
                    ngon = null;
                Assert.AreEqual(xorOutput, result);
            }
        }
    }
}