using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFApp.ViewModels;

namespace WPFApp
{
    public class DynamicMenuBuilder
    {
        #region fields
        private DynamicMenuWindowViewModel model;
        private Window window;
        private FormAround form = new FormAround();
        private PagesList pages = new PagesList();
        #endregion
        #region ctor
        public DynamicMenuBuilder(Window incWindow)
        {
            window = incWindow;
            try
            {
                model = window.DataContext as DynamicMenuWindowViewModel;
                model.ResponseChangedEvent += BuildMenuOnReponse;
                model.PropertyChanged += IsLoadingMenuChanged;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        private void IsLoadingMenuChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsLoadingMenu")
            {
                window.Content = null;
                window.Content = "Loading...";
            }
        }

        #region Properties
        public bool IsPageAvaiable => pages.IsNextAvaible;
        #endregion
        private void BuildMenuOnReponse()
        {
            pages = new PagesList();
            BuildsPages();
            if(pages.Count == 1)
            {
                window.Content = pages.Page;
            }
            if (pages.Count > 1)
            {
                ChangeCommandsFromVMToViewClick();
                window.Content = pages.Page;
            }
        }

        private void ChangeCommandsFromVMToViewClick()
        {
            for (int i = 0; i < pages.Count-2; i++)
            {
                var page = pages[i];
                foreach (UIElement item in page.Children)
                {
                    RecurseChildren(item);                    
                }
            }
        }
        private void RecurseChildren(UIElement arg)
        {
            if(arg is Button)
            {
                Button button = arg as Button;                
                button.Command = null;
                if (button.Command is Prism.Commands.DelegateCommand<object>)
                    button.CommandParameter = null;
            }
            if (arg is Panel)
            {
                Panel pnl = arg as Panel;
                foreach (UIElement item in pnl.Children)
                {
                    RecurseChildren(item);
                }
            }
        }

        private void NextPage()
        {
            if(pages.IsNextAvaible)
            {
                window.Content = pages.NextPage();
            }
            else
            {

            }
        }

        private void BuildsPages()
        {
            if (model == null) return;
            var rootResponse = model.Responce;
            var resp = rootResponse.GetListResponse;
            var paylist = resp.PayRecord;
            if (paylist.Count > 1)
            {
                pages.NewPage();
                foreach (var payrec in paylist)
                {
                    var button = ButtonSelect(payrec);
                    pages.AddControl(button);
                }
            }
            if (paylist.Count == 1)
            {
                var attrRecords = paylist.First().AttrRecord;
                foreach (var attr in attrRecords)
                {
                    //Display every lookup as 1 page
                    if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                    {
                        var lookups = paylist.First().Lookups;
                        var selectedLookup = lookups.Where(x => x.Name.ToLower() == attr.Lookup.ToLower()).Single();
                        pages.NewPage();
                        LookupButtons(selectedLookup);
                        pages.NewPage();
                    }
                }
                pages.NewPage();
                foreach (var attr in attrRecords)
                {
                    //first display filled data attrs
                    if (attr.Edit != 1 && attr.View == 1)
                    {
                        var label = new Label();
                        label.Content = $"{attr.Name} = {attr.Value}";

                        pages.AddControl(label);
                        pages.AddControl(new TextBlock());
                    }
                }
                foreach (var attr in attrRecords)
                {
                    //at last display attrs need to enter with textbox
                    if (attr.Edit == 1 && attr.View == 1 && string.IsNullOrEmpty(attr.Lookup))
                    {
                        var label = new Label();
                        label.Content = $"{attr.Name}:";
                        var inputbox = new TextBox();
                        inputbox.SetBinding(TextBox.TextProperty, new Binding("Test.StornoMode") { Source=model });
                        
                        pages.AddControl(label);
                        pages.AddControl(inputbox);
                        pages.AddControl(new TextBlock());
                        break;
                    }
                }
                pages.AddControl(new TextBlock());
                pages.AddControl(new TextBlock());
                pages.AddControl(new Button() 
                { 
                    Content = "Продолжить", 
                });
            }
        }

        private void LookupButtons(XmlStructureComplat.Lookup selectedLookup)
        {
            pages.AddControl(new Label(){ Content = selectedLookup.Name});
            var lookItems = selectedLookup.Item;
            lookItems.ForEach(x =>
            {
                pages.AddControl(new Button()
                {
                    Content = $"{x.Value}",
                });
                pages.AddControl(new TextBlock());
            });
        }

        private StackPanel ButtonSelect(XmlStructureComplat.PayRecord payrec)
        {
            var frame = new StackPanel() { Orientation = Orientation.Vertical };
            var button = new Button();
            button.Command = model.NextPageCommand;
            button.CommandParameter = payrec;
            button.Content = payrec.Name;
            //button.SetBinding(Button.CommandProperty, "NextPageCommand");
            //button.SetBinding(Button.CommandParameterProperty, new Binding(){ Source = it });
            frame.Children.Add(button);
            frame.Children.Add(new TextBlock());
            //form.Controls.Add(frame);
            return frame;
        }
    }
}
