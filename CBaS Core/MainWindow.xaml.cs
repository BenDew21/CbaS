using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Project.Business;

namespace CBaSCore
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ProjectViewHandler.GetInstance().SetTreeView(Explorer);
            // TabHandler.GetInstance().RegisterTabControl(CircuitsTabControl);
            // ProgressBarHandler.GetInstance().SetControls(ProgressBarStatus, LabelStatus);

            // TabHandler.GetInstance().AddTab(new BuilderTab());
        }
    }
}