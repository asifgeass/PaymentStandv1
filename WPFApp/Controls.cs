using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using XmlStructureComplat;
using System.Windows.Controls;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using WPFApp.Views;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    public static class Controls
    {
        private const double Margin = 50;
        private const double MarginBetween = 27;
        public static Label CentralLabelBorder(string arg = "", SolidColorBrush brushArg=null)
        {
            var lbl = new Label();
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
            lbl.Foreground = brushArg ?? lbl.Foreground;
            var textBlock = new TextBlock();
            textBlock.Text = arg;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextAlignment = TextAlignment.Center;
            lbl.Content = textBlock;
            return lbl;
        }

        public static PackIcon IconBig(PackIconKind iconKindArg, SolidColorBrush brushArg =null)
        {
            var pic = new PackIcon();
            pic.Kind = iconKindArg;
            pic.Height = 250;
            pic.Width = 250;
            pic.HorizontalAlignment = HorizontalAlignment.Center;
            pic.VerticalAlignment = VerticalAlignment.Center;
            pic.Foreground = brushArg ?? pic.Foreground;
            return pic;
        }

        public static CardButton ButtonCard(string argName=null)
        {
            var cardButton = new CardButton();
            if(argName!=null) cardButton.Text = argName; 
            //button.Style = Application.Current.TryFindResource("CardButton") as Style;            
            cardButton.Margin = new Thickness(0, MarginBetween, 0, MarginBetween);       
            return cardButton;
        }

        public static ButtonTextblock ButtonAccept(string argName = null)
        {
            var button = new ButtonTextblock();
            if (argName != null) button.Text = argName;
            button.Margin = new Thickness(0, MarginBetween, 0, MarginBetween);
            return button;
        }

        public static TextBlock LabelInfo(string argName = null)
        {
            var txtBlock = new TextBlock();
            if (argName != null) txtBlock.Text = argName;
            txtBlock.TextWrapping = TextWrapping.Wrap;
            txtBlock.FontSize = 38;
            return txtBlock;
        }

        public static Label LabelHeader(string argName = null)
        {
            var label = new Label();
            if (argName != null) label.Content = argName;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(0, MarginBetween, 0, MarginBetween);
            return label;
        }

        public static TextBox TextBoxHint(string hintArg, string nameArg = null)
        {
            var textBox = new TextBox();
            textBox.Margin = new Thickness(0, MarginBetween, 0, MarginBetween);
            if (hintArg != null) HintAssist.SetHint(textBox, hintArg);
            if (nameArg != null) textBox.Text = nameArg;            
            return textBox;
        }

        public static Grid PayScreen()
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
            imgLeft.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/POS_ContactlessBlack.png"));
            imgLeft.Stretch = Stretch.Uniform;
            var imgRight = new Image();
            imgRight.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/POS_chip1.png"));
            imgRight.Stretch = Stretch.None;
            var imgRight2 = new Image();
            imgRight2.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/POS_side.png"));
            imgRight2.Stretch = Stretch.None;

            var center = new StackPanel();
            center.VerticalAlignment = VerticalAlignment.Center;
            center.Children.Add(new TextBlock()
            {
                Text = "Пожалуйста, поднесите карту к POS-терминалу и совершите оплату любым способом."
                ,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            });
            center.Children.Add(bar);

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
    }
}
