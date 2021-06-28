using CBaS_Core.Framework.UI.Base_Classes;
using CBaS_Core.Framework.UI.Controls;
using CBaS_Core.Framework.UI.Utility_Classes;
using System;
using System.Windows;
using System.Windows.Input;

namespace CBaS_Core.Framework.UI.Handlers
{
    public class DragHandler
    {
        private static DragHandler _instance;

        public bool IsActive { get; set; }

        private CircuitView _circuitView;

        private ICanvasElement _selectedElement;

        private Point _offsetPoint;

        public static DragHandler GetInstance()
        {
            if (_instance == null) _instance = new DragHandler();
            return _instance;
        }

        public void RegisterCircuitView(CircuitView view)
        {
            _circuitView = view;
        }

        /// <summary>
        /// Called by shape itself
        /// </summary>
        /// <param name="e"></param>
        public void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsActive)
            {
                var element = sender as ICanvasElement;
                Mouse.Capture(element.GetControl());
                _offsetPoint = e.GetPosition(element.GetControl());
                _selectedElement = element;
            }
        }

        /// <summary>
        /// Called by canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedElement != null)
            {
                Mouse.OverrideCursor = Cursors.SizeAll;
                // _circuitView = (sender as ICanvasElement) as CircuitView;
                var mousePoint = e.GetPosition(_circuitView);

                Tuple<double, double> snappedValues = Utilities.GetSnap(mousePoint.X - _offsetPoint.X,
                    mousePoint.Y - _offsetPoint.Y, _selectedElement.GetSnap());

                _selectedElement.UpdatePosition(snappedValues.Item1, snappedValues.Item2);
            }
        }

        /// <summary>
        /// Called by shape itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsActive)
            {
                Mouse.Capture(null);
                _selectedElement = null;
                Mouse.OverrideCursor = null;
            }
        }

        private DragHandler()
        {
            IsActive = false;
        }
    }
}