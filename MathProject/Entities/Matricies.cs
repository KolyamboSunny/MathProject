using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Tools
{
    public abstract class Matrix
    {
        public double[][] columnVectors;
        public Matrix() { }
        public int[][] signChambers(double[][] matrix)
        {
            int[][] result = new int[matrix.Count()][];
            for (int i = 0; i < matrix.Count(); i++)
            {

                result[i] = matrix[i].Select(n => Math.Sign(n)).ToArray();
            }

            return result;
        }
        public int[][] signChambers() { return signChambers(columnVectors); }



        public override bool Equals(object obj)
        {
            try
            {
                Matrix other = (Matrix)obj;
                Matrix<double> m1 = Matrix<double>.Build.DenseOfColumnArrays(this.columnVectors);
                Matrix<double> m2 = Matrix<double>.Build.DenseOfColumnArrays(other.columnVectors);
                return m1.Equals(m2);
            }
            catch (Exception e) { }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class PluckerMatrix:Matrix
    {
        public PluckerMatrix(Ngon ngon):this(ngon.getOrthonormal()){}
        public PluckerMatrix(double[][] vectors)
        {
            int length = vectors[0].Count();
            this.columnVectors = new double[length][];
           

            for (int j = 0; j < length; j++)
            {
                double[] column = new double[length];

                double aj = vectors[0][j], bj = vectors[1][j];

                for (int i = 0; i < length; i++)
                {
                    double ai = vectors[0][i], bi = vectors[1][i];
                    column[i] = ai * bj - aj * bi;
                }
                columnVectors[j] = column;
            }
        }
        public PluckerMatrix() { }
    }

    public class ProjectionMatrix:Matrix
    {
        public ProjectionMatrix(Ngon ngon) : this(ngon.getOrthonormal()) { }
        public ProjectionMatrix(double[][] vectors)
        {

            int length = vectors[0].Count();
            this.columnVectors = new double[length][];

            for (int j = 0; j < length; j++)
            {
                double[] column = new double[length];

                double aj = vectors[0][j], bj = vectors[1][j];

                for (int i = 0; i < length; i++)
                {
                    double ai = vectors[0][i], bi = vectors[1][i];
                    column[i] = ai * aj + bj * bi;
                }
                columnVectors[j] = column;
            }
        }
        public ProjectionMatrix() { }
    }
}
