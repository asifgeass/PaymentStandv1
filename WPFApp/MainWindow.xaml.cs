using Logic;
using Logic.XML;
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
using System.Xml.Linq;
using System.Xml.Serialization;
using Logic.Helpers;
using Logic.Models;

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
            textboxResponceXML.AppendText(arg + Environment.NewLine + Environment.NewLine);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonXMLCustom_Click(object sender, RoutedEventArgs e)
        {
            PostGetHTTP.PostXML(textboxXMLCustom.Text);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            textboxResponceXML.Clear();
        }

        private async void buttonReceiveXML_Click(object sender, RoutedEventArgs e)
        {
            //myGrid.Children.
            XDocument xml = await PostGetHTTP.XmlLoadAsync(textboxXMLCustom.Text);

            PS_ERIP erip = SerializationUtil.Deserialize<PS_ERIP>(xml);
            var resp = erip.GetListResponse;
            var paylist = resp.PayRecord;

            var model = new PageModel();
            var form = new FormAround();
            var pnl = form.panelStk;
            form.DataContext = model;


            if (paylist.Count>1)
            {
                model.PayRecords = paylist;
                foreach (var it in paylist)
                {
                    var button = new Button();
                    button.Content = it.Name;
                    button.Click += (s, arg) => 
                    {
                        PageModel mdl = (form.GetDataContext as PageModel);
                        mdl.SelectedPayRecord = it; 
                    };
                    pnl.Children.Add(button);
                }
            }
            new Window() { Content = form }.Show();
        }
    }
}
