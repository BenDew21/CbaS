using Combinatorics_Calculator.Framework.UI;
using System;

namespace Combinatorics_Calculator.Framework.Business
{
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public CustomMenuItem SelectedItem { get; set; }
    }
}