using CBaS_Core.Framework.UI;
using CBaS_Core.Logic.Resources;
using CBaS_Core.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Toolbar_Buttons
{
    public class SquareWaveGeneratorToolbarButton : BaseToolbarItem
    {
        public SquareWaveGeneratorToolbarButton() : base(MenuName.Logic, Logic_Resources.Square_Wave, "Square Wave", true)
        {
        }

        public override void ButtonClicked(object sender, MouseButtonEventArgs args)
        {
            toolbarEventHandler.CanvasButtonPressed(IsSelected, new SquareWaveGenerator());
        }
    }
}