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

namespace WPFApp
{
    public static class Controls
    {
        private const double Margin = 50;
        private const double MarginBetween = 27;
        public static Label CentralLabelBorder(string arg = "")
        {
            var info = new Label();
            info.HorizontalContentAlignment = HorizontalAlignment.Center;
            info.VerticalContentAlignment = VerticalAlignment.Center;
            info.BorderThickness = new Thickness(2);
            info.BorderBrush = Brushes.Red;
            info.Margin = info.BorderThickness;
            var text = new TextBlock();
            text.Text = arg;
            text.TextWrapping = TextWrapping.Wrap;
            info.Content = text;
            return info;
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

        public static Label LabelInfo(string argName = null)
        {
            var label = new Label();
            if (argName != null) label.Content = argName;
            return label;
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
    }
}
