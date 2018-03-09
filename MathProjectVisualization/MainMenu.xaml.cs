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
using System.Windows.Shapes;

namespace MathProjectVisualization
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void but_5gons_Click(object sender, RoutedEventArgs e)
        {
            Window n = new MaskExperiment();
            n.ShowDialog();
        }

        private void but_4gons_Click(object sender, RoutedEventArgs e)
        {
            Window n = new MaskExperiment4();
            n.ShowDialog();
        }
    }
}
