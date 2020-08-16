using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenKeyboard
{
    /// <summary>
    /// Interaction logic for FullKeyboard.xaml
    /// </summary>
    public partial class FullKeyboardNoCaps : UserControl
    {
        public FullKeyboardNoCaps()
        {
            InitializeComponent();
            LangRusKB.LanguageKeyClicked += () => ChangeLanguage();
            LangEngKB.LanguageKeyClicked += () => ChangeLanguage();
        }

        #region Dependency Property Registration
        //public static readonly DependencyProperty IsShiftLockingProperty = DependencyProperty.RegisterAttached(nameof(IsNumeric), typeof(object), typeof(FullKeyboard),
        //new PropertyMetadata(false));

        //static FullKeyboard()
        //{ DefaultStyleKeyProperty.OverrideMetadata(typeof(FullKeyboard), new FrameworkPropertyMetadata(typeof(FullKeyboard))); }

        //public bool IsNumeric
        //{
        //    get { return (bool)GetValue(IsShiftLockingProperty); }
        //    set { SetValue(IsShiftLockingProperty, value); }
        //}
        #endregion

        public bool isRusLang { get; set; }

        public FullKeyboardNoCaps ChangeLanguage()
        {
            isRusLang = !isRusLang;
            double rusSize = isRusLang ? 1 : 0;
            double engSize = isRusLang ? 0 : 1;
            EngKBColumn.Width = new GridLength(engSize*11, GridUnitType.Star);
            RusKBColumn.Width = new GridLength(rusSize*11, GridUnitType.Star);
            return this;
        }
        public FullKeyboardNoCaps SetEngLang(bool isEng=true)
        {
            isRusLang = !isEng;
            double rusSize = isRusLang ? 1 : 0;
            double engSize = isRusLang ? 0 : 1;
            EngKBColumn.Width = new GridLength(engSize*11, GridUnitType.Star);
            RusKBColumn.Width = new GridLength(rusSize*11, GridUnitType.Star);
            return this;
        }
        public FullKeyboardNoCaps SetNumericType()
        {
            NumericKBColumn.Width= new GridLength(1, GridUnitType.Star);
            TextKBColumn.Width = new GridLength(0, GridUnitType.Star);
            return this;
        }

        public FullKeyboardNoCaps SetTextType()
        {
            NumericKBColumn.Width = new GridLength(0, GridUnitType.Star);
            TextKBColumn.Width = new GridLength(1, GridUnitType.Star);
            return this;
        }
    }
}
