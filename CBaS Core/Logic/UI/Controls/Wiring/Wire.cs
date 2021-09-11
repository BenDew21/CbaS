﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CBaSCore.Logic.UI.Utility_Classes;

namespace CBaSCore.Logic.UI.Controls.Wiring
{
    public class Wire : Shape, IWireObserver
    {
        private WireTerminal _endEllipse;

        private readonly LineGeometry _line = new();

        private WireTerminal _sourceEllipse;

        private bool _status;

        private IWireObserver _wireObserver;

        private readonly WireStatus _wireStatus = WireStatus.GetInstance();

        public List<Wire> OutputWires = new();

        public Wire()
        {
            Stroke = Brushes.Black;
            StrokeThickness = 1.3;
            MouseDown += Wire_MouseDown;
        }

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public int ID { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {
                _line.StartPoint = new Point(X1, Y1);
                _line.EndPoint = new Point(X2, Y2);
                return _line;
            }
        }

        public void WireStatusChanged(Wire wire, bool status)
        {
            foreach (var outputWire in OutputWires)
                if (outputWire != this)
                    outputWire.ToggleStatus(status);
        }

        private void Wire_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _wireStatus.GetSelected())
            {
                var point = _wireStatus.GetPointRelativeToCanvas(e);

                if (_wireStatus.GetWire() == null)
                {
                    _wireStatus.SetStart(point.Item1, point.Item2);
                    var wire = sender as Wire;
                    wire.AddOutputWire(_wireStatus.GetWire());
                }
                else
                {
                    _wireStatus.SetEnd(point.Item1, point.Item2, this);
                }
            }

            e.Handled = true;
        }

        public void Terminal_MouseDown(Wire wire, Point point)
        {
            if (_wireStatus.GetSelected())
            {
                if (_wireStatus.GetWire() == null)
                {
                    _wireStatus.SetStart(point.X, point.Y);
                    wire.AddOutputWire(_wireStatus.GetWire());
                }
                else
                {
                    _wireStatus.SetEnd(point.X, point.Y, this);
                }
            }
        }

        public Wire GetControl()
        {
            return this;
        }

        public void CreateEllipses()
        {
            CreateStartEllipse(new Point(X1, Y1));
            CreateEndEllipse(new Point(X2, Y2));
        }

        public void CreateStartEllipse(Point centre)
        {
            _sourceEllipse = new WireTerminal(this, centre);
        }

        public void CreateEndEllipse(Point centre)
        {
            _endEllipse = new WireTerminal(this, centre);
        }

        public WireTerminal GetStartEllipse()
        {
            return _sourceEllipse;
        }

        public WireTerminal GetEndEllipse()
        {
            return _endEllipse;
        }

        public void SetStart(double x, double y)
        {
            X1 = x;
            Y1 = y;

            var startPoint = new Point(X1, Y1);
            _line.StartPoint = startPoint;
            CreateStartEllipse(startPoint);

            Canvas.SetLeft(_sourceEllipse, x - 2.5);
            Canvas.SetTop(_sourceEllipse, y - 2.5);
        }

        public void SetEnd(double x, double y)
        {
            X2 = x;
            Y2 = y;

            var endPoint = new Point(X2, Y2);
            _line.EndPoint = endPoint;
            CreateEndEllipse(endPoint);

            Canvas.SetLeft(_endEllipse, x - 2.5);
            Canvas.SetTop(_endEllipse, y - 2.5);
        }

        public void SetEnd(double x, double y, IWireObserver observer)
        {
            SetEnd(x, y);

            if (observer != null)
            {
                if (observer is Wire)
                {
                    var wire = (Wire) observer;
                    wire.AddOutputWire(this);
                }
                else
                {
                    _wireObserver = observer;
                }
            }
        }

        public void ToggleStatus(bool status)
        {
            _status = status;
            if (_sourceEllipse != null) _sourceEllipse.ToggleStatus(status);

            if (_endEllipse != null) _endEllipse.ToggleStatus(status);

            if (status)
                Stroke = Brushes.Green;
            else
                Stroke = Brushes.Black;
            if (_wireObserver != null) _wireObserver.WireStatusChanged(this, status);
            WireStatusChanged(this, status);
        }

        public bool GetStatus()
        {
            return _status;
        }

        public void AddOutputWire(Wire wire)
        {
            if (wire.ID != ID) OutputWires.Add(wire);
        }

        public void RegisterWireObserver(IWireObserver observer)
        {
            _wireObserver = observer;
        }
    }
}