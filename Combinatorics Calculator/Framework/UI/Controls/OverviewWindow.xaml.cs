using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Framework.UI.Controls
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : UserControl
    {
        public OverviewWindow()
        {
            InitializeComponent();
        }

        private void overview_Loaded(object sender, RoutedEventArgs e)
        {
            overview.ScaleToFit();
        }
    }
}
