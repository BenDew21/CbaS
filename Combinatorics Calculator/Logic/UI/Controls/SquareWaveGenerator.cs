﻿using Combinatorics_Calculator.Framework.UI.Base_Classes;
using Combinatorics_Calculator.Framework.UI.Utility_Classes;
using Combinatorics_Calculator.Logic.Resources;
using Combinatorics_Calculator.Logic.UI.Controls.Wiring;
using Combinatorics_Calculator.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace Combinatorics_Calculator.Logic.UI.Controls
{
    public class SquareWaveGenerator : ICanvasElement
    {
        private Grid _canvas;
        private System.Windows.Controls.Image _image;

        private Wire _outputWire;
        private bool _isRunning = false;

        private Dictionary<string, double> _frequencies = new Dictionary<string, double>();
        private ContextMenu _frequencyMenu = new ContextMenu();

        private MenuItem _selectedFrequency;
        private MenuItem _runningMenuItem;

        private string _selectedFrequencyString = "1Hz";

        private SquareWaveGeneratorTask _task;

        public SquareWaveGenerator()
        {
            _task = new SquareWaveGeneratorTask(this);

            CreateControl();
            CreateFrequencies();
            CreateContextMenu();
        }

        public void CreateControl()
        {
            _canvas = new Grid();
            _canvas.Width = 40;
            _canvas.Height = 40;
            _canvas.Background = System.Windows.Media.Brushes.White;

            _image = new System.Windows.Controls.Image();
            _image.Source = UIIconConverter.BitmapToBitmapImage(Logic_Resources.Square_Wave);
            _image.Stretch = System.Windows.Media.Stretch.Fill;
            _image.Width = 40;
            _image.Height = 40;

            _canvas.Children.Add(_image);
        }

        private void CreateFrequencies()
        {
            _frequencies.Add("0.5Hz", 0.5);
            _frequencies.Add("1Hz", 1);
            _frequencies.Add("2Hz", 2);
            _frequencies.Add("4Hz", 4);
            _frequencies.Add("8Hz", 8);
            _frequencies.Add("16Hz", 16);
        }

        private void CreateContextMenu()
        {
            _runningMenuItem = new MenuItem();
            _runningMenuItem.IsCheckable = true;
            _runningMenuItem.Header = "Running?";
            _runningMenuItem.Checked += (sender, e) =>
            {
                _isRunning = true;
                _task.Start();
            };
            _runningMenuItem.Unchecked += (sender, e) =>
            {
                _isRunning = false;
                _task.Stop();
            };
            _frequencyMenu.Items.Add(_runningMenuItem);
            _frequencyMenu.Items.Add(new Separator());

            foreach (KeyValuePair<string, double> frequencyPair in _frequencies)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.IsCheckable = true;
                menuItem.Header = frequencyPair.Key;
                menuItem.Checked += (sender, e) =>
                {
                    if (_selectedFrequency != null)
                    {
                        _selectedFrequency.IsChecked = false;
                    }
                    _selectedFrequency = menuItem;
                    _selectedFrequencyString = frequencyPair.Key;
                    _task.UpdateDelay(frequencyPair.Value);
                    // TODO: update simulation if still running
                };
                // menuItem.Unchecked += (sender, e)
                if (frequencyPair.Value == 1)
                {
                    menuItem.IsChecked = true;
                }
                _frequencyMenu.Items.Add(menuItem);
            }
        }

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && e.ClickCount == 2)
            {
                
            }
            else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                _frequencyMenu.IsOpen = true;
                _frequencyMenu.PlacementTarget = _canvas;
            }
            else
            {
                if (WireStatus.GetInstance().GetSelected() &&
                WireStatus.GetInstance().GetWire() == null)
                {
                    double x = Canvas.GetLeft(_canvas) + 20;
                    double y = Canvas.GetTop(_canvas) + 20;

                    WireStatus.GetInstance().SetStart(x, y);
                    _outputWire = WireStatus.GetInstance().GetWire();
                }
            }

            e.Handled = true;
        }

        public void SetOutputting(bool status)
        {
            _outputWire.ToggleStatus(status);
        }

        public UIElement GetControl()
        {
            return _canvas;
        }

        public ICanvasElement GetNew()
        {
            return new SquareWaveGenerator();
        }

        public void SetPlaced()
        {
            _canvas.MouseDown += Canvas_MouseDown;
            Canvas.SetZIndex(_canvas, 3);
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "SquareWaveGenerator");
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(_canvas).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(_canvas).ToString());
            writer.WriteElementString(SaveLoadTags.RUNNING, _isRunning.ToString());
            writer.WriteElementString(SaveLoadTags.FREQUENCY, _selectedFrequencyString);
            writer.WriteStartElement(SaveLoadTags.OUTPUT_WIRES_NODE);
            writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
            if (_outputWire != null)
            {
                writer.WriteElementString(SaveLoadTags.OUTPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _outputWire.ID.ToString());
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_canvas, Convert.ToInt32(element.Element("Top").Value));
            Canvas.SetLeft(_canvas, Convert.ToInt32(element.Element("Left").Value));
            _selectedFrequencyString = Convert.ToString(element.Element(SaveLoadTags.FREQUENCY).Value);

            if (outputWires.Count > 0)
            {
                _outputWire = outputWires[1];
            }

            _task.UpdateDelay(_frequencies[_selectedFrequencyString]);
            _runningMenuItem.IsChecked = Convert.ToBoolean(element.Element(SaveLoadTags.RUNNING).Value);
        }
    }
}