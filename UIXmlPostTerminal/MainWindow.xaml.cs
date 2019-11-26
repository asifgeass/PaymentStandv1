using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
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
using Logic;
using XmlStructureComplat;

namespace UIXmlPostTerminal
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
            try
            {
                StaticMain.LoadSettings();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void WriteToTextbox(string arg)
        {
            textboxResponceXML.AppendText(arg + Environment.NewLine + Environment.NewLine);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> strings = new List<string>() { "0.01", "0,01", "0", "1" };
            strings.ForEach(x =>
            {
                decimal dec = decimal.Parse(x.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture);
                WriteToTextbox(dec.ToString());
            });

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonXMLCustom_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string url = StaticMain.Settings.ERIP.url;
                PostGetHTTP.PostStringGetString(url, textboxXMLCustom.Text);
            }
            catch (Exception ex)
            {
                WriteToTextbox(ex?.InnerException?.Message ?? ex.Message);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            textboxResponceXML.Clear();
        }

        private async void buttonReceiveXML_Click(object sender, RoutedEventArgs e)
        {
            //myGrid.Children.
            //XDocument xml = await PostGetHTTP.XmlLoadAsync(textboxXMLCustom.Text);

            //PS_ERIP rootResponse = await SerializationUtil.Deserialize<PS_ERIP>(xml);
            //var resp = rootResponse.GetListResponse;
            //var paylist = resp.PayRecord;

            //var model = new PageVM();
            //var form = new FormAround();
            //form.DataContext = model;

            //if (paylist.Count>1)
            //{
            //    model.PayRecords = paylist;
            //    foreach (var it in paylist)
            //    {
            //        var button = new Button();
            //        button.Content = it.Name;
            //        button.DataContext = it;
            //        //button.Command = model.CurrMenuHeader;
            //        //button.CommandParameter = it.Name;
            //        //button.SetBinding(Button.CommandProperty, new Binding()
            //        //{ Source = model.SetCurrMenuHeader });                    
            //        button.SetBinding(Button.CommandProperty, "SetCurrMenuHeader");
            //        button.SetBinding(Button.CommandParameterProperty, new Binding()
            //        { Source = it.Name });

            //        form.Controls.Add(button);
            //        form.Controls.Add(new Label());
            //    }
            //}

            //if (paylist.Count == 1)
            //{
            //    var attrRecords = paylist.First().AttrRecord;
            //    model.AttrRecords = attrRecords;

            //    foreach (var it in attrRecords)
            //    {
            //        if (it.Edit == 1 && it.View == 1 && !string.IsNullOrEmpty(it.Lookup))
            //        {
            //            List<Lookup> lookups = paylist.First().Lookups;
            //            Lookup selectedLookup = lookups.Where(x => x.Name.ToLower() == it.Lookup.ToLower()).Single();
            //            form.Controls.Add(new Label()
            //            {
            //                Content= selectedLookup.Name,
            //                HorizontalContentAlignment = HorizontalAlignment.Center
            //            });
            //            List<LookupItem> lookItems = selectedLookup.Item;
            //            lookItems.ForEach(x =>
            //            {
            //                form.Controls.Add(new Button() 
            //                { 
            //                    Content=$"{x.Value}", 
            //                });
            //                form.Controls.Add(new Label());
            //            });
            //            new Window() { Content = form, Width=450, Height=800 }.Show();
            //            form = new FormAround();
            //        }
            //    }

            //    foreach (var it in attrRecords)
            //    {
            //        if(it.Edit != 1 && it.View == 1)
            //        {
            //            var label = new Label();
            //            label.Content = $"{it.Name} = {it.Value}";

            //            form.Controls.Add(label);
            //            form.Controls.Add(new Label());
            //        }
            //    }
            //    foreach (var it in attrRecords)
            //    {
            //        if (it.Edit == 1 && it.View == 1 && string.IsNullOrEmpty(it.Lookup))
            //        {
            //            var label = new Label();
            //            label.Content = $"{it.Name}:";
            //            var inputbox = new TextBox();

            //            form.Controls.Add(label);
            //            form.Controls.Add(inputbox);
            //            form.Controls.Add(new Label());
            //        }
            //    }

            //    form.Controls.Add(new Label());
            //    form.Controls.Add(new Label());
            //    form.Controls.Add(new Button() { Content="Продолжить" });
            //}

            //new Window() { Content = form }.Show();
        }

        private void buttonTerminalPost_Click(object sender, RoutedEventArgs e)
        {
            string url = StaticMain.Settings.Terminal_MdomPOS.url;
            string request = $"xml=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n{textboxXMLCustom.Text}";
            try
            {
                PostGetHTTP.PostStringGetXML(url, request);
            }
            catch (Exception ex)
            {
                WriteToTextbox(ex?.InnerException?.Message ?? ex.Message);
            }
        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            var sets = new PrinterSettings();

            //IEnumerable<PaperSize> paperSizes = sets.PaperSizes.Cast<PaperSize>();
            //PaperSize mySizePaper = paperSizes.First<PaperSize>(size => size.Kind == PaperKind.A6); ; // setting paper size to A4 size
            //printDocument.DefaultPageSettings.PaperSize = mySizePaper;
            //printDocument.PrinterSettings = sets;

            //printDocument.DefaultPageSettings.PaperSize = new PaperSize("80 x 80 mm", 400, 400);             

            string str = textboxXMLCustom.Text;
            string path = $@"{Environment.CurrentDirectory}\Resources\Courier.ttf";
            //Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UIXmlPostTerminal.Courier.ttf");

            bool ist = File.Exists(path);

            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddFontFile(path);
            var fontFam = privateFontCollection.Families.First();

            Font font;

            font = new Font(fontFam, 9f);
            //font = new Font("Courier", 8.8f);

            printDocument.PrintPage += (s, arg) => arg.Graphics.DrawString(str, font, System.Drawing.Brushes.Black, 0, 0);
            printDocument.Print();
        }

        private void googlePrint()
        {
            // задаем текст для печати
            var print = textboxXMLCustom.Text;
            // объект для печати
            PrintDocument printDocument = new PrintDocument();

            // обработчик события печати
            printDocument.PrintPage += PrintPageHandler;
            printDocument.Print();

            // диалог настройки печати
            PrintDialog printDialog = new PrintDialog();

            // установка объекта печати для его настройки
            //printDialog.Document = printDocument;
            //printDialog.PrintDocument();

            // если в диалоге было нажато ОК
            //if (printDialog.ShowDialog() == DialogResult.OK)
            //    printDialog.Document.Print(); // печатаем
        }

        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // печать строки result
            e.Graphics.DrawString(textboxXMLCustom.Text, new Font("Arial", 12), System.Drawing.Brushes.Black, 0, 0);
        }
    }
}
