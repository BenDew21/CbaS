﻿using CBaS_Core.Framework.UI.Handlers;
using System.Windows.Controls;

namespace CBaS_Core.Framework.UI.Controls
{
    public class CustomMenuBar : StackPanel
    {
        private MenuBarHandler _handler;

        public CustomMenuBar()
        {
            Orientation = Orientation.Horizontal;
            _handler = MenuBarHandler.GetInstance();
            _handler.SetMenuBar(this);
            _handler.RenderMenuBar();
        }
    }
}