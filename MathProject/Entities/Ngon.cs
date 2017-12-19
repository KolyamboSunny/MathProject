using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathProject.Entities
{
    public class Ngon
    {
        public long NgonId { get; set; }
        public List<Vertex> Verticies = new List<Vertex>();
        public Dictionary<Vertex, double> relatedAngles = new Dictionary<Vertex, double>();
        public List<Edge> Edges = new List<Edge>();
        private double[][] EdgeVectors { get; set; }
        public double[][] getEdgeVectors()
        {
            double[][] result = new double[this.EdgeVectors.Length][];
            Array.Copy(this.EdgeVectors, result, this.EdgeVectors.Length);
            return result;
        }
        private double[][] Orthonormal;//fix name, does not sound correct
        public double[][] getOrthonormal()
        {
            double[][] result = new double[this.Orthonormal.Length][];
            Array.Copy(this.Orthonormal, result, this.Orthonormal.Length);
            return result;
        }

        public double AngleSum
        {
            get
            {
                double result = relatedAngles.Values.Sum();
                return result;
            }
        }

        public Ngon() { }
        public Ngon(double[][] edgeVectors)
        {
            this.EdgeVectors = edgeVectors;
            double[] cumulative = new double[] { 0, 0 };
            foreach (double[] entry in edgeVectors)
            {
                Vertex from = new Vertex(cumulative[0], cumulative[1]);
                Verticies.Add(from);
                cumulative[0] += entry[0];
                cumulative[1] += entry[1];
                Vertex to = new Vertex(cumulative[0], cumulative[1]);
                Edges.Add(new Edge(to, from, entry));
            }
            if (Math.Round(cumulative[0], 8) != 0 && Math.Round(cumulative[1], 8) != 0) throw (new Exception("Ngon is not closed!"));
            calculateAngles();
        }
        public Ngon(double[][] edgeVectors, double[][] orthonormal) : this(edgeVectors)
        {
            this.Orthonormal = orthonormal;
        }

        public void calculateAngles()
        {
            foreach (Vertex vertex in this.Verticies)
            {
                Edge a = this.Edges.Find(e => e.vertex2.Equals(vertex));
                Edge b = this.Edges.Find(e => e.vertex1.Equals(vertex));
                double angle = angleBetweenEdgesDegrees(a, b);

                relatedAngles[vertex] = angle;
            }
        }
        public double angleBetweenEdgesRadians(Edge a, Edge b) //in radians
        {
            Vector<double> av = Vector<double>.Build.DenseOfArray(a.vector);
            Vector<double> bv = Vector<double>.Build.DenseOfArray(b.vector);

            double dotProduct = av.DotProduct(bv);
            double magnitudeA = Math.Sqrt(Math.Pow(av[0], 2) + Math.Pow(av[1], 2));
            double magnitudeB = Math.Sqrt(Math.Pow(bv[0], 2) + Math.Pow(bv[1], 2));

            double result = Math.PI - Math.Acos(dotProduct / (magnitudeA * magnitudeB));
            return result;
        }
        public double angleBetweenEdgesDegrees(Edge a, Edge b) //in degrees
        {

            double result = angleBetweenEdgesRadians(a, b) * 180 / Math.PI;
            return result;
        }

        public bool edgesIntersect(Edge e1, Edge e2)
        {
            Vertex v1 = e1.vertex1;
            Vertex v2 = e1.vertex2;
            //build equation of form Ax+By+C=0
            double[] params1 = new double[] {
                (v1.coordY - v2.coordY),
                (v2.coordX-v1.coordX),
                (v1.coordX*v2.coordY-v2.coordX*v1.coordY)
            };

            v1 = e2.vertex1;
            v2 = e2.vertex2;
            double[] params2 = new double[] {
                (v1.coordY - v2.coordY),
                (v2.coordX-v1.coordX),
                (v1.coordX*v2.coordY-v2.coordX*v1.coordY)
            };
            double A1 = params1[0], B1 = params1[1], C1 = params1[2];
            double A2 = params2[0], B2 = params2[1], C2 = params2[2];

            double factor = (A1 * B2 - A2 * B1);
            if (factor == 0) return false;
            double? resX = -(C1 * B2 - C2 * B1) / factor;
            double? resY = -(A1 * C2 - A2 * C1) / factor;

            bool inE1 = (resX <= Math.Max(e1.vertex1.coordX, e1.vertex2.coordX) &&
                resX >= Math.Min(e1.vertex1.coordX, e1.vertex2.coordX) &&
                resY <= Math.Max(e1.vertex1.coordY, e1.vertex2.coordY) &&
                resY >= Math.Min(e1.vertex1.coordY, e1.vertex2.coordY));
            bool inE2 = (resX <= Math.Max(e2.vertex1.coordX, e2.vertex2.coordX) &&
                resX >= Math.Min(e2.vertex1.coordX, e2.vertex2.coordX) &&
                resY <= Math.Max(e2.vertex1.coordY, e2.vertex2.coordY) &&
                resY >= Math.Min(e2.vertex1.coordY, e2.vertex2.coordY));
            if (inE1 && inE2)
                return true;
            else return false;
        }

        public NgonType getType()
        {
            double convexSum = 180 * (Verticies.Count - 2);
            if (Math.Round(AngleSum, 10) == Math.Round(convexSum, 10)) return NgonType.Convex;

            for (int i = 0; i < Edges.Count; i++)
            {
                for (int j = i + 1; j < Edges.Count; j++)
                {
                    Edge e1 = Edges[i];
                    Edge e2 = Edges[j];
                    if (e1.vertex1.Equals(e2.vertex2) || e1.vertex1.Equals(e2.vertex2) ||
                        e1.vertex2.Equals(e2.vertex1) || e1.vertex2.Equals(e2.vertex2)) continue;
                    if (edgesIntersect(e1, e2))
                        return NgonType.Self_Intersecting;
                }
            }

            return NgonType.Reflex;

        }

        public NgonType Type
        {
            get
            {
                return getType();
            }
        }

    }
    public enum NgonType { Convex, Reflex, Self_Intersecting, Unknown };
    public class Vertex
    {
        [Key]
        public ulong VertexId { get; set; }
        public double coordX { get; set; }
        public double coordY { get; set; }

        public Vertex(double coordX, double coordY)
        {
            this.coordX = coordX;
            this.coordY = coordY;
        }
        public Vertex(double[] coords)
        {
            this.coordX = coords[0];
            this.coordY = coords[1];
        }
        public Vertex() { }
        public override bool Equals(object obj)
        {
            int precision = 10;
            try
            {
                Vertex other = (Vertex)obj;
                if (Math.Round(other.coordY, precision) == Math.Round(this.coordY, precision)
                    && Math.Round(other.coordX, precision) == Math.Round(this.coordX, precision)) return true;
            }
            catch (Exception e) { }
            return false;
        }
        public override int GetHashCode()
        {
            return Tuple.Create(coordX, coordY).GetHashCode();
        }
    }
    public class Edge
    {
        [Key]
        public long EdgeId { get; set; }
        public Vertex vertex1 { get; set; }
        public Vertex vertex2 { get; set; }
        public double length
        {
            get { return Math.Sqrt(Math.Pow(vertex2.coordX - vertex1.coordX, 2) + Math.Pow(vertex2.coordY - vertex1.coordY, 2)); }
        }

        public double[] vector;

        #region Ctors
        public Edge(Vertex vertex1, Vertex vertex2)
        {            
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
        public Edge(Vertex vertex1, Vertex vertex2, double[] vector)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vector = vector;        
        }
        public Edge(Vertex vertex1, double[] vector) : this(vertex1, new Vertex(vertex1.coordX + vector[0], vertex1.coordY + vector[1]), vector)
        { }
        public Edge() { }
        #endregion

        public override bool Equals(object obj)
        {
            try
            {
                Edge other = (Edge)obj;
                if (other.vertex1 == this.vertex1
                    && other.vertex2 == this.vertex2) return true;
            }
            catch (Exception e) { }
            return false;
        }
        public override int GetHashCode()
        {
            return Tuple.Create(vertex1, vertex2).GetHashCode();
        }
    }
}
