﻿using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    public class Wire : Shape, IWireObserver
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        // private Line _line;
        private IWireObserver _wireObserver;

        private bool _status;

        private List<Wire> _linkedWires = new List<Wire>();
        private WireStatus _wireStatus = WireStatus.GetInstance();

        private Ellipse _sourceEllipse;
        private Ellipse _endEllipse;

        private LineGeometry _line = new LineGeometry();


        protected override Geometry DefiningGeometry
        {
            get
            {
                _line.StartPoint = new Point(X1, Y1);
                _line.EndPoint = new Point(X2, Y2);
                return _line;
            }
        }

        public Wire()
        {
            //_line = new Line();
            //_line.Stroke = Brushes.Black;
            //_line.StrokeThickness = 1.3;
            //_line.MouseDown += Wire_MouseDown;

            Stroke = Brushes.Black;
            StrokeThickness = 1.3;
            MouseDown += Wire_MouseDown;

            _sourceEllipse = new Ellipse();
            _sourceEllipse.Width = 5;
            _sourceEllipse.Height = 5;
            _sourceEllipse.Stroke = Brushes.Black;
            _sourceEllipse.Fill = Brushes.Black;

            _endEllipse = new Ellipse();
            _endEllipse.Width = 5;
            _endEllipse.Height = 5;
            _endEllipse.Stroke = Brushes.Black;
            _endEllipse.Fill = Brushes.Black;
        }

        private void Wire_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _wireStatus.GetSelected())
            {
                Tuple<double, double> point = _wireStatus.GetPointRelativeToCanvas(e);
                if (_wireStatus.GetWire() == null)
                {
                    _wireStatus.SetStart(point.Item1, point.Item2);
                    Wire wire = sender as Wire;
                    wire.SetOutput(_wireStatus.GetWire());
                }
                else
                {
                    _wireStatus.SetEnd(point.Item1, point.Item2, this);
                }
            }

            e.Handled = true;
        }

        public Wire GetControl()
        {
            // return _line;
            return this;
        }

        public Ellipse GetStartEllipse()
        {
            return _sourceEllipse;
        }

        public Ellipse GetEndEllipse()
        {
            return _endEllipse;
        }

        public void SetStart(double x, double y)
        {
            X1 = x;
            Y1 = y;
            _line.StartPoint = new Point(X1, Y1);
            // _line.X1 = x;
            // _line.Y1 = y;

            Canvas.SetLeft(_sourceEllipse, x - 2.5);
            Canvas.SetTop(_sourceEllipse, y - 2.5);
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            X2 = x;
            Y2 = y;
            _line.EndPoint = new Point(X2, Y2);
            // _line.X2 = x;
            // _line.Y2 = y;

            Canvas.SetLeft(_endEllipse, x - 2.5);
            Canvas.SetTop(_endEllipse, y - 2.5);

            if (observer != null)
            {
                if (observer is Wire)
                {
                    Wire wire = (Wire)observer;
                    wire.AddLinkedWire(this);
                }

                _wireObserver = observer;
            }
        }

        public void SetOutput(IWireObserver observer)
        {
            _wireObserver = observer;
        }

        public void ToggleStatus(bool status)
        {
            _status = status;
            if (status)
            {
                // _line.Stroke = Brushes.Green;
                Stroke = Brushes.Green;
                _sourceEllipse.Stroke = Brushes.Green;
                _sourceEllipse.Fill = Brushes.Green;
                _endEllipse.Stroke = Brushes.Green;
                _endEllipse.Fill = Brushes.Green;
            }
            else
            {
                // _line.Stroke = Brushes.Black;
                Stroke = Brushes.Black;
                _sourceEllipse.Stroke = Brushes.Black;
                _sourceEllipse.Fill = Brushes.Black;
                _endEllipse.Stroke = Brushes.Black;
                _endEllipse.Fill = Brushes.Black;
            }
            if (_wireObserver != null) _wireObserver.WireStatusChanged(this, status);
        }

        public bool GetStatus()
        {
            return _status;
        }

        public void AddLinkedWire(Wire wire)
        {
            _linkedWires.Add(wire);
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            ToggleStatus(wire.GetStatus());

            foreach (Wire linkedWire in _linkedWires)
            {
                linkedWire.ToggleStatus(status);
            }
        }
    }
}