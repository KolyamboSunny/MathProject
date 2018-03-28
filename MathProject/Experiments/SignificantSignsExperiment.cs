using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using MathProject.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Experiments
{
    public class SignificantSignsExperiment
    {
        NgonDatabase database = new NgonDatabase();
        static Dictionary<SignMatrix, bool> pluckerMatrices = new Dictionary<SignMatrix, bool>();
        int dimensions;
        public SignificantSignsExperiment(int dimensions)
        {
            this.database = new NgonDatabase(dimensions);
            this.dimensions = dimensions;
        }

        public void findSignificantSigns()
        {
            StreamWriter file =new StreamWriter( new FileStream("significant" + dimensions + "signs.txt", FileMode.Create));
            foreach (SignMatrix matrix in database.PluckerSignMatrices)
            {
                pluckerMatrices.Add(matrix, matrix.Ngons.Count(n => n.Type == NgonType.Convex) != 0);
            }
            List<Matrix<double>> goodMasks = new List<Matrix<double>>();
            List<Matrix<double>> masks = createMasks();
            List<Matrix<double>> result;
            int i = 2;
            while (true)
            {
                result = checkMasks(masks);
           
                goodMasks.AddRange(result);

                if (result.Count == 0) break;
                count(result);
                foreach (var mask in result)
                {
                    Console.WriteLine(mask);
                    file.WriteLine(mask);
                    file.Flush();
                }                                

                
                masks = createMasks(result, i);
                i++;
            }

            Console.WriteLine("Final result:");
            //file.WriteLine("Final result:");
            foreach (var mask in goodMasks)
            {
                Console.WriteLine(mask);
                //file.WriteLine(mask);
            }
            
        } 
        private void count(List<Matrix<double>> result)
        {            
            for (int i = 1; i < 6; i++)
            {
                double s = result[0].ColumnSums().Sum();
                Console.WriteLine(i + ": " + result.Count(m => m.ColumnSums().Sum() == dimensions*dimensions-i));                
            }
            Console.WriteLine();           
        }

        private List<Matrix<double>> checkMasks(List<Matrix<double>> masks)
        {
            List<Matrix<double>> goodMasks = new List<Matrix<double>>();
            foreach (var mask in masks)
            {
                bool ambiguity = false;
                foreach (var matrix in pluckerMatrices.Where(p=>p.Value==true))
                {
                    if (ambiguity) break;
                    var matrix1 = matrix.Key;
                    bool matrix1Convex = matrix.Value;
                    if (matrix1Convex)
                    {
                        //var similarMatricies = pluckerMatrices.Keys.Where(m2 => matricesEqual(matrix1, m2, mask.AsColumnArrays()));
                        if (pluckerMatrices.Keys.Any(m2 => !pluckerMatrices[m2] && matrix1.EqualsWithMask(m2, mask)))
                            ambiguity = true;
                    }
                }
                if (!ambiguity)
                {
                    goodMasks.Add(mask);                    
                }
            }
            return goodMasks;
        }

        private List<Matrix<double>> createMasks(List<Matrix<double>> masks)
        {
            List<Matrix<double>> newMasks = new List<Matrix<double>>();
            int dimensions = pluckerMatrices.Keys.First().columnVectors[0].Length;
            foreach (Matrix<double> mask in masks)
                for (int ni = 1; ni < dimensions; ni++)
                {
                    for (int nj = 0; nj < ni; nj++)
                    {
                        var newMask = Matrix<double>.Build.DenseOfMatrix(mask);
                        newMask[nj,ni] = 0;
                        if(!newMasks.Contains(newMask))
                            newMasks.Add(newMask);
                    }
                }
            return newMasks;
        }
        private List<Matrix<double>> createMasks(List<Matrix<double>> masks,int numberOfZeros)
        {
            List<Matrix<double>> newMasks = createMasks(masks);
            return newMasks.Where(m => m.ColumnSums().Sum()== dimensions * dimensions-numberOfZeros).ToList();
        }
        private List<Matrix<double>> createMasks()
        {
            int dimensions = pluckerMatrices.Keys.First().columnVectors[0].Length;
            Matrix<double> mask = Matrix<double>.Build.Dense(dimensions, dimensions, 1);
            return  createMasks(new List<Matrix<double>>() { mask });
        }

        private string printMask(int[][] mask)
        {
            string result = "";
            for (int i = 0; i < mask[0].Count(); i++)
            {
                result += "| ";
                for (int j = 0; j < mask.Count(); j++)
                {
                    if (j < i)
                    {
                        result += "  ";
                        continue;
                    }
                    if (mask[j][i] == -1) result += "- ";
                    if (mask[j][i] == 1) result += "+ ";
                    if (mask[j][i] == 0) result += "0 ";
                }
                result += "|\n";
            }
            return result;
        }
    }
}
