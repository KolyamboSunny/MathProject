using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject.Entities
{
    public class Triangle
    {
        public double a, b, c;
        public double angleAB, angleBC, angleCA;

        public bool isObtuse
        {
            get
            {
                if (angleAB > 90) return true;
                if (angleBC > 90) return true;
                if (angleCA > 90) return true;
                return false;
            }
        }

        public Triangle(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            if ((a + b < c) || (a + c < b) || (c + b < a))
            {
                throw (new Exception("Is not a triangle=("));
            }

            angleAB = calculateAngle(c, a, b);
            angleBC = calculateAngle(a, c, b);
            angleCA = calculateAngle(b, a, c);

            //Console.WriteLine("Created triangle");
        }
        public Triangle(Ngon polygon)
        {
            if (polygon.Edges.Count > 3) throw (new Exception("Can only use a polygon with 3 verticies"));
            this.a = polygon.Edges[0].length;
            this.b = polygon.Edges[1].length;
            this.c = polygon.Edges[2].length; 
            if ((a + b < c) || (a + c < b) || (c + b < a))
            {
                throw (new Exception("Is not a triangle=("));
            }

            angleAB = calculateAngle(c, a, b);
            angleBC = calculateAngle(a, c, b);
            angleCA = calculateAngle(b, a, c);

            //Console.WriteLine("Created triangle");
        }
        private double calculateAngle(double oppositeSide, double adjacentSide1, double adjacentSide2)
        {
            double cos = (Math.Pow(adjacentSide1, 2) + Math.Pow(adjacentSide2, 2) - Math.Pow(oppositeSide, 2)) / (2 * adjacentSide1 * adjacentSide2);
            double radians = Math.Acos(cos);
            double degrees = radians * 180 / Math.PI;
            return degrees;
        }

        public override string ToString()
        {
            string result = "Triangle:\n";
            result += "side A:\t" + a + "\n";
            result += "side B:\t" + b + "\n";
            result += "side C:\t" + c + "\n";
            return result;
        }
    }
}
