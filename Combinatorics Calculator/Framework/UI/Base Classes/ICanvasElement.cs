using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Framework.UI.Base_Classes
{
    /// <summary>
    /// TODO: Extract this to abstract class
    /// Needs to include instance variables for Top, Left, Input Wires, Output Wires
    /// </summary>

    public interface ICanvasElement
    {
        public UIElement GetControl();

        void CreateControl();

        ICanvasElement GetNew();

        void SetPlaced();

        int GetSnap();

        int GetOffset();

        void Save(XmlWriter writer);

        void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires);
    }
}