using CBaS_Core.Framework.UI;
using System;
using System.Windows.Input;

namespace CBaS_Core.Collab.UI.Controls
{
    public class HostToolbarButton : BaseToolbarItem
    {
        public HostToolbarButton() : base(MenuName.Collab, Collab.Resources.Collab_Resources.Host, "Host", false)
        {
        }
        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
        }
    }
}
