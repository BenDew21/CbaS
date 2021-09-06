using System.Windows;
using System.Windows.Controls;

namespace CBaSCore.Framework.UI.Controls
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