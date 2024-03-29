﻿using System;
using System.Collections.Generic;
using CBaS_Core.Collab.UI.Controls;
using CBaSCore.Drawing.UI.Toolbar_Buttons;
using CBaSCore.Framework.Business;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Logic.UI.Toolbar_Buttons.EEPROMs;

using CBaSCore.Project.UI;

namespace CBaSCore.Framework.UI.Handlers
{
    public class MenuBarHandler
    {
        private static MenuBarHandler _instance;

        private SortedDictionary<MenuName, CustomMenuItem> _menuItems =
            new();

        private BaseToolbarItem _selectedButton;

        private readonly SortedDictionary<MenuName, List<BaseToolbarItem>> _toolbarItems =
            new();

        private CustomMenuBar _wpfMenuBar;
        private CustomToolBar _wpfToolbar;

        private CustomMenuItem selectedItem;

        public static MenuBarHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MenuBarHandler();
                GetInstance().RegisterButtons();
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
                var name = (MenuName) menuName;

                var item = new CustomMenuItem(name);
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
            // File Menu Items
            _ = new NewToolbarButton();
            _ = new SaveToolbarItem();
            _ = new OpenToolbarItem();
            _ = new OpenProjectToolbarButton();

            // Drawing Menu Items
            _ = new LabelToolbarButton();
            _ = new MoveToolbarButton();

            // Collab Menu Items
            _ = new ConnectToolbarButton();
            _ = new HostToolbarButton();
        }
    }
}