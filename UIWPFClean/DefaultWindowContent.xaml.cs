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
    /// Interaction logic for DefaultWindowContent.xaml
    /// </summary>
    public partial class DefaultWindowContent : UserControl
    {
        public DefaultWindowContent()
        {
            InitializeComponent();
        }

        private FrameworkElement PaymentScreenV2()
        {
            var columnGrid = new Grid() { ColumnDefinitions =
                {
                    new ColumnDefinition() { Width = new GridLength(250) }
                    , new ColumnDefinition() { Width = new GridLength(250) }
                    , new ColumnDefinition() { Width = new GridLength(250) }
                }
            };
            columnGrid.HorizontalAlignment = HorizontalAlignment.Center;

            var img1 = new Image();
            img1.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/true_png.png"));
            img1.Stretch = Stretch.Uniform;
            img1.Opacity = 0.5;
            var img2 = new Image();
            img2.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_chip3.png"));
            img2.Stretch = Stretch.Uniform;
            img2.Opacity = 0.5;
            var img3 = new Image();
            img3.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_side.png"));
            img3.Stretch = Stretch.Uniform;
            img3.Opacity = 0.5;

            columnGrid.Children.Add(img1);
            columnGrid.Children.Add(img2);
            columnGrid.Children.Add(img3);
            Grid.SetColumn(img1, 0);
            Grid.SetColumn(img2, 1);
            Grid.SetColumn(img3, 2);

            var rowGrid = new Grid() { RowDefinitions = 
            { 
                new RowDefinition() { Height = new GridLength(250) }
                , new RowDefinition() { Height = new GridLength(10, GridUnitType.Auto) }
                , new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) }
            } };
            rowGrid.VerticalAlignment = VerticalAlignment.Center;

            var bar = new ProgressBar();
            bar.IsIndeterminate = true;
            bar.Margin = new Thickness(50);
            var text = new TextBlock()
            {
                Text = "Пожалуйста, поднесите карту к POS-терминалу и совершите оплату любым способом."
                , TextWrapping = TextWrapping.Wrap
                , TextAlignment = TextAlignment.Center
            };

            var panel = new StackPanel();
            panel.Children.Add(bar);
            panel.Children.Add(text);

            rowGrid.Children.Add(columnGrid);
            rowGrid.Children.Add(panel);
            Grid.SetRow(columnGrid, 0);
            Grid.SetRow(panel, 1);

            return rowGrid;
        }

        private void buttonPayScreen_Click(object sender, RoutedEventArgs e)
        {
            //this.Content = PaymentScreen(); //PaymentScreenV2
            this.Content = PaymentScreenV2();
        }

        private static FrameworkElement PaymentScreen()
        {
            var columnGrid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol1.Width = new GridLength(30, GridUnitType.Star);
            gridCol2.Width = new GridLength(50, GridUnitType.Star);
            gridCol3.Width = new GridLength(250);
            columnGrid.ColumnDefinitions.Add(gridCol1);
            columnGrid.ColumnDefinitions.Add(gridCol2);
            columnGrid.ColumnDefinitions.Add(gridCol3);

            var bar = new ProgressBar();
            bar.IsIndeterminate = true;
            bar.Margin = new Thickness(50);

            var imgLeft = new Image();
            imgLeft.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/true_png.png"));
            imgLeft.Stretch = Stretch.Uniform;
            
            var imgRight = new Image();
            imgRight.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_chip3.png"));
            imgRight.Stretch = Stretch.None;
            
            var imgRight2 = new Image();
            imgRight2.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_side.png"));
            imgRight2.Stretch = Stretch.None;

            imgLeft.Opacity = 0.85;
            imgRight.Opacity = 0.85;
            imgRight2.Opacity = 0.85;

            var centerGrid = new Grid();
            ColumnDefinition Col1 = new ColumnDefinition();
            ColumnDefinition Col2 = new ColumnDefinition();
            ColumnDefinition Col3 = new ColumnDefinition();
            gridCol1.Width = new GridLength(20, GridUnitType.Star);
            gridCol2.Width = new GridLength(50, GridUnitType.Star);
            gridCol3.Width = new GridLength(20, GridUnitType.Star);
            centerGrid.Children.Add(bar);
            Grid.SetColumn(bar, 1);

            var center = new StackPanel();
            center.VerticalAlignment = VerticalAlignment.Center;
            center.Children.Add(new TextBlock()
            {
                Text = "Пожалуйста, поднесите карту к POS-терминалу и совершите оплату любым способом."
                ,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            });
            center.Children.Add(centerGrid);

            var doublePic = new StackPanel();
            doublePic.VerticalAlignment = VerticalAlignment.Center;
            doublePic.Children.Add(imgRight);
            doublePic.Children.Add(imgRight2);

            columnGrid.Children.Add(imgLeft);
            columnGrid.Children.Add(center);
            columnGrid.Children.Add(doublePic);
            Grid.SetColumn(imgLeft, 0);
            Grid.SetColumn(center, 1);
            Grid.SetColumn(doublePic, 2);
            return columnGrid;
        }

        private void ButtonKeyboard_Click(object sender, RoutedEventArgs e)
        {
            var textbox = new TextBox();
            HintAssist.SetHint(textbox, "Number");
            textbox.InputScope = new InputScope()
            {
                Names = { new InputScopeName(InputScopeNameValue.Number) }
            };
            StackPanel1.Children.Add(textbox);

            textbox = new TextBox();
            HintAssist.SetHint(textbox, "NumberFullWidth");
            textbox.InputScope = new InputScope()
            {
                Names = { new InputScopeName(InputScopeNameValue.NumberFullWidth) }
            };
            StackPanel1.Children.Add(textbox);

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window();
            win.WindowStyle = WindowStyle.None;
            win.Show();
        }
    }
}
