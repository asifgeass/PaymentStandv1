using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFApp.Interfaces;
using WPFApp.ViewModels;
using XmlStructureComplat;

namespace WPFApp
{
    public class DynamicMenuBuilder
    {
        #region fields
        private DynamicMenuWindowViewModel model;
        private Window window;
        private FormAround form = new FormAround();
        private PagesList pages = new PagesList();
        private iControls _Controls;
        private int fakeCount;
        #endregion
        #region ctor
        public DynamicMenuBuilder(Window incWindow, iControls Controls)
        {
            window = incWindow;
            _Controls = Controls;
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
        private void ClearOnResponse()
        {
            pages = new PagesList();
            fakeCount = 0;
        }
        private void BuildMenuOnReponse()
        {
            ClearOnResponse();
            BuildsPages();
        }
        private void BuildsPages()
        {
            this.ResponseAnalizeAndBuild();
            this.fakeCount = pages.Count;
            if (pages.Count == 1)
            {
                window.Content = pages.Page;
            }
            if (pages.Count > 1)
            {
                this.pages = new PagesList();
                this.ResponseAnalizeAndBuild();
            }
            if (pages.Count == 0)
            {
                throw new NotImplementedException($"{nameof(DynamicMenuBuilder)}.{nameof(BuildsPages)}(): if (pages.Count == 0)");
            }
        }
        private void ResponseAnalizeAndBuild(bool isView=false)
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
                    CheckButtonCommand(button, paylist.Last() == payrec);
                    pages.AddControl(button);
                }
            }
            if (paylist.Count == 1)
            {
                var attrRecords = paylist.First().AttrRecord;
                //Display every lookup as 1 page
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                    {
                        List<Lookup> lookups = paylist?.First()?.Lookups;
                        Lookup selectedLookup = lookups?.Where(x => x.Name.ToLower() == attr.Lookup.ToLower())?.Single();
                        
                        pages.NewPage();
                        LookupButtons(selectedLookup);
                        pages.NewPage();
                    }
                }
                pages.NewPage();
                //first display filled data attrs
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit != 1 && attr.View == 1)
                    {
                        var label = new Label();
                        label.Content = $"{attr.Name} = {attr.Value}";

                        pages.AddControl(label);
                        pages.AddControl(new TextBlock());
                    }
                }
                //at last display attrs need to enter with textbox
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit == 1 && attr.View == 1 && string.IsNullOrEmpty(attr.Lookup))
                    {
                        var label = new Label();
                        label.Content = $"{attr.Name}:";
                        var inputbox = new TextBox();
                        int index = model.PayrecToSend.AttrRecord.FindIndex(x => x==attr);
                        var binding = new Binding($"{nameof(model.PayrecToSend)}.AttrRecord[{index}].Value");
                        inputbox.SetBinding(TextBox.TextProperty, binding);

                        pages.AddControl(label);
                        pages.AddControl(inputbox);
                        pages.AddControl(new TextBlock());
                        break;
                    }
                }
                pages.AddControl(new TextBlock());
                pages.AddControl(new TextBlock());
                var button = new Button() 
                { 
                    Content="Продолжить",
                    Command = model.SendVmPayrecCommand
                };
                pages.AddControl(button);
            }
        }

        private void CheckButtonCommand(Button button, bool isLastPage)
        {
            if (isLastPage) return;
            if (fakeCount > 1)
            {
                button.Command = null;
                button.CommandParameter = null;
                button.Click += (s, arg) => NextPage();
            }
        }
        private void RecurseRemoveButtonCommands(UIElement arg)
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
                    RecurseRemoveButtonCommands(item);
                }
            }
        }
        private void ChangeCommandsFromVMToViewClick()
        {
            for (int i = 0; i < pages.Count - 1; i++)
            {
                var page = pages[i];
                foreach (UIElement item in page.Children)
                {
                    RecurseRemoveButtonCommands(item);
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
                throw new NotImplementedException();
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
            });
        }
        private Button ButtonSelect(XmlStructureComplat.PayRecord payrec)
        {
            var button = new Button();
            button.Content = payrec.Name;
            button.Command = model.SendParamCommand;
            button.CommandParameter = payrec;
            return button;
        }
    }
}
