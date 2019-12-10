using ExceptionManager;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
            this.Topmost = false;
#else
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
#endif
            //this.WindowState = WindowState.Maximized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void Window_Initialized(object sender, EventArgs e)
        {
            var builder = new ViewDynamicMenuBuilder(this);
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            //theme.SetBaseTheme(new MatDesDarkerLightTheme());
            theme.SetBaseTheme(new MatDesDarkerLightTheme());
            paletteHelper.SetTheme(theme);
            Task.Run(async () =>
            {
                await Task.Delay(6000);
                Ex.Log($"Printers:");
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    Ex.Log(printer);
                }
            });
        }
    }
}
