using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathProject_Triangles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MathProject_Triangles.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void generatePointTest()
        {
            double[] samples = new double[3000];
            for (int i=0;i<samples.Length;i+=3)
            {
                double[] point=Program.generatePoint();
                samples[i] = point[0];
                samples[i+1] = point[1];
                samples[i+2] = point[2];
            }
            writeCSV(samples);
        }

        private void writeCSV(double[] toWrite)
        {
            StreamWriter writer = new StreamWriter((new FileStream("randomValues.csv",FileMode.Create)));
            foreach(var line in toWrite)
            {
                writer.WriteLine(line.ToString() + ';');
            }
            writer.Close();
        }
    }
}