using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using XmlStructureComplat;
using System.Windows.Controls;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using WPFApp.Views;
using System.Windows.Media.Imaging;
using ExceptionManager;
using System.Windows.Input;

namespace WPFApp
{
    public static class Controls
    {
        private const double Margin = 50;
        private const double MarginBetweenButtons = 12;
        private const double MarginBetweenInputs = 20;
        public static Label CentralLabelBorder(string arg = "", SolidColorBrush brushArg=null)
        {
            var lbl = new Label();
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
            lbl.Foreground = brushArg ?? lbl.Foreground;
            var textBlock = new TextBlock();
            textBlock.Text = arg;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextAlignment = TextAlignment.Center;
            lbl.Content = textBlock;
            return lbl;
        }

        public static PackIcon IconBig(PackIconKind iconKindArg, SolidColorBrush brushArg =null)
        {
            var pic = new PackIcon();
            pic.Kind = iconKindArg;
            pic.Height = 250;
            pic.Width = 250;
            pic.HorizontalAlignment = HorizontalAlignment.Center;
            pic.VerticalAlignment = VerticalAlignment.Center;
            pic.Foreground = brushArg ?? pic.Foreground;
            return pic;
        }

        public static CardButton ButtonCard(string argName=null)
        {
            var cardButton = new CardButton();
            if(argName!=null) cardButton.Text = argName; 
            //button.Style = Application.Current.TryFindResource("CardButton") as Style;            
            cardButton.Margin = new Thickness(MarginBetweenButtons);       
            return cardButton;
        }

        public static ButtonTextblock ButtonAccept(string argName = null)
        {
            var button = new ButtonTextblock();
            if (argName != null) button.Text = argName;
            button.Margin = new Thickness(MarginBetweenButtons);
            return button;
        }

        public static TextBlock LabelInfo(string argName = null)
        {
            var txtBlock = new TextBlock();
            if (argName != null) txtBlock.Text = argName;
            txtBlock.TextWrapping = TextWrapping.Wrap;
            txtBlock.FontSize = 38;
            return txtBlock;
        }

        public static Label LabelHeader(string argName = null)
        {
            var label = new Label();
            if (argName != null) label.Content = argName;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(MarginBetweenButtons);
            return label;
        }

        public static TextBox TextBoxHint(string hintArg, FormAround around, string nameArg = null)
        {
            if (around == null) throw new NullReferenceException($"Controls.TextBoxHint() FormAround around=null");
            var textBox = new TextBox();
            textBox.Margin = new Thickness(MarginBetweenButtons
                , MarginBetweenInputs, MarginBetweenButtons, MarginBetweenInputs);
            if (hintArg != null) HintAssist.SetHint(textBox, hintArg);
            if (nameArg != null) textBox.Text = nameArg;
            textBox.GotFocus += async (s, e) =>{for (int i = 0; i < 3; i++){ await Task.Delay(50); textBox.SelectAll(); } };            
            textBox.GotFocus += (s, arg) =>
            {
                Dock dockArg = Dock.Bottom;
                var screenHeight = SystemParameters.PrimaryScreenHeight;
                var midleScreen = screenHeight / 2;
                var strokeHeight = midleScreen / 3;
                var kbHeight = midleScreen - strokeHeight;
                var control = s as TextBox;
                var locationSys = control.PointToScreen(new Point(0, 0));
                var yLocationControl = screenHeight - locationSys.Y;
                var kbviewspace = kbHeight + control.ActualHeight * 1.29;
                dockArg = (yLocationControl > kbviewspace) ? Dock.Bottom : Dock.Top;

                around.TopFullKeyboard.SetEngLang().Height = kbHeight;
                around.BotFullKeyboard.SetEngLang().Height = kbHeight;

                var inputScope = control.InputScope;
                InputScopeNameValue inputType = InputScopeNameValue.Default;
                try
                {
                    inputType = ((InputScopeName)inputScope.Names[0]).NameValue;
                }
                catch { }

                if (inputType == InputScopeNameValue.Number || inputType == InputScopeNameValue.NumberFullWidth)
                {
                    around.TopFullKeyboard.SetNumericType();
                    around.BotFullKeyboard.SetNumericType();
                }
                else //inputType != InputScopeNameValue.Number
                {
                    around.TopFullKeyboard.SetTextType();
                    around.BotFullKeyboard.SetTextType();
                }

                DrawerHost.OpenDrawerCommand.Execute(dockArg, /*around.drawer1*/ control);
            };
            //textBox.text
            textBox.LostFocus += (s, arg) =>
            {
                DrawerHost.CloseDrawerCommand.Execute(null, around.drawer1);
            };
            //TextFieldAssist.SetHasClearButton(textBox, true);          
            return textBox;
        }

        private static Style GetBorderStyleTrigger()
        {
            var setBrush = new Setter()
            {
                Property = TextBox.BorderBrushProperty,
                Value = Brushes.Red,
            };

            var setBorderThick = new Setter()
            {
                Property = TextBox.BorderThicknessProperty,
                Value = new Thickness(5),
            };

            var trigger = new Trigger()
            {
                Property = Validation.HasErrorProperty,
                Value = true,
                Setters = { setBrush, setBorderThick },
            };

            var style = new Style()
            { Triggers = { trigger } };

            return style;
        }

        public static Grid PayScreen()
        {
            var columnGrid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            gridCol1.Width = new GridLength(30, GridUnitType.Star);
            gridCol2.Width = new GridLength(50, GridUnitType.Star);
            gridCol3.Width = new GridLength(250);
            columnGrid.ColumnDefinitions.Add(gridCol3);
            columnGrid.ColumnDefinitions.Add(gridCol2);
            columnGrid.ColumnDefinitions.Add(gridCol1);

            var bar = new ProgressBar();
            bar.IsIndeterminate = true;
            bar.Margin = new Thickness(50);            

            var imgLeft = new Image();
            var imgRight = new Image();
            var imgRight2 = new Image();
            Ex.TryLog("Не удалось загрузить изображение из ресурсов.", () =>
            {
                imgLeft.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/true_png.png"));
                imgLeft.Stretch = Stretch.Uniform;

                imgRight.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/POS_chip1.png"));
                imgRight.Stretch = Stretch.None;

                imgRight2.Source = new BitmapImage(new Uri(@"pack://application:,,,/WPFApp;component/Resources/POS_side.png"));
                imgRight2.Stretch = Stretch.None;
            });

            var center = new StackPanel();
            center.VerticalAlignment = VerticalAlignment.Center;
            center.Children.Add(new TextBlock()
            {
                Text = "Пожалуйста, поднесите карту к POS-терминалу и совершите оплату любым способом."
                ,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            });
            center.Children.Add(bar);

            var doublePic = new StackPanel();
            doublePic.VerticalAlignment = VerticalAlignment.Center;
            doublePic.Children.Add(imgRight);
            doublePic.Children.Add(imgRight2);

            columnGrid.Children.Add(imgLeft);
            columnGrid.Children.Add(center);
            columnGrid.Children.Add(doublePic);
            Grid.SetColumn(imgLeft, 2);
            Grid.SetColumn(center, 1);
            Grid.SetColumn(doublePic, 0);

            return columnGrid;
        }
    }
}
