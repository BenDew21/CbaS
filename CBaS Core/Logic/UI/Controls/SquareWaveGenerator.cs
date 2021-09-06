using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls.Wiring;
using CBaSCore.Logic.UI.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace CBaSCore.Logic.UI.Controls
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
            _frequencies.Add("32Hz", 32);
            _frequencies.Add("64Hz", 32);
            _frequencies.Add("128Hz", 32);
            _frequencies.Add("256Hz", 32);
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

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler.GetInstance().MouseUp(this, e);
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
            DragHandler.GetInstance().MouseMove(this, e);
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
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
                else
                {
                    DragHandler.GetInstance().MouseDown(this, e);
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
            _canvas.MouseDown += Control_MouseDown;
            _canvas.MouseMove += Control_MouseMove;
            _canvas.MouseUp += Control_MouseUp;
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

            if (_outputWire != null)
            {
                writer.WriteStartElement(SaveLoadTags.WIRE_DETAIL_NODE);
                writer.WriteElementString(SaveLoadTags.OUTPUT, "1");
                writer.WriteElementString(SaveLoadTags.WIRE_ID, _outputWire.ID.ToString());
                writer.WriteEndElement();
            }

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

        public int GetSnap()
        {
            return 10;
        }

        public int GetOffset()
        {
            return 0;
        }

        public void UpdatePosition(double topX, double topY)
        {
            Canvas.SetLeft(GetControl(), topX);
            Canvas.SetTop(GetControl(), topY);

            _outputWire.SetStart(topX + 10, topY + 10);
        }
    }
}