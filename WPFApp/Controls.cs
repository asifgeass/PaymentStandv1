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
