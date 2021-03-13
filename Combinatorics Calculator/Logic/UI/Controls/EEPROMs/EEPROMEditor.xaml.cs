using Combinatorics_Calculator.Logic.UI.Utility_Classes;
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
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Logic.UI.Controls.EEPROMs
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
    }
}
