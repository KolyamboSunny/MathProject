using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathProject_Triangles
{
    public class Program
    {
        static void Main(string[] args)
        {
            var point = generatePoint(100);
            Console.WriteLine("x:\t"+point[0]);
            Console.WriteLine("y:\t" + point[1]);
            Console.WriteLine("z:\t" + point[2]);

            Console.Read();
        }

        public static double[] generatePoint(double radius)
        {
            Random randomizer = new Random((int)System.DateTime.Now.Ticks);
            double[] point=new double[3];
            point[0] = randomizer.NextDouble()*radius;  //x coord
            point[1] = randomizer.NextDouble() * radius;    //y coord
            point[2] = Math.Sqrt( Math.Pow(radius, 2) - Math.Pow(point[0], 2) - Math.Pow(point[1], 2)); //sqrt(r-x^2-y^2)
            return point;
        }
    }
}
