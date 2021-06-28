using CBaS_Core.Framework.Business;
using CBaS_Core.Framework.UI.Controls;
using CBaS_Core.Logic.UI.Utility_Classes;
using CBaS_Core.Project.Storage;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CBaS_Core.Framework.UI.Handlers
{
    public class TabHandler
    {
        private static TabHandler _instance = null;

        private TabControl _control;
        private Dictionary<int, ZoomTabContent> _tabs = new Dictionary<int, ZoomTabContent>();
        private Dictionary<int, double> _zoomLevels = new Dictionary<int, double>();

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
            ZoomTabContent content = (ZoomTabContent)_control.SelectedItem;
            ToolbarEventHandler.GetInstance().RegisterCircuitView(content.GetCircuitView());
            WireStatus.GetInstance().SetCircuitView(content.GetCircuitView());
        }

        public void AddTab(int id, string name)
        {
            Circuit c = CircuitHandler.GetInstance().GetCircuit(id);
            ZoomTabContent content = new ZoomTabContent(c);
            content.SetHeader(name);
            _control.Items.Add(content);
            _tabs.Add(id, content);
            _control.SelectedItem = content;
        }

        public void RemoveTab(ZoomTabContent tab)
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