using CBaSCore.Drawing.UI.Handlers;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.GitIntegration.Business;
using CBaSCore.Project.Business;
using Microsoft.Win32;

namespace CBaSCore
{
    /// <summary>
    ///     Interaction logic for MainWindowAvalonia.xaml
    /// </summary>
    public partial class MainWindowAvalonia
    {
        #region Singleton references

        private readonly LayoutHandler _layoutHandler = LayoutHandler.GetInstance();

        #endregion

        /// <summary>
        ///     Constructor - called at runtime
        /// </summary>
        public MainWindowAvalonia()
        {
            InitializeComponent();

            ProjectViewHandler.GetInstance().SetTreeView(Explorer);
            TabHandler.GetInstance().RegisterTabControl(DocumentPane);
            ToolboxHandler.GetInstance().SetTreeView(ToolboxTreeView);
            
            GitHandler.GetInstance().SetRepository(@"C:\CC Example Project");
            
            // Initialise the control
            InitializePanes();
        }

        private void InitializePanes()
        {
            // Set the panes in the LayoutHandler
            _layoutHandler.AddPosition(LayoutPosition.TopLeft, TopLeftPane);
            _layoutHandler.AddPosition(LayoutPosition.BottomLeft, BottomLeftPane);
            _layoutHandler.AddPosition(LayoutPosition.TopRight, TopRightPane);
            _layoutHandler.AddPosition(LayoutPosition.BottomRight, BottomRightPane);
        }

        #region Button Methods

        private void OpenProjectButtonPressed(object x, object y)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var business = new OpenBusiness();
                business.OpenFile(openFileDialog.FileName, openFileDialog.SafeFileName);
            }
        }

        #endregion
    }
}