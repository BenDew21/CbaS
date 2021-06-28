using CBaS_Core.Framework.UI;
using System;

namespace CBaS_Core.Framework.Business
{
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public CustomMenuItem SelectedItem { get; set; }
    }
}