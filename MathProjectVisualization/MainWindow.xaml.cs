using MathProject.Entities;
using MathProject_Triangles;
using MathProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathProjectVisualization
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Polygon currentlyDrawn;
        public MainWindow()
        {
            InitializeComponent();
        }
        void drawNgonBySteps(Ngon ngon)
        {
            
        }
        void displayAngleSum(Ngon ngon)
        {
            lab_angles.Content = ngon.AngleSum;
        }
        void displayType(Ngon ngon)
        {
            lab_type.Content = ngon.Type;
        }
        private void but_randomNgon_Click(object sender, RoutedEventArgs e)
        {
            if (currentlyDrawn != null) grid.Children.Remove(currentlyDrawn);
            frame.Children.Clear();
            Ngon ngon = Program.generateRandomNgon(4);
            VisualNgon n = new VisualNgon(ngon);
            n.draw(frame);
            displayAngleSum(ngon);
            displayType(ngon);
        }
    }

    class VisualNgon
    {
        Ngon ngonModel;
        PointCollection rawVerticies = new PointCollection();
        PointCollection verticies = new PointCollection();

        public VisualNgon(Ngon ngon)
        {
            this.ngonModel = ngon;
        }

        public void draw(Panel container)
        {
            List<Line> ngonEdges = new List<Line>();
            foreach (Vertex vertex in ngonModel.Verticies)
            {
                Point point = new Point(vertex.coordX, vertex.coordY);
                rawVerticies.Add(point);
            }
            
            foreach (Point vertex in rawVerticies)
            {
                Point screenVertex = convertToContainerCoords(container, vertex);
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
                if (i==verticies.Count-1)
                {
                    edge.X2 = verticies[0].X;
                    edge.Y2 = verticies[0].Y;
                }
                else
                {
                    edge.X2 = verticies[i+1].X;
                    edge.Y2 = verticies[i+1].Y;
                }
                double angle=Math.Round(ngonModel.relatedAngles[ngonModel.Verticies[i]]);
                Label vertexInfo = new Label() { Content = (vertexLetter + " " + angle),
                                                 Margin = new Thickness(verticies[i].X, verticies[i].Y, 0, 0) };
                vertexLetter++;
                container.Children.Add(edge);
                container.Children.Add(vertexInfo);
            }           
        }

        private Point convertToContainerCoords(FrameworkElement container, Point p)
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

            double scaleCoefficientX = (container.ActualWidth - 50) / (maxX - minX);
            double scaleCoefficientY = (container.ActualHeight - 50) / (maxY - minY);
            result.X = (p.X - minX) * scaleCoefficientX;
            result.Y = (p.Y - minY) * scaleCoefficientY;
            return result;
        }
    }
}
