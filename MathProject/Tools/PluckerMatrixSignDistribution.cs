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
        public Dictionary<PluckerSignMatrix, List<Ngon>> similarFullMatrix = new Dictionary<PluckerSignMatrix, List<Ngon>>();
        public Dictionary<ReducedPluckerSignMatrix, List<Ngon>> similarReducedMatrix = new Dictionary<ReducedPluckerSignMatrix, List<Ngon>>();

        public PluckerSignMatrixDistribution(long sampleSize, int dimensions)
        {
            for(long i=sampleSize;i>0;i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                PluckerSignMatrix signMatrix = new PluckerSignMatrix(ngon);

                PluckerSignMatrix fullKey = similarFullMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));                
                if (fullKey == null)
                {
                    similarFullMatrix.Add(signMatrix, new List<Ngon>());
                    fullKey = signMatrix;
                }
                similarFullMatrix[fullKey].Add(ngon);

                ReducedPluckerSignMatrix reducedKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix.Reduced));
                if (reducedKey == null)
                {
                    similarReducedMatrix.Add(signMatrix.Reduced, new List<Ngon>());
                    reducedKey = signMatrix.Reduced;
                }
                similarReducedMatrix[reducedKey].Add(ngon);
            }
            
            
        }

        #region PrintingFullMatrix
        public void saveFullToHtml(string filename)
        {
            saveFullToHtml(filename,similarFullMatrix.OrderBy(n => n.Key.Reduced));
        }
        public void saveFullToHtml(string filename,NgonType neededType)
        {
            var typed = similarFullMatrix.Where(p => p.Value.Count(n => n.Type != neededType)==0);
            var ordered=typed.OrderBy(n => n.Key.Reduced);

            saveFullToHtml(filename, ordered);
        }
        private void saveFullToHtml(string filename,IOrderedEnumerable<KeyValuePair<PluckerSignMatrix,List<Ngon>>> matricies)
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
                writer.WriteLine(htmlTableRowDistribution(matrix.Key,matrix.Value));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(IPrintableMatrix matrix,IEnumerable<Ngon> relatedNgons)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";                         

            result += "<td style=\"border: 1px solid black;\">";            
                result += "<p>" + relatedNgons.Count(n=>n.Type==NgonType.Convex) + "</p>";                 
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + relatedNgons.Count(n => n.Type == NgonType.Reflex) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + relatedNgons.Count(n => n.Type == NgonType.Self_Intersecting) + "</p>";
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + relatedNgons.Count() + "</p>";
            result += "</td>";

            result += "</tr>\n";
            return result;
        }
        #endregion
        #region PrintingReducedMatrix
        public void saveReducedToHtml(string filename)
        {
            saveReducedToHtml(filename, similarReducedMatrix.OrderBy(n=> n.Key));
        }
        public void saveReducedToHtml(string filename, NgonType neededType)
        {
            var typed = similarReducedMatrix.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key);

            saveReducedToHtml(filename, ordered);
        }
        private void saveReducedToHtml(string filename, IOrderedEnumerable<KeyValuePair<ReducedPluckerSignMatrix, List<Ngon>>> matricies)
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
                writer.WriteLine(htmlTableRowDistribution(matrix.Key, matrix.Value));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }        
        #endregion
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

                foreach (PluckerSignMatrix matrix in similarFullMatrix.Keys)
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

            foreach (Ngon ngon in similarFullMatrix[matrix])
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
            foreach(PluckerSignMatrix signMatrix in similarFullMatrix.Keys)
            {
                if(signMatrix.countPositive()%2==0)
                {
                    int si = similarFullMatrix[signMatrix].Count(n => n.Type == NgonType.Self_Intersecting);
                    if (si > 0) numFalse++;
                }
            }
            return numFalse == 0;
        }
    }    
}
