using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Statistics.Distributions.Univariate;

namespace MathProject_Triangles
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many triangles to generate?");
            long sampleSize = Int64.Parse(Console.ReadLine());
            long areObtuse=0;
            for(long i = sampleSize;i>0;i--)
            {
                Console.WriteLine(i + " left");
                Triangle t = generateRandomTriangle();
                if (t.isObtuse) areObtuse++;
            }
            double percentObtuse = (double) areObtuse / sampleSize *100;
            Console.WriteLine(percentObtuse + " were obtuse");
            Console.Read();
        }

        static Triangle generateRandomTriangle()
        {
            var point = generatePoint();

            double radius = Math.Sqrt(Math.Pow(point[0], 2) + Math.Pow(point[1], 2) + Math.Pow(point[2], 2));
            double xp = point[0] / radius;
            double yp = point[1] / radius;
            double zp = point[2] / radius;

            double a = 1 - Math.Pow(xp, 2);
            double b = 1 - Math.Pow(yp, 2);
            double c = 1 - Math.Pow(zp, 2);

            return new Triangle(a, b, c);
            //Console.WriteLine("Generated triangle");
        }
        public static double[] generatePoint()
        {
            NormalDistribution randomizer = new NormalDistribution(0, 1);
            double[] point=new double[3];
            point[0] = randomizer.Generate();  //x
            point[1] = randomizer.Generate();  //y
            point[2] = randomizer.Generate();  //z
            return point;
        }
            
        
    }
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
            if((a+b<c)|| (a + c < b)|| (c +b  < a))
            {
                throw (new Exception("Is not a triangle=("));
            }

            angleAB = calculateAngle(c, a, b);
            angleBC = calculateAngle(a, c, b);
            angleCA = calculateAngle(b, a, c);

            //Console.WriteLine("Created triangle");
        }

        private double calculateAngle(double oppositeSide,double adjacentSide1, double adjacentSide2)
        {
            double cos = (Math.Pow(adjacentSide1, 2) + Math.Pow(adjacentSide2, 2) - Math.Pow(oppositeSide, 2)) / (2 * adjacentSide1 * adjacentSide2);
            double radians = Math.Acos(cos);
            double degrees = radians*180 / Math.PI;
            return degrees;
        }

        public override string ToString()
        {
            string result = "Triangle:\n";
            result += "side A:\t" + a +"\n";
            result += "side B:\t" + b + "\n";
            result += "side C:\t" + c + "\n";
            return result;
        }
    }
}
