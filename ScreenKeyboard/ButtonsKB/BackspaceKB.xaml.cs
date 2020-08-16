﻿using MaterialDesignThemes.Wpf;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenKeyboard.ButtonsKB
{
    /// <summary>
    /// Interaction logic for BackspaceKB.xaml
    /// </summary>
    public partial class BackspaceKB : UserControl
    {
        public BackspaceKB()
        {
            InitializeComponent();
        }

        public PackIcon Icon => this.IconControl;

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            KeyButton1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
