using System;
using System.Collections.Generic;
using System.Diagnostics;
using CBaSCore.Framework.Business;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Logic.UI.Utility_Classes;
using Xceed.Wpf.AvalonDock.Layout;

namespace CBaSCore.Framework.UI.Handlers
{
    public class TabHandler
    {
        private static TabHandler _instance;

        private LayoutDocumentPane _control;

        private IdentifiableTabItem _selectedTab;
        private Dictionary<int, double> _zoomLevels = new();

        public static TabHandler GetInstance()
        {
            if (_instance == null) _instance = new TabHandler();
            return _instance;
        }

        public void RegisterTabControl(LayoutDocumentPane control)
        {
            _control = control;
        }

        public void Tab_SelectionChanged(object sender, EventArgs e)
        {
            var newSelectedItem = (IdentifiableTabItem) _control.SelectedContent;

            // Newly selected tab is not null, so register it
            if (newSelectedItem != null)
            {
                ToolbarEventHandler.GetInstance().RegisterCircuitView(newSelectedItem.CircuitView);
                WireStatus.GetInstance().SetCircuitView(newSelectedItem.CircuitView);
            }

            _selectedTab = newSelectedItem;
        }

        public void AddTab(int id, string name)
        {
            Debug.WriteLine("Opening " + id);

            var exists = false;

            foreach (var item in _control.Children)
                if (item is IdentifiableTabItem identifiableItem && identifiableItem.ID == id)
                {
                    identifiableItem.IsSelected = true;
                    exists = true;
                    break;
                }

            if (!exists)
            {
                var c = CircuitHandler.GetInstance().GetCircuit(id);
                var content = new ZoomTabContent(c);
                content.Title = name;
                content.ID = id;
                // content.SetID(id);
                // content.SetHeader(name);
                _control.Children.Add(content);
                content.IsSelected = true;
            }
        }
    }
}