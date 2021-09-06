using System;
using CBaSCore.Chip.UI.Controls;
using CBaSCore.Framework.Business;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Logic.UI.Utility_Classes;
using CBaSCore.Project.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace CBaSCore.Framework.UI.Handlers
{
    public class TabHandler
    {
        private static TabHandler _instance = null;

        private TabControl _control;
        private Dictionary<int, double> _zoomLevels = new Dictionary<int, double>();

        private IdentifiableTabItem _selectedTab;
        
        public static TabHandler GetInstance()
        {
            if (_instance == null) _instance = new TabHandler();
            return _instance;
        }

        public void RegisterTabControl(TabControl control)
        {
            _control = control;
            _control.SelectionChanged += Tab_SelectionChanged;
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IdentifiableTabItem newSelectedItem = (IdentifiableTabItem) _control.SelectedItem;

            // Newly selected tab is not null, so register it
            if (newSelectedItem != null)
            {
                ToolbarEventHandler.GetInstance().RegisterCircuitView(newSelectedItem.CircuitView);
                WireStatus.GetInstance().SetCircuitView(newSelectedItem.CircuitView);
            }

            _selectedTab = newSelectedItem;
        }

        public void AddTab(TabItem item)
        {
            item.Header = "Test Tab";
            _control.Items.Add(item);
            _control.SelectedItem = item;
            CircuitHandler.GetInstance().RegisterCircuitView((item as BuilderTab).GetCircuitView());
        }

        public void AddTab(int id, string name)
        {
            Debug.WriteLine("Opening " + id);
            
            bool exists = false;
            
            foreach (var item in _control.Items)
            {
                var identifiableItem = item as IdentifiableTabItem;
                if (identifiableItem != null && identifiableItem.ID == id)
                {
                    _control.SelectedItem = item;
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                Circuit c = CircuitHandler.GetInstance().GetCircuit(id);
                ZoomTabContent content = new ZoomTabContent(c);
                content.SetID(id);
                content.SetHeader(name);
                _control.Items.Add(content);
                _control.SelectedItem = content;

                //BuilderTab tab = new BuilderTab();
                //tab.GetCircuitView().Draw(c);

                //AddTab(tab);
            }
        }

        public void RemoveTab(ZoomTabContent tab)
        {
            if (_selectedTab != null && tab.ID == _selectedTab.ID)
            {
                int index = _control.SelectedIndex;

                // Index 0 - the first tab in the collection
                if (index == 0)
                {
                    if (_control.Items.Count < 1)
                    {
                        _control.SelectedIndex = index + 1;
                    }
                }
                else
                {
                    _control.SelectedIndex = index - 1;
                }
            }
            
            _control.Items.Remove(tab);
            
            // Tidy it up - required for GC
            tab.CircuitView.UnregisterView();
            tab.UnregisterControl();
            tab.Header = null;
            tab.Content = null;
            tab = null;
        }

        public void RemoveTab(TabItem tab)
        {
            if (_control.SelectedItem == tab)
            {
                int index = _control.SelectedIndex;

                // Index 0 - the first tab in the collection
                if (index == 0)
                {
                    if (_control.Items.Count < 1)
                    {
                        _control.SelectedIndex = index + 1;
                    }
                    _control.Items.Remove(tab);
                }
                else
                {
                    _control.SelectedIndex = index - 1;
                    _control.Items.Remove(tab);
                }
            }
            else
            {
                _control.Items.Remove(tab);
            }
        }
    }
}