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
            for (int j = 0; j < ngon.getOrthonormal().Length; j++)
            {
                for (int i = 0; i < ngon.getOrthonormal()[j].Length; i++)
                {
                    //double fromEdgeVectors = ngon.getOrthonormal()[i][0] * ngon.getOrthonormal()[j][1] - ngon.getOrthonormal()[j][0] * ngon.getOrthonormal()[i][1];
                    //Assert.AreEqual(fromEdgeVectors, D.columnVectors[i][j]);
                }
            }
            return D;
        }

        [TestMethod()]
        public ProjectionMatrix ProjectionMatrixTest(Ngon ngon)
        {
            ProjectionMatrix P = new ProjectionMatrix(ngon);
            for (int j = 0; j < ngon.getOrthonormal().Length; j++)
            {
                for (int i = 0; i < ngon.getOrthonormal()[j].Length; i++)
                {
                    //double fromEdgeVectors = ngon.getOrthonormal()[i][0] * ngon.getOrthonormal()[j][0] + ngon.getOrthonormal()[i][1] * ngon.getOrthonormal()[j][1];
                    //Assert.AreEqual(fromEdgeVectors, P.columnVectors[i][j]);
                }
            }
            return P;
        }

        [TestMethod()]
        public void PluckerAndProjectionMatrixEquality()
        {
            Ngon ngon = Program.generateRandomNgon(4);
            PluckerMatrix plucker = PluckerMatrixTest(ngon);
            ProjectionMatrix projection = ProjectionMatrixTest(ngon);

            Matrix<double> D = Matrix<double>.Build.DenseOfColumnArrays(plucker.columnVectors);
            string d = D.ToString();
            Matrix<double> Dnsq = (D.Multiply(D)).Negate();
            string dnsq = Dnsq.ToString();
            Matrix<double> P = Matrix<double>.Build.DenseOfColumnArrays(projection.columnVectors);
            string p = P.ToString();
            compareMatricies(Dnsq, P);
        }

        private void compareMatricies(Matrix<double> a, Matrix<double> b)
        {
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    Assert.AreEqual(a[i, j], b[i, j], 0.00000000001);
                }
            }
        }

        [TestMethod()]
        public void SignMatrixEqualsTest()
        {
            Ngon ngon = Program.generateRandomNgon(6);
            SignMatrix a = new SignMatrix(new PluckerMatrix(ngon));
            SignMatrix b = new SignMatrix(new PluckerMatrix(ngon));
            Assert.IsTrue(a.Equals(b));
        }      
    }
}