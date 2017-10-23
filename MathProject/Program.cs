using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Distributions.Univariate;
using Accord.Math.Decompositions;
using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using System.IO;

namespace MathProject_Triangles
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many times to repeat the experiment?");
            long outerSampleSize = Int64.Parse(Console.ReadLine());
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());
             
            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            double[] rawLengths = new double[n];
            for (long j = outerSampleSize; j > 0; j--)
            {
                long convex = 0;
                long reflex = 0;
                long self_intersecting = 0;
                for (long i = sampleSize; i > 0; i--)
                {
                    if (i % 1000 == 0) Console.WriteLine(i + " | " + sampleSize);
                    Ngon ngon = generateRandomNgon(n);

                    switch (ngon.Type)
                    {
                        case (NgonType.Convex):
                            convex++;
                            break;
                        case (NgonType.Reflex):
                            reflex++;
                            break;
                        case (NgonType.Self_Intersecting):
                            self_intersecting++;
                            break;
                    }
                }
                Console.WriteLine(j + ", "+n + ", " + sampleSize + ": " + convex + " | " + reflex + " | " + self_intersecting);
                writeCSV(new double[] { n, sampleSize, convex, reflex, self_intersecting });
            }
            
            Console.Read();
        }

        private static void writeCSV(double[] toWrite)
        {
            StreamWriter writer = new StreamWriter((new FileStream("experiment1.csv", FileMode.Append)));
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

