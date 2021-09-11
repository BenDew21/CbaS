using System;
using CBaSCore.Framework.UI;

namespace CBaSCore.Framework.Business
{
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public CustomMenuItem SelectedItem { get; set; }
    }
}