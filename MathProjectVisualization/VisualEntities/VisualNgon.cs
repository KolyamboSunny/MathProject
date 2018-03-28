using MathProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathProjectVisualization.VisualEntities
{
    public class VisualNgon
    {
        public Ngon ngonModel;
        PointCollection rawVerticies = new PointCollection();       
        PointCollection verticies = new PointCollection();

        List<Line> rawVectors = new List<Line>();
        List<Line> vectors = new List<Line>();

        public VisualNgon(Ngon ngon)
        {
            this.ngonModel = ngon;
            foreach (Vertex vertex in ngonModel.Verticies)
            {
                Point point = new Point(vertex.coordX, vertex.coordY);
                rawVerticies.Add(point);
            }
            var vectors = ngonModel.getOrthonormal();
            for (int i = 0; i < vectors[0].Length; i++)
            {
                Line visualVector = new Line()
                {
                    Y1 = 0,
                    X1 = 0,

                    Y2 = vectors[0][i],
                    X2 = vectors[1][i]
                };
                rawVectors.Add(visualVector);
            }
        }

        public void draw(Panel container)
        {
            draw(container, container.ActualWidth, container.ActualHeight);
        }
        public void draw(Panel container,double width,double height)
        {
            List<Line> ngonEdges = new List<Line>();


            foreach (Point vertex in rawVerticies)
            {
                Point screenVertex = convertToContainerCoords(width, height, vertex);
                verticies.Add(screenVertex);
            }

            char vertexLetter = 'A';
            for (int i = 0; i < verticies.Count; i++)
            {
                Line edge = new Line()
                {
                    X1 = verticies[i].X,
                    Y1 = verticies[i].Y,
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 2
                };
                if (i == verticies.Count - 1)
                {
                    edge.X2 = verticies[0].X;
                    edge.Y2 = verticies[0].Y;
                }
                else
                {
                    edge.X2 = verticies[i + 1].X;
                    edge.Y2 = verticies[i + 1].Y;
                }
                //double angle = Math.Round(ngonModel.relatedAngles[ngonModel.Verticies[i]]);
                Label vertexInfo = new Label()
                {
                    Content = (vertexLetter + " " ),
                    Margin = new Thickness(verticies[i].X, verticies[i].Y, 0, 0)
                };
                vertexLetter++;
                container.Children.Add(edge);
                container.Children.Add(vertexInfo);
            }
        }
        public Polygon getPolygon()
        {
            Polygon polygon = new Polygon() {
                Points = rawVerticies,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 10, 10, 10),
                StrokeThickness = 5,
                Stroke = new SolidColorBrush(Colors.Red)
            };
            return polygon;
        }

        public void drawVectors(Panel container)
        {
            drawVectors(container, container.ActualWidth, container.ActualHeight);
        }
        public void drawVectors(Panel container, double width, double height)
        {
            
            for (int i = 0; i < rawVectors.Count; i++)
            {
                var vector = convertToContainerCoords(container, rawVectors[i]);
                vector.StrokeThickness = 3;
                vector.Stroke = Brushes.Black;
                Label vertexInfo = new Label()
                {
                    Content = (i + " "),
                    Margin = new Thickness(vector.X2, vector.Y2, 0, 0)
                };
                container.Children.Add(vector);
                container.Children.Add(vertexInfo);
            }
        }

        private Point convertToContainerCoords(double width,double height, Point p)
        {            
            Point result = new Point();            

            double minX = Double.MaxValue, minY = Double.MaxValue;
            double maxX = Double.MinValue, maxY = Double.MinValue;

            foreach (Point rp in rawVerticies)
            {
                minX = Math.Min(rp.X, minX);
                minY = Math.Min(rp.Y, minY);
                maxX = Math.Max(rp.X, maxX);
                maxY = Math.Max(rp.Y, maxY);
            }

            double scaleCoefficientX = (width - 50) / (maxX - minX);
            double scaleCoefficientY = (height - 50) / (maxY - minY);
            result.X = (p.X - minX) * scaleCoefficientX;
            result.Y = (p.Y - minY) * scaleCoefficientY;
            return result;
        }
        private Point convertToContainerCoords(FrameworkElement container, Point p)
        {
            return convertToContainerCoords(container.ActualWidth, container.ActualHeight, p);
        }

        private Line convertToContainerCoords(double width, double height, Line rawVector)
        {
            Line result = new Line();

            double margin = 25;

            width -= margin*2;
            height -= margin * 2;


            double minX = rawVectors.Min(vec => vec.X2);
            double minY = rawVectors.Min(vec => vec.Y2); 
            double maxX = rawVectors.Max(vec => vec.X2);
            double maxY = rawVectors.Max(vec => vec.Y2);

            double scaleCoefficientX = width/(2*Math.Max(Math.Abs(maxX), Math.Abs(minX)));
            double scaleCoefficientY = height / (2 * Math.Max(Math.Abs(maxY), Math.Abs(minY)));

            double centerX = width / 2 +margin;
            double centerY = height / 2 + margin;

            result.X1 = centerX;
            result.Y1 = centerY;           

            result.X2 = centerX + (rawVector.X2* scaleCoefficientX);
            result.Y2 = centerY + (rawVector.Y2 * scaleCoefficientY);

            return result;
        }
        private Line convertToContainerCoords(FrameworkElement container, Line p)
        {
            return convertToContainerCoords(container.ActualWidth, container.ActualHeight, p);
        }
    }

}
