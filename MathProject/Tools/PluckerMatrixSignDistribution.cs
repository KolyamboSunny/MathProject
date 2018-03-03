using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using MathNet.Numerics.LinearAlgebra;

namespace MathProject.Tools
{
    public class PluckerSignMatrixDistribution
    {
        public Dictionary<SignMatrix, List<Ngon>> similarFullMatrix = new Dictionary<SignMatrix, List<Ngon>>();
        public Dictionary<ReducedSignMatrix2, List<Ngon>> similarReducedMatrix = new Dictionary<ReducedSignMatrix2, List<Ngon>>();

        public PluckerSignMatrixDistribution(long sampleSize, int dimensions)
        {
            Matrix<double> mask = Matrix<double>.Build.Dense(5, 5, 1);
            mask[2, 0] = 0;
            mask[3, 0] = 0;
            mask[3, 1] = 0;
            for (long i=sampleSize;i>0;i--)
            {
                Ngon ngon = Program.generateRandomNgon(dimensions);
                PluckerMatrix plucker = new PluckerMatrix(ngon);
                SignMatrix signMatrix = new SignMatrix(plucker,mask);

                SignMatrix fullKey = similarFullMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix));                
                if (fullKey == null)
                {
                    similarFullMatrix.Add(signMatrix, new List<Ngon>());
                    fullKey = signMatrix;
                }
                similarFullMatrix[fullKey].Add(ngon);

