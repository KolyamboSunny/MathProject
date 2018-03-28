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
            Window n = new MaskExperiment(5);
            n.ShowDialog();
        }

        private void but_4gons_Click(object sender, RoutedEventArgs e)
        {
            Window n = new MaskExperiment(4);
            n.ShowDialog();
        }
        private void but_6gons_Click(object sender, RoutedEventArgs e)
        {
            Window n = new MaskExperiment(6);
            n.ShowDialog();
        }
        private void but_dbManager_Click(object sender, RoutedEventArgs e)
        {
            Window n = new DatabaseManager();
            n.ShowDialog();
        }
    }
}
