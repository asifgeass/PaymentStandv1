using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    public class PagesList
    {
        #region fields
        private List<FrameworkElement> pages = new List<FrameworkElement>();
        private int currentPageIndex = -1;
        private StackPanel tempPnl = new StackPanel();
        #endregion
        #region Public Methods
        #region Get Page
        public PagesList AddPage(FrameworkElement arg)
        {
            pages.Add(arg);
            return this;
        }
        public PagesList NextPage()
        {
            if (pages.Count-1 > currentPageIndex)
            {
                currentPageIndex++;
            }
            return this;
        }
        public PagesList BackPage()
        {
            if (currentPageIndex > 0)
            { 
                currentPageIndex--; 
            }
            return this;
        }
        #endregion
        public PagesList New()
        {
            if(tempPnl.Children.Count > 0)
            {
                pages.Add(tempPnl);
                ClearPnl();
            }
            return this;
        }
        public PagesList AddControl(FrameworkElement arg)
        {
            tempPnl.Children.Add(arg);
            return this;
        }
        #endregion
        #region Properties
        public bool IsNextAvaible => pages.Count-1 > currentPageIndex;
        public int Count { get => pages.Count; }
        public FrameworkElement Page
            => (currentPageIndex < 0 || currentPageIndex >= pages.Count)
            ? null : pages[currentPageIndex];

        public FrameworkElement PrevPage
            => (currentPageIndex < 1 || currentPageIndex >= pages.Count)
            ? null : pages[currentPageIndex - 1];
        #endregion
        #region Private Methods
        private void ClearPnl()
        {
            tempPnl = new StackPanel();
        }
        #endregion
    }
}
