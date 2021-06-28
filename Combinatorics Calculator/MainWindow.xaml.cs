using CBaS_Core.Framework.UI.Handlers;
using CBaS_Core.Project.Business;
using System.Windows;

namespace CBaS_Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ProjectViewHandler.GetInstance().SetTreeView(Explorer);
            TabHandler.GetInstance().RegisterTabControl(CircuitsTabControl);
            ProgressBarHandler.GetInstance().SetControls(ProgressBarStatus, LabelStatus);
        }
    }
}