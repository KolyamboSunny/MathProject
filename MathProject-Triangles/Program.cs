using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Statistics.Distributions.Univariate;

namespace MathProject_Triangles
{
    public class Program
    {
        static void Main(string[] args)
        {
            foreach(double entry in generateVector(5))
            {
                Console.WriteLine(entry);
            }
            Console.Read();
        }

        public static double[] generateVector(int n)
        {
            NormalDistribution randomizer = new NormalDistribution(0, 1);
            double[] vector=new double[n];

            for (int i=0;i < vector.Length;i++)
            {
                vector[i] = randomizer.Generate(); 
            }
            return vector;
        }
            
        
    }
}
