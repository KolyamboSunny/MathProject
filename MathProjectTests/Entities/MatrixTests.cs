using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathProject.Entities;
using MathNet.Numerics.LinearAlgebra;

namespace MathProject.Tools.Tests
{
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public PluckerMatrix PluckerMatrixTest(Ngon ngon)
        {
            PluckerMatrix D = new PluckerMatrix(ngon);
            for (int i = 0; i < ngon.getEdgeVectors().Length; i++)
            {
                for (int j = 0; j < ngon.getEdgeVectors().Length; j++)
                {
                    double fromEdgeVectors = ngon.getEdgeVectors()[i][0] * ngon.getEdgeVectors()[j][1] - ngon.getEdgeVectors()[j][0] * ngon.getEdgeVectors()[i][1];
                    Assert.AreEqual(fromEdgeVectors, D.columnVectors[j][i]);
                }
            }
            return D;
        }

        [TestMethod()]
        public ProjectionMatrix ProjectionMatrixTest(Ngon ngon)
        {
            ProjectionMatrix P = new ProjectionMatrix(ngon);
            for (int j = 0; j < ngon.getEdgeVectors().Length; j++)
            {
                for (int i = 0; i < ngon.getEdgeVectors().Length; i++)
                {
                    double fromEdgeVectors = ngon.getEdgeVectors()[i][0] * ngon.getEdgeVectors()[j][0] + ngon.getEdgeVectors()[i][1] * ngon.getEdgeVectors()[j][1];
                    Assert.AreEqual(fromEdgeVectors, P.columnVectors[j][i]);
                }
            }
            return P;
        }

        [TestMethod()]
        public void PluckerAndProjectionMatrixEquality()
        {
            Ngon ngon = Program.generateRandomNgon(3);           
            PluckerMatrix plucker = PluckerMatrixTest(ngon);
            ProjectionMatrix projection = ProjectionMatrixTest(ngon);

            Matrix<double> D = Matrix<double>.Build.DenseOfColumnArrays(plucker.columnVectors);
            string d = D.ToString();
            Matrix<double> Dnsq = (D.Multiply(D)).Negate();
            string dnsq = Dnsq.ToString();
            Matrix<double> P = Matrix<double>.Build.DenseOfColumnArrays(projection.columnVectors);
            string p = P.ToString();
            Assert.AreEqual(Dnsq, P);
        }
    }
}