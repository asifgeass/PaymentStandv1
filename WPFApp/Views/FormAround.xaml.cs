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

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for FormAround.xaml
    /// </summary>
    public partial class FormAround : UserControl
    {
        public FormAround()
        {
            InitializeComponent();
        }

        public object GetDataContext => this.DataContext;

        public object ContentControls
        {
            get => this.panelStk.Content;
            set => this.panelStk.Content = value;
        }

        private void MainGrid_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
