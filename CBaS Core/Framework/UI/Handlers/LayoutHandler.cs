using System.Collections.Generic;
using CBaSCore.Framework.UI.Utility_Classes;
using Xceed.Wpf.AvalonDock.Layout;

namespace CBaSCore.Framework.UI.Handlers
{
    /// <summary>
    ///     LayoutHandler
    /// </summary>
    public class LayoutHandler
    {
        private static LayoutHandler _instance;

        #region Member Variables

        private readonly Dictionary<LayoutPosition, LayoutAnchorablePane> positions = new();

        #endregion

        #region Singleton Accessor

        public static LayoutHandler GetInstance()
        {
            if (_instance == null) _instance = new LayoutHandler();

            return _instance;
        }

        #endregion

        #region Methods

        public void AddPosition(LayoutPosition position, LayoutAnchorablePane pane)
        {
            positions[position] = pane;
        }

        public void AddLayout(LayoutPosition position, LayoutAnchorableDetails details)
        {
            var layout = LayoutAnchorableFactory.CreateLayout(details);

            var parentLayout = positions[position];
            parentLayout.Children.Add(layout);

            layout.IsSelected = true;
        }

        #endregion
    }
}