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

namespace ScreenKeyboard.ButtonsKB
{
    /// <summary>
    /// Interaction logic for LangRuEnKB.xaml
    /// </summary>
    public partial class LangRuEnKB : UserControl
    {
        public LangRuEnKB()
        {
            InitializeComponent();
        }
        public event Action LanguageKeyClicked = () => { };
        private void LangKey_Click(object sender, RoutedEventArgs e)
        {
            LanguageKeyClicked();
        }
    }
}
