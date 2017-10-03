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
    public class TriangleTests
    {
        [TestMethod()]
        public void TriangleFromNgonTest()
        {
            double[][] edgeVectors = new double[][]
            {
                new double[]{0,3},
                new double[]{2,-3},
                new double[]{-2,0}
            };
            Ngon ngon = new Ngon(edgeVectors);
            Triangle t = new Triangle(ngon);
            Assert.IsTrue(Math.Round(t.angleAB)==90|| Math.Round(t.angleBC) == 90||Math.Round(t.angleCA) == 90);

            edgeVectors = new double[][]
            {
                new double[]{3,8},
                new double[]{-3,8},
                new double[]{0,-16}
            };
            ngon = new Ngon(edgeVectors);
            t = new Triangle(ngon);
            Assert.IsTrue(t.isObtuse);
        }
    }
}