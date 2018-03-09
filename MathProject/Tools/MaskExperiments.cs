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
}
