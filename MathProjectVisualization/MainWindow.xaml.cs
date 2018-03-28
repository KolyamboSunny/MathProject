using MathProject.Entities;
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
using MathProjectVisualization.VisualEntities;
using MathProject.Tools;

namespace MathProjectVisualization
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ngon currentNgon;
        public MainWindow()
        {
            InitializeComponent();
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
            frame.Children.Clear();

            int dimensions = Int32.Parse(text_dimensions.Text);

            currentNgon = Program.generateRandomNgon(dimensions);
            but_ngonPermutations.IsEnabled = true;
            VisualNgon n = new VisualNgon(currentNgon);
            lab_PluckerMatrix.Content = (new SignMatrix(new PluckerMatrix(currentNgon))).ToString();
            n.draw(frame);
            displayAngleSum(currentNgon);
            displayType(currentNgon);
        }

        private void but_ngonDistribution_Click(object sender, RoutedEventArgs e)
        {
            Window n = new NgonDistribution();
            n.ShowDialog();
        }

        private void but_ngonPermutations_Click(object sender, RoutedEventArgs e)
        {
            Window n = new NgonEdgePermutation(currentNgon);
            n.ShowDialog();
        }

        private void but_signMatricies_Click(object sender, RoutedEventArgs e)
        {
            //Window n = new SignMatrix();
            //n.ShowDialog();
        }

        private void but_maskExperiment_Click(object sender, RoutedEventArgs e)
        {
            Window n = new MaskExperiment(5);
            n.ShowDialog();
        }
    }

    }
