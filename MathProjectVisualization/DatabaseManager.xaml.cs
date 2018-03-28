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

using MathProjectVisualization.VisualEntities;

namespace MathProjectVisualization
{
    /// <summary>
    /// Interaction logic for DatabaseManager.xaml
    /// </summary>
    public partial class DatabaseManager : Window
    {
        public DatabaseManager()
        {
            InitializeComponent();
            section0.Children.Add(new DatabaseNgonSummary(4));
            section1.Children.Add(new DatabaseNgonSummary(5));
            section2.Children.Add(new DatabaseNgonSummary(6));
        }
    }
}
