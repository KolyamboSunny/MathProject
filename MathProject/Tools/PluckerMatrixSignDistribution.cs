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
        public Dictionary<SignMatrix, List<Ngon>> similarFullMatrix = new Dictionary<SignMatrix, List<Ngon>>();
        public Dictionary<ReducedSignMatrix, List<Ngon>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<Ngon>>();

        public PluckerSignMatrixDistribution(long sampleSize, int dimensions)
        {
            for(long i=sampleSize;i>0;i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                PluckerMatrix plucker = new PluckerMatrix(ngon);
                SignMatrix signMatrix = new SignMatrix(plucker);

                SignMatrix fullKey = similarFullMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));                
                if (fullKey == null)
                {
                    similarFullMatrix.Add(signMatrix, new List<Ngon>());
                    fullKey = signMatrix;
                }
                similarFullMatrix[fullKey].Add(ngon);

                ReducedSignMatrix reducedKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix.Reduced));
                if (reducedKey == null)
                {
                    similarReducedMatrix.Add(signMatrix.Reduced, new List<Ngon>());
                    reducedKey = signMatrix.Reduced;
                }
                similarReducedMatrix[reducedKey].Add(ngon);
                if (i % (sampleSize / 100) == 0) Console.Write("\rCreating sign matrix table: {0}%    ", i / (sampleSize / 100));                
                                    
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
        private void saveFullToHtml(string filename,IOrderedEnumerable<KeyValuePair<SignMatrix,List<Ngon>>> matricies)
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
            writer.WriteLine("<h4>Total matricies: "+matricies.Count()+"</h4>");
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
        private void saveReducedToHtml(string filename, IOrderedEnumerable<KeyValuePair<ReducedSignMatrix, List<Ngon>>> matricies)
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

                foreach (SignMatrix matrix in similarFullMatrix.Keys)
                        writer.WriteLine(htmlTableRowPermutations(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");            
            writer.Close();
        }
        private string htmlTableRowPermutations(SignMatrix matrix)
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
            foreach(SignMatrix signMatrix in similarFullMatrix.Keys)
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
    public class ProjectionSignMatrixDistribution
    {
        public Dictionary<SignMatrix, List<Ngon>> similarFullMatrix = new Dictionary<SignMatrix, List<Ngon>>();
        public Dictionary<ReducedSignMatrix, List<Ngon>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<Ngon>>();

        public ProjectionSignMatrixDistribution(long sampleSize, int dimensions)
        {
            for (long i = sampleSize; i > 0; i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                ProjectionMatrix projetion = new ProjectionMatrix(ngon);
                SignMatrix signMatrix = new SignMatrix(projetion);

                SignMatrix fullKey = similarFullMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));
                if (fullKey == null)
                {
                    similarFullMatrix.Add(signMatrix, new List<Ngon>());
                    fullKey = signMatrix;
                }
                similarFullMatrix[fullKey].Add(ngon);

                ReducedSignMatrix reducedKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix.Reduced));
                if (reducedKey == null)
                {
                    similarReducedMatrix.Add(signMatrix.Reduced, new List<Ngon>());
                    reducedKey = signMatrix.Reduced;
                }
                similarReducedMatrix[reducedKey].Add(ngon);
                if (i % (sampleSize / 100) == 0) Console.Write("\rCreating sign matrix table: {0}%    ", i / (sampleSize / 100));

            }


        }

        #region PrintingFullMatrix
        public void saveFullToHtml(string filename)
        {
            saveFullToHtml(filename, similarFullMatrix.OrderBy(n => n.Key.Reduced));
        }
        public void saveFullToHtml(string filename, NgonType neededType)
        {
            var typed = similarFullMatrix.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key.Reduced);

            saveFullToHtml(filename, ordered);
        }
        private void saveFullToHtml(string filename, IOrderedEnumerable<KeyValuePair<SignMatrix, List<Ngon>>> matricies)
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
            writer.WriteLine("<h4>Total matricies: " + matricies.Count() + "</h4>");
            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Projection matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var matrix in matricies)
                writer.WriteLine(htmlTableRowDistribution(matrix.Key, matrix.Value));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(IPrintableMatrix matrix, IEnumerable<Ngon> relatedNgons)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;\">";
            result += "<p>" + relatedNgons.Count(n => n.Type == NgonType.Convex) + "</p>";
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
            saveReducedToHtml(filename, similarReducedMatrix.OrderBy(n => n.Key));
        }
        public void saveReducedToHtml(string filename, NgonType neededType)
        {
            var typed = similarReducedMatrix.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key);

            saveReducedToHtml(filename, ordered);
        }
        private void saveReducedToHtml(string filename, IOrderedEnumerable<KeyValuePair<ReducedSignMatrix, List<Ngon>>> matricies)
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
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "</tr>");

            foreach (SignMatrix matrix in similarFullMatrix.Keys)
                writer.WriteLine(htmlTableRowPermutations(matrix));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowPermutations(SignMatrix matrix)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";

            List<NgonEdgePermutations> permutations = new List<NgonEdgePermutations>();

            foreach (Ngon ngon in similarFullMatrix[matrix])
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
            foreach (SignMatrix signMatrix in similarFullMatrix.Keys)
            {
                if (signMatrix.countPositive() % 2 == 0)
                {
                    int si = similarFullMatrix[signMatrix].Count(n => n.Type == NgonType.Self_Intersecting);
                    if (si > 0) numFalse++;
                }
            }
            return numFalse == 0;
        }
    }
    public class PluckerAndProjectionMatricesDistribution
    {
        public Dictionary<SignMatrix, List<Ngon>> similarPluckerFullMatrix = new Dictionary<SignMatrix, List<Ngon>>();
        public Dictionary<SignMatrix, List<Ngon>> similarProjectionFullMatrix = new Dictionary<SignMatrix, List<Ngon>>();
        public Dictionary<ReducedSignMatrix, List<Ngon>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<Ngon>>();
        public Dictionary<SignMatrix, Dictionary<SignMatrix, int>> matrixMap = new Dictionary<SignMatrix, Dictionary<SignMatrix, int>>();

        public PluckerAndProjectionMatricesDistribution(long sampleSize, int dimensions)
        {
            for (long i = sampleSize; i > 0; i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);

                #region Plucker
                PluckerMatrix plucker = new PluckerMatrix(ngon);
                SignMatrix pluckerSign = new SignMatrix(plucker);
                SignMatrix fullPluckerKey = similarPluckerFullMatrix.Keys.FirstOrDefault(m => m.Equals(pluckerSign));
                if (fullPluckerKey == null)
                {
                    similarPluckerFullMatrix.Add(pluckerSign, new List<Ngon>());
                    fullPluckerKey = pluckerSign;
                }
                similarPluckerFullMatrix[fullPluckerKey].Add(ngon);
                
                ReducedSignMatrix reducedPluckerKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(pluckerSign.Reduced));
                if (reducedPluckerKey == null)
                {
                    similarReducedMatrix.Add(pluckerSign.Reduced, new List<Ngon>());
                    reducedPluckerKey = pluckerSign.Reduced;
                }
                similarReducedMatrix[reducedPluckerKey].Add(ngon);
                #endregion
                #region Projection
                ProjectionMatrix projection = new ProjectionMatrix(ngon);
                SignMatrix projectionSign = new SignMatrix(projection);
                SignMatrix fullProjectionKey = similarProjectionFullMatrix.Keys.FirstOrDefault(m => m.Equals(projectionSign));
                if (fullProjectionKey == null)
                {
                    similarProjectionFullMatrix.Add(projectionSign, new List<Ngon>());
                    fullProjectionKey = projectionSign;
                }
                similarProjectionFullMatrix[fullProjectionKey].Add(ngon);
                #endregion
                SignMatrix fullMatrixKey = matrixMap.Keys.FirstOrDefault(m => m.Equals(pluckerSign));
                if (fullMatrixKey == null)
                {
                    matrixMap.Add(pluckerSign, new Dictionary<SignMatrix, int>());
                    fullMatrixKey = pluckerSign;
                }
                SignMatrix projectionResult = matrixMap[fullMatrixKey].Keys.FirstOrDefault(m => m.Equals(projectionSign));
                if (projectionResult == null)
                {
                    matrixMap[fullMatrixKey].Add(projectionSign,0);
                    projectionResult = projectionSign;
                }
                matrixMap[fullMatrixKey][projectionResult]++;
                if (i % (sampleSize / 100) == 0) Console.Write("\rCreating sign matrix table: {0}%    ", i / (sampleSize / 100));

            }


        }

        public void saveToHtml(string filename)
        {
            saveFullToHtml("plucker "+filename, similarPluckerFullMatrix);
            saveFullToHtml("projection "+filename, similarProjectionFullMatrix);
            saveMapToHtml("plucker-projection" + filename);            
        }
        #region PrintingMap
        public void saveMapToHtml(string filename)
        {
            saveMapToHtml(filename, matrixMap.Keys);
        }
        private void saveMapToHtml(string filename, IEnumerable<SignMatrix> matricies)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

            string title = "Plucker to projection";
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<h4>Total plucker matricies: " + similarPluckerFullMatrix.Keys.Count() + "   Total projection matricies: "+ similarProjectionFullMatrix.Keys.Count() + "</h4>");
            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            //writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var matrix in matricies)
                writer.WriteLine(htmlTableRowDistribution(matrix, matrixMap[matrix].Keys));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(SignMatrix matrix, IEnumerable<SignMatrix> relatedMatrices)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";
            foreach (SignMatrix relatedMatrix in relatedMatrices)
            {
                result += "<td style=\"border: 1px solid black;\">";
                result += "<p>" + matrixMap[matrix][relatedMatrix] +relatedMatrix.ToHtml() + "</p>";
                result += "</td>";
            }                        

            result += "<td style=\"border: 1px solid black;>\">";
            result += "<p>" + matrixMap[matrix].Values.Sum() + "</p>";
            result += "</td>";

            result += "</tr>\n";
            return result;
        }
        #endregion
        #region PrintingFullMatrix
        private void saveFullToHtml(string filename, Dictionary<SignMatrix, List<Ngon>> dictionary)
        {
            saveFullToHtml(filename, dictionary.OrderBy(n => n.Key.Reduced));
        }
        private void saveFullToHtml(string filename, NgonType neededType, Dictionary<SignMatrix, List<Ngon>> dictionary)
        {
            var typed = dictionary.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key.Reduced);

            saveFullToHtml(filename, ordered);
        }
        private void saveFullToHtml(string filename, IOrderedEnumerable<KeyValuePair<SignMatrix, List<Ngon>>> matricies)
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
            writer.WriteLine("<h4>Total matricies: " + matricies.Count() + "</h4>");
            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var matrix in matricies)
                writer.WriteLine(htmlTableRowDistribution(matrix.Key, matrix.Value));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(IPrintableMatrix matrix, IEnumerable<Ngon> relatedNgons)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += matrix.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;\">";
            result += "<p>" + relatedNgons.Count(n => n.Type == NgonType.Convex) + "</p>";
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
            saveReducedToHtml(filename, similarReducedMatrix.OrderBy(n => n.Key));
        }
        public void saveReducedToHtml(string filename, NgonType neededType)
        {
            var typed = similarReducedMatrix.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key);

            saveReducedToHtml(filename, ordered);
        }
        private void saveReducedToHtml(string filename, IOrderedEnumerable<KeyValuePair<ReducedSignMatrix, List<Ngon>>> matricies)
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
        #region TwoKeyPrinting
        public void save2KToHtml(string filename)
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
            writer.WriteLine("<h4>Total plucker matricies: " + similarPluckerFullMatrix.Count() +"       Total projection matrices: "+ similarProjectionFullMatrix.Count()+ "</h4>");
            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Plucker matrix signs </th>"+ "<th> Projection matrix signs </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var plucker in similarPluckerFullMatrix.Keys)
                foreach (var projection in similarProjectionFullMatrix.Keys)
                {
                    IEnumerable<Ngon> intersection = similarPluckerFullMatrix[plucker].Intersect(similarProjectionFullMatrix[projection]);
                    if(intersection.Count()>0)
                        writer.WriteLine(htmlTableRow2K(plucker, projection,intersection));
                }
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRow2K(IPrintableMatrix plucker, IPrintableMatrix projection, IEnumerable<Ngon> relatedNgons)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += plucker.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += projection.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;\">";
            result += "<p>" + relatedNgons.Count(n => n.Type == NgonType.Convex) + "</p>";
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
    }
    
    
}
