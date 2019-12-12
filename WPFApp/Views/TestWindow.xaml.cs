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
using WPFApp.ViewModels;

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
            var vm = this.DataContext as TestWindowViewModel;
            var binding = new Binding($"{nameof(vm.Payrec)}.AttrRecord[0].Name");
            

            var textbox = new TextBox();            
            textbox.SetBinding(TextBox.TextProperty, binding);

            var text2 = new TextBox();
            text2.SetBinding(TextBox.TextProperty, new Binding($"{nameof(vm.Payrec)}.AttrRecord[1].Name"));            

            var button = new Button() { Content="Coded" };
            button.Command = vm.payCommand;

            var label = new Label();
            label.SetBinding(Label.ContentProperty, binding);

            #region children.Add
            PnlStack.Children.Add(new TextBlock());
            PnlStack.Children.Add(textbox);
            PnlStack.Children.Add(new TextBlock());
            PnlStack.Children.Add(text2);
            PnlStack.Children.Add(new TextBlock());
            PnlStack.Children.Add(button);
            PnlStack.Children.Add(new TextBlock());
            PnlStack.Children.Add(label);
            #endregion
        }
    }
}
