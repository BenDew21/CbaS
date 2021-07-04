using CBaS_Core.Framework.UI;
using System;
using System.Windows.Input;

namespace CBaS_Core.Collab.UI.Controls
{
    public class ConnectToolbarButton : BaseToolbarItem
    {
        public ConnectToolbarButton() : base(MenuName.Collab, Collab.Resources.Collab_Resources.Connect, "Connect", false)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
        }
    }
}
