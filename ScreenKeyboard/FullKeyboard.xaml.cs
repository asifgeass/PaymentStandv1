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
    public partial class FullKeyboard : UserControl
    {
        public FullKeyboard()
        {
            InitializeComponent();
        }
        #region Dependency Property Registration
        public static readonly DependencyProperty IsShiftLockingProperty = DependencyProperty.RegisterAttached(nameof(IsNumeric), typeof(object), typeof(FullKeyboard),
        new PropertyMetadata(false));

        static FullKeyboard()
        { DefaultStyleKeyProperty.OverrideMetadata(typeof(FullKeyboard), new FrameworkPropertyMetadata(typeof(FullKeyboard))); }
        #endregion
        public bool IsNumeric
        {
            get { return (bool)GetValue(IsShiftLockingProperty); }
            set { SetValue(IsShiftLockingProperty, value); }
        }
    }
}
