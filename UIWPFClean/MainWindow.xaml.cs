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
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes;
using MaterialDesignColors;
using WPFApp.ViewModels;

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        private DynamicMenuWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.vm = new DynamicMenuWindowViewModel();
            this.DataContext = this.vm;
        }

        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = new Button();
            var inputbox = new TextBox();
            var binding = new Binding($"{nameof(vm.PayrecToSendSumma)}");
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            inputbox.SetBinding(TextBox.TextProperty, binding);

            SolidColorBrush brush = Application.Current.TryFindResource("PrimaryHueDarkBrush") as SolidColorBrush;
            btn.SetResourceReference(Button.ForegroundProperty, "MaterialDesignBody");

            this.StackPanel1.Children.Add(inputbox);
            this.StackPanel1.Children.Add(new TextBlock());
            this.StackPanel1.Children.Add(btn);
            count++;
        }
    }
}
