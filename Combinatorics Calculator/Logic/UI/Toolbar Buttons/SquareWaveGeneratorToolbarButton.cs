using Combinatorics_Calculator.Framework.UI;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Logic.UI.Toolbar_Buttons
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