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
    public abstract class Matrix
    {       
        public double[][] columnVectors;
        public Matrix(Ngon ngon) { }
        public Matrix(double[][] vectors) { }
    }

    public class SignMatrix : IPrintableMatrix
    {
        public long SignMatrixId { get; set; }

        public int dimensions { get; set; }
        public string encodedColumnVectors { get; set; }
        public int[][] columnVectors { get { return decodeArray(encodedColumnVectors); } }

        public virtual List<Ngon> Ngons { get; set; }
        public ReducedSignMatrix getReduced() {            
                return (new ReducedSignMatrix(this));           
        }
        public ReducedSignMatrix2 getReduced2(Matrix<double> mask)
        {
                return (new ReducedSignMatrix2(this,mask));           
        }

        private int[][] decodeArray(string encoded)
        {
            string[] oneDimentional = encoded.Split(';');
            int[][] result = new int[oneDimentional.Length - 1][];
            for (int i = 0; i < oneDimentional.Length - 1; i++)
            {
                result[i] = Array.ConvertAll(oneDimentional[i].Split(':'), Int32.Parse);
            }
            return result;
        }
        private string encodeArray(int[][] array)
        {
            string result = "";
            foreach (int[] subArray in array)
            {
                for (int i = 0; i < subArray.Length - 1; i++)
                {
                    result += subArray[i] + ":";
                }
                result += subArray[subArray.Length - 1] + ";";
            }
            return result;
        }

        public SignMatrix(Matrix D)
        {
            this.dimensions = D.columnVectors.Count();
            int[][] columnVectors = new int[D.columnVectors.Count()][];
            for (int i = 0; i < D.columnVectors.Count(); i++)
            {
                columnVectors[i] = D.columnVectors[i].Select(n => Math.Sign(n)).ToArray();
            }
            this.encodedColumnVectors = encodeArray(columnVectors);
            this.Ngons = new List<Ngon>();            
        }

        public SignMatrix(Matrix D, Matrix<double> mask)
        {
            this.dimensions = D.columnVectors.Count();
            int[][] columnVectors = new int[D.columnVectors.Count()][];
            for (int i = 0; i < D.columnVectors.Count(); i++)
            {
                columnVectors[i] = D.columnVectors[i].Select(n => Math.Sign(n)).ToArray();
            }
            this.encodedColumnVectors = encodeArray(columnVectors);
            this.Ngons = new List<Ngon>();          
        }
        public SignMatrix() { }

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
                    if (columnVectors[j][i] == sign) result++;
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
        public virtual string ToHtml()
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
            for (int i = 0; i < this.columnVectors.Count(); i++)
            {
                if (!this.columnVectors[i].SequenceEqual(((SignMatrix)obj).columnVectors[i])) return false;
            }
            return true;
        }

        public bool EqualsWithMask(SignMatrix other,Matrix<double> mask)
        {
            int dimensions = this.columnVectors[0].Length;
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = i; j < dimensions; j++)
                {
                    if (this.columnVectors[i][j] * mask[i, j] != other.columnVectors[i][j] * mask[i, j])
                        return false;
                }
            }
            return true;
        }

        public bool IncludedInMask(Matrix<double> mask)
        {
            int dimensions = this.columnVectors[0].Length;
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = i; j < dimensions; j++)
                {
                    int fromMatrix = this.columnVectors[j][i];
                    double fromMask = mask[i, j];
                    if ( fromMask!=0 && fromMask!=fromMatrix)
                        return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }        
    }

    public class PluckerMatrix:Matrix
    {
       
        public PluckerMatrix(Ngon ngon):this(ngon.getOrthonormal()){}
        public PluckerMatrix(double[][] vectors):base(vectors)
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
        
    }
    public class ReducedSignMatrix2 : IComparable, IPrintableMatrix
    {
        public int[][] data;
       
        public ReducedSignMatrix2(SignMatrix matrix,Matrix<double> mask)
        {

            int length = matrix.columnVectors.Count();
            this.data = new int[length][];
            for (int i = 0; i < length; i++)
            {
                int[] column = new int[length];
                for (int j = 0; j < i; j++)
                {
                    if (mask[i, j] == 1)
                        column[j] = Math.Sign(matrix.columnVectors[i][j]);
                    else
                        column[j] = 0;                    
                }
                this.data[i] = column;
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < data[0].Count(); i++)
            {
                result += "| ";
                for (int j = 0; j < data.Count(); j++)
                {
                    if (j < i)
                    {
                        result += "  ";
                        continue;
                    }
                    if (data[j][i] == -1) result += "- ";
                    if (data[j][i] == 1) result += "+ ";
                    if (data[j][i] == 0) result += "0 ";
                }
                result += "|\n";
            }
            return result;
        }
        public virtual string ToHtml()
        {
            string result = "<table style = \"border: 1px solid black; \">\n";
            for (int i = 0; i < data[0].Count(); i++)
            {
                result += "<tr>";
                for (int j = 0; j < data.Count(); j++)
                {
                    result += "<td>";
                    if (j < i)
                    {
                        result += " ";
                        continue;
                    }
                    if (data[j][i] == -1) result += "-";
                    if (data[j][i] == 1) result += "+";
                    if (data[j][i] == 0) result += "0";
                    result += "</td>";
                }
                result += "</tr>\n";
            }
            result += "</table>";
            return result;
        }


        public override bool Equals(object obj)
        {
            for (int i = 0; i < this.data.Count(); i++)
            {
                if (!this.data[i].SequenceEqual(((ReducedSignMatrix2)obj).data[i])) return false;
            }
            return true;
        }
        public int CompareTo(object obj)
        {
            ReducedSignMatrix2 other = (ReducedSignMatrix2)obj;
            if (this.data.Length > other.data.Length) return 1;
            if (this.data.Length < other.data.Length) return -1;
            for (int i = 0; i < this.data.Length; i++)
            {
                for (int j = 0; j < this.data.Length; j++)
                {
                    if (this.data[i][j] == other.data[i][j]) continue;
                    if (this.data[i][j] > other.data[i][j]) return -1;
                    if (this.data[i][j] < other.data[i][j]) return 1;
                }
            }
            return 0;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public class ReducedSignMatrix: IComparable, IPrintableMatrix
    {
        public int[] data;

        public ReducedSignMatrix(SignMatrix matrix)
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
                
            }
            result += "|";
            return result;
        }
        public string ToHtml()
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
            if (!this.data.SequenceEqual(((ReducedSignMatrix)obj).data)) return false;            
            return true;
        }
        public int CompareTo(object obj)
        {
            ReducedSignMatrix other = (ReducedSignMatrix)obj;
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

    public class ProjectionMatrix:Matrix
    {
        public ProjectionMatrix(Ngon ngon) : this(ngon.getOrthonormal()) { }
        public ProjectionMatrix(double[][] vectors):base(vectors)
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
        //public ProjectionMatrix() { }
    }
}
