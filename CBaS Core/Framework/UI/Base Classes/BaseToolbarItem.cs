using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;

namespace CBaSCore.Framework.UI
{
    public abstract class BaseToolbarItem : StackPanel
    {
        private readonly bool _isSelectable;
        private readonly Image imageControl = new();
        private readonly Label label = new();

        protected bool isEnabled = true;
        protected bool IsSelected;

        protected ToolbarEventHandler toolbarEventHandler = ToolbarEventHandler.GetInstance();

        protected BaseToolbarItem(MenuName parentMenu, Bitmap icon, string buttonName,
            bool isSelectable)
        {
            _isSelectable = isSelectable;
            Draw(icon, buttonName);
            Register(parentMenu);
            MouseLeftButtonDown += (sender, args) =>
            {
                if (isEnabled) ButtonClickedEvent(sender, args);
            };
        }

        protected void DisableButton()
        {
            isEnabled = false;
            Opacity = 0.2;
        }

        protected void EnableButton()
        {
            isEnabled = true;
            Opacity = 1.0;
        }

        protected void Draw(Bitmap icon, string buttonName)
        {
            Orientation = Orientation.Vertical;

            var image = UiIconConverter.BitmapToBitmapImage(icon);

            imageControl.Width = 30;
            imageControl.Height = 30;
            imageControl.Source = image;

            label.Content = buttonName;
            label.HorizontalAlignment = HorizontalAlignment.Center;

            Children.Add(imageControl);
            Children.Add(label);
            Margin = new Thickness(20, 5, 0, 0);

            MouseLeave += (sender, args) => { Background = null; };
            MouseEnter += (sender, args) => { Background = Brushes.LightBlue; };
            MouseLeave += (sender, args) =>
            {
                if (IsSelected)
                    Background = Brushes.Beige;
                else
                    Background = null;
            };
        }

        public void Register(MenuName parentMenu)
        {
            MenuBarHandler.GetInstance().RegisterToolbarButton(parentMenu, this);
        }

        public void ButtonClickedEvent(object sender, MouseButtonEventArgs args)
        {
            if (_isSelectable) MenuBarHandler.GetInstance().ResetSelection(this);
            ButtonClicked(sender, args);
        }

        public void ToggleSelected(bool isSelected)
        {
            IsSelected = isSelected;
            if (isSelected)
                Background = Brushes.Beige;
            else
                Background = null;
        }

        public abstract void ButtonClicked(object sender, MouseButtonEventArgs args);
    }
}