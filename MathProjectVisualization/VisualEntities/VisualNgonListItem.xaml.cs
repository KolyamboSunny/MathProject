using MathProject.Entities;
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
    /// Interaction logic for VisualNgonListItem.xaml
    /// </summary>
    public partial class VisualNgonListItem : UserControl
    {
        public VisualNgonListItem(Ngon rawngon)
        {
            VisualNgon ngon = new VisualNgon(rawngon);         
            InitializeComponent();
            ngon.draw(can_shape,300,200);
            ngon.drawVectors(can_vectors,300,200);           
        }
    }
}
