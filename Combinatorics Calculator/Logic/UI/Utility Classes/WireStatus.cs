﻿using CBaS_Core.Framework.UI.Controls;
using CBaS_Core.Framework.UI.Utility_Classes;
using CBaS_Core.Logic.UI.Controls.Wiring;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CBaS_Core.Logic.UI.Utility_Classes
{
    public class WireStatus
    {
        private CircuitView _canvas;

        private bool _isSelected = false;
        private Wire _wire;

        private static WireStatus _instance = null;

        public static WireStatus GetInstance()
        {
            if (_instance == null) _instance = new WireStatus();
            return _instance;
        }

        public void SetCircuitView(CircuitView canvas)
        {
            _canvas = canvas;
        }

        public Tuple<double, double> GetPointRelativeToCanvas(MouseButtonEventArgs e)
        {
            Point basePoint = e.GetPosition(_canvas);
            return Utilities.GetSnap(basePoint.X, basePoint.Y, 10);
        }

        public Point GetPointCanvas(MouseButtonEventArgs e)
        {
            return e.GetPosition(_canvas);
        }

        public bool GetSelected()
        {
            return _isSelected;
        }

        public void SetSelected(bool selected)
        {
            _isSelected = selected;
        }

        public Wire GetWire()
        {
            return _wire;
        }

        public void SetWire(Wire wire)
        {
            _wire = wire;
            if (wire != null)
            {
                _canvas.Children.Add(wire.GetControl());
            }
        }

        public void SetStart(double x, double y)
        {
            _wire = new Wire();
            _wire.SetStart(x, y);
            _canvas.Children.Add(_wire.GetControl());
            _canvas.Children.Add(_wire.GetStartEllipse());
            Canvas.SetZIndex(_wire.GetControl(), 1);
            Canvas.SetZIndex(_wire.GetStartEllipse(), 2);

            _wire.ID = _canvas.GetCircuit().GetNextWireIterator();
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            _wire.SetEnd(x, y, observer);
            _canvas.Children.Add(_wire.GetEndEllipse());
            Canvas.SetZIndex(_wire.GetEndEllipse(), 2);
            _canvas.GetCircuit().AddWire(_wire);
            _wire = null;
        }
    }
}