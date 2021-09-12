using System.Windows.Input;
using CBaS_Core.Collab.Resources;
using CBaSCore.Framework.UI;

namespace CBaS_Core.Collab.UI.Controls
{
    public class HostToolbarButton : BaseToolbarItem
    {
        public HostToolbarButton() : base(MenuName.Collab, Collab_Resources.Host, "Host", false)
        {
        }
        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
        }
    }
}
