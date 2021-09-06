using CBaSCore.Logic.UI.Utility_Classes;
using System.Collections.Generic;
using System.Windows;

namespace CBaSCore.Logic.UI.Controls.EEPROMs
{
    /// <summary>
    /// Interaction logic for EEPROMEditor.xaml
    /// </summary>
    public partial class EEPROMEditor : Window
    {
        private List<EEPROMRow> rows;

        public EEPROMEditor(List<EEPROMRow> rows)
        {
            InitializeComponent();
            this.rows = rows;

            RowTable.ItemsSource = rows;
        }

        private void FromProject_Click(object sender, RoutedEventArgs e)
        {
        }

        private void FromFile_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ToProject_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ToFile_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}