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

namespace WPFApp.Views.Elements
{
    /// <summary>
    /// Interaction logic for PrinterMessage.xaml
    /// </summary>
    public partial class PrinterMessage : UserControl
    {
        public PrinterMessage()
        {
            InitializeComponent();
        }
        public TextBlock Text1msg => this.text1msg;
        public TextBlock Test2stat => this.text2stat;
        public TextBlock Text3name => this.text3name;
    }
}
