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
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes;
using MaterialDesignColors;
using WPFApp.ViewModels;

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        private DynamicMenuWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.vm = new DynamicMenuWindowViewModel();
            this.DataContext = this.vm;
        }

        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
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
            imgLeft.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_ContactlessBlack.png"));
            imgLeft.Stretch = Stretch.Uniform;
            var imgRight = new Image();
            imgRight.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_chip3.png"));
            imgRight.Stretch = Stretch.None;
            var imgRight2 = new Image();
            imgRight2.Source = new BitmapImage(new Uri(@"pack://application:,,,/UIWPFClean;component/Resources/POS_side.png"));
            imgRight2.Stretch = Stretch.None;

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

            this.Content = columnGrid;
        }
    }
}
