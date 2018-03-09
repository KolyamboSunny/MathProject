using MathNet.Numerics.LinearAlgebra;
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
    /// Interaction logic for MaskGenerator5.xaml
    /// </summary>
    public partial class MaskGenerator5 : UserControl
    {
        public Matrix<double> Mask = Matrix<double>.Build.Dense(5, 5, 0);
        public MaskGenerator5()
        {
            InitializeComponent();
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            Button but = ((Button)sender);
            string name = but.Name;
            int i = name.ElementAt(3)-'0';
            int j = name.ElementAt(4) - '0';
            if (Mask[i,j]==0)
            {
                Mask[i, j] = 1;
                but.Content = "+";
                return;
            }
            if (Mask[i, j] == 1)
            {
                Mask[i, j] = -1;
                but.Content = "-";
                return;
            }
            if (Mask[i, j] == -1)
            {
                Mask[i, j] = 0;
                but.Content = "0";
                return;
            }
        }
    }
}
