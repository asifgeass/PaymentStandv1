using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for DynamicMenuWindow.xaml
    /// </summary>
    public partial class DynamicMenuWindow : Window
    {
        public DynamicMenuWindow()
        {
            InitializeComponent();
#if DEBUG
            this.WindowState = WindowState.Normal;
#else
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
#endif
            //this.WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var builder = new ViewDynamicMenuBuilder(this);
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            //theme.SetBaseTheme(new MatDesDarkerLightTheme());
            theme.SetBaseTheme(new MatDesDarkerLightTheme());
            paletteHelper.SetTheme(theme);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
