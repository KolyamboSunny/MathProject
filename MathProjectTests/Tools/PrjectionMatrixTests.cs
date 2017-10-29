using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathProject.Entities;
using MathProject_Triangles;

namespace MathProject.Tools.Tests
{
    [TestClass()]
    public class PrjectionMatrixTests
    {
        [TestMethod()]
        public void ProjectionMatrixTest()
        {
            Ngon n = Program.generateRandomNgon(10);
            double[][] P = new ProjectionMatrix(n).columnVectors;
            for (int i = 0; i < P.Count(); i++)
            {
                for (int j = 0; j < P[i].Count(); j++)
                {
                    Assert.AreEqual(P[i][j], P[j][i]);
                }
            }
        }

        [TestMethod()]
        public void signChambersProjectionTest()
        {
            double[][] problem = new double[][]
            {
                new double[]{ 1, -10, 35},
                new double[]{ 3, -49, 80},
                new double[]{ -13, 10, 99}
            };
            int[][] answer = new int[][]
            {
                new int[]{ 1, -1, 1},
                new int[]{ 1, -1, 1},
                new int[]{ -1, 1, 1}
            };
            int[][] result = new ProjectionMatrix() { columnVectors = problem }.signChambers();
            for (int i = 0; i < answer.Count(); i++)
                for (int j = 0; j < answer[i].Count(); j++)
                    Assert.IsTrue(answer[i][j] == result[i][j]);
        }
    }
}