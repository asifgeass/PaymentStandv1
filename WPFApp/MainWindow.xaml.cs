using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static event Action<string> WriteTextBox = (x) => { };
        public MainWindow()
        {
            InitializeComponent();
            PostGetHTTP.WriteTextBox += WriteToTextbox;
        }

        private void WriteToTextbox(string arg)
        {
            textbox1.AppendText(arg + Environment.NewLine + Environment.NewLine);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            PostGetHTTP.DoParalel();
            //listBox1.Items.Clear();
            //Assembly[] sborki = AppDomain.CurrentDomain.GetAssemblies();
            //foreach (Assembly item in sborki)
            //{
            //    listBox1.Items.Add(item.FullName);
            //}
            //var hzxml = PostGetHTTP.XML();

            //textbox1.Text = await PostGetHTTP.AllTypePost();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PostGetHTTP.RunOpReq(textBoxId2.Text);
        }
    }
}
