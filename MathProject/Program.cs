using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Distributions.Univariate;
using Accord.Math.Decompositions;
using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using System.IO;

namespace MathProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculate average distances between ngon verticies");
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());
             
            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            double[] rawLengths = new double[n];
            for (long i = sampleSize; i > 0; i--)
            {
                if (i % 1000 == 0) Console.WriteLine(i + " | " + sampleSize);
                Ngon ngon = generateRandomNgon(n);

                Vertex origin = ngon.Verticies[0];
                for (int j = 1; j < ngon.Verticies.Count; j++)
                {
                    Vertex to = ngon.Verticies[j];
                    if (to != origin)
                    {
                        Edge diagonal = new Edge(origin, to);
                        rawLengths[j] += Math.Pow(diagonal.length, 2);
                    }
                }
            }
            double k = 0;
            double maxerror = 0;
            foreach (double length in rawLengths)
            {
                double average = length / sampleSize;
                double expected = (8 * k * (n - k) / ((n - 1.00) * n * (n + 2.00)));
                k++;
                double error = (average - expected) / expected * 100;
                if (error > maxerror) maxerror = error;
                Console.WriteLine(average);
                Console.WriteLine(expected);

                Console.WriteLine(error);
                Console.WriteLine();
            }
            writeCSV(new double[] { n, sampleSize, maxerror });
            Console.Read();
        }

        private static void writeCSV(double[] toWrite)
        {
            StreamWriter writer = new StreamWriter((new FileStream("correctnessAnalysis.csv", FileMode.Append)));
            for(int i = 0;i < toWrite.Length-1;i++)
            {
                writer.Write(toWrite[i].ToString() + ';');
            }
            writer.WriteLine(toWrite[toWrite.Length - 1]);
            writer.Close();
        }


        public static Ngon generateRandomNgon(int n)
        {
            double[] vector1 = generateVector(n);
            double[] vector2 = generateVector(n);

            double[][] orthonormal = GramSchmidt(new double[][] { vector1, vector2 });
            double[][] edgeVectors = squareEntries(orthonormal);

            return new Ngon(edgeVectors);
        }

        public static double[] generateVector(int n)
        {
            NormalDistribution randomizer = new NormalDistribution(0, 1);
            double[] vector = new double[n];

            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = randomizer.Generate();
            }
            return vector;
        }

        public static double[][] GramSchmidt(double[][] vectors)
        {
            Matrix<double> matrix = CreateMatrix.DenseOfColumnArrays(vectors);
            var result = matrix.GramSchmidt();
            return result.Q.ToColumnArrays();
        }

        public static double[][] squareEntries(double[][] complexNumbers)
        {
            double[][] result =new double[complexNumbers[0].Length][];
            for(int j=0;j<complexNumbers[0].Length;j++)
            {
                double real = Math.Pow(complexNumbers[0][j], 2)- Math.Pow(complexNumbers[1][j], 2); //Math.Pow(row[0],2)
                double imaginary = 2 * complexNumbers[0][j] * complexNumbers[1][j]; //2*u_j*v_j
                result[j] = new double[] { real, imaginary };
            }
            return result;
        }
    }

   

    
}

