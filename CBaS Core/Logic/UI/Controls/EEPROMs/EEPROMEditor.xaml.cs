using System.Collections.Generic;
using System.Windows;
using CBaSCore.Logic.Business;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.EEPROMs
{
    /// <summary>
    ///     Interaction logic for EEPROMEditor.xaml
    /// </summary>
    public partial class EEPROMEditor : Window
    {
        private EEPROM28C16Business _business;
        
        public EEPROMEditor(EEPROM28C16Business _business)
        {
            InitializeComponent();
            this._business = _business;
            RowTable.ItemsSource = _business.Rows;
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