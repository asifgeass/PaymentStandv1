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
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;

namespace ModernUINavigation.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var hz = AppearanceManager.LightThemeSource;
            var manager = AppearanceManager.Current;
            var child = ScrollView1.Content;
            var cmd = manager.LightThemeCommand;
            
            //manager.AccentColor = (Color)ColorConverter.ConvertFromString("#f6f4ef");
            //cmd.Execute("Light");
        }
    }
}
