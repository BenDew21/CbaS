using Combinatorics_Calculator.Framework.Business;
using Combinatorics_Calculator.Framework.UI.Controls;
using Combinatorics_Calculator.Logic.UI.Toolbar_Buttons;
using Combinatorics_Calculator.Logic.UI.Toolbar_Buttons.Logic_Gates;
using System;
using System.Collections.Generic;

namespace Combinatorics_Calculator.Framework.UI.Handlers
{
    public class MenuBarHandler
    {
        private static MenuBarHandler _instance;

        private SortedDictionary<MenuName, UI.CustomMenuItem> _menuItems =
            new SortedDictionary<MenuName, UI.CustomMenuItem>();

        private SortedDictionary<MenuName, List<BaseToolbarItem>> _toolbarItems =
            new SortedDictionary<MenuName, List<BaseToolbarItem>>();

        private CustomMenuBar _wpfMenuBar;
        private CustomToolBar _wpfToolbar;

        private CustomMenuItem selectedItem;
        private BaseToolbarItem _selectedButton;

        public static MenuBarHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MenuBarHandler();
                MenuBarHandler.GetInstance().RegisterButtons();
            }

            return _instance;
        }

        public void SetMenuBar(CustomMenuBar panel)
        {
            _wpfMenuBar = panel;
        }

        public void SetToolbar(CustomToolBar panel)
        {
            _wpfToolbar = panel;
        }

        public void RenderMenuBar()
        {
            foreach (var menuName in Enum.GetValues(typeof(MenuName)))
            {
                MenuName name = (MenuName)menuName;

                UI.CustomMenuItem item = new CustomMenuItem(name);
                item.MenuItemSelectedEvent += MenuItem_Clicked;
                // _menuItems.Add(name, item);
                _wpfMenuBar.Children.Add(item);

                if (_wpfMenuBar.Children.Count == 1)
                {
                    selectedItem = item;
                    selectedItem.Selected = true;
                    selectedItem.FormatSelected();
                }
            }
        }

        public void RenderToolBar(MenuName selectedItem)
        {
            // Refresh the toolbar
            _wpfToolbar.Children.Clear();

            // Add items to toolbar
            try
            {
                foreach (var button in _toolbarItems[selectedItem]) _wpfToolbar.Children.Add(button);
            }
            catch (KeyNotFoundException)
            {
                // Squash for debug purposes
            }
        }

        public void RegisterToolbarButton(MenuName parentName, BaseToolbarItem button)
        {
            try
            {
                _toolbarItems[parentName].Add(button);
            }
            catch (KeyNotFoundException)
            {
                _toolbarItems.Add(parentName, new List<BaseToolbarItem>());
                _toolbarItems[parentName].Add(button);
            }
        }

        public void ResetSelection(BaseToolbarItem newSelection)
        {
            // If the selected button is not null
            if (_selectedButton != null)
            {
                _selectedButton.ToggleSelected(false);
                if (_selectedButton == newSelection)
                {
                    _selectedButton = null;
                }
                else
                {
                    _selectedButton = newSelection;
                    _selectedButton.ToggleSelected(true);
                }
            }
            else
            {
                _selectedButton = newSelection;
                _selectedButton.ToggleSelected(true);
            }
        }

        private void MenuItem_Clicked(object sender, MenuItemSelectedEventArgs args)
        {
            selectedItem.FormatDefault();
            selectedItem.Selected = false;
            selectedItem = args.SelectedItem;
            selectedItem.FormatSelected();
            RenderToolBar(selectedItem.MenuName);
        }

        public void RegisterButtons()
        {
            _ = new WireToolbarButton();

            _ = new ANDGateToolBarButton();
            _ = new NANDGateToolbarButton();
            _ = new ORGateToolbarButton();
            _ = new NORGateToolbarButton();
            _ = new XORGateToolbarButton();
            _ = new XNORGateToolbarButton();
            _ = new NOTGateToolbarButton();

            _ = new TerminalToolBarButton();
            _ = new OutputToolbarButton();
        }
    }
}