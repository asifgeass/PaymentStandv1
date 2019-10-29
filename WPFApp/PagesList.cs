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
        private List<Panel> pages = new List<Panel>();
        private int currentPageIndex = 0;
        private StackPanel tempPnl = new StackPanel();
        #endregion
        #region Public Methods
        #region Get Page
        public PagesList AddPage(Panel arg)
        {
            pages.Add(arg);
            return this;
        }
        public Panel NextPage()
        {
            //var retur = pages[currentPageIndex];
            if (pages.Count - 1 > currentPageIndex)
            {
                currentPageIndex++;
            }
            return pages[currentPageIndex];
        }
        #endregion
        public PagesList NewPage()
        {
            if(tempPnl.Children.Count > 0)
            {
                pages.Add(tempPnl);
                ClearPnl();
            }
            if (pages.Count <= 0)
            {
                pages.Add(tempPnl);
            }
            return this;
        }
        public PagesList AddControl(FrameworkElement arg)
        {
            if (pages.Count <= 0)
            {
                pages.Add(tempPnl);
            }
            tempPnl.Children.Add(arg);
            return this;
        }
        #endregion
        #region Properties
        public Panel this[int index] => pages[index];
        public bool IsNextAvaible => pages.Count-1 > currentPageIndex;
        public int Count { get => (tempPnl.Children.Count==0) ? pages.Count-1 : pages.Count; }
        public Panel Page
            => (currentPageIndex < 0 || currentPageIndex >= pages.Count || pages.Count==0)
            ? null : pages[currentPageIndex];

        public Panel PrevPage
            => (currentPageIndex < 1 || currentPageIndex >= pages.Count || pages.Count == 0)
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
