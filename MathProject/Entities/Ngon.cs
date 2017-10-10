using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;

namespace MathProject.Entities
{
    public class Ngon
    {
        public List<Vertex> Verticies = new List<Vertex>();
        public Dictionary<Vertex, double> relatedAngles = new Dictionary<Vertex, double>();
        public List<Edge> Edges = new List<Edge>();

        public double AngleSum
        {
            get
            {
                double result = relatedAngles.Values.Sum();
                return result;
            }
        }

        public Ngon(double[][] edgeVectors)
        {

            double[] cumulative = new double[] { 0, 0 };
            foreach (double[] entry in edgeVectors)
            {
                Vertex from = new Vertex(cumulative[0], cumulative[1]);
                Verticies.Add(from);
                cumulative[0] += entry[0];
                cumulative[1] += entry[1];
                Vertex to = new Vertex(cumulative[0], cumulative[1]);
                Edges.Add(new Edge(to, from,entry));
            }
            if (Math.Round(cumulative[0],8) != 0 && Math.Round(cumulative[1], 8) != 0) throw (new Exception("Ngon is not closed!"));
            calculateAngles();
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
        public double angleBetweenEdgesRadians(Edge a,Edge b) //in radians
        {
            Vector<double> av = Vector<double>.Build.DenseOfArray(a.vector);
            Vector<double> bv = Vector<double>.Build.DenseOfArray(b.vector);

            double dotProduct=av.DotProduct(bv);
            double magnitudeA = Math.Sqrt(Math.Pow(av[0],2)+ Math.Pow(av[1], 2));
            double magnitudeB = Math.Sqrt(Math.Pow(bv[0], 2) + Math.Pow(bv[1], 2));

            double result = Math.PI-Math.Acos(dotProduct / (magnitudeA * magnitudeB));
            return result;
        }
        public double angleBetweenEdgesDegrees(Edge a, Edge b) //in degrees
        {

            double result = angleBetweenEdgesRadians(a,b)*180/Math.PI;
            return result;
        }

        public NgonType getType()
        {
            double convexSum = 180 * (Verticies.Count - 2);
            if (Math.Round(AngleSum, 10) == Math.Round(convexSum, 10)) return NgonType.Convex;
            else
                return NgonType.Unknown;
        }

        public NgonType Type
        {
            get
            {
                return getType();
            }
        }
        
    }
    public enum NgonType { Convex, Reflex, Self_Inserting,Unknown};
    public class Vertex
    {
        public double coordX, coordY;
        public Vertex(double coordX,double coordY)
        {
            this.coordX = coordX;
            this.coordY = coordY;
        }
        public Vertex(double[] coords)
        {
            this.coordX = coords[0];
            this.coordY = coords[1];
        }
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
        public Vertex vertex1, vertex2;
        public double length;
        public double[] vector;

        #region Ctors
        public Edge(Vertex vertex1,Vertex vertex2,double length)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.length = length;
        }
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            this.length = Math.Sqrt( Math.Pow(vertex2.coordX- vertex1.coordX, 2) + Math.Pow(vertex2.coordY- vertex1.coordY, 2));
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
        public Edge(Vertex vertex1, Vertex vertex2, double[] vector)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.vector = vector;
            this.length = Math.Sqrt(Math.Pow(vector[0],2)+Math.Pow(vector[1],2));
        }
        public Edge(Vertex vertex1, double[] vector) : this(vertex1, new Vertex(vertex1.coordX + vector[0], vertex1.coordY + vector[1]), vector)
        { }
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
