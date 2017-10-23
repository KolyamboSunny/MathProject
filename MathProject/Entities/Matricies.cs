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
        public PluckerMatrix(Ngon ngon):this(ngon.EdgeVectors){}
        public PluckerMatrix(double[][] vectors)
        {
            this.columnVectors = new double[vectors.Count()][];
            for (int i = 0; i < vectors.Count(); i++) columnVectors[i] = new double[vectors.Count()];

            for (int i = 0; i < vectors.Count(); i++)
            {
                double ai = vectors[i][0], bi = vectors[i][1];
                for (int j = 0; j < vectors.Count(); j++)
                {
                    double aj = vectors[j][0], bj = vectors[j][1];
                    columnVectors[j][i] = ai * bj - aj * bi;
                }
            }
        }
        public PluckerMatrix() { }
    }

    public class ProjectionMatrix:Matrix
    {
        public ProjectionMatrix(Ngon ngon) : this(ngon.EdgeVectors) { }
        public ProjectionMatrix(double[][] vectors)
        {
            columnVectors = new double[vectors.Count()][];
            for (int i = 0; i < vectors.Count(); i++) columnVectors[i] = new double[vectors.Count()];

            for (int i = 0; i < vectors.Count(); i++)
            {
                double ai = vectors[i][0], bi = vectors[i][1];
                for (int j = 0; j < vectors.Count(); j++)
                {
                    double aj = vectors[j][0], bj = vectors[j][1];
                    columnVectors[j][i] = ai * aj + bi*bj;
                }
            }
        }
        public ProjectionMatrix() { }
    }
}
