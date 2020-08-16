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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for KeyboardTest.xaml
    /// </summary>
    public partial class KeyboardTest : UserControl
    {
        public KeyboardTest()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DrawerHost.OpenDrawerCommand.Execute(Dock.Bottom, sender as TextBox);
        }
    }
}
