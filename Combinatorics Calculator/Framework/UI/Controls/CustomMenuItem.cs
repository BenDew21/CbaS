using Combinatorics_Calculator.Framework.Business;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Combinatorics_Calculator.Framework.UI
{
    public class CustomMenuItem : StackPanel
    {
        private readonly Brush _defaultBackground = new SolidColorBrush(Color.FromRgb(77, 157, 224));
        private readonly Brush _defaultTextColor = Brushes.White;

        private readonly Brush _hoverBackground = new SolidColorBrush(Color.FromRgb(142, 184, 229));
        private readonly Brush _hoverTextColor = Brushes.Black;

        private readonly Label _label = new Label();

        private readonly Brush _selectedBackground = new SolidColorBrush(Color.FromRgb(241, 241, 241));
        private readonly Brush _selectedTextColor = new SolidColorBrush(Color.FromRgb(53, 133, 200));

        public bool Selected { get; set; }

        public MenuName MenuName { get; }

        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelectedEvent;

        public CustomMenuItem(MenuName menuName)
        {
            MenuName = menuName;
            Draw();
        }

        protected void Draw()
        {
            Orientation = Orientation.Vertical;
            Width = 80;
            Background = _defaultBackground;

            _label.Content = MenuName;
            _label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _label.Foreground = _defaultTextColor;

            Children.Add(_label);

            // Register handlers
            MouseEnter += (sender, args) =>
            {
                if (!Selected)
                {
                    Background = _hoverBackground;
                    _label.Foreground = _hoverTextColor;
                }
            };

            MouseLeave += (sender, args) =>
            {
                if (!Selected)
                {
                    Background = _defaultBackground;
                    _label.Foreground = _defaultTextColor;
                }
            };

            MouseLeftButtonDown += (sender, args) =>
            {
                if (!Selected)
                {
                    Selected = true;
                    var selectedEventArgs = new MenuItemSelectedEventArgs();
                    selectedEventArgs.SelectedItem = this;
                    MenuItemSelectedEvent(this, selectedEventArgs);
                }
            };
        }

        public void FormatSelected()
        {
            Background = _selectedBackground;
            _label.Foreground = _selectedTextColor;
        }

        public void FormatDefault()
        {
            Background = _defaultBackground;
            _label.Foreground = _defaultTextColor;
        }
    }
}