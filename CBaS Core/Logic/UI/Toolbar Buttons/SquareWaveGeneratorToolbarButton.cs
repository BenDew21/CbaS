using CBaSCore.Framework.UI;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls;
using System.Windows.Input;

namespace CBaSCore.Logic.UI.Toolbar_Buttons
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