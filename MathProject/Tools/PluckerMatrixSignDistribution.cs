using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MathProject.Tools
{
    public class PluckerSignMatrixDistribution
    {

        Dictionary<PluckerSignMatrix, List<Ngon>> similarMatrix = new Dictionary<PluckerSignMatrix, List<Ngon>>();

        public PluckerSignMatrixDistribution(long sampleSize, int dimensions)
        {
            for(long i=sampleSize;i>0;i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                PluckerSignMatrix signMatrix = new PluckerSignMatrix(ngon);
                PluckerSignMatrix key = similarMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));
                if (key == null)
                {
                    similarMatrix.Add(signMatrix, new List<Ngon>());
                    key = signMatrix;
                }
                similarMatrix[key].Add(ngon);
            }
            
            
        }
        
        public void saveToHtml(string filename)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

                string title = "Sign Matrix experiment";
                writer.WriteLine("<title>" + title +"</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" +"}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>"+ "<th>Self Intersecting</th>"+"</tr>");

                foreach (PluckerSignMatrix matrix in similarMatrix.Keys)
                        writer.WriteLine(htmlTableRow(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");            
            writer.Close();
        }
        private string htmlTableRow(PluckerSignMatrix matrix)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();      
            result += "</td>";

            List<NgonEdgePermutations> permutations = new List<NgonEdgePermutations>();

            foreach (Ngon ngon in similarMatrix[matrix])
                permutations.Add(new NgonEdgePermutations(ngon));

            result += "<td style=\"border: 1px solid black;\">";
                foreach(NgonEdgePermutations permutation in permutations)
                {
                    result += "<p>" + permutation.convex+"</p>";
                }
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            foreach (NgonEdgePermutations permutation in permutations)
            {
                result += "<p>" + permutation.reflex + "</p>";
            }
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            foreach (NgonEdgePermutations permutation in permutations)
            {
                result += "<p>" + permutation.self_intersecting + "</p>";
            }
            result += "</td>";

            result += "</tr>\n";
            return result;
        }
    }
}
