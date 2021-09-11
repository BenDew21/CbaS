using CBaSCore.Chip.UI.Controls;
using CBaSCore.Framework.Business;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Logic.UI.Utility_Classes;
using CBaSCore.Project.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace CBaSCore.Framework.UI.Handlers
{
    public class TabHandler
    {
        private static TabHandler _instance = null;

        private LayoutDocumentPane _control;
        private Dictionary<int, double> _zoomLevels = new Dictionary<int, double>();

        private IdentifiableTabItem _selectedTab;

        public static TabHandler GetInstance()
        {
            if (_instance == null) _instance = new TabHandler();
            return _instance;
        }

        public void RegisterTabControl(LayoutDocumentPane control)
        {
            _control = control;
        }

        // TODO - Re-add this
        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // IdentifiableTabItem newSelectedItem = (IdentifiableTabItem) _control.SelectedItem;
            //
            // // Newly selected tab is not null, so register it
            // if (newSelectedItem != null)
            // {
            //     ToolbarEventHandler.GetInstance().RegisterCircuitView(newSelectedItem.CircuitView);
            //     WireStatus.GetInstance().SetCircuitView(newSelectedItem.CircuitView);
            // }
            //
            // _selectedTab = newSelectedItem;
        }

        public void AddTab(int id, string name)
        {
            Debug.WriteLine("Opening " + id);
            
            bool exists = false;
            
            foreach (var item in _control.Children)
            {
                var identifiableItem = item as IdentifiableTabItem;
                if (identifiableItem != null && identifiableItem.ID == id)
                {
                    identifiableItem.IsSelected = true;
                    exists = true;
                    break;
                }
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

        public void RemoveTab(ZoomTabContent tab)
        {
            // if (_selectedTab != null && tab.ID == _selectedTab.ID)
            // {
            //     int index = _control.SelectedIndex;
            //
            //     // Index 0 - the first tab in the collection
            //     if (index == 0)
            //     {
            //         if (_control.Items.Count < 1)
            //         {
            //             _control.SelectedIndex = index + 1;
            //         }
            //     }
            //     else
            //     {
            //         _control.SelectedIndex = index - 1;
            //     }
            // }
            //
            // _control.Items.Remove(tab);
            //
            // // Tidy it up - required for GC
            // tab.CircuitView.UnregisterView();
            // tab.UnregisterControl();
            // tab.Header = null;
            // tab.Content = null;
            // tab = null;
        }

        public void RemoveTab(TabItem tab)
        {
            //     if (_control.SelectedItem == tab)
            //     {
            //         int index = _control.SelectedIndex;
            //
            //         // Index 0 - the first tab in the collection
            //         if (index == 0)
            //         {
            //             if (_control.Items.Count < 1)
            //             {
            //                 _control.SelectedIndex = index + 1;
            //             }
            //             _control.Items.Remove(tab);
            //         }
            //         else
            //         {
            //             _control.SelectedIndex = index - 1;
            //             _control.Items.Remove(tab);
            //         }
            //     }
            //     else
            //     {
            //         _control.Items.Remove(tab);
            //     }
            // }
        }
    }
}