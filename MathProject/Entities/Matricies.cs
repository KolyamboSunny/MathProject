using MathNet.Numerics.LinearAlgebra;
using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Tools
{
    public interface IPrintableMatrix
    {
        string ToHtml();
    }

    public abstract class SignMatrix:IPrintableMatrix
    {
        public SignMatrix(Ngon ngon) { }

        public abstract string ToHtml();
    }

    public class PluckerMatrix
    {
        public double[][] columnVectors;
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

    public class PluckerSignMatrix: SignMatrix
    {
        public int[][] columnVectors;
        public Ngon ngon;
        public ReducedPluckerSignMatrix Reduced;

        public PluckerSignMatrix(Ngon ngon):base(ngon)
        {
            this.ngon = ngon;
            PluckerMatrix D = new PluckerMatrix(ngon);

            this.columnVectors= new int[D.columnVectors.Count()][];
            for (int i = 0; i < D.columnVectors.Count(); i++)
            {
                this.columnVectors[i] = D.columnVectors[i].Select(n => Math.Sign(n)).ToArray();
            }
            this.Reduced = new ReducedPluckerSignMatrix(this);
        }       
       
        public int countPositive()
        { return count(1); }
        public int countNegative()
        { return count(-1); }
        private int count(int sign)
        {
            int result = 0;
            for (int i = 0; i < columnVectors[0].Count(); i++)
            {
                for (int j = 0; j < columnVectors.Count(); j++)
                {
                    if (j < i)
                    {                        
                        continue;
                    }                    
                    if (columnVectors[j][i] == sign) result ++;                    
                }                
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < columnVectors[0].Count(); i++)
            {
                result += "| ";
                for (int j = 0; j < columnVectors.Count(); j++)
                {
                    if (j < i)
                    {
                        result += "  ";
                        continue;
                    }
                    if (columnVectors[j][i] == -1) result += "- ";
                    if (columnVectors[j][i] == 1) result += "+ ";
                    if (columnVectors[j][i] == 0) result += "0 ";
                }
                result += "|\n";
            }
            return result;
        }
        public override string ToHtml()
        {
            string result = "<table style = \"border: 1px solid black; \">\n";
            for (int i = 0; i < columnVectors[0].Count(); i++)
            {
                result += "<tr>";
                for (int j = 0; j < columnVectors.Count(); j++)
                {
                    result += "<td>";
                    if (j < i)
                    {
                        result += " ";
                        continue;
                    }
                    if (columnVectors[j][i] == -1) result += "-";
                    if (columnVectors[j][i] == 1) result += "+";
                    if (columnVectors[j][i] == 0) result += "0";
                    result += "</td>";
                }
                result += "</tr>\n";
            }
            result += "</table>";
            return result;
        }
        
        public override bool Equals(object obj)
        {
            for(int i=0;i<this.columnVectors.Count();i++)
            {
                if (!this.columnVectors[i].SequenceEqual(((PluckerSignMatrix)obj).columnVectors[i])) return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class ReducedPluckerSignMatrix: SignMatrix,IComparable
    {
        public int[] data;

        public ReducedPluckerSignMatrix(PluckerSignMatrix matrix):base(matrix.ngon)
        {
            
            int length = matrix.columnVectors.Count()-1;
            this.data = new int[length+1];
            for (int i = 0; i < length; i++)
            {
                this.data[i] = Math.Sign(matrix.columnVectors[i + 1][i]);
            }
            this.data[length] = Math.Sign(matrix.columnVectors[length][0]);

        }

        public int countPositive()
        { return count(1); }
        public int countNegative()
        { return count(-1); }
        private int count(int sign)
        {
            return data.Count(n => n == sign);
        }

        public override string ToString()
        {
            string result = "";
            result += "| ";
            for (int j = 0; j < data.Count(); j++)
            {
                if (data[j] == -1) result += "- ";
                if (data[j] == 1) result += "+ ";
                if (data[j] == 0) result += "0 ";                
                result += "|\n";
            }
            return result;
        }
        public override string ToHtml()
        {
            string result = "<table style = \"border: 1px solid black; \">\n";
            result += "<tr>";
            for (int j = 0; j < data.Count(); j++)
            {
                result += "<td>";
                
                if (data[j] == -1) result += "-";
                if (data[j] == 1) result += "+";
                if (data[j] == 0) result += "0";
                    result += "</td>";
            }
            result += "</tr>\n";

            result += "</table>";
            return result;
        }

        public override bool Equals(object obj)
        {
            if (!this.data.SequenceEqual(((ReducedPluckerSignMatrix)obj).data)) return false;            
            return true;
        }
        public int CompareTo(object obj)
        {
            ReducedPluckerSignMatrix other = (ReducedPluckerSignMatrix)obj;
            if (this.data.Length > other.data.Length) return 1;
            if (this.data.Length < other.data.Length) return -1;
            for (int i=0;i< this.data.Length; i++ )
            {
                if (this.data[i] == other.data[i]) continue;
                if (this.data[i] > other.data[i]) return -1;
                if (this.data[i] < other.data[i]) return 1;
            }
            return 0;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class ProjectionMatrix
    {
        public double[][] columnVectors;
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
