using Combinatorics_Calculator.Framework.UI.Handlers;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using Combinatorics_Calculator.Project.Business;
using System;
using System.Windows;
using System.Windows.Input;

namespace Combinatorics_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MouseHandlingMode _mouseMode;
        private Point _originalMouseDownPoint;

        public MainWindow()
        {
            InitializeComponent();
            // WireStatus.GetInstance().SetCircuitView(CircuitViewControl);
            ProjectViewHandler.GetInstance().SetTreeView(Explorer);

            TabHandler.GetInstance().RegisterTabControl(CircuitsTabControl);
        }
    }
}