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

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            var textbox = new TextBox();
            var binding = new Binding("List[1]");
            textbox.SetBinding(TextBox.TextProperty, binding);
            var button = new Button() { Content="Coded" };
            PnlStack.Children.Add(new TextBlock());
            PnlStack.Children.Add(textbox);
            PnlStack.Children.Add(button);
            var label = new Label();
            label.SetBinding(Label.ContentProperty, binding);
            PnlStack.Children.Add(label);
        }
    }
}
