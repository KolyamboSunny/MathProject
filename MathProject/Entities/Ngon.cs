using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Entities
{
    public class Ngon
    {
        public List<Vertex> Verticies = new List<Vertex>();
        public List<Edge> Edges = new List<Edge>();
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
                Edges.Add(new Edge(to, from));
            }
            if (Math.Round(cumulative[0],8) != 0 && Math.Round(cumulative[1], 8) != 0) throw (new Exception("Ngon is not closed!"));
        }
    }

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
            try
            {
                Vertex other = (Vertex)obj;
                if (other.coordY == this.coordY && other.coordX == this.coordX) return true; 
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
        public Edge(Vertex vertex1,Vertex vertex2,double length)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.length = length;
        }
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            this.length = Math.Sqrt(Math.Pow(vertex1.coordX, 2) + Math.Pow(vertex1.coordY, 2) + Math.Pow(vertex2.coordX, 2) + Math.Pow(vertex2.coordY, 2));
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
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
