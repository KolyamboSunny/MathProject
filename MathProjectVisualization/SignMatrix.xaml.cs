using MathProject.Tools;
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
    /// Логика взаимодействия для SignMatrix.xaml
    /// </summary>
    public partial class SignMatrix : Page
    {
        PluckerSignMatrixDistribution signMatricies;
        public SignMatrix()
        {
            this.signMatricies = new PluckerSignMatrixDistribution(1000000, 4);
            InitializeComponent();
        }
        public void draw()
        {
            foreach(PluckerSignMatrix s in signMatricies.similarMatrix.Keys)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