                ReducedSignMatrix2 reducedKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix.getReduced2(mask)));
                if (reducedKey == null)
                {
                    similarReducedMatrix.Add(signMatrix.getReduced2(mask), new List<Ngon>());
                    reducedKey = signMatrix.getReduced2(mask);
                }
                similarReducedMatrix[reducedKey].Add(ngon);
                if (i % (sampleSize / 100) == 0) Console.Write("\rCreating sign matrix table: {0}%    ", i / (sampleSize / 100));                
                                    
            }
            
            
        }

        #region PrintingFullMatrix
        public void saveFullToHtml(string filename)
        {
            saveFullToHtml(filename,similarFullMatrix.OrderBy(n => n.Key.getReduced()));
        }
        public void saveFullToHtml(string filename,NgonType neededType)
        {
            var typed = similarFullMatrix.Where(p => p.Value.Count(n => n.Type != neededType)==0);
            var ordered=typed.OrderBy(n => n.Key.getReduced());

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
        private void saveReducedToHtml(string filename, IOrderedEnumerable<KeyValuePair<ReducedSignMatrix2, List<Ngon>>> matricies)
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

                ReducedSignMatrix reducedKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(signMatrix.getReduced()));
                if (reducedKey == null)
                {
                    similarReducedMatrix.Add(signMatrix.getReduced(), new List<Ngon>());
                    reducedKey = signMatrix.getReduced();
                }
                similarReducedMatrix[reducedKey].Add(ngon);
                if (i % (sampleSize / 100) == 0) Console.Write("\rCreating sign matrix table: {0}%    ", i / (sampleSize / 100));

            }


        }

        #region PrintingFullMatrix
        public void saveFullToHtml(string filename)
        {
            saveFullToHtml(filename, similarFullMatrix.OrderBy(n => n.Key.getReduced()));
        }
        public void saveFullToHtml(string filename, NgonType neededType)
        {
            var typed = similarFullMatrix.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key.getReduced());

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
                
                ReducedSignMatrix reducedPluckerKey = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(pluckerSign.getReduced()));
                if (reducedPluckerKey == null)
                {
                    similarReducedMatrix.Add(pluckerSign.getReduced(), new List<Ngon>());
                    reducedPluckerKey = pluckerSign.getReduced();
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
            saveFullToHtml(filename, dictionary.OrderBy(n => n.Key.getReduced()));
        }
        private void saveFullToHtml(string filename, NgonType neededType, Dictionary<SignMatrix, List<Ngon>> dictionary)
        {
            var typed = dictionary.Where(p => p.Value.Count(n => n.Type != neededType) == 0);
            var ordered = typed.OrderBy(n => n.Key.getReduced());

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
    public class MultipleReducedMatricesDistribution
    {
        NgonDatabase db = new NgonDatabase(5);
        
        public Dictionary<ReducedSignMatrix, List<ReducedSignMatrix2>> similarReducedMatrix = new Dictionary<ReducedSignMatrix, List<ReducedSignMatrix2>>();
        public Dictionary<ReducedSignMatrix2, List<SignMatrix>> similarReduced2Matrix = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();

        public MultipleReducedMatricesDistribution()
        {
            Matrix<double> mask = Matrix<double>.Build.Dense(5, 5, 1);
            mask[3, 0] = 0;
            mask[3,1] = 0;
            mask[4, 1] = 0;
         

            foreach (SignMatrix matrix in db.PluckerSignMatrices)
            {
                var reduced1 = matrix.getReduced();
                var reduced2 = matrix.getReduced2(mask);                

                ReducedSignMatrix reduced1Key = similarReducedMatrix.Keys.FirstOrDefault(m => m.Equals(reduced1));
                if (reduced1Key == null)
                {
                    similarReducedMatrix.Add(reduced1, new List<ReducedSignMatrix2>());
                    reduced1Key = reduced1;
                }                

                ReducedSignMatrix2 reduced2Key = similarReduced2Matrix.Keys.FirstOrDefault(m => m.Equals(reduced2));
                if (reduced2Key == null)
                {
                    similarReducedMatrix[reduced1Key].Add(reduced2);
                    similarReduced2Matrix.Add(reduced2, new List<SignMatrix>());
                    reduced2Key = reduced2;
                }

                similarReduced2Matrix[reduced2Key].Add(matrix);
            }
                        
        }

        public void printMetadata()
        {
            Dictionary<int, int> reduced2variance = new Dictionary<int, int>();
            Console.WriteLine("Each reduced1 has 4 reduced2: " +similarReducedMatrix.All(m => m.Value.Count == 4));

            //Dictionary<ReducedSignMatrix2, List<SignMatrix>> onlyOneFull = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();
            //Dictionary<ReducedSignMatrix2, List<SignMatrix>> manyFull = new Dictionary<ReducedSignMatrix2, List<SignMatrix>>();

            var oneFull = similarReduced2Matrix.Where(m => m.Value.Count == 1);
            var threeFull = similarReduced2Matrix.Where(m => m.Value.Count == 3);
            var fiveFull = similarReduced2Matrix.Where(m => m.Value.Count == 5);

            //Console.WriteLine("Each reduced2 1 full -> convex only: " + oneFull.All(m=>m.Value.All(f=>f.Ngons.Count(n=>n.Type==NgonType.Convex)>0)==true));
            //Console.WriteLine("Each reduced2 3 full -> all non-convex: " + threeFull.All(m => m.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) == 0) == true));
            //Console.WriteLine("Each reduced2 5 full -> all non-convex: " + fiveFull.All(m => m.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) == 0) == true));
            foreach (var reduced1 in similarReducedMatrix.Keys)
            {
                IOrderedEnumerable<ReducedSignMatrix2> sortedReduced2 = similarReducedMatrix[reduced1].OrderBy(m => similarReduced2Matrix[m].Count());
                foreach (var reduced2 in sortedReduced2.Where(n=> similarReduced2Matrix[n].Count == 3))
                {
                    foreach (var full in similarReduced2Matrix[reduced2])
                    {
                        Console.WriteLine(reduced1.ToString() + " >>>> "
                                          + reduced2.data[2][0] + reduced2.data[4][2] + " >>>> " +
                                          + full.columnVectors[3][0]+ full.columnVectors[3][1] +full.columnVectors[4][1]+
                                           "   : " + similarReduced2Matrix[reduced2].Count());
                    }
                }
                Console.WriteLine();
            }


            foreach (var reduced2 in similarReduced2Matrix)
            {
                var reduced1 = similarReducedMatrix.First(m => m.Value.Contains(reduced2.Key)).Key;

                //Console.WriteLine(reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]);
                var predicted = predictConvexReduced2(reduced1);

                bool IWasRightEquationWorks = predicted[0] == reduced2.Key.data[2][0] && predicted[1] == reduced2.Key.data[4][2];               
                bool isConvex = reduced2.Value.All(f => f.Ngons.Count(n => n.Type == NgonType.Convex) > 0);              
                if (IWasRightEquationWorks && !isConvex)
                    Console.WriteLine("Mistake in convex formula: " +reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]); //+ "  : " + IWasRightEquationWorks);

                foreach (var full in reduced2.Value)
                {
                    var predictedFullConvex = predictConvexFull(reduced1);

                    //var fullConvex = reduced2.Value.Where(f => f.Ngons.Count(n => n.Type == NgonType.Convex) > 0);
                    bool equationConvexFullWorks = predictedFullConvex[0] == full.columnVectors[3][0] &&
                                                   predictedFullConvex[1] == full.columnVectors[3][1] &&
                                                   predictedFullConvex[2] == full.columnVectors[4][1];

                    bool fullIsConvex = full.Ngons.Count(n => n.Type == NgonType.Convex)>0;
                    if (equationConvexFullWorks && !fullIsConvex)
                        Console.WriteLine("Mistake in full convex formula: " + reduced1.ToString() + " >>>> "
                                                   + reduced2.Key.data[2][0] + reduced2.Key.data[4][2] + " >>>> " +
                                                   +full.columnVectors[3][0] + full.columnVectors[3][1] + full.columnVectors[4][1] +
                                                    "   : " + similarReduced2Matrix[reduced2.Key].Count()); //+ "  : " + IWasRightEquationWorks);
                }

                bool fiveIsNegatedConvex = -predicted[0] == reduced2.Key.data[2][0] && -predicted[1] == reduced2.Key.data[4][2];
                bool isFive = reduced2.Value.Count == 5;
                if (fiveIsNegatedConvex && !isFive)
                    Console.WriteLine("Mistake in 5Full formula: " + reduced1.ToString() + " >>>> " + reduced2.Key.data[2][0] + reduced2.Key.data[4][2]);
            }
        }

        public int[] predictFiveReduced2(ReducedSignMatrix reduced1)
        {
            var result = predictConvexReduced2(reduced1);
            
            result[0] = -result[0];
            result[1] = -result[1];
            
            return result;
        }
        public int[] predictConvexReduced2(ReducedSignMatrix reduced1)
        {
            int[] result=new int[2];
            result[0] =((reduced1.data[0]==1) ^ (reduced1.data[1] == 1)) ? 1 : -1 ;
            result[1] = ((reduced1.data[2] == 1) ^ (reduced1.data[3] == 1)) ? 1 : -1;
            if(reduced1.countPositive()%2==1)
            {
                result[0] = -result[0];
                result[1] = -result[1];
            }
            return result;
        }
        public int[] predictConvexFull(ReducedSignMatrix reduced1)
        {
            int[] result = new int[3];
            result[0] = ((reduced1.data[3] == 1) ^ (reduced1.data[4] == 1)) ? 1 : -1;
            result[1] = ((reduced1.data[1] == 1) ^ (reduced1.data[2] == 1)) ? 1 : -1;
            result[2] = ((reduced1.data[4] == 1) ^ (reduced1.data[0] == 1)) ? 1 : -1;

            if (reduced1.countPositive() % 2 == 1)
            {
                result[0] = -result[0];
                result[1] = -result[1];
                result[2] = -result[2];
            }
            return result;
        }



        public void saveToHtml(string filename)
        {
            StreamWriter writer = new StreamWriter((new FileStream(filename, FileMode.Create)));

            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");

            string title = "Double Reduced experiment";
            writer.WriteLine("<title>" + title + "</title>");

            writer.WriteLine("<style>");
            writer.WriteLine("td {  text-align:center;" + "}");
            writer.WriteLine("</style>");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");            
            writer.WriteLine("<table>");// style=\"border: 3px solid black;>\"");
            writer.WriteLine("<tr>" + "<th> Reduced1 </th>" + "<th> Reduced2 </th>" + "<th> Full </th>" + "<th>Convex</th>" + "<th>Reflex</th>" + "<th>Self Intersecting</th>" + "<th>Total</th>" + "</tr>");

            foreach (var reduced1 in similarReducedMatrix.Keys)
                foreach (var reduced2 in similarReducedMatrix[reduced1])
                    foreach (var matrix in similarReduced2Matrix[reduced2])
                        writer.WriteLine(htmlTableRowDistribution(reduced1,reduced2,matrix,matrix.Ngons));
            writer.WriteLine("</table>");

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
        private string htmlTableRowDistribution(IPrintableMatrix reduced1, IPrintableMatrix reduced2, IPrintableMatrix matrix, IEnumerable<Ngon> relatedNgons)
        {
            string result = "<tr style=\"border: 2px solid black;>\">";

            result += "<td style=\"border: 1px solid black;>\">";
            result += reduced1.ToHtml();
            result += "</td>";

            result += "<td style=\"border: 1px solid black;>\">";
            result += reduced2.ToHtml();
            result += "</td>";

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
        
    }

}
