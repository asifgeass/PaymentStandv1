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
            info.BorderBrush = System.Windows.Media.Brushes.Black;
            info.Margin = info.BorderThickness;
            var text = new TextBlock();
            text.Text = arg;
            text.TextWrapping = TextWrapping.Wrap;
            info.Content = text;
            return info;
        }

        public static Button ButtonSelect(PayRecord payrec)
        {
            var button = new Button();
            button.Content = payrec.Name;
            //button.Command = model.SendParamCommand;
            button.CommandParameter = payrec;
            return button;
        }
    }
}
