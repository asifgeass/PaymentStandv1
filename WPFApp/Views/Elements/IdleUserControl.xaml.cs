using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for IdleUserControl.xaml
    /// </summary>
    public partial class IdleUserControl : UserControl
    {
        public IdleUserControl()
        {
            InitializeComponent();            
        }
        private async Task StartTimer(DialogSession sessionArg=null)
        {
            int timer = 10;            
            Trace.WriteLine($"=======IDLE COUNTDOWN STARTS==========timer={timer}");
            while(timer>=0 && !sessionArg.IsEnded)
            {
                textBlock1.Text = $"Вам еще нужно время? ({timer})";
                Trace.WriteLine($"=======IDLE WHILE==timer={timer}");
                await Task.Delay(1000);
                timer--;
            }
            Trace.WriteLine("=======IDLE COUNTDOWN close dialog timeout==========");
            DialogHost.CloseDialogCommand.Execute(false, this);
        }

        public void OnDialogOpened(object sender, DialogOpenedEventArgs eventArgs)
        {            
            StartTimer(eventArgs.Session);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(true, this);
        }
    }
}
