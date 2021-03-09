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

            InitBindings();
        }

        public void InitBindings()
        {
            for (long i = 0; i < 32753; i++)
            {
                EEPROMRow row = new EEPROMRow(i.ToString("X"));
                
                if (i == 0)
                {
                    row.Zero = "AA";
                    row.One = "07";
                }

                rows.Add(row);
            }

            RowTable.ItemsSource = rows;
        }
    }
}
