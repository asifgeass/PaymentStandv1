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

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = new TextBox();
            HintAssist.SetHint(txtbox, "Hint #"+count);

            Button btn = new Button();

            btn.BorderBrush = null;
            btn.Background = Brushes.Gray;
            btn.SetResourceReference(Button.BackgroundProperty, "MaterialDesignPaper");
            btn.Content = $"Added {count} button";
            ShadowAssist.SetShadowDepth(btn, ShadowDepth.Depth3);
            ShadowAssist.SetShadowEdges(btn, ShadowEdges.Right);
            ShadowAssist.SetShadowEdges(btn, ShadowEdges.Top);
            SolidColorBrush brush = Application.Current.TryFindResource("PrimaryHueDarkBrush") as SolidColorBrush;
            RippleAssist.SetFeedback(btn, brush);
            //HintAssist.SetFloatingScale
            
            //ShadowAssist.SetShadowEdges(btn, ShadowEdges.Bottom);
            btn.SetResourceReference(Button.ForegroundProperty, "MaterialDesignBody");
            //btn.SetResourceReference(Button.FontFamilyProperty, "MaterialDesignFont");

            this.StackPanel1.Children.Add(new TextBlock());
            this.StackPanel1.Children.Add(btn);
            count++;
        }
    }
}
