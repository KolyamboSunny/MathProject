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
using MathProject.Entities;
using MathProject;
using MathProject.Tools;
using MathProjectVisualization.VisualEntities;
using MathProjectVisualization.VisualEntities.VisualSignMatrices;

namespace MathProjectVisualization
{
    /// <summary>
    /// Логика взаимодействия для MaskExperiment.xaml
    /// </summary>

    public partial class MaskExperiment4 : Window
    {
        private SignMatrix SelectedMatrix;
        NgonDatabase db = new NgonDatabase(4);

        public MaskExperiment4()
        {
            InitializeComponent();
        }

        private void but_find_Click(object sender, RoutedEventArgs e)
        {
            var Fulls = db.PluckerSignMatrices.ToList();
            var Edited = Fulls.Where(ps => ps.IncludedInMask(this.maskGenerator.Mask));
            var VisualSignMatrices = new List<VisualSignMatrix>();
            foreach (var signmatrix in Edited)
            {
                VisualSignMatrices.Add(new VisualSignMatrix(signmatrix));
            }
            this.fullList.ItemsSource = VisualSignMatrices;
        }

        private void fullList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.SelectedMatrix = ((VisualSignMatrix)fullList.SelectedItem).signmatrix;
                List<VisualNgon> ngons = new List<VisualNgon>();
                List<Canvas> ngonsvizs = new List<Canvas>();
                if (SelectedMatrix != null)
                {

                    foreach (var rawngon in SelectedMatrix.Ngons)
                    {
                        VisualNgon ngon = new VisualNgon(rawngon);

                        ngons.Add(ngon);
                        double width = 300, height = 200;
                        Canvas box = new Canvas() { Height = height, Width = width };
                        ngon.draw(box, width, height);
                        ngonsvizs.Add(box);

                    }
                }
                panel_ngons.ItemsSource = ngonsvizs;
            }
            catch(Exception exception)
            {
                panel_ngons.ItemsSource = null;
            }
        }     

        private void panel_ngons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = 0;
            var s = (ListView)sender;
        }
    }
}
