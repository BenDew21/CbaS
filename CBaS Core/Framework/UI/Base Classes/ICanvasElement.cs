using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using CBaSCore.Logic.UI.Controls.Wiring;

namespace CBaSCore.Framework.UI.Base_Classes
{
    /// <summary>
    ///     TODO: Extract this to abstract class
    ///     Needs to include instance variables for Top, Left, Input Wires, Output Wires
    /// </summary>
    public interface ICanvasElement
    {
        public UIElement GetControl();

        void CreateControl();

        ICanvasElement GetNew();

        void SetPlaced();

        int GetSnap();

        int GetOffset();

        void UpdatePosition(double topX, double topY);

        void Control_MouseMove(object sender, MouseEventArgs e);

        void Control_MouseDown(object sender, MouseButtonEventArgs e);

        void Control_MouseUp(object sender, MouseButtonEventArgs e);

        void Save(XmlWriter writer);

        void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires);
    }
}