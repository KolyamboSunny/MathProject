using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using MathProject.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MathProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignMatrixExperiment();
            Console.Read();
        }        
        private static void SignMatrixExperiment()
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());
            //ProjectionSignMatrixDistribution D = new ProjectionSignMatrixDistribution(sampleSize, n);
            //PluckerSignMatrixDistribution p = new PluckerSignMatrixDistribution(sampleSize, n);
            PluckerAndProjectionMatricesDistribution pd = new PluckerAndProjectionMatricesDistribution(sampleSize, n);
            pd.saveToHtml(n + "_" + sampleSize + ".html");
            //Console.WriteLine("Even number of signs means convex: " + p.testEvenSignsHypothesis());
            //p.sortFullBySimilarity();
            //p.showOnly("experiment4.html",NgonType.Convex);
            //pd.saveFullToHtml(n+"_"+sampleSize+"_Full"+".html");
            //p.saveReducedToHtml(n + "_" + sampleSize + "_Reduced" + ".html");
            Console.WriteLine("DONE");
        }
        /*
        private static void ReducedSignMatrixDistribution()
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            ReducedPluckerSignMatrixDistribution p = new ReducedPluckerSignMatrixDistribution(sampleSize, n);
            Console.WriteLine("Even number of signs means convex: " + p.testEvenSignsHypothesis());
            p.saveToHtmlDistribution("experiment3.html");
            Console.WriteLine("DONE");
        }*/
        private static void SignMatrixDistribution()
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            PluckerSignMatrixDistribution p = new PluckerSignMatrixDistribution(sampleSize, n);
            Console.WriteLine("Even number of signs means convex: "+p.testEvenSignsHypothesis());            
            p.saveFullToHtml("experiment3.html");
            Console.WriteLine("DONE");
        }
        private static void SignMatrixPermutation()
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            PluckerSignMatrixDistribution p = new PluckerSignMatrixDistribution(sampleSize, n);
            p.saveToHtmlPermutations("experiment3.html");
            Console.WriteLine("DONE");
        }

        private static void NgonEdgePermutationExperiment()
        {
            Console.WriteLine("How many times to repeat the experiment?");
            long outerSampleSize = Int64.Parse(Console.ReadLine());
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());
            List<double[]> results = new List<double[]>();
            for (long i = sampleSize; i > 0; i--)
            {
                if (i % (sampleSize / 10) == 0) Console.WriteLine(i / (sampleSize / 100) + "%");
                Ngon ngon = generateRandomNgon(n);

                var permutations = (new NgonEdgePermutations(ngon)).edgePermutations();
                int convex = permutations.Count(m => m.Type == NgonType.Convex);
                int reflex = permutations.Count(m => m.Type == NgonType.Reflex);
                int self_intersecting = permutations.Count(m => m.Type == NgonType.Self_Intersecting);

                double[] entry = results.Find(e => e[0] == convex && e[1] == reflex && e[2] == self_intersecting);
                if (entry != null) entry[3]++;
                else results.Add(new double[] { convex, reflex, self_intersecting, 1 });
            }
            foreach (double[] entry in results)
            {
                writeCSV(entry);
            }
            Console.WriteLine("DONE");
        }


        private static void writeCSV(double[] toWrite)
        {
            StreamWriter writer = new StreamWriter((new FileStream("experiment2.csv", FileMode.Append)));
            for(int i = 0;i < toWrite.Length-1;i++)
            {
                writer.Write(toWrite[i].ToString() + ';');
            }
            writer.WriteLine(toWrite[toWrite.Length - 1]);
            writer.Close();
        }

        #region NgonGeneration
        public static Ngon generateRandomNgon(int n)
        {
            double[] vector1 = generateVector(n);
            double[] vector2 = generateVector(n);

            double[][] orthonormal = GramSchmidt(new double[][] { vector1, vector2 });
            double[][] edgeVectors = squareEntries(orthonormal);

            return new Ngon(edgeVectors,orthonormal);
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
        #endregion
    }

   

    
}

