using MathNet.Numerics.LinearAlgebra;
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

namespace MathProjectVisualization.VisualEntities
{
    /// <summary>
    /// Interaction logic for VisualSignMatrix.xaml
    /// </summary>
    public partial class MaskGenerator: UserControl
    {        
        public Matrix<double> Mask;

        private Button[,] buttons;
        private int ButtonWidth = 30;

        int dimensions;
        public MaskGenerator(int dimensions)
        {
            this.dimensions = dimensions;
            Mask = Matrix<double>.Build.Dense(dimensions, dimensions, 0);

            buttons = new Button[dimensions,dimensions];
            InitializeComponent();
            displayButtons();
            colorButtons();
        }
        private void displayButtons()
        {
            for (int i = 0; i < dimensions; i++)
                for (int j = 0; j <=i ; j++)
                {
                    Button button = new Button()
                    {
                        Height = ButtonWidth,
                        Width = ButtonWidth,
                        Margin = new Thickness(i * (ButtonWidth+1), j * (ButtonWidth+1), 0, 0),
                        Content = masksign(i, j),
                        Name="but"+j + i,
                       
                        HorizontalAlignment=HorizontalAlignment.Left,
                        VerticalAlignment=VerticalAlignment.Top
                    };
                    button.Click += new RoutedEventHandler(but_Click);
                    this.grid.Children.Add(button);
                    buttons[i, j] = button;
                }
        }
        private void but_Click(object sender, RoutedEventArgs e)
        {
            Button but = ((Button)sender);
            string name = but.Name;
            int i = name.ElementAt(3) - '0';
            int j = name.ElementAt(4) - '0';
            if (Mask[i, j] == 0)
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
        
        private void colorButtons()
        {
            for(int i=0;i<dimensions;i++)
            {
                buttons[i,i].IsEnabled = false;
            }
            switch (this.dimensions)
            {
                case 4:
                    //colorSigns4();
                    break;
                case 5:
                    //colorSigns5();
                    break;
            }
        }
        /*
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
            int[][] reduced1 = { new int[] { 1, 0 }, new int[] { 2, 1 }, new int[] { 3, 2 }, new int[] { 3, 0 } };
            //int[][] reduced2 = { new int[] { 2, 0 }, new int[] { 4, 2 } };
            int[][] full = { new int[] { 2, 0 }, new int[] { 3, 1 } };

            foreach (int[] pair in reduced1)
                labels[pair[0], pair[1]].Background = Brushes.Blue;
                

            /*foreach (int[] pair in reduced2)
                labels[pair[0], pair[1]].Background = Brushes.Red;

            foreach (int[] pair in full)
                labels[pair[0], pair[1]].Background = Brushes.Green;

        }
    */
        private string masksign(int i, int j)
        {
            switch (Mask[i,j])
            {
                case (1):
                    return "+";
                case (-1):
                    return "-";
                default:
                    return Mask[i,j].ToString();
            }
        }
    }
}
