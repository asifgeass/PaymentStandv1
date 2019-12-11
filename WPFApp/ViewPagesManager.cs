using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Views.Elements;

namespace WPFApp
{
    public class ViewPagesManager
    {
        #region fields
        private List<FrameworkElement> pages = new List<FrameworkElement>();
        private int currentPageIndex = -1;
        private List<UIElement> tempUICollection = new List<UIElement>();
        private StackPanel _stackPanel = new StackPanel();
        private const int column = 1;
        private Dictionary<int, string> headers = new Dictionary<int, string>();
        private FrameworkElement finalWrapper;
        #endregion

        #region Properties
        public FrameworkElement this[int index] => pages[index];
        public bool IsNextAvaible => this.Count - 1 > currentPageIndex;
        public bool IsPrevAvaible => currentPageIndex > 0;
        public int Count => (tempUICollection.Count > 0) ? pages.Count + 1 : pages.Count;
        public int CurrIndex => currentPageIndex;
        public string NextHeader
        {
            get
            {
                string _return = null;
                Ex.Try(false, () => _return = headers[currentPageIndex + 1]);
                return _return;
            }
        }
        public string PrevHeader
        {
            get
            {
                string _return = null;
                Ex.Try(false, () => _return = headers[currentPageIndex - 1]);
                return _return;
            }
        }
        public FrameworkElement Page
            => (currentPageIndex < 0 || currentPageIndex >= pages.Count || pages.Count == 0)
            ? null : pages[currentPageIndex];
        public FrameworkElement PreviosPage
            => (currentPageIndex < 1 || currentPageIndex >= pages.Count || pages.Count == 0)
            ? null : pages[currentPageIndex - 1];
        #endregion

        #region Public Methods
        public FrameworkElement NextPage()
        {
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(NextPage)}(): index={currentPageIndex}; count={pages.Count};");
            NewPage();
            if (IsNextAvaible)
            {
                currentPageIndex++;
            }
            return GetPage();
        }        
        public FrameworkElement PrevPage()
        {
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(PrevPage)}(): index={currentPageIndex}; count={pages.Count};");
            if (IsPrevAvaible)
            {
                currentPageIndex--;
            }
            return GetPage();
        }
        public ViewPagesManager SetToLast()
        {
            currentPageIndex = pages.Count - 1;
            return this;
        }
        public ViewPagesManager NewPage(FrameworkElement containerArg=null)
        {            
            if (tempUICollection.Count > 0)
            {
                if (finalWrapper == null) finalWrapper = containerArg ?? _stackPanel;
                pages.Add(AssemblePage());
                _stackPanel = new StackPanel();
                tempUICollection = new List<UIElement>();
                finalWrapper = null;
            }
            finalWrapper = containerArg ?? _stackPanel;
            CheckEmpty();
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(NewPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl.Children.Count}");
            Ex.Try(false, () =>
            {
                int ind = pages.Count - 1;
                //Ex.Log($"ViewPagesManager.NewPage(): page[{ind}]={(pages[ind].Children[0] as ContentControl).Content};");
                //Ex.Log($"ViewPagesManager.NewPage(): page[{ind-1}]={(pages[ind-1].Children[0] as ContentControl).Content};");
            });
            return this;
        }
        public ViewPagesManager AddControl(FrameworkElement arg)
        {
            CheckEmpty();
            tempUICollection.Add(arg);            
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(AddControl)}(): pages={pages.Count}; tempPanel.Children={tempPnl?.Children?.Count}");
            return this;
        }
        public ViewPagesManager SetHeader(string arg)
        {
            Ex.Try(false, () => headers[pages.Count] = arg);            
            return this;
        }
        public ViewPagesManager AddDataContext(object arg)
        {
            Ex.Log($"ViewPagesManager.AddDataContext(): pages={pages.Count}; tempPanel.Children={tempUICollection?.Count}");
            finalWrapper.DataContext = arg;
            return this;
        }
        #endregion
        
        #region Private Methods
        private FrameworkElement GetPage()
        {
            return pages[currentPageIndex];
        }
        private void CheckEmpty()
        {
            if (pages.Count <= 0)
            {
                //pages.Add(grid);
            }
        }
        private FrameworkElement AssemblePage()
        {
            if (finalWrapper is TilePanelNoScroller)
            {
                if(tempUICollection.Count>4)
                {
                    var tilePanel = finalWrapper as TilePanelNoScroller;
                    tilePanel.ItemsSource = tempUICollection;
                    tilePanel.DataContext = finalWrapper.DataContext;
                    return tilePanel;
                }
                else
                {
                    _stackPanel.DataContext = finalWrapper.DataContext;
                    tempUICollection.ForEach(x => _stackPanel.Children.Add(x));
                    var returnWrap = WrapIntoDefaultGridScroller(_stackPanel);
                    return returnWrap;
                }
            }
            else if (finalWrapper is Panel)
            {
                var panel = finalWrapper as Panel;
                tempUICollection.ForEach(x => panel.Children.Add(x));
                var returnWrap = WrapIntoDefaultGridScroller(panel);                
                return returnWrap;
            }
            else return null;
        }
        private FrameworkElement WrapIntoDefaultGridScroller(Panel panel)
        {
            var grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(_stackPanel, column);
            grid.Children.Add(_stackPanel);
            var scroller = new ScrollViewer()
            { 
                PanningMode = PanningMode.VerticalOnly
                , CanContentScroll=true
            };
            scroller.Content = grid;
            return scroller;
        }
        #endregion
    }
}
