using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TimeManWP
{
    public partial class CheckButtonWithGrid : UserControl
    {
        public CheckButtonWithGrid()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
        }
    }
}
