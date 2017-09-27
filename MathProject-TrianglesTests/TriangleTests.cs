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
    public class TriangleTests
    {
        [TestMethod()]
        public void angleCalculationTest()
        {
            //calculate angles
                //isosceles
                Triangle isosceles = new Triangle(1, 1, 1);
                Assert.AreEqual((int)isosceles.angleAB, 60);
                Assert.AreEqual((int)isosceles.angleBC, 60);
                Assert.AreEqual((int)isosceles.angleCA, 60);
                //right
                Triangle right = new Triangle(4, 3, 5);
                Assert.AreEqual((int)right.angleAB, 90 );
                Assert.AreEqual((int)right.angleBC,(int) (Math.Asin((double)4 / 5) * 180 / Math.PI));
                Assert.AreEqual((int)right.angleCA,(int) (Math.Asin((double)3 /5) * 180 / Math.PI));
        }

        [TestMethod()]
        public void obtusityTest()
        {
            Triangle isosceles = new Triangle(1, 1, 1);
            Assert.IsFalse(isosceles.isObtuse);

            Triangle right = new Triangle(3, 4, 5);
            Assert.IsFalse(right.isObtuse);

            Triangle obtuse = new Triangle(2, 4, 5);
            Assert.IsTrue(obtuse.isObtuse);
        }
    }
}