using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    public class ViewPagesManager
    {
        #region fields
        private List<Panel> pages = new List<Panel>();
        private int currentPageIndex = -1;
        private StackPanel tempPnl = new StackPanel();
        #endregion
        #region Public Methods
        #region Get Page
        public ScrollViewer NextPage()
        {
            Trace.WriteLine($"{nameof(ViewPagesManager)}.{nameof(NextPage)}(): index={currentPageIndex}; count={pages.Count};");
            if (IsNextAvaible)
            {
                currentPageIndex++;
            }
            return new ScrollViewer() { Content = pages[currentPageIndex] };
        }
        #endregion
        public ViewPagesManager NewPage()
        {
            if(tempPnl.Children.Count > 0)
            {
                tempPnlClear();
                pages.Add(tempPnl);
            }
            CheckEmpty();
            Trace.WriteLine($"{nameof(ViewPagesManager)}.{nameof(NewPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl.Children.Count}");
            try 
            { 
                Trace.WriteLine($"page[0]={(pages[0].Children[0] as ContentControl).Content};");
                Trace.WriteLine($"page[1]={(pages[1].Children[0] as ContentControl).Content};");
            }
            catch (Exception){}
            return this;
        }
        public ViewPagesManager AddControl(FrameworkElement arg)
        {
            CheckEmpty();
            tempPnl.Children.Add(arg);
            tempPnl.Children.Add(new TextBlock());//TEMPORAL MARGIN
            Trace.WriteLine($"{nameof(ViewPagesManager)}.{nameof(AddControl)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            return this;
        }

        private void CheckEmpty()
        {
            if (pages.Count <= 0)
            {
                pages.Add(tempPnl);
            }
        }

        public ViewPagesManager AddDataContext(object arg)
        {
            Trace.WriteLine($"{nameof(ViewPagesManager)}.{nameof(AddDataContext)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            tempPnl.DataContext = arg;
            return this;
        }
        public ViewPagesManager AddPage(Panel arg)
        {
            pages.Add(arg);
            Trace.WriteLine($"{nameof(ViewPagesManager)}.{nameof(AddPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            return this;
        }
        #endregion
        #region Properties
        public Panel this[int index] => pages[index];
        public bool IsNextAvaible => pages.Count-1 > currentPageIndex;
        public int Count  => (tempPnl.Children.Count<=0) ? pages.Count-1 : pages.Count;
        public int CurrIndex => currentPageIndex;
        public Panel Page
            => (currentPageIndex < 0 || currentPageIndex >= pages.Count || pages.Count==0)
            ? null : pages[currentPageIndex];

        public Panel PrevPage
            => (currentPageIndex < 1 || currentPageIndex >= pages.Count || pages.Count == 0)
            ? null : pages[currentPageIndex - 1];
        #endregion
        #region Private Methods
        private void tempPnlClear()
        {
            tempPnl = new StackPanel();
        }
        #endregion
    }
}
