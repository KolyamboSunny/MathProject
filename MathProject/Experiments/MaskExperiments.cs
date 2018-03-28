using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Tools
{
    public class MaskExperiment4
    {
        NgonDatabase db;
        
        public Dictionary<ReducedSignMatrix, List<SignMatrix>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<SignMatrix>>();

        public MaskExperiment4()
        {
            int dimensions = 4;
            db = new NgonDatabase(dimensions);
            Matrix<double> mask = Matrix<double>.Build.Dense(dimensions, dimensions, 1);           
                mask[2, 0] = 0;
                mask[3, 2] = 0;            

            foreach (SignMatrix matrix in db.PluckerSignMatrices)
            {
                var reduced1 = matrix.getReduced();
                var reduced2 = matrix.getReduced2(mask);

                ReducedSignMatrix reduced1Key = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(reduced1));
                if (reduced1Key == null)
                {
                    similarReducedMatrix.Add(reduced1, new List<SignMatrix>());
                    reduced1Key = reduced1;
                }               
                similarReducedMatrix[reduced1Key].Add(matrix);
            }
        }

        public int[] predictFull(ReducedSignMatrix reduced1)
        {
            int[] result = new int[3];
            result[0] = ((reduced1.data[3] == 1) ^ (reduced1.data[4] == 1)) ? 1 : -1;
            result[1] = ((reduced1.data[1] == 1) ^ (reduced1.data[2] == 1)) ? 1 : -1;           

            if (reduced1.countPositive() % 2 == 1)
            {
                result[0] = -result[0];
                result[1] = -result[1];
            }
            return result;
        }


        public void runExperiment()
        {
            foreach (var reduced1 in similarReducedMatrix.Keys)
            {                
                foreach (var full in similarReducedMatrix[reduced1])
                {
                        Console.WriteLine(reduced1.ToString() + " >>>> "
                                          + stringSign(full.columnVectors[2][0]) + stringSign(full.columnVectors[3][1]) + " >>>> " +
                                           "   convex: " + (full.Ngons.Count(n => n.Type == NgonType.Convex) > 0));             
                }
                Console.WriteLine();
            }
        }
        private string stringSign(int sign)
        {
            switch (sign)
            {
                case 1:
                    return "+";
                case -1:
                    return "-";
                default:
                    return sign.ToString();
            }
        }

    }

    public class MaskExperiment6
    {
        NgonDatabase db;

        public Dictionary<ReducedSignMatrix, List<ReducedSignMatrix2>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<ReducedSignMatrix2>>();
        public Dictionary<ReducedSignMatrix2, List<SignMatrix>> similarReduced2Matrix = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();

        public MaskExperiment6(int dimensions = 6)
        {
            db = new NgonDatabase(dimensions);
            Matrix<double> mask = Matrix<double>.Build.Dense(dimensions, dimensions, 1);
            if (dimensions == 6)
            {
                mask[2, 0] = 0;
                mask[3, 1] = 0;
                mask[4, 2] = 0;
                mask[5, 3] = 0;

                mask[4, 0] = 0;
                mask[5, 1] = 0;
            }            

            foreach (SignMatrix matrix in db.PluckerSignMatrices)
            {
                var reduced1 = matrix.getReduced();
                var reduced2 = matrix.getReduced2(mask);

                ReducedSignMatrix reduced1Key = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(reduced1));
                if (reduced1Key == null)
                {
                    similarReducedMatrix.Add(reduced1, new List<ReducedSignMatrix2>());
                    reduced1Key = reduced1;
                }

                ReducedSignMatrix2 reduced2Key = similarReduced2Matrix.Keys.FirstOrDefault(m => m.Equals(reduced2));
                if (reduced2Key == null)
                {
                    similarReducedMatrix[reduced1Key].Add(reduced2);
                    similarReduced2Matrix.Add(reduced2, new List<SignMatrix>());
                    reduced2Key = reduced2;
                }

                similarReduced2Matrix[reduced2Key].Add(matrix);
            }

        }

        private string stringSign(int sign)
        {
            switch (sign)
            {
                case 1:
                    return "+";
                case -1:
                    return "-";
                default:
                    return sign.ToString();
            }
        }
        public void runExperiment4gons()
        {
            foreach (var reduced1 in similarReducedMatrix.Keys)
            {
                IOrderedEnumerable<ReducedSignMatrix2> sortedReduced2 = similarReducedMatrix[reduced1].OrderBy(m => similarReduced2Matrix[m].Count());
                foreach (var reduced2 in sortedReduced2)
                {
                    foreach (var full in similarReduced2Matrix[reduced2])
                    {
                        Console.WriteLine(reduced1.ToString() + " >>>> "
                                          + full.columnVectors[2][0] + full.columnVectors[3][2] + " >>>> " +
                                           "   convex: " + (full.Ngons.Count(n => n.Type == NgonType.Convex) > 0));
                    }
                }
                Console.WriteLine();
            }
        }
        public void runExperiment6gons()
        {
            foreach (var reduced1 in similarReducedMatrix.Keys)
            {
                IOrderedEnumerable<ReducedSignMatrix2> sortedReduced2 = similarReducedMatrix[reduced1].OrderBy(m => similarReduced2Matrix[m].Count());
                foreach (var reduced2 in sortedReduced2)//.Where(n => similarReduced2Matrix[n].Count == 3))
                {
                    List<Ngon> Ngons = new List<Ngon>();
                    
                    foreach (var full in similarReduced2Matrix[reduced2])
                    {
                        Ngons.AddRange(full.Ngons);
                    }
                    Console.WriteLine(reduced1.ToString() + " >>>> "
                                          + stringSign(reduced2.data[3][0]) + stringSign(reduced2.data[4][1]) + stringSign(reduced2.data[5][2]) + " >>>> " +
                                           //+full.columnVectors[3][0] + full.columnVectors[3][1] + full.columnVectors[4][1] +
                                           "  : " + similarReduced2Matrix[reduced2].Count() +
                                           "  convex: " + (Ngons.Count(n => n.Type == NgonType.Convex) > 0));
                }
                Console.WriteLine();
            }
        }
        private void runExperiment5gons()
        {
            Dictionary<int, int> reduced2variance = new Dictionary<int, int>();
            Console.WriteLine("Each reduced1 has 4 reduced2: " + similarReducedMatrix.All(m => m.Value.Count == 4));

            //Dictionary<ReducedSignMatrix2, List<SignMatrix>> onlyOneFull = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();
            //Dictionary<ReducedSignMatrix2, List<SignMatrix>> manyFull = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();

            var oneFull = similarReduced2Matrix.Where(m => m.Value.Count == 1);
            var threeFull = similarReduced2Matrix.Where(m => m.Value.Count == 3);
            var fiveFull = similarReduced2Matrix.Where(m => m.Value.Count == 5);

            //Console.WriteLine("Each reduced2 1 full -> convex only: " + oneFull.All(m=>m.Value.All(f=>f.Ngons.Count(n=>n.Type==NgonType.Convex)>0)==true));
            //Console.WriteLine("Each reduced2 3 full -> all non-convex: " + threeFull.All(m => m.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) == 0) == true));
            //Console.WriteLine("Each reduced2 5 full -> all non-convex: " + fiveFull.All(m => m.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) == 0) == true));
            foreach (var reduced1 in similarReducedMatrix.Keys)
            {
                IOrderedEnumerable<ReducedSignMatrix2> sortedReduced2 = similarReducedMatrix[reduced1].OrderBy(m => similarReduced2Matrix[m].Count());
                foreach (var reduced2 in sortedReduced2.Where(n => similarReduced2Matrix[n].Count == 3))
                {
                    foreach (var full in similarReduced2Matrix[reduced2])
                    {
                        Console.WriteLine(reduced1.ToString() + " >>>> "
                                          + reduced2.data[2][0] + reduced2.data[4][2] + " >>>> " +
                                          +full.columnVectors[3][0] + full.columnVectors[3][1] + full.columnVectors[4][1] +
                                           "   : " + similarReduced2Matrix[reduced2].Count());
                    }
                }
                Console.WriteLine();
            }


            foreach (var reduced2 in similarReduced2Matrix)
            {
                var reduced1 = similarReducedMatrix.First(m => m.Value.Contains(reduced2.Key)).Key;

                //Console.WriteLine(reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]);
                var predicted = predictConvexReduced2(reduced1);

                bool IWasRightEquationWorks = predicted[0] == reduced2.Key.data[2][0] && predicted[1] == reduced2.Key.data[4][2];
                bool isConvex = reduced2.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) > 0);
                if (IWasRightEquationWorks && !isConvex)
                    Console.WriteLine("Mistake in convex formula: " + reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]); //+ "  : " + IWasRightEquationWorks);

                foreach (var full in reduced2.Value)
                {
                    var predictedFullConvex = predictConvexFull(reduced1);

                    //var fullConvex = reduced2.Value.Where(f => f.Ngons.Count(n => n.Type == NgonType.Convex) > 0);
                    bool equationConvexFullWorks = predictedFullConvex[0] == full.columnVectors[3][0] &&
                                                   predictedFullConvex[1] == full.columnVectors[3][1] &&
                                                   predictedFullConvex[2] == full.columnVectors[4][1];

                    bool fullIsConvex = full.Ngons.Count(n => n.Type == NgonType.Convex) > 0;
                    if (equationConvexFullWorks && !fullIsConvex)
                        Console.WriteLine("Mistake in full convex formula: " + reduced1.ToString() + " >>>> "
                                                   + reduced2.Key.data[2][0] + reduced2.Key.data[4][2] + " >>>> " +
                                                   +full.columnVectors[3][0] + full.columnVectors[3][1] + full.columnVectors[4][1] +
                                                    "   : " + similarReduced2Matrix[reduced2.Key].Count()); //+ "  : " + IWasRightEquationWorks);
                }

                bool fiveIsNegatedConvex = -predicted[0] == reduced2.Key.data[2][0] && -predicted[1] == reduced2.Key.data[4][2];
                bool isFive = reduced2.Value.Count == 5;
                if (fiveIsNegatedConvex && !isFive)
                    Console.WriteLine("Mistake in 5Full formula: " + reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]);
            }
        }

        public int[] predictFiveReduced2(ReducedSignMatrix reduced1)
        {
            var result = predictConvexReduced2(reduced1);

            result[0] = -result[0];
            result[1] = -result[1];

            return result;
        }
        public int[] predictConvexReduced2(ReducedSignMatrix reduced1)
        {
            int[] result = new int[2];
            result[0] = ((reduced1.data[0] == 1) ^ (reduced1.data[1] == 1)) ? 1 : -1;
            result[1] = ((reduced1.data[2] == 1) ^ (reduced1.data[3] == 1)) ? 1 : -1;
            if (reduced1.countPositive() % 2 == 1)
            {
                result[0] = -result[0];
                result[1] = -result[1];
            }
            return result;
        }
        public int[] predictConvexFull(ReducedSignMatrix reduced1)
        {
            int[] result = new int[3];
            result[0] = ((reduced1.data[3] == 1) ^ (reduced1.data[4] == 1)) ? 1 : -1;
            result[1] = ((reduced1.data[1] == 1) ^ (reduced1.data[2] == 1)) ? 1 : -1;
            result[2] = ((reduced1.data[4] == 1) ^ (reduced1.data[0] == 1)) ? 1 : -1;

            if (reduced1.countPositive() % 2 == 1)
            {
                result[0] = -result[0];
                result[1] = -result[1];
                result[2] = -result[2];
            }
            return result;
        }       

    }

}
