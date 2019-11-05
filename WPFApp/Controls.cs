using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using XmlStructureComplat;
using System.Windows.Controls;
using System.Windows;

namespace WPFApp
{
    public static class Controls
    {
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

        public static Button Button(string argName=null)
        {
            var button = new Button();
            if(argName!=null) button.Content = argName;
            button.Margin = new Thickness(0,0,0, button.Height/2);
            return button;
        }

        public static Button ButtonAccept(string argName = null)
        {
            var button = new Button();
            if (argName != null) button.Content = argName;
            button.Margin = new Thickness(0, 0, 0, button.Height / 2);
            button.FontSize = new TextBlock().FontSize;
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
    }
}
