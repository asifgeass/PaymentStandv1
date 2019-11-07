using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WPFApp.Interfaces;
using WPFApp.ViewModels;
using XmlStructureComplat;

namespace WPFApp
{
    public class ViewDynamicMenuBuilder
    {
        #region fields
        private DynamicMenuWindowViewModel model;
        private Window window;
        private FormAround form = new FormAround();
        private ViewPagesManager views = new ViewPagesManager();
        private iControls _Controls;
        private int fakeCount;
        private Style loadingBarStyle;
        #endregion
        #region ctor
        public ViewDynamicMenuBuilder(Window incWindow)
        {
            window = incWindow;
            try
            {
                model = window.DataContext as DynamicMenuWindowViewModel;
                model.NewResponseComeEvent += BuildMenuOnReponse;
                model.PropertyChanged += IsLoadingMenuChanged;
                loadingBarStyle = Application.Current.FindResource("MaterialDesignCircularProgressBar") as Style;
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
                var bar = new ProgressBar();
                bar.IsIndeterminate = true;
                bar.Value = 0;
                bar.Style = loadingBarStyle;
                bar.Height = 50;
                bar.Width = 50;
                SetWindow(bar);
            }
            if (e.PropertyName == nameof(model.Exception))
            {
                try
                {
                    throw new Exception("",model.Exception);
                }
                catch (Exception ex)
                {
                    DisplayErrorPage(ex?.InnerException ?? ex);
                }
            }
        }

        private void SetWindow(UIElement argElement)
        {
            var around = new FormAround();
            around.BackButton.Click += (s, e) => PrevPage();
            around.ContentControls = argElement;
            window.Content = around;
        }

        #region Properties
        public bool IsPageAvaiable => views.IsNextAvaible;
        #endregion
        private void ClearOnResponse()
        {
            views = new ViewPagesManager();
            fakeCount = 0;
        }
        private void BuildMenuOnReponse()
        {
            try
            {
                ClearOnResponse();
                BuildsPages();
            }
            catch (Exception ex)
            {
                DisplayErrorPage(ex);
            }
        }

        private void DisplayErrorPage(Exception ex)
        {
            Ex.Log($"{ex.Message}\n\n{ex.StackTrace}");
            DisplayErrorPage($"{ex.Message}\n\n{ex.StackTrace}");
        }
        private void DisplayErrorPage(string msgError)
        {
            var str = "Извините, произошла непредвиденная ошибка. " +
                $"Обратитесь к администратору.\n\n{msgError}";
            SetWindow(Controls.CentralLabelBorder(str) );
        }

        private void BuildsPages()
        {
            this.ResponseAnalizeAndBuild();
            this.fakeCount = views.Count;
            if (views.Count == 1)
            {
                this.NextPage();
            }
            if (views.Count > 1)
            {
                this.views = new ViewPagesManager();
                model.ClearChildLookupVM();
                Ex.Log($"{nameof(BuildsPages)}(): if(pages.Count > 1) Clear & reBuild");
                this.ResponseAnalizeAndBuild();
                this.NextPage();
            }
            if (views.Count <= 0)
            {
                var str = "Извините, мы не смогли построить никакой " +
                    "страницы на ответ сервера.\n(pages.Count <= 0)";
                SetWindow(Controls.CentralLabelBorder(str));                
            }
        }
        private void ResponseAnalizeAndBuild()
        {
            if (model == null) { throw new NullReferenceException("main VM = null"); };
            var rootResponse = model.Responce;
            var resp = rootResponse.ResponseReq;
            if (resp != null && resp?.ErrorCode != 0)
            {
                string str = $"{resp.ErrorCode}\n{resp.ErrorText}";
                var control = Controls.CentralLabelBorder(str);
                views.AddControl(control);
            }
            if (rootResponse.EnumType == EripQAType.GetPayListResponse)
            {
                var paylist = resp.PayRecord;
                if (paylist.Count > 1)
                {
                    views.NewPage();
                    foreach (var payrec in paylist)
                    {
                        var button = Controls.Button(payrec.Name);
                        button.Command = model.SendParamCommand;
                        button.CommandParameter = payrec;
                        CheckButtonCommand(button, paylist.Last() == payrec);
                        views.AddControl(button);
                    }
                }
                if (paylist.Count == 1)
                {
                    var payrec = paylist.First();
                    var attrRecords = payrec.AttrRecord;
                    //LOOKUPs display every lookup as 1 page
                    foreach (var attr in attrRecords)
                    {
                        if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                        {
                            Ex.Log($"{nameof(ResponseAnalizeAndBuild)}(): LOOKUP display add: attr={attr.Name} Lookup={attr.Lookup}");
                            List<Lookup> lookups = paylist?.First()?.Lookups;
                            Lookup selectedLookup = lookups?.Where(x => x.Name.ToLower() == attr.Lookup.ToLower())?.Single();
                            int index = model.PayrecToSend.AttrRecord.FindIndex(x => x == attr);
                            views.NewPage();
                            LookupVM childVM = model.GetNewLookupVM();
                            childVM.Lookup = selectedLookup;
                            views.AddDataContext(childVM);
                            views.AddControl(Controls.LabelHeader(selectedLookup.Name));
                            var lookItems = selectedLookup.Item;
                            lookItems.ForEach(arg =>
                            {
                                var btn = Controls.Button(arg.Value);
                                btn.Command = childVM?.SelectLookupCommand;
                                btn.CommandParameter = arg;
                                btn.Click += (sender, evArg) => NextPage();
                                views.AddControl(btn);
                            });
                            //LookupButtons(selectedLookup);
                            views.NewPage();
                        }
                    }
                    Ex.Log($"{nameof(ResponseAnalizeAndBuild)}(): After LOOKup ViewPages={views.Count};");
                    views.NewPage();
                    //ATTRs first display filled data attrs
                    foreach (var attr in attrRecords)
                    {
                        if (attr.Edit != 1 && attr.View == 1)
                        {
                            Ex.Log($"{nameof(ResponseAnalizeAndBuild)}(): ATTR filled info display: attr={attr.Name} value={attr.Value}");
                            var label = Controls.LabelInfo();
                            label.Content = $"{attr.Name} = {attr.Value}";
                            views.AddControl(label);
                        }
                    }
                    var payLabel = Controls.LabelInfo();
                    payLabel.Content = $"Summa = {payrec.Summa}";
                    views.AddControl(payLabel);
                    payLabel = Controls.LabelInfo();
                    payLabel.Content = $"Commission = {payrec.Commission}";
                    views.AddControl(payLabel);
                    payLabel = Controls.LabelInfo();
                    payLabel.Content = $"Fine = {payrec.Fine}";
                    views.AddControl(payLabel);
                    //ATTRs at last display attrs need to enter with textbox
                    foreach (var attr in attrRecords)
                    {
                        if (attr.Edit == 1 && attr.View == 1 && string.IsNullOrEmpty(attr.Lookup))
                        {
                            Ex.Log($"{nameof(ResponseAnalizeAndBuild)}(): ATTR input: attr={attr.Name}");
                            var label = Controls.LabelInfo(attr.Name);
                            var inputbox = Controls.TextBox();
                            int index = model.PayrecToSend.AttrRecord.FindIndex(x => x == attr);
                            var binding = new Binding($"{nameof(model.PayrecToSend)}.AttrRecord[{index}].Value");
                            binding.Mode = BindingMode.TwoWay;
                            inputbox.SetBinding(TextBox.TextProperty, binding);

                            views.AddControl(label);
                            views.AddControl(inputbox);
                        }
                    }
                    views.AddControl(new TextBlock());
                    var button = Controls.ButtonAccept("Продолжить");
                    if (payrec.GetPayListType == "0")
                    { button.Content = "Оплатить"; }
                    button.Command = model.SendVmPayrecCommand;
                    views.AddControl(button);
                }
            }
            if (rootResponse.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"ResponseAnalizeAndBuild(): rootResponse.EnumType==RunOperationResponse");
                if (resp.ErrorCode == 0)
                {
                    var control = Controls.CentralLabelBorder("Оплата успешно произведена!");
                    control.BorderBrush = Brushes.Green;
                    views.AddControl(control);
                }
                if (resp.ErrorCode != 0)
                {
                    var control = Controls.CentralLabelBorder($"Оплата отменена!\n{resp.ErrorCode}\n{resp.ErrorText}");                    
                    views.AddControl(control);
                }
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
            for (int i = 0; i < views.Count - 1; i++)
            {
                var page = views[i];
                foreach (UIElement item in page.Children)
                {
                    RecurseRemoveButtonCommands(item);
                }
            }
        }
        private void NextPage(object param=null)
        {
            Ex.Log($"DynamicMenuBuilder.{nameof(NextPage)}(): {views.IsNextAvaible}; Current={views.CurrIndex} Count={views.Count}");
            if(views.IsNextAvaible)
            {
                SetWindow(views.NextPage());
            }
            else
            {
                model.SendParamCommand.Execute(param);
            }
        }
        private void PrevPage()
        {
            Ex.Log($"DynamicMenuBuilder.{nameof(PrevPage)}(): {views.IsNextAvaible}; Current={views.CurrIndex} Count={views.Count}");
            if (views.IsPrevAvaible)
            {
                SetWindow(views.PrevPage());
            }
            else
            {
                if (model.BackUserCommand.CanExecute())
                {
                    model.BackUserCommand.Execute();
                }
            }
        }
        public void LookupButtons(Lookup selectedLookup)
        {
            views.AddControl(new Label() { Content = selectedLookup.Name });
            var lookItems = selectedLookup.Item;
            lookItems.ForEach(x =>
            {
                views.AddControl(new Button()
                {
                    Content = $"{x.Value}",
                });
            });
        }
    }
}
