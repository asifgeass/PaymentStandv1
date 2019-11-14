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
        public static Label CentralLabelBorder(string arg = "")
        {
            var info = new Label();
            info.HorizontalContentAlignment = HorizontalAlignment.Center;
            info.VerticalContentAlignment = VerticalAlignment.Center;
            info.BorderThickness = new Thickness(2);
            info.BorderBrush = System.Windows.Media.Brushes.Red;
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
            cardButton.Margin = new Thickness(cardButton.Margin.Left, cardButton.Margin.Top, cardButton.Margin.Right, Margin);            
            return cardButton;
        }

        public static Button ButtonAccept(string argName = null)
        {
            var button = new Button();
            if (argName != null) button.Content = argName;
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
            return label;
        }

        public static TextBox TextBox(string argName = null)
        {
            var TextBox = new TextBox();
            if (argName != null) TextBox.Text = argName;
            return TextBox;
        }

        public static TextBox TextBoxHint(string hintArg, string nameArg = null)
        {
            var textBox = new TextBox();
            textBox.Margin = new Thickness(textBox.Margin.Left, textBox.Margin.Top, textBox.Margin.Right, Margin);
            if (hintArg != null) HintAssist.SetHint(textBox, hintArg);
            if (nameArg != null) textBox.Text = nameArg;            
            return textBox;
        }
    }
}
