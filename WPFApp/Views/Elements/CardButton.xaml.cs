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
    /// Interaction logic for CardButton.xaml
    /// </summary>
    public partial class CardButton : UserControl
    {
        public CardButton()
        {
            InitializeComponent();
        }
        public Button Button => this.CardButtonControl;
        public TextBlock TextBlock => this.Textblock;
        public string Text
        {
            get => this.Textblock.Text;
            set => this.Textblock.Text = value;
        }
    }
}
