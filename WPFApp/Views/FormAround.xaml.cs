using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPFApp.Views;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for FormAround.xaml
    /// </summary>
    public partial class FormAround : UserControl
    {
        public FormAround()
        {
            InitializeComponent();
        }
        public RoutedEventHandler ScrollerLoadedEvent = (s,e) => { };
        public object GetDataContext => this.DataContext;
        public Visibility ScrollerBarVisibility => scroller1.ComputedVerticalScrollBarVisibility;

        private void SetUIElementCollection(UIElementCollection childrenArg)
        {
            wrapPanel1.Children.Clear();
            foreach (UIElement item in childrenArg)
            {                
                this.wrapPanel1.Children.Add(item);
            }
        }
        public void SetContent(WrapPanelItemsCount elementArg)
        {
            //wrapPanel1 = elementArg as WrapPanelItemsCount;
            scroller1.Content = elementArg;
        }
        public void SetContent(UIElement elementArg)
        {            
            if (elementArg is WrapPanelItemsCount)
            {                
                SetContent(elementArg as WrapPanelItemsCount);
            }
            else
            {
                wrapPanel1.Children.Clear();
                this.wrapPanel1.Children.Add(elementArg);
            }
        }

        private void MainGrid_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        
        private void scroller_Loaded(object sender, RoutedEventArgs e)
        {
            //if(scroller1.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            //{
            //    Debug.WriteLine("panelStk_Loaded SCROLLING BAR IS VISIBLE!!!");
            //    if (scroller1.Content is WrapPanelItemsCount)
            //    {
            //        var wraper = scroller1.Content as WrapPanelItemsCount;
            //        if (wraper.MaxItemsCount == 1) wraper.MaxItemsCount = 2;
            //    }
            //}
            ScrollerLoadedEvent(sender, e);
            this.UpdateLayout();
        }
    }
}
