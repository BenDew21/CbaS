using System.Windows;

namespace Combinatorics_Calculator.Framework.UI.Base_Classes
{
    public interface ICanvasElement
    {
        public UIElement GetControl();

        void CreateControl();

        ICanvasElement GetNew();

        void SetPlaced();
    }
}