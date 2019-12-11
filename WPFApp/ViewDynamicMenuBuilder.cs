using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WPFApp.Interfaces;
using WPFApp.ViewModels;
using XmlStructureComplat;
using XmlStructureComplat.Validators;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using WPFApp.Views.Elements;

namespace WPFApp
{
    public class ViewDynamicMenuBuilder
    {
        #region fields
        const int idleTimedefault = 40;
        const int idleTimeAfterPayment = 9;
        //const int idleTimedefault = 2000;
        //const int idleTimeAfterPayment = 2000;
        private List<string> headerHistory = new List<string>();
        private DynamicMenuWindowViewModel vmodel;
        private Window window;
        private FormAround form = new FormAround();
        private ViewPagesManager views = new ViewPagesManager();
        private int fakeCount;
        private Style loadingBarStyle;
        private Task CheckHomeButtonDisabled = Task.CompletedTask;
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private IdleDetector idleDetector;
        private bool isLastPrevPage = false;
        FormAround around = new FormAround();
        #endregion

        #region ctor
        public ViewDynamicMenuBuilder(Window incWindow)
        {
            window = incWindow;            
            try
            {
                //token = cancelTokenSource.Token;
                Ex.Log($"ViewDynamicMenuBuilder ctor()");
                around.BackButton.Click += (s, e) => PrevPage();
                vmodel = window.DataContext as DynamicMenuWindowViewModel;
                vmodel.NewResponseComeEvent += OnReponseCome;
                vmodel.PropertyChanged += OnPropertyChanged;
                vmodel.PaymentWaitingEvent += OnWaitingPayment;                            
                loadingBarStyle = Application.Current.FindResource("MaterialDesignCircularProgressBar") as Style;
                idleDetector = new IdleDetector(window, idleTimedefault);
                idleDetector.IsIdle += OnIdle;
                Ex.Log($"ViewDynamicMenuBuilder ctor() end success");
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }
        #endregion

        #region Navigation Pages
        private void NextPage(object param = null)
        {
            Ex.Log($"DynamicMenuBuilder.{nameof(NextPage)}(): {views.IsNextAvaible};");
            if (views.IsNextAvaible)
            {
                if (views.NextHeader != null)
                {
                    var header = vmodel.HeaderHistory.Add(views.NextHeader);
                    vmodel.LabelCurrent = header.CurrentHeader;
                }
                SetWindow(views.NextPage());
            }
            else
            {
                vmodel.SendParamCommand.Execute(param);
            }
        }

        private void SetHeader()
        {
            var paylist = vmodel?.Responce?.ResponseReq?.PayRecord;
            if (paylist != null)
            {
                var payrec = paylist.FirstOrDefault();
                if (payrec != null)
                {
                    //string header = string.Empty;
                    //if (paylist.Count > 1)
                    //{
                    //    header = payrec.GroupRecord?.Name;
                    //}
                    //if (paylist.Count == 1)
                    //{
                    //    header = payrec.Name;
                    //}
                    //Ex.Log($"SetHeader() old={vmodel.HeaderHistory.CurrentHeader}");
                    //var headerHistory = vmodel.HeaderHistory.Add(header);
                    //Ex.Log($"SetHeader() new={headerHistory.CurrentHeader}");
                    //vmodel.LabelCurrent = headerHistory.CurrentHeader;
                }
            }
        }

        private void PrevPage()
        {
            Ex.Log($"DynamicMenuBuilder.{nameof(PrevPage)}(): {views.IsPrevAvaible};");
            if (views.IsPrevAvaible)
            {
                if(views.PrevHeader != null)
                {
                    vmodel.HeaderHistory.BackTo(views.PrevHeader, true);
                    vmodel.LabelCurrent = vmodel.HeaderHistory.CurrentHeader;
                }                
                //vmodel.LabelCurrent = views.PrevHeader ?? vmodel.LabelCurrent;
                SetWindow(views.PrevPage());
            }
            else
            {
                var gplt = vmodel.Responce?.ResponseReq?.PayRecord?.FirstOrDefault()?.GetPayListType;
                if (gplt != null && gplt == "0")
                {
                    //vmodel.HomePageCommand.Execute();
                }
                /*else*/
                if (vmodel.BackUserCommand.CanExecute())
                {
                    isLastPrevPage = true;
                    vmodel.BackUserCommand.Execute();
                }
            }
        }
        private void SetWindow(UIElement argElement)
        {
            around.ContentControls = argElement;
            window.Content = around;
        }
        #endregion

        #region View Building
        private void HandleResponse()
        {
            Ex.Log($"{nameof(ViewDynamicMenuBuilder)}.{nameof(HandleResponse)}()");
            if (vmodel == null) { throw new NullReferenceException("main VM = null"); };
            ResetBeforeHandle();
            var rootResponse = vmodel.Responce;
            var resp = rootResponse.ResponseReq;
            if (resp != null && resp?.ErrorCode != 0)
            {
                BuildErrorPage(rootResponse);
                return;
            }
            if (rootResponse.EnumType == EripQAType.GetPayListResponse)
            {
                var paylist = resp.PayRecord;
                BuildPayRecords(paylist);
                if (paylist.Count == 1)
                {
                    var payrec = paylist.FirstOrDefault();
                    var attrRecords = payrec.AttrRecord;
                    //var header = vmodel.HeaderHistory.Add(payrec?.Name);
                    //vmodel.LabelCurrent = header.CurrentHeader;
                    //vmodel.LabelCurrent = $"{payrec.GroupRecord?.Name} / {payrec.Name}";
                    this.BuildLookups(paylist);
                    this.BuildDisplayInfo(payrec);
                    this.BuildInputFields(attrRecords);
                    this.BuildFinalButton(payrec);
                    views.NewPage();
                }
            }
            if (rootResponse.EnumType == EripQAType.RunOperationResponse)
            {
                Ex.Log($"ResponseAnalizeAndBuild(): rootResponse.EnumType==RunOperationResponse");
                if (resp.ErrorCode == 0)
                {
                    var control = Controls.CentralLabelBorder("Оплата успешно произведена!");
                    control.Foreground = Brushes.DarkOliveGreen;
                    var pic = Controls.IconBig(PackIconKind.CheckboxMarkedCircle, Brushes.Green);
                    views.AddControl(pic);
                    views.AddControl(control);
                    var button = Controls.ButtonAccept("Вернуться");
                    button.ButtonControl.Command = vmodel.HomePageCommand;
                    views.AddControl(new TextBlock());
                    views.AddControl(button);
                    idleDetector.ChangeIdleTime(idleTimeAfterPayment);
                }
                if (resp.ErrorCode != 0)
                {
                    var control = Controls.CentralLabelBorder($"Ошибка оплаты {resp.ErrorCode}!\n{resp.ErrorText}");
                    views.AddControl(control);
                }
            }
        }
        private void BuildPayRecords(List<PayRecord> paylist)
        {
            if (paylist.Count > 1)
            {
                views.NewPage(new UniTilePanel());
                foreach (var payrec in paylist)
                {
                    var cardButton = Controls.ButtonCard(payrec.Name);
                    cardButton.ButtonControl.Command = vmodel.SendParamCommand;
                    cardButton.ButtonControl.CommandParameter = payrec;
                    cardButton.ButtonControl.Click += (s,arg)=>
                    {
                        var headerHistory = vmodel.HeaderHistory.Add(payrec.Name);
                        vmodel.LabelCurrent = headerHistory.CurrentHeader;
                    };
                    //vmodel.LabelCurrent = payrec?.GroupRecord?.Name;//????
                    //views.SetHeader(payrec?.GroupRecord?.Name);
                    views.AddControl(cardButton);
                }
                var headerName = paylist?.FirstOrDefault()?.GroupRecord?.Name;
                //var header = vmodel.HeaderHistory.Add(headerName);
                //vmodel.LabelCurrent = header.CurrentHeader;
            }
        }
        private void BuildLookups(List<PayRecord> paylist)
        {
            var payrec = paylist.FirstOrDefault();
            var attrRecords = payrec?.AttrRecord;
            var preheader = $"{payrec?.GroupRecord?.Name} / {payrec?.Name}";
            foreach (var attr in attrRecords)
            {
                if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                {
                    //Ex.Log($"{nameof(HandleResponse)}(): LOOKUP display add: attr={attr.Name} Lookup={attr.Lookup}");
                    List<Lookup> lookups = paylist?.FirstOrDefault()?.Lookups;
                    var list = lookups?.Where(x => x.Name.ToLower() == attr.Lookup.ToLower());
                    Lookup selectedLookup = list?.FirstOrDefault();
                    if (selectedLookup == null) { continue; }
                    int index = vmodel.PayrecToSend.AttrRecord.FindIndex(x => x == attr);
                    views.NewPage(new UniTilePanel());
                    LookupVM childVM = vmodel.GetNewLookupVM();
                    childVM.Lookup = selectedLookup;
                    views.AddDataContext(childVM);
                    //views.SetHeader($"{preheader} / {attr.Lookup}");
                    views.SetHeader($"{attr.Lookup}");
                    var lookItems = selectedLookup.Item;
                    lookItems.ForEach(arg =>
                    {
                        var btn = Controls.ButtonCard(arg.Value);
                        btn.ButtonControl.Command = childVM?.SelectLookupCommand;
                        btn.ButtonControl.CommandParameter = arg;
                        btn.ButtonControl.Click += (sender, evArg) => NextPage();
                        views.AddControl(btn);
                    });
                    views.NewPage();
                }
            }
        }
        private void BuildDisplayInfo(PayRecord payrec)
        {
            foreach (var attr in payrec.AttrRecord)
            {
                if (attr.Edit != 1 && attr.View == 1)
                {
                    //Ex.Log($"{nameof(HandleResponse)}(): ATTR filled info display: attr={attr.Name} value={attr.Value}");
                    var label = Controls.LabelInfo($"{attr.Name} = {attr.Value};");
                    views.AddControl(label);
                }
            }
            #region display PAYRecord Parameters

            if (!string.IsNullOrEmpty(payrec.Summa) && payrec.Summa != "0.00")
            {
                var payLabel = Controls.LabelInfo($"Сумма оплаты = {payrec.Summa}");
                views.AddControl(payLabel);
            }
            if (!string.IsNullOrEmpty(payrec.Commission) && payrec.Commission != "0.00")
            {
                var payLabel = Controls.LabelInfo($"Коммиссия = {payrec.Commission}");
                views.AddControl(payLabel);
            }
            if (!string.IsNullOrEmpty(payrec.Fine) && payrec.Fine != "0.00")
            {
                var payLabel = Controls.LabelInfo($"Пеня = {payrec.Fine}");
                views.AddControl(payLabel);
            }
            #endregion
        }
        private void BuildInputFields(List<AttrRecord> attrRecords)
        {
            foreach (var attr in attrRecords)
            {
                if (attr.Edit == 1 && attr.View == 1 && string.IsNullOrEmpty(attr.Lookup))
                {
                    if (string.IsNullOrEmpty(attr.Type))
                    {
                        BuildInputAttr(attr);
                    }
                    else
                    {
                        if (attr.Type?.ToUpper() == "L")
                        {
                            var checkbox = new CheckBox();
                            checkbox.Content = attr.Name;
                            views.AddControl(checkbox);
                        }
                        if (attr.Type?.ToUpper() != "L")
                        {
                            BuildInputAttr(attr);
                        }
                    }
                }
            }
        }
        private void BuildInputAttr(AttrRecord attr)
        {
            //Ex.Log($"{nameof(BuildInputAttr)}(): ATTR input: attr={attr.Name}");
            string hint = attr.Name;
            if (attr.Mandatory != null && attr.Mandatory == "1") hint += '*';
            var inputbox = Controls.TextBoxHint(hint, around);
            inputbox.CharacterCasing = CharacterCasing.Upper;
            if (!string.IsNullOrEmpty(attr.MaxLength))
            {
                int maxLength;
                if (int.TryParse(attr.MaxLength, out maxLength))
                {
                    inputbox.MaxLength = maxLength;
                }
            }
            if (!string.IsNullOrEmpty(attr.Type) && (attr.Type == "I" || attr.Type == "R"))
            {
                inputbox.InputScope = new InputScope()
                {
                    Names = { new InputScopeName(InputScopeNameValue.Number) }
                };
            }
            if (!string.IsNullOrEmpty(attr.Hint))
            {
                //HintAssist.SetHelperText(inputbox, attr.Hint);
            }
            AttrValidationVM vmAttr = vmodel.GetNewAttrVM(attr);
            inputbox.DataContext = vmAttr;

            //int index = vmodel.PayrecToSend.AttrRecord.FindIndex(x => x == attr);
            var binding = new Binding($"{nameof(vmAttr.ValueAttrRecord)}");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.Mode = BindingMode.TwoWay;
            inputbox.SetBinding(TextBox.TextProperty, binding);

            views.AddControl(inputbox);
        }
        private void BuildFinalButton(PayRecord payrec)
        {
            if ((payrec.Summa == "0.00" || payrec.Summa == string.Empty) && payrec.GetPayListType == "0")
            {
                views.AddControl(new TextBlock()); //отступ
                var inputbox = Controls.TextBoxHint("Сумма оплаты", around);
                inputbox.InputScope = new InputScope()
                {
                    Names = { new InputScopeName(InputScopeNameValue.Number) }
                };
                var binding = new Binding($"{nameof(vmodel.SummaPayrecToSend)}");
                binding.Mode = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                inputbox.SetBinding(TextBox.TextProperty, binding);
                inputbox.TextChanged += (s, e) =>
                {
                    char fraction = '.';
                    if (inputbox.Text.Contains(fraction))
                    {
                        var index = inputbox.Text.IndexOf(fraction);
                        inputbox.MaxLength = index + 3;
                    }
                    else inputbox.MaxLength = 0;
                };
                views.AddControl(inputbox);
            }

            views.AddControl(new TextBlock()); //отступ
            var button = Controls.ButtonAccept("Продолжить");
            //views.SetHeader($"{payrec.GroupRecord?.Name} / {payrec.Name}");
            views.SetHeader($"Реквизиты");
            button.ButtonControl.Command = vmodel.NextPageAttrValidateCommand;
            if (payrec.GetPayListType == "0")
            {
                button.Text = "Оплатить";
                button.ButtonControl.Command = vmodel.NextPagePayValidateCommand;
                views.SetHeader($"Подтверждение платежа");
                Ex.Log("========== ОПЛАТИТЬ КНОПКА ПОСТРОЕНА =============");
            }
            views.AddControl(button);
        }
        private void OnReponseCome()
        {
            try
            {
                idleDetector.ChangeIdleTime(idleTimedefault);
                ClearViewPagesOnResponse();
                BuildsPages();
            }
            catch (Exception ex)
            {
                DisplayErrorPage(ex);
            }
        }
        private void BuildsPages()
        {
            this.HandleResponse();
            this.fakeCount = views.Count;
            SetHeader();
            if (views.Count == 1)
            {                
                this.NextPage();
            }
            else if (views.Count > 1)
            {
                this.HandleResponse();                
                this.NextPage();
            }
            else //if (views.Count <= 0)
            {
                var str = "Извините, мы не смогли построить никакой " +
                    "страницы на ответ сервера.\n(pages.Count <= 0)";
                DisplayErrorPage(str);
            }
        }
        #endregion

        #region other stuff
        private async void OnIdle(object sender, EventArgs arg)
        {
            try
            {
                if (idleDetector.TimerCurrent < idleTimedefault || vmodel?.Responce?.ResponseReq?.PayRecord?.Count > 1)
                    HomeIdle().RunAsync();
                else
                {
                    //Trace.WriteLine($"around.dialogHostTop.IsOpen={around.dialogHostTop.IsOpen}");
                    if (!around.dialogHostTop.IsOpen)
                    {
                        var result = await around.dialogHostTop.ShowDialog(around.dialogHostTop.DialogContent);
                        if (result != null && result is bool)
                        {
                            bool boolResult = (bool)result;
                            //Trace.WriteLine($"ViewDynamicMenuBuilder.Idle: result is bool={boolResult}");
                            if (!boolResult) HomeIdle().RunAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
        private async Task HomeIdle()
        {
            Trace.WriteLine($"ViewDynamicMenuBuilder.HomeIdle()");
            vmodel.HomePageCommand.Execute();
            for (short i = 0; i < 3; i++)
            {
                await Task.Delay(70); vmodel.IsBackButtonActive = false;
            }
        }
        private void OnWaitingPayment()
        {
            var screen = Controls.PayScreen();
            this.SetWindow(screen);
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(vmodel.IsLoadingMenu))
            {
                var bar = new ProgressBar();
                bar.IsIndeterminate = true;
                bar.Value = 0;
                bar.Style = loadingBarStyle;
                bar.Height = 50;
                bar.Width = 50;
                SetWindow(bar);
            }
            if (e.PropertyName == nameof(vmodel.Exception))
            {
                DisplayErrorPage(vmodel.Exception);
            }
            if (e.PropertyName == nameof(vmodel.IsHomeButtonActive))
            {
                //try
                //{
                //    if (model.IsHomeButtonActive)
                //    {
                //        cancelTokenSource.Cancel();
                //        if (!CheckHomeButtonDisabled.IsCanceled)
                //        { await CheckHomeButtonDisabled; }
                //    }
                //    else
                //    {
                //        //await CheckHomeButtonDisabled;   
                //        CheckHomeButtonDisabled = new Task(async ()=> await testsome(cancelTokenSource.Token));
                //        await CheckHomeButtonDisabled;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    DisplayErrorPage(ex);
                //}
            }
        }
        private void ResetBeforeHandle()
        {
            this.views = new ViewPagesManager();
            vmodel.LookupVMList.Clear();
            vmodel.AttrVMList.Clear();
        }
        private void DisplayErrorPage(Exception ex)
        {
            ex.Log();
            var innerEx = ex.InnerGetLast();
            string msg = string.Empty;
            if (innerEx is System.Net.WebException)
            {
                msg = "Извините, произошла ошибка интернет-соединения.\n"
                    + "Попробуйте еще раз или обратитесь к администратору.";
                DisplayErrorPage($"{ex.Message.prefix(msg, 2)}");
                return;
            }
            if (innerEx is System.IO.IOException)
            {
                msg = "Ошибка: у данного IP-адреса нет доступа к серверу.";
                DisplayErrorPage($"{ex.Message.prefix(msg, 2)}");
                return;
            }
            msg = "Извините, произошла непредвиденная ошибка. " +
                $"Обратитесь к администратору.";
            DisplayErrorPage($"{ex.Message.prefix(msg, 2)}");
        }
        private void DisplayErrorPage(string msgError)
        {
            var panel = new StackPanel();
            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.Children.Add(Controls.IconBig(PackIconKind.CloseCircleOutline, Brushes.Red));
            panel.Children.Add(Controls.CentralLabelBorder(msgError, Brushes.DarkRed));
            var button = Controls.ButtonAccept("Вернуться");
            button.ButtonControl.Command = vmodel.HomePageCommand;
            panel.Children.Add(new TextBlock());
            panel.Children.Add(new TextBlock());
            panel.Children.Add(button);
            SetWindow(panel);
            idleDetector.ChangeIdleTime(idleTimeAfterPayment * 2);
        }
        private void BuildErrorPage(PS_ERIP rootResponse)
        {
            var resp = rootResponse.ResponseReq;
            string str = $"{resp.ErrorText}";
            if (rootResponse.EnumType == EripQAType.POSPayResponse)
            {
                str = $"Оплата не была произведена.\n\nОшибка {resp.ErrorCode}: {resp.ErrorText};";
                if (resp.ErrorCode != 128 && resp.ErrorCode != 16)
                { Ex.Error($"Unknown POS Error:\nОшибка {resp.ErrorCode}: {resp.ErrorText};"); }
            }
            if (resp.ErrorCode == 128) //timeout 30s
            { str = $"Истекло время ожидания карты.(Код 128)\nОплата не была произведена.\n\nПопробуйте еще раз."; }
            if (resp.ErrorCode == 16) //canceled by user
            { str = $"Отменено пользователем.(Код 16)\nОплата не была произведена."; }
            Ex.Info($"View Error Page building: response={rootResponse.EnumType} displayed to user:\n{str}");
            var control = Controls.CentralLabelBorder(str);
            control.Foreground = Brushes.DarkRed;
            var pic = Controls.IconBig(PackIconKind.CloseCircleOutline, Brushes.Red);
            idleDetector.ChangeIdleTime(idleTimeAfterPayment * 2);
            views.AddControl(pic);
            views.AddControl(control);
            var button = Controls.ButtonAccept("Вернуться");
            button.ButtonControl.Command = vmodel.HomePageCommand;
            views.AddControl(new TextBlock());
            views.AddControl(new TextBlock());
            views.AddControl(button);
        }
        private void RecurseRemoveButtonCommands(UIElement arg)
        {
            if (arg is Button)
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
        private async Task testsome(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                if (token.IsCancellationRequested) return;
                Ex.Log("Я тут тикаю пока ХОУМ батон отключена");
            }
            Ex.Log("Я тут врубаю кнопку ХОУМ");
            vmodel.IsHomeButtonActive = true;
            return;
        }
        private void ClearViewPagesOnResponse()
        {
            views = new ViewPagesManager();
            fakeCount = 0;
        }
        private void ChangeThemePalette()
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(new MatDesDarkerLightTheme());
            paletteHelper.SetTheme(theme);            
        }
        #endregion
    }
}
