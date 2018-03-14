using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using MathProject.Experiments;
using MathProject.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MathProject
{
    public class Program
    {
        static Func<long, long> Factorial = x => x == 0 ? 1 : x + Factorial(x - 1);
        static void Main(string[] args)
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());
            SignificantSignsExperiment e = new SignificantSignsExperiment(n);
            e.findSignificantSigns();
            //PopulateDatabaseWithNgons();
            
            //MaskExperiment4 p = new MaskExperiment4();
            //p.runExperiment();
            //p.saveToHtml("Reduced2Distribution"+"" + ".html");         
            
            Console.WriteLine("DONE");

            Console.Read();
        }        
        private static void SameSignInTwoEntries()
        {
            NgonDatabase database = new NgonDatabase();
            long length = Factorial(database.PluckerSignMatrices.First().columnVectors.Length - 1) - 1;
            long actualLength = 0;
            int[,] incrementMatrix = new int[length, length];
            var l = (from r in database.PluckerSignMatrices select r);
            foreach (SignMatrix matrix in l)
                if (matrix.Ngons.Count(n => n.Type == NgonType.Convex) != -1)
                {
                    actualLength++;
                    int l1 = 0;
                    for (int i = 0; i < matrix.columnVectors.Length; i++)
                        for (int j = i + 1; j < matrix.columnVectors.Length; j++)
                        {
                            int l2 = 0;
                            for (int m = 0; m < matrix.columnVectors.Length; m++)
                                for (int n = m + 1; n < matrix.columnVectors.Length; n++)
                                {
                                    //Console.Write(i + "-" + j + ":" + m + "-" + n + "   ");
                                    //Console.WriteLine();

                                    if (matrix.columnVectors[i][j] == matrix.columnVectors[m][n])
                                    {
                                        incrementMatrix[l1, l2]++;

                                    }
                                    l2++;

                                }
                            l1++;
                            
                        }

                    
                }
            for (int h = 0; h < length; h++)
            {
                for (int b = 0; b < length; b++)
                    Console.Write((double)incrementMatrix[h, b] + " ");
                Console.WriteLine();
            }
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                    Console.Write((double)incrementMatrix[i, j] / actualLength + " ");
                Console.WriteLine();
            }
        }
        private static void PopulateDatabaseWithNgons()
        {
            Console.WriteLine("How many dimensions are we working with?");
            int n = Int32.Parse(Console.ReadLine());
            Console.WriteLine("How big is the sample size?");
            long sampleSize = Int64.Parse(Console.ReadLine());

            for (long i = sampleSize; i > 0; i-=500)
            {
                if (i % (sampleSize / 10) == 0)
                    Console.WriteLine(i / (sampleSize / 100) + "%");
                addNgonToDatabase(n, 500);                
            }          
        }
        private static void EditNgonMatrixLinks()
        {
            NgonDatabase database = new NgonDatabase();
            foreach(Ngon ngon in database.Ngons)
            {
                ngon.PluckerSignMatrix.Ngons.Add(ngon);
                
            }
            database.SaveChanges();
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
            pd.save2KToHtml("plucker+projction " + n + "_" + sampleSize + ".html");
            //pd.saveToHtml(n + "_" + sampleSize + ".html");
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
        private static void addNgonToDatabase(int n,int sampleSize= 1)
        {
            NgonDatabase database = new NgonDatabase(n);

            database.Database.Connection.Open();
            List<Ngon> ngons = new List<Ngon>();
            for(int i=0;i<sampleSize;i++)
            {
                Ngon ngon = Program.generateRandomNgon(n);
                PluckerMatrix plucker = new PluckerMatrix(ngon);
                SignMatrix signMatrix = new SignMatrix(plucker);
                SignMatrix existing=null;
                try { existing = database.PluckerSignMatricesStorage.FirstOrDefault(m => m.encodedColumnVectors == signMatrix.encodedColumnVectors); }
                catch (Exception e) { }
                if (existing == null)
                {
                    database.Add(signMatrix);
                    existing = signMatrix;
                }
                ngon.PluckerSignMatrix = existing;
                ngons.Add(ngon);
                                
            }
            database.Add(ngons);
        }
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

