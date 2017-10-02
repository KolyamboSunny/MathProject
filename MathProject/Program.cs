using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accord.Statistics.Distributions.Univariate;
using Accord.Math.Decompositions;
using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;

namespace MathProject_Triangles
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());
            

            Console.Read();
        }
        public static Ngon generteRandomNgon(int n)
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

