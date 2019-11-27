using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Views;

namespace WPFApp
{
    public class ViewPagesManager
    {
        #region fields
        private List<Panel> pages = new List<Panel>();
        private int currentPageIndex = -1;
        Panel returnWraper;
        private WrapPanelItemsCount tempPnl = new WrapPanelItemsCount();
        private const int column = 1;
        #endregion

        #region Public Methods
        public Panel NextPage()
        {
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(NextPage)}(): index={currentPageIndex}; count={pages.Count};");
            if (IsNextAvaible)
            {
                currentPageIndex++;
            }
            return GetPage();
        }
        
        public Panel PrevPage()
        {
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(PrevPage)}(): index={currentPageIndex}; count={pages.Count};");
            if (IsPrevAvaible)
            {
                currentPageIndex--;
            }
            return GetPage();
        }

        public ViewPagesManager NewPage()
        {
            if (tempPnl.Children.Count > 0)
            {
                BuildWraper();
                pages.Add(returnWraper);
            }
            CheckEmpty();
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(NewPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl.Children.Count}");
            Ex.Try(false, () =>
            {
                Ex.Log($"page[0]={(pages[0].Children[0] as ContentControl).Content};");
                Ex.Log($"page[1]={(pages[1].Children[0] as ContentControl).Content};");
            });
            return this;
        }
        public ViewPagesManager AddControl(FrameworkElement arg)
        {
            CheckEmpty();
            //var gridItem = WrapToGrid(arg);
            tempPnl.Children.Add(arg);
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(AddControl)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            return this;
        }

        public ViewPagesManager AddDataContext(object arg)
        {
            Ex.Log($"{nameof(ViewPagesManager)}.{nameof(AddDataContext)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            returnWraper.DataContext = arg;
            return this;
        }

        #endregion

        #region Properties
        public Panel this[int index] => pages[index];
        public bool IsNextAvaible => pages.Count-1 > currentPageIndex;
        public bool IsPrevAvaible => 0 < currentPageIndex;
        public int Count  => (tempPnl.Children.Count<=0) ? pages.Count-1 : pages.Count;
        public int CurrIndex => currentPageIndex;
        public Panel Page
            => (currentPageIndex < 0 || currentPageIndex >= pages.Count || pages.Count==0)
            ? null : pages[currentPageIndex];

        public Panel PreviosPage
            => (currentPageIndex < 1 || currentPageIndex >= pages.Count || pages.Count == 0)
            ? null : pages[currentPageIndex - 1];
        #endregion

        #region Private Methods
        private Panel BuildWraper()
        {
            tempPnl = new WrapPanelItemsCount();
            tempPnl.HorizontalAlignment = HorizontalAlignment.Center;
            tempPnl.VerticalAlignment = VerticalAlignment.Center;
            tempPnl.MaxItemsCount = 1;
            tempPnl.Orientation = Orientation.Vertical;
            returnWraper = tempPnl;            
            return returnWraper;
        }
        private Grid GridCreate(double left, double center, double right)
        {
            var grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(left, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(center, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(right, GridUnitType.Star) });
            //grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(left, GridUnitType.Star) });
            //grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(center, GridUnitType.Star) });
            //grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(right, GridUnitType.Star) });
            return grid;
        }
        private Grid WrapToGrid(FrameworkElement arg)
        {
            Grid grid = GridCreate(1, 7, 1);
            grid.Children.Add(arg);
            Grid.SetColumn(arg, column);
            Grid.SetRow(arg, column);
            return grid;
        }
        private ViewPagesManager AddPage(Panel arg)
        {
            pages.Add(arg);
            Ex.Log($"{nameof(ViewPagesManager)}.{nameof(AddPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            return this;
        }
        private Panel GetPage()
        {
            return pages[currentPageIndex];
        }
        private void CheckEmpty()
        {
            if (pages.Count <= 0)
            {
                pages.Add(returnWraper);
            }
        }

        #endregion

        public ViewPagesManager()
        {
            BuildWraper();
        }
    }
}
