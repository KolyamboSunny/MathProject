using MathProject;
using MathProject.Entities;
using MathProjectVisualization.VisualEntities;
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
    /// Логика взаимодействия для NgonDistribution.xaml
    /// </summary>
    public partial class NgonDistribution : Window
    {
        long sampleSize;
        int dimensions;
        List<VisualNgon> ngons;
        List<Polygon> polygons = new List<Polygon>();
        public NgonDistribution()
        {
            InitializeComponent();
        }

        private void but_generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sampleSize = Int64.Parse(text_sampleSize.Text);
                dimensions = Int32.Parse(text_dimensions.Text);

                ngons = new List<VisualNgon>((int)sampleSize);
                for(long i= sampleSize; i>0;i--)
                {
                    VisualNgon ngon = new VisualNgon(Program.generateRandomNgon(dimensions));

                    ngons.Add(ngon);
                    double width = 300, height = 200;
                    Canvas box = new Canvas() { Height = height, Width = width };
                    ngon.draw(box,width,height);
                    
                    if (ngon.ngonModel.Type == NgonType.Convex) panel_convex.Items.Add(box);
                    if (ngon.ngonModel.Type == NgonType.Reflex) panel_reflex.Items.Add(box);
                    if (ngon.ngonModel.Type == NgonType.Self_Intersecting) panel_selfintersecting.Items.Add(box);
                    
                }
            }
            catch(Exception)
            {
                text_sampleSize.Text = "";
                text_dimensions.Text = "";
            }
        }
    }
}
