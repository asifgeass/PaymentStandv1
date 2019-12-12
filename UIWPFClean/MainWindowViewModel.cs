using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIWPFClean
{
    public class MainWindowViewModel : BindableBase
    {
        private bool _isKeyboardOn;
        public bool IsKeyboardOn
        {
            get => _isKeyboardOn;
            set => SetProperty(ref _isKeyboardOn, value);
        }

        private void test()
        {

        }

        public void ShowKeyboard()
        {

        }
    }
}
