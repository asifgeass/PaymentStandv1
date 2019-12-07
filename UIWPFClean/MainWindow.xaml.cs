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

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        private MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.vm = new MainWindowViewModel();
            this.DataContext = this.vm;
        }

        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
