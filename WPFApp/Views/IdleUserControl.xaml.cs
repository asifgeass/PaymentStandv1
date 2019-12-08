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

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for IdleUserControl.xaml
    /// </summary>
    public partial class IdleUserControl : UserControl
    {
        private int timer = 11;
        public IdleUserControl()
        {
            InitializeComponent();
            StartTimer();
        }
        private async Task StartTimer()
        {
            while(timer>=0)
            {
                textBlock1.Text = $"Вам еще нужно время? ({timer})";
                await Task.Delay(1000);
                timer--;
            }
            timer = 10;
        }
    }
}
