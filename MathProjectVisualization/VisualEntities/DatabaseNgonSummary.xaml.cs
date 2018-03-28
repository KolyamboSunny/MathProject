using MathProject;
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
    /// Interaction logic for DatabaseNgonSummary.xaml
    /// </summary>
    public partial class DatabaseNgonSummary : UserControl
    {
        NgonDatabase db;
        public DatabaseNgonSummary(int dimensions)
        {
            this.db = new NgonDatabase(dimensions);
            InitializeComponent();

            this.lab_title.Content = this.lab_title.Content.ToString().Replace("$DIM$", dimensions.ToString());
            
            this.lab_numberNgons.Text = this.lab_numberNgons.Text.Replace("$NUM_NGONS$", db.Ngons.Count().ToString());
            this.lab_numberPluckerSMs.Text = this.lab_numberPluckerSMs.Text.Replace("$NUM_MATRICES$", db.PluckerSignMatrices.Count().ToString());
            this.lab_expectedPluckerSMs.Text = this.lab_expectedPluckerSMs.Text.Replace("$EXP_MATRICES$", "no idea");
            this.lab_minPluckerSMs.Text = this.lab_minPluckerSMs.Text.Replace("$EXP_MATRICES$", db.PluckerSignMatrices.Min(m=>m.Ngons.Count).ToString());
            this.lab_maxPluckerSMs.Text = this.lab_maxPluckerSMs.Text.Replace("$EXP_MATRICES$", db.PluckerSignMatrices.Max(m => m.Ngons.Count).ToString());

        }

        private void but_add_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
