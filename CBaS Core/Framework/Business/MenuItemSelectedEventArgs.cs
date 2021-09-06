using CBaSCore.Framework.UI;
using System;

namespace CBaSCore.Framework.Business
{
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public CustomMenuItem SelectedItem { get; set; }
    }
}