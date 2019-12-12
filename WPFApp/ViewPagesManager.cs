using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WPFApp.Helpers;
using WPFApp.Views.Elements;

namespace WPFApp
{
    public class ViewPagesManager
    {
        #region fields
        private List<FrameworkElement> pages = new List<FrameworkElement>();
        private int currentPageIndex = -1;
        private List<UIElement> tempUICollection = new List<UIElement>();
        private const int column = 1;
        private Dictionary<int, string> headers = new Dictionary<int, string>();
        //private FrameworkElement finalWrapper;
        private bool isPanelAdd=false;
        private FrameworkElement tempPanel;
        private object tempDataContext = null;
        private List<UIElement> subTempPanelList = new List<UIElement>();
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
        public ViewPagesManager NewPage()
        {
            Ex.Log($"ViewPagesManager.NewPage() UIcount={tempUICollection.Count}");
            if (tempUICollection.Count > 0)
            {
                //if (finalWrapper == null) finalWrapper = containerArg ?? _stackPanel;
                this.EndContainer();
                pages.Add(AssemblePage());
                tempUICollection = new List<UIElement>();
                tempDataContext = null;
                //finalWrapper = null;
            }
            //finalWrapper = containerArg ?? _stackPanel;
            //Ex.Log($"{nameof(ViewPagesManager)}.{nameof(NewPage)}(): pages={pages.Count}; tempPanel.Children={tempPnl.Children.Count}");
            Ex.Try(false, () =>
            {
                int ind = pages.Count - 1;
                //Ex.Log($"ViewPagesManager.NewPage(): page[{ind}]={(pages[ind].Children[0] as ContentControl).Content};");
                //Ex.Log($"ViewPagesManager.NewPage(): page[{ind-1}]={(pages[ind-1].Children[0] as ContentControl).Content};");
            });
            return this;
        }
        public ViewPagesManager SetContainer<T>(T containerArg) where T: FrameworkElement, IAddChild
        {
            Ex.Log($"ViewPagesManager.SetContainer()");
            if (isPanelAdd) this.EndContainer();

            tempPanel = containerArg;
            tempUICollection.Add(tempPanel);
            isPanelAdd = true;
            return this;
        }
        public ViewPagesManager EndContainer() 
        {
            Ex.Log($"ViewPagesManager.EndContainer()");
            if (tempPanel is iItemsSource)
            {
                if(subTempPanelList.Count >= 4)
                {
                    var source = tempPanel as iItemsSource;
                    source.ItemsSource = subTempPanelList;
                }
                else
                {
                    var stack = new StackPanel();
                    subTempPanelList.ForEach(x => stack.Children.Add(x));
                    int index = tempUICollection.FindIndex(ind => ind.Equals(tempPanel));
                    if(index>=0)
                    {
                        tempUICollection[index] = stack;
                    }
                }
                subTempPanelList = new List<UIElement>();
            }
            isPanelAdd = false;
            tempPanel = new FrameworkElement();
            return this;
        }
        public ViewPagesManager AddControl(FrameworkElement controlArg)
        {
            Ex.Log($"ViewPagesManager.AddControl({controlArg}) panel={isPanelAdd}");          
            if (isPanelAdd)
            {
                if (tempPanel is Panel)
                {
                    var panel = tempPanel as Panel;
                    panel.Children.Add(controlArg);
                }
                else if (tempPanel is iItemsSource)
                {
                    subTempPanelList.Add(controlArg);
                }
                else Ex.Error($"ViewPagesManager.AddControl() unknown panel={tempPanel}; arg={controlArg}");
            }
            else
            {
                tempUICollection.Add(controlArg);
            }

            return this;
        }
        public ViewPagesManager SetHeader(string arg)
        {
            Ex.Try(false, () => headers[pages.Count] = arg);            
            return this;
        }
        public ViewPagesManager AddDataContext(object dataContextArg)
        {
            Ex.Log($"ViewPagesManager.AddDataContext(): tempUICollection={tempUICollection?.Count}");
            tempDataContext = dataContextArg;
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
            //return FinalWrapAssemblePage();
            return WrapIntoStackPanel();
        }
        private FrameworkElement FinalWrapAssemblePage()
        {
            //if (finalWrapper is TilePanelNoScroller)
            //{
            //    if (tempUICollection.Count > 4)
            //    {
            //        var tilePanel = finalWrapper as TilePanelNoScroller;
            //        tilePanel.ItemsSource = tempUICollection;
            //        tilePanel.DataContext = finalWrapper.DataContext;
            //        return tilePanel;
            //    }
            //    else
            //    {
            //        _stackPanel.DataContext = finalWrapper.DataContext;
            //        tempUICollection.ForEach(x => _stackPanel.Children.Add(x));
            //        var returnWrap = WrapIntoDefaultGridScroller(_stackPanel);
            //        return returnWrap;
            //    }
            //}
            //else if (finalWrapper is Panel)
            //{
            //    var panel = finalWrapper as Panel;
            //    tempUICollection.ForEach(x => panel.Children.Add(x));
            //    var returnWrap = WrapIntoDefaultGridScroller(panel);
            //    return returnWrap;
            //}
            //else return null;
            return null;
        }
        private FrameworkElement WrapIntoDefaultGridScroller(Panel panel)
        {
            var grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            //Grid.SetColumn(alterTempPanel, column);
            //grid.Children.Add(alterTempPanel);
            var scroller = new ScrollViewer()
            { 
                PanningMode = PanningMode.VerticalOnly
                , CanContentScroll=true
            };
            scroller.Content = grid;
            return scroller;
        }
        private FrameworkElement WrapIntoStackPanel()
        {
            var stack = new StackPanel();
            stack.VerticalAlignment = VerticalAlignment.Center;
            Ex.Log($"ViewPagesManager.WrapIntoStackPanel() DataContext={tempDataContext}");
            stack.DataContext = tempDataContext;
            tempUICollection.ForEach(x => stack.Children.Add(x));
            return stack;
        }
        #endregion
    }
}
