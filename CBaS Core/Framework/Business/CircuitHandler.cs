using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Controls;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Project.Storage;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace CBaSCore.Framework.Business
{
    /// <summary>
    /// What needs to be saved:
    ///     Wires:
    ///         - Start pos
    ///         - End pos
    ///         - Linked wires
    ///         - Output gate/wire
    ///
    ///     ICanvasElements:
    ///         - Top position
    ///         - Left position
    ///         - Input wires (if applicable)
    ///         - Output wires (if applicable)
    ///         - State (if applicable)
    ///
    /// Saving logic:
    ///     - Save start, end x and y for wires
    ///     - Create linked wires list based on IDs
    ///     - Save top and left points for ICanvasElements
    ///     - Save input wires based on IDs
    ///     - Save output wires based on IDs
    ///     - Add output gates/wires to wires
    /// </summary>
    public class CircuitHandler
    {
        private static CircuitHandler _instance;

        private CircuitView _view;

        public int WireIterator = 1;
        public int ICanvasElementIterator = 1;

        private Dictionary<int, Circuit> _circuits = new Dictionary<int, Circuit>();

        public static CircuitHandler GetInstance()
        {
            if (_instance == null) _instance = new CircuitHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _view = view;
        }

        public void LoadCircuit(int nodeID, string name, string path)
        {
            Circuit circuit;
            try
            {
                XElement document = XElement.Load(path);
                circuit = new Circuit(document);
            }
            catch (Exception)
            {
                circuit = new Circuit();
            }

            circuit.Name = name;
            circuit.Path = path;
            _circuits.Add(nodeID, circuit);
        }

        public void OpenCircuit(int nodeID)
        {
            CircuitView view = new CircuitView();
            view.Draw(_circuits[nodeID]);
        }

        public Circuit GetCircuit(int id)
        {
            return _circuits[id];
        }

        public void SaveAll()
        {
            foreach (var circuit in _circuits)
            {
                circuit.Value.Save();
            }
        }
    }
}