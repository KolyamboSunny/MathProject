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

namespace MathProjectVisualization
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Ngon ngon = Program.generateRandomNgon(4);
            VisualNgon n = new VisualNgon(ngon);
            n.draw(frame);
            displayAngleSum(ngon);
            displayType(ngon);
        }

        private void but_ngonDistribution_Click(object sender, RoutedEventArgs e)
        {
            Window n = new NgonDistribution();
            n.ShowDialog();
        }
    }

    }
