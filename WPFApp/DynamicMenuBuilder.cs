using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ViewPagesList pages = new ViewPagesList();
        private iControls _Controls;
        private int fakeCount;
        #endregion
        #region ctor
        public DynamicMenuBuilder(Window incWindow/*, iControls Controls*/)
        {
            window = incWindow;
            //_Controls = Controls;
            try
            {
                model = window.DataContext as DynamicMenuWindowViewModel;
                model.NewResponseComeEvent += BuildMenuOnReponse;
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
            if(e.PropertyName == nameof(model.IsLoadingMenu))
            {
                window.Content = "Loading...";
            }
        }

        #region Properties
        public bool IsPageAvaiable => pages.IsNextAvaible;
        #endregion
        private void ClearOnResponse()
        {
            pages = new ViewPagesList();
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
                //window.Content = pages.Page;
                this.NextPage();
            }
            if (pages.Count > 1)
            {
                this.pages = new ViewPagesList();
                model.ClearChildLookupVM();
                Trace.WriteLine($"{nameof(BuildsPages)}(): if(pages.Count > 1) Clear & reBuild");
                this.ResponseAnalizeAndBuild();
                this.NextPage();
            }
            if (pages.Count <= 0)
            {
                throw new NotImplementedException($"{nameof(DynamicMenuBuilder)}.{nameof(BuildsPages)}(): if (pages.Count == 0)");
            }
        }
        private void ResponseAnalizeAndBuild()
        {
            if (model == null) { throw new NullReferenceException("main VM = null"); };
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
                //LOOKUPs display every lookup as 1 page
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                    {
                        Trace.WriteLine($"{nameof(ResponseAnalizeAndBuild)}(): LOOKUP display add: attr={attr.Name} Lookup={attr.Lookup}");
                        List<Lookup> lookups = paylist?.First()?.Lookups;
                        Lookup selectedLookup = lookups?.Where(x => x.Name.ToLower() == attr.Lookup.ToLower() )?.Single();
                        int index = model.PayrecToSend.AttrRecord.FindIndex(x => x == attr);
                        pages.NewPage();                        
                        LookupVM childVM = model.GetNewLookupVM();
                        childVM.Lookup = selectedLookup;
                        pages.AddDataContext(childVM);                        
                        pages.AddControl(new Label() { Content = selectedLookup.Name });
                        var lookItems = selectedLookup.Item;
                        lookItems.ForEach(arg =>
                        {
                            //var binding = new Binding($"{nameof(model.PayrecToSend)}.AttrRecord[{index}].Value");
                            //binding.Mode = BindingMode.OneWay;
                            var btn = new Button();
                            btn.Content = arg.Value;
                            btn.Command = childVM?.SelectLookupCommand;
                            btn.CommandParameter = arg;
                            btn.Click += (sender, evArg) => NextPage();
                            pages.AddControl(btn);
                        });
                        //LookupButtons(selectedLookup);
                        pages.NewPage();
                    }
                }
                Trace.WriteLine($"{nameof(ResponseAnalizeAndBuild)}(): After LOOKup ViewPages={pages.Count};");
                pages.NewPage();
                //ATTRs first display filled data attrs
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit != 1 && attr.View == 1)
                    {
                        Trace.WriteLine($"{nameof(ResponseAnalizeAndBuild)}(): ATTR filled info display: attr={attr.Name} value={attr.Value}");
                        var label = new Label();
                        label.Content = $"{attr.Name} = {attr.Value}";
                        label.BorderThickness = new Thickness(4);
                        pages.AddControl(label);
                    }
                }
                //ATTRs at last display attrs need to enter with textbox
                foreach (var attr in attrRecords)
                {
                    if (attr.Edit == 1 && attr.View == 1 && string.IsNullOrEmpty(attr.Lookup))
                    {
                        Trace.WriteLine($"{nameof(ResponseAnalizeAndBuild)}(): ATTR input: attr={attr.Name}");
                        var label = new Label();
                        label.Content = $"{attr.Name}:";
                        var inputbox = new TextBox();
                        int index = model.PayrecToSend.AttrRecord.FindIndex(x => x==attr);
                        var binding = new Binding($"{nameof(model.PayrecToSend)}.AttrRecord[{index}].Value");
                        inputbox.SetBinding(TextBox.TextProperty, binding);

                        pages.AddControl(label);
                        pages.AddControl(inputbox);
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
        private void NextPage(object param=null)
        {
            Trace.WriteLine($"DynamicMenuBuilder.Next VIEW Page: {pages.IsNextAvaible}; Current={pages.CurrIndex} Count={pages.Count}");
            if(pages.IsNextAvaible)
            {
                window.Content = pages.NextPage();
            }
            else
            {
                model.SendParamCommand.Execute(param);
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
