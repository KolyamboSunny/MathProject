using MathProject;
using MathProject.Entities;
using MathProject.Tools;
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
    public partial class NgonEdgePermutation : Window
    {
        long sampleSize;
        int dimensions;
        Ngon modelNgon;
        ICollection<Ngon> permutations;
        List<VisualNgon> ngons;
        List<Polygon> polygons = new List<Polygon>();
        public NgonEdgePermutation(Ngon ngon)
        {
            this.modelNgon = ngon;
            this.permutations = (new NgonEdgePermutations(ngon)).edgePermutations();
            InitializeComponent();
            updateCollection();
        }

        private void updateCollection()
        {
            try
            {

                sampleSize = permutations.Count();
                dimensions = modelNgon.Edges.Count();

                ngons = new List<VisualNgon>((int)sampleSize);
                foreach(Ngon modelNgon in permutations)
                {
                    VisualNgon ngon = new VisualNgon(modelNgon);

                    ngons.Add(ngon);
                    double width = 300, height = 200;
                    Canvas box = new Canvas() { Height = height, Width = width };
                    ngon.draw(box,width,height);
                    
                    if (ngon.ngonModel.Type == NgonType.Convex) panel_convex.Items.Add(box);
                    if (ngon.ngonModel.Type == NgonType.Reflex) panel_reflex.Items.Add(box);
                    if (ngon.ngonModel.Type == NgonType.Self_Intersecting) panel_selfintersecting.Items.Add(box);
                    
                }

                lab_convex.Content = "Convex: " + panel_convex.Items.Count;
                lab_reflex.Content = "Reflex: " + panel_reflex.Items.Count;
                lab_selfintersecting.Content = "Self-Intersecting: " + panel_selfintersecting.Items.Count;
            }
            catch(Exception)
            {                 
            }
        }
    }
}
