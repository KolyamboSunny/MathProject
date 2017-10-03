using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject_Triangles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MathNet.Numerics.LinearAlgebra;

using MathProject.Entities;

namespace MathProject_Triangles.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void generateVectorTest()
        {
            double[] samples = new double[3000];
            for (int i = 0; i < samples.Length; i += 3)
            {
                double[] point = Program.generateVector(3);
                samples[i] = point[0];
                samples[i + 1] = point[1];
                samples[i + 2] = point[2];
            }
            writeCSV(samples);
        }

        private void writeCSV(double[] toWrite)
        {
            StreamWriter writer = new StreamWriter((new FileStream("randomValues.csv", FileMode.Create)));
            foreach (var line in toWrite)
            {
                writer.WriteLine(line.ToString() + ';');
            }
            writer.Close();
        }

        [TestMethod()]
        public void GramSchmidtTest()
        {
            //checking against WolframAlpha Orthogonalize[] function
            double[] vector1 = { 1, 2, 3 };
            double[] vector2 = { 2, 3, 4 };
            double[][] result = Program.GramSchmidt(new double[][] { vector1, vector2 });

            double[][] answer = new double[][]{
                                    new double[]{1.0/Math.Sqrt(14.0),Math.Sqrt(2.0/7.0),3.0/Math.Sqrt(14.0) },
                                    new double[]{4.0/Math.Sqrt(21.0),1.0/Math.Sqrt(21.0),-2.0/Math.Sqrt(21.0) },
                                };
            double precision = 0.001;

            for (int i = 0; i < result.Length; i++)
                for (int j = 0; j < result[i].Length; j++)
                    Assert.AreEqual(result[i][j], answer[i][j], precision);

            //checking against identity matrix equation
            vector1 = Program.generateVector(5);
            vector2 = Program.generateVector(5);
            result = Program.GramSchmidt(new double[][] { vector1, vector2 });

            Matrix<double> resultMatrix = Matrix<double>.Build.DenseOfColumnArrays(result);
            Matrix<double> transpose = resultMatrix.Transpose();
            Matrix<double> identity = transpose.Multiply(resultMatrix);

            double[][] identityArrayAnswer = new double[2][] { new double[] { 1, 0 }, new double[] { 0, 1 } };
            for (int i = 0; i < identity.RowCount; i++)
                for (int j = 0; j < identity.ColumnCount; j++)
                    Assert.AreEqual(identity[i, j], identityArrayAnswer[i][j], precision);

        }

        [TestMethod()]
        public void TriangleTest()
        {
            long sampleSize = 1000000;
            long areObtuse = 0;
            for (long i = sampleSize; i > 0; i--)
            {
                Ngon ngon = Program.generateRandomNgon(3);
                Triangle t = new Triangle(ngon);
                if (t.isObtuse) areObtuse++;
            }
            double percentObtuse = (double)areObtuse / sampleSize * 100;
            Assert.AreEqual(percentObtuse, 83.8093, 1);
        }

    }


}