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

        public Dictionary<PluckerSignMatrix, List<Ngon>> similarMatrix = new Dictionary<PluckerSignMatrix, List<Ngon>>();

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
        public void saveToHtmlDistribution(string filename)
        { saveToHtmlDistribution(filename, similarMatrix); }
        private void saveToHtmlDistribution(string filename,IEnumerable<KeyValuePair<PluckerSignMatrix, List<Ngon>>> matricies)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

            string title = "Sign Matrix experiment";
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var matrix in matricies)
                writer.WriteLine(htmlTableRowDistribution(matrix.Key));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(PluckerSignMatrix matrix)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";                         

            result += "<td style=\"border: 1px solid black;\">";            
                result += "<p>" + similarMatrix[matrix].Count(n=>n.Type==NgonType.Convex) + "</p>";                 
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count(n => n.Type == NgonType.Reflex) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count(n => n.Type == NgonType.Self_Intersecting) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count() + "</p>";
            result += "</td>";

            result += "</tr>\n";
            return result;
        }

        public void showOnly(String filename, NgonType neededType)
        {
            IEnumerable<KeyValuePair<PluckerSignMatrix, List<Ngon>>> convexMatricies = similarMatrix.Where(m => m.Value.Count(n => n.Type != neededType) == 0);
            saveToHtmlDistribution(filename, convexMatricies);
        }
        public void saveToHtmlPermutations(string filename)
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
                        writer.WriteLine(htmlTableRowPermutations(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");            
            writer.Close();
        }
        private string htmlTableRowPermutations(PluckerSignMatrix matrix)
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
        public bool testEvenSignsHypothesis()
        {
            int numFalse = 0;
            foreach(PluckerSignMatrix signMatrix in similarMatrix.Keys)
            {
                if(signMatrix.countPositive()%2==0)
                {
                    int si = similarMatrix[signMatrix].Count(n => n.Type == NgonType.Self_Intersecting);
                    if (si > 0) numFalse++;
                }
            }
            return numFalse == 0;
        }
    }
    public class ReducedPluckerSignMatrixDistribution
    {

        public Dictionary<ReducedPluckerSignMatrix, List<Ngon>> similarMatrix = new Dictionary<ReducedPluckerSignMatrix, List<Ngon>>();

        public ReducedPluckerSignMatrixDistribution(long sampleSize, int dimensions)
        {
            for (long i = sampleSize; i > 0; i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                ReducedPluckerSignMatrix signMatrix = new ReducedPluckerSignMatrix(ngon);
                ReducedPluckerSignMatrix key = similarMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));
                if (key == null)
                {
                    similarMatrix.Add(signMatrix, new List<Ngon>());
                    key = signMatrix;
                }
                similarMatrix[key].Add(ngon);
            }


        }
        public void saveToHtmlDistribution(string filename)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

            string title = "Sign Matrix experiment";
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (ReducedPluckerSignMatrix matrix in similarMatrix.Keys)
                writer.WriteLine(htmlTableRowDistribution(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(ReducedPluckerSignMatrix matrix)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;\">";
            result += "<p>" + similarMatrix[matrix].Count(n => n.Type == NgonType.Convex) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count(n => n.Type == NgonType.Reflex) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count(n => n.Type == NgonType.Self_Intersecting) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + similarMatrix[matrix].Count() + "</p>";
            result += "</td>";

            result += "</tr>\n";
            return result;
        }

        public void saveToHtmlPermutations(string filename)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

            string title = "Sign Matrix experiment";
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "</tr>");

            foreach (ReducedPluckerSignMatrix matrix in similarMatrix.Keys)
                writer.WriteLine(htmlTableRowPermutations(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowPermutations(ReducedPluckerSignMatrix matrix)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";

            List<NgonEdgePermutations> permutations = new List<NgonEdgePermutations>();

            foreach (Ngon ngon in similarMatrix[matrix])
                permutations.Add(new NgonEdgePermutations(ngon));

            result += "<td style=\"border: 1px solid black;\">";
            foreach (NgonEdgePermutations permutation in permutations)
            {
                result += "<p>" + permutation.convex + "</p>";
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
        public bool testEvenSignsHypothesis()
        {
            int numFalse = 0;
            foreach (ReducedPluckerSignMatrix signMatrix in similarMatrix.Keys)
            {
                if (signMatrix.data.Count(m=>m==1) % 2 != 0)
                {
                    int convex = similarMatrix[signMatrix].Count(n => n.Type == NgonType.Convex);
                    if (convex > 0) numFalse++;
                }
            }
            return numFalse == 0;
        }
    }
}
