using MaterialDesignThemes.Wpf;
using ScreenKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIWPFClean
{
    /// <summary>
    /// Interaction logic for FormAround.xaml
    /// </summary>
    public partial class FormAround : UserControl
    {
        private bool istest;
        private bool isPrevLangRus;
        public FormAround()
        {
            InitializeComponent();
        }

        public object GetDataContext => this.DataContext;

        public object ContentControls
        {
            get => this.scroller.Content;
            set => this.scroller.Content = value;            
        }

        private void MainGrid_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
            var uniform = new UniformGrid();
            uniform.Columns = 2;
            var hz = new ScrollViewer();
            var textb = new TextBox();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var keybox = new TextBoxDrawer();
            var textbox = new TextBox();
            textbox.InputScope = new InputScope()
            {
                Names = { istest ? new InputScopeName(InputScopeNameValue.Number) : new InputScopeName(InputScopeNameValue.Default) }
            };
            istest = !istest;
            textbox.GotFocus += (s, arg) =>
            {
                Dock dockArg = Dock.Bottom;
                var screenHeight = SystemParameters.PrimaryScreenHeight;
                var midleScreen = screenHeight / 2;
                var strokeHeight = midleScreen / 5;
                var kbHeight = midleScreen - strokeHeight;
                var control = s as TextBox;
                var location = control.PointToScreen(new Point(0, 0));
                dockArg = (location.Y > midleScreen + strokeHeight / 8) ? Dock.Top : Dock.Bottom;

                TopFullKeyboard.Height = kbHeight;
                BotFullKeyboard.Height = kbHeight;

                var inputScope = control.InputScope;
                InputScopeNameValue inputType = InputScopeNameValue.Default;
                try
                {
                    inputType = ((InputScopeName)inputScope.Names[0]).NameValue;
                }
                catch { }

                if (inputType == InputScopeNameValue.Number || inputType == InputScopeNameValue.NumberFullWidth)
                {
                    TopFullKeyboard.SetNumericType();
                    BotFullKeyboard.SetNumericType();
                }
                else //inputType != InputScopeNameValue.Number
                {
                    TopFullKeyboard.SetTextType();
                    BotFullKeyboard.SetTextType();
                }
                #region without full keyboard
                //TestWithoutFullKB(dockArg, midleScreen, strokeHeight, inputType);
                #endregion

                DrawerHost.OpenDrawerCommand.Execute(dockArg, drawer1);
            };
            textbox.LostFocus += (s, arg) =>
            {
                DrawerHost.CloseDrawerCommand.Execute(null, drawer1);
            };
            stackPanel1.Children.Add(textbox);
        }

        private void TestWithoutFullKB(Dock dockArg, double midleScreen, double strokeHeight, InputScopeNameValue inputType)
        {
            if (dockArg == Dock.Top)
            {
                if (inputType != InputScopeNameValue.Number)
                {
                    if (TopDrawerColorZone.Content == null || TopDrawerColorZone.Content is NumPadSolo)
                    {
                        TopDrawerColorZone.Content = new rusNumTop() { Height = midleScreen - strokeHeight };
                    }
                }
                if (inputType == InputScopeNameValue.Number)
                {
                    if (TopDrawerColorZone.Content == null || TopDrawerColorZone.Content is rusNumTop)
                    {
                        TopDrawerColorZone.Content = new NumPadSolo() { Height = midleScreen - strokeHeight };
                    }
                }
            }
            else
            {
                if (inputType != InputScopeNameValue.Number)
                {
                    if (BotDrawerColorZone.Content == null || BotDrawerColorZone.Content is NumPadSolo)
                    {
                        BotDrawerColorZone.Content = new rusNumTop() { Height = midleScreen - strokeHeight };
                    }
                }
                if (inputType == InputScopeNameValue.Number)
                {
                    if (BotDrawerColorZone.Content == null || BotDrawerColorZone.Content is rusNumTop)
                    {
                        BotDrawerColorZone.Content = new NumPadSolo() { Height = midleScreen - strokeHeight };
                    }
                }
            }
        }

        void SetTrigger(ContentControl contentControl)
        {
            // create the command action and bind the command to it
            var invokeCommandAction = new InvokeCommandAction { CommandParameter = Dock.Bottom };
            var binding = new Binding { Path = new PropertyPath(@"x:Static materialDesign:DrawerHost.CloseDrawerCommand") };
            BindingOperations.SetBinding(invokeCommandAction, InvokeCommandAction.CommandProperty, binding);

            // create the event trigger and add the command action to it
            var eventTrigger = new System.Windows.Interactivity.EventTrigger { EventName = "GotFocus" };
            eventTrigger.Actions.Add(invokeCommandAction);

            // attach the trigger to the control
            var triggers = Interaction.GetTriggers(contentControl);
            triggers.Add(eventTrigger);
        }
    }
}
