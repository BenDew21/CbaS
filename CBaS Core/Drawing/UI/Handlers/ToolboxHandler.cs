using System.Windows.Controls;
using CBaSCore.Displays.UI.Controls;
using CBaSCore.Drawing.UI.Controls;
using CBaSCore.Framework.UI.Base_Classes;
using CBaSCore.Logic.Resources;
using CBaSCore.Logic.UI.Controls;
using CBaSCore.Logic.UI.Controls.EEPROMs;
using CBaSCore.Logic.UI.Controls.Logic_Gates;
using CBaSCore.Project.Storage;
using CBaSCore.Project.UI.Nodes;

namespace CBaSCore.Drawing.UI.Handlers
{
    /// <summary>
    /// ToolboxHandler - Singleton handler for the Toolbox
    /// </summary>
    public class ToolboxHandler
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ToolboxHandler _instance;

        /// <summary>
        /// The TreeView control
        /// </summary>
        private TreeView _treeView;
        
        /// <summary>
        /// Singleton instance getter
        /// </summary>
        /// <returns>The Singleton instance</returns>
        public static ToolboxHandler GetInstance()
        {
            return _instance ??= new ToolboxHandler();
        }

        /// <summary>
        /// Sets the TreeView control
        /// </summary>
        /// <param name="view">The view</param>
        public void SetTreeView(TreeView view)
        {
            _treeView = view;
            GenerateView();
        }

        /// <summary>
        /// Generate the view
        /// </summary>
        private void GenerateView()
        {
            var idIterator = 0;
            
            var logicGatesFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "Logic Gates"
            });
            
            var ioFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "I/O"
            });
            
            var timingFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "Timing"
            });
            
            var eepromFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "EEPROMs"
            });
            
            var connectionsFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "I/O"
            });
            
            var drawingFolder = new FolderNode(new StructureModel()
            {
                ID = idIterator++,
                ParentID = 0,
                Name = "Drawing Tools"
            });
            
            var commonFolder = new FolderNode(new StructureModel()
            {
                // ReSharper disable once RedundantAssignment - This is for the sake of adding new folders with unique IDs
                ID = idIterator++,
                ParentID = 0,
                Name = "Common Tools"
            });

            _treeView.SelectedItemChanged += (sender, args) =>
            {
                if (args.OldValue is ToolNode oldSelection)
                {
                    oldSelection.OnUnselect();
                }

                if (args.NewValue is ToolNode newSelection)
                {
                    newSelection.OnSelect();
                }
            };
            
            // Add items to the folders
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "AND Gate"}, Logic_Resources.AND, new ANDGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "OR Gate"}, Logic_Resources.OR, new ORGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "NOT Gate"}, Logic_Resources.NOT, new NOTGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "NAND Gate"}, Logic_Resources.NAND, new NANDGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "NOR Gate"}, Logic_Resources.NOR, new NORGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "XOR Gate"}, Logic_Resources.XOR, new XORGateToolboxItem()));
            logicGatesFolder.Items.Add(new ToolNode(new StructureModel {Name = "XNOR Gate"}, Logic_Resources.XNOR, new XNORGateToolboxItem()));

            ioFolder.Items.Add(new ToolNode(new StructureModel {Name = "Input"}, Logic_Resources.Terminal, new InputToolboxItem()));
            ioFolder.Items.Add(new ToolNode(new StructureModel {Name = "Output"}, Logic_Resources.Output, new OutputToolboxItem()));
            ioFolder.Items.Add(new ToolNode(new StructureModel {Name = "7-Segment Display"}, Logic_Resources.Output, new SevenSegmentDisplayToolboxItem()));

            timingFolder.Items.Add(new ToolNode(new StructureModel {Name = "Square Wave Generator"}, Logic_Resources.Square_Wave, new SquareWaveGeneratorToolboxItem()));

            eepromFolder.Items.Add(new ToolNode(new StructureModel {Name = "28C16"}, Logic_Resources._28C16, new EEPROM2816CToolboxItem()));
            
            connectionsFolder.Items.Add(new WiringToolNode(new StructureModel {Name = "Wire"}, Logic_Resources.Wire));
            
            _treeView.Items.Add(logicGatesFolder);
            _treeView.Items.Add(ioFolder);
            _treeView.Items.Add(timingFolder);
            _treeView.Items.Add(eepromFolder);
            _treeView.Items.Add(connectionsFolder);
            _treeView.Items.Add(drawingFolder);
            _treeView.Items.Add(commonFolder);
        }
    }
    
    /// <summary>
    /// Interface for items stored in the toolbox
    /// </summary>
    public interface IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        ICanvasElement GetSelectedItem();
    }
    
    /// <summary>
    /// ANDGateToolboxItem - Toolbox item for AND gate
    /// </summary>
    internal class ANDGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new ANDGate();
        }
    }

    /// <summary>
    /// NANDGateToolboxItem - Toolbox item for NAND gate
    /// </summary>
    internal class NANDGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new NANDGate();
        }
    }

    /// <summary>
    /// NORGateToolboxItem - Toolbox item for NOR gate
    /// </summary>
    internal class NORGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new NORGate();
        }
    }

    /// <summary>
    /// NOTGateToolboxItem - Toolbox item for NOT gate
    /// </summary>
    internal class NOTGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new NOTGate();
        }
    }

    /// <summary>
    /// ORGateToolboxItem - Toolbox item for OR gate
    /// </summary>
    internal class ORGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new ORGate();
        }
    }

    /// <summary>
    /// XNORGateToolboxItem - Toolbox item for XNOR gate
    /// </summary>
    internal class XNORGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new XNORGate();
        }
    }

    /// <summary>
    /// XORGateToolboxItem - Toolbox item for XOR gate
    /// </summary>
    internal class XORGateToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new XORGate();
        }
    }
    
    /// <summary>
    /// InputToolboxItem - Toolbox item for input control
    /// </summary>
    internal class InputToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new InputControl();
        }
    }
    
    /// <summary>
    /// OutputToolboxItem - Toolbox item for output gate
    /// </summary>
    internal class OutputToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new OutputControl();
        }
    }
    
    /// <summary>
    /// SevenSegmentDisplayToolboxItem - Toolbox item for seven-segment display
    /// </summary>
    internal class SevenSegmentDisplayToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new SegmentedDisplay();
        }
    }
    
    /// <summary>
    /// SquareWaveGeneratorToolboxItem - Toolbox item for square wave generator
    /// </summary>
    internal class SquareWaveGeneratorToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new SquareWaveGenerator();
        }
    }
    
    /// <summary>
    /// EEPROM2816CToolboxItem - Toolbox item for square wave generator
    /// </summary>
    internal class EEPROM2816CToolboxItem : IToolboxItem
    {
        /// <summary>
        /// Get the control when this item is selected
        /// </summary>
        /// <returns>The control when this item is selected</returns>
        public ICanvasElement GetSelectedItem()
        {
            return new EEPROM28C16();
        }
    }
}