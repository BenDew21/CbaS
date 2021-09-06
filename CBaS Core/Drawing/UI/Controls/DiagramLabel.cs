using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Framework.UI.Handlers;
using CBaSCore.Framework.UI.Utility_Classes;
using CBaSCore.Logic.UI.Controls.Wiring;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace CBaSCore.Drawing.UI.Controls
{
    public class DiagramLabel : ICanvasElement
    {
        private Grid _grid;
        private Label _labelContent;
        private TextBox _editTextBox;

        private string _labelValue = "Label";

        public DiagramLabel()
        {
            CreateControl();
        }

        public void CreateControl()
        {
            _labelContent = new Label();
            _labelContent.Content = _labelValue;

            _grid = new Grid();
            _grid.Background = System.Windows.Media.Brushes.Transparent;

            _grid.Children.Add(_labelContent);
        }

        public UIElement GetControl()
        {
            return _grid;
        }

        public ICanvasElement GetNew()
        {
            return new DiagramLabel();
        }

        public void SetPlaced()
        {
            Canvas.SetZIndex(_grid, 3);
            _grid.MouseDown += Control_MouseDown;
            _grid.MouseMove += Control_MouseMove;
            _grid.MouseUp += Control_MouseUp;
        }

        public void Control_MouseMove(object sender, MouseEventArgs e)
        {
            DragHandler.GetInstance().MouseMove(this, e);
        }

        public void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Edit(true);
            }
            else
            {
                DragHandler.GetInstance().MouseDown(this, e);
            }
        }

        public void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler.GetInstance().MouseUp(this, e);
        }

        private void Edit(bool shouldEdit)
        {
            if (shouldEdit)
            {
                _editTextBox = new TextBox();
                _editTextBox.Width = _labelContent.Width;
                _editTextBox.Height = _labelContent.Height;

                _editTextBox.Text = _labelValue;
                _editTextBox.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Escape || e.Key == Key.Enter)
                    {
                        _labelValue = _editTextBox.Text;
                        Edit(false);
                    }
                };

                _grid.Children.Remove(_labelContent);
                _grid.Children.Add(_editTextBox);
            }
            else
            {
                _grid.Children.Clear();
                _labelContent.Content = _labelValue;
                _grid.Children.Add(_labelContent);
            }
        }

        public void Load(XElement element, Dictionary<int, Wire> inputWires, Dictionary<int, Wire> outputWires)
        {
            Canvas.SetTop(_grid, Convert.ToDouble(element.Element("Top").Value));
            Canvas.SetLeft(_grid, Convert.ToDouble(element.Element("Left").Value));

            _labelValue = element.Element("Content").Value;
            _labelContent.Content = _labelValue;
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(SaveLoadTags.CANVAS_ELEMENT_NODE);
            writer.WriteElementString(SaveLoadTags.TYPE, "DiagramLabel");
            writer.WriteElementString("Content", _labelValue);
            writer.WriteElementString(SaveLoadTags.TOP, Canvas.GetTop(_grid).ToString());
            writer.WriteElementString(SaveLoadTags.LEFT, Canvas.GetLeft(_grid).ToString());
            writer.WriteEndElement();
        }

        public int GetSnap()
        {
            return 0;
        }

        public int GetOffset()
        {
            return 0;
        }

        public void UpdatePosition(double topX, double topY)
        {
            Canvas.SetLeft(GetControl(), topX);
            Canvas.SetTop(GetControl(), topY);
        }
    }
}