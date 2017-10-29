using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathProject;
namespace MathProject.Entities.Tests
{
    [TestClass()]
    public class NgonEdgePermutationsTests
    {
        [TestMethod()]
        public void edgePermutationsTest()
        {
            Ngon ngon = Program.generateRandomNgon(4);
            NgonEdgePermutations permutations = new NgonEdgePermutations(ngon);
            var result = permutations.edgePermutations();
            Assert.AreEqual(result.Count(), Factorial(4));

            ngon = Program.generateRandomNgon(5);
            permutations = new NgonEdgePermutations(ngon);
            result = permutations.edgePermutations();
            Assert.AreEqual(result.Count(), Factorial(5));

            ngon = Program.generateRandomNgon(6);
            permutations = new NgonEdgePermutations(ngon);
            result = permutations.edgePermutations();
            Assert.AreEqual(result.Count(), Factorial(6));
        }
        private long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }
    }
}