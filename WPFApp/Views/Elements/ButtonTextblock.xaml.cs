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

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for ButtonTextblock.xaml
    /// </summary>
    public partial class ButtonTextblock : UserControl
    {
        public ButtonTextblock()
        {
            InitializeComponent();
        }

        public Button ButtonControl => this.ButtonXaml;
        public TextBlock TextBlock => this.Textblock;
        public string Text
        {
            get => this.Textblock.Text;
            set => this.Textblock.Text = value;
        }

        public Style StyleButton 
        {
            get => this.ButtonControl.Style;
            set=> this.ButtonXaml.Style = value; 
        }
    }
}
