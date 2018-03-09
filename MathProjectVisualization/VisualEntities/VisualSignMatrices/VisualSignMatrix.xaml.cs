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

namespace MathProjectVisualization.VisualEntities.VisualSignMatrices
{
    /// <summary>
    /// Interaction logic for VisualSignMatrix.xaml
    /// </summary>
    public partial class VisualSignMatrix: UserControl
    {
        public SignMatrix signmatrix;
        private Label[,] labels;
        private int LabelWidth = 30;
        public VisualSignMatrix(SignMatrix signmatrix)
        {
            this.signmatrix = signmatrix;
            labels = new Label[signmatrix.dimensions,signmatrix.dimensions];
            InitializeComponent();            
            displaySigns();
            colorSigns();
        }
        private void displaySigns()
        {
            for (int i = 0; i < signmatrix.dimensions; i++)
                for (int j = 0; j <=i ; j++)
                {
                    Label sign = new Label()
                    {
                        Height = LabelWidth,
                        Width = LabelWidth,
                        Margin = new Thickness(i * LabelWidth, j * LabelWidth, 0, 0),
                        Content = matrixsign(i, j)
                    };
                    this.grid.Children.Add(sign);
                    labels[i, j] = sign;
                }
        }
        private void colorSigns()
        {
            switch (signmatrix.dimensions)
            {
                case 4:
                    colorSigns4();
                    break;
                case 5:
                    colorSigns5();
                    break;
            }
        }
        private void colorSigns5()
        {
            int[][] reduced1 = { new int[] { 1, 0 }, new int[] { 2, 1 }, new int[] { 3, 2 }, new int[] { 4, 3 }, new int[] { 4, 0 } };
            int[][] reduced2 = { new int[] { 2, 0 }, new int[] { 4, 2 } };
            int[][] full = { new int[] { 3, 0 }, new int[] { 4, 1 }, new int[] {3,1 } };

            foreach (int[] pair in reduced1)            
                labels[pair[0],pair[1]].Background = Brushes.Blue;                
            
            foreach (int[] pair in reduced2)
                labels[pair[0], pair[1]].Background = Brushes.Red;

            foreach (int[] pair in full)
                labels[pair[0], pair[1]].Background = Brushes.Green;

        }
        private void colorSigns4()
        {
            //int[][] reduced1 = { new int[] { 1, 0 }, new int[] { 2, 1 }, new int[] { 3, 2 }, new int[] { 4, 3 }, new int[] { 4, 0 } };
            //int[][] reduced2 = { new int[] { 2, 0 }, new int[] { 4, 2 } };
            int[][] full = { new int[] { 2, 0 }, new int[] { 3, 1 } };

        /*    foreach (int[] pair in reduced1)
                labels[pair[0], pair[1]].Background = Brushes.Blue;
                */

            /*foreach (int[] pair in reduced2)
                labels[pair[0], pair[1]].Background = Brushes.Red;
*/
            foreach (int[] pair in full)
                labels[pair[0], pair[1]].Background = Brushes.Green;

        }
        private string matrixsign(int i, int j)
        {
            switch (signmatrix.columnVectors[i][j])
            {
                case (1):
                    return "+";
                case (-1):
                    return "-";
                default:
                    return signmatrix.columnVectors[i][j].ToString();
            }
        }
    }
}
