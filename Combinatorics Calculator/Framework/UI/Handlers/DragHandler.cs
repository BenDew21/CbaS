using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Combinatorics_Calculator.Framework.UI.Handlers
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
            var element = sender as ICanvasElement;
            Mouse.Capture(element.GetControl());
            _offsetPoint = e.GetPosition(element.GetControl());
            _selectedElement = element;
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
                // _circuitView = (sender as ICanvasElement) as CircuitView;
                var mousePoint = e.GetPosition(_circuitView);
                Canvas.SetLeft(_selectedElement.GetControl(), mousePoint.X - _offsetPoint.X);
                Canvas.SetTop(_selectedElement.GetControl(), mousePoint.Y - _offsetPoint.Y);
            }
        }

        /// <summary>
        /// Called by shape itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            _selectedElement = null;
        }

        private DragHandler()
        {
            IsActive = false;
        }
    }
}
