using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenLab.Lib
{
    public class Mapping
    {
        public Func<string> Get { get; private set; }
        public Action<string> Set { get; private set; }

        public Mapping(Func<string> Get, Action<string> Set)
        {
            this.Get = Get;
            this.Set = Set;
        }
    }

    public class Control : FlowLayoutPanel
    {
        public new Mapping Text;
        public Mapping Value;
        public string Pin;
        public bool LoggingEnabled
        {
            get
            {
                return LogValueMenuItem.Enabled;
            }
            set
            {
                LogValueMenuItem.Enabled = value;
            }
        }
        public bool Log
        {
            get
            {
                return LogValueMenuItem.Checked;
            }
            set
            {
                LogValueMenuItem.Checked = value;
            }
        }
        public bool Editing
        {
            set
            {
                ContextMenuStrip.Enabled = value;
                Enabled = value;

                foreach (var control in Controls.OfType<System.Windows.Forms.Control>())
                {
                    control.Enabled = !value;
                }

                if (value)
                {
                    BorderStyle = BorderStyle.FixedSingle;

                    MouseDown += new MouseEventHandler(Control_MouseDown);
                    MouseMove += new MouseEventHandler(Control_MouseMove);
                    MouseUp += new MouseEventHandler(Control_MouseUp);
                }
                else
                {
                    BorderStyle = BorderStyle.None;

                    MouseDown -= new MouseEventHandler(Control_MouseDown);
                    MouseMove -= new MouseEventHandler(Control_MouseMove);
                    MouseUp -= new MouseEventHandler(Control_MouseUp);
                }
            }
        }
        public IDictionary<string, Mapping> Settings { get; private set; } =
            new Dictionary<string, Mapping>();
        public IControlPlugin ControlPlugin { get; private set; }
        public delegate void SetValueDelagate_T(string Value);
        public SetValueDelagate_T SetValueDelagate;

        public static Control FromLocation(IControlPlugin ControlPlugin, Board Board, bool LoggingEnabled, Point Location)
        {
            var control = new Control(ControlPlugin, Board, LoggingEnabled, Location);

            control.LogValueMenuItem.Checked = false;
            ControlPlugin.Create(control);
            control.Text.Set(ControlPlugin.GetType().Assembly.GetName().Name);
            control.ControlLabelTextBox.Text = control.Text.Get();
            control.CreateSettingsMenuItems();

            return control;
        }

        public static Control FromConfig(IControlPlugin ControlPlugin, Board Board, bool LoggingEnabled, XElement Config,
            XNamespace Ns)
        {
            var control = new Control(ControlPlugin, Board, LoggingEnabled,
                new Point(Convert.ToInt32(Config.Element(Ns + "x").Value),
                Convert.ToInt32(Config.Element(Ns + "y").Value)));

            control.Pin = Config.Element(Ns + "pin").Value;
            Board.Pins.First(p => p.Name == control.Pin).Assigned = true;
            control.LogValueMenuItem.Checked = bool.Parse(Config.Element(Ns + "log").Value);
            ControlPlugin.Create(control);
            control.Text.Set(Config.Element(Ns + "label").Value);
            control.ControlLabelTextBox.Text = control.Text.Get();

            foreach (var setting in Config.Descendants(Ns + "setting"))
            {
                control.Settings[setting.Element(Ns + "label").Value]
                    .Set(setting.Element(Ns + "value").Value);
            }

            control.CreateSettingsMenuItems();

            return control;
        }

        public static Control Copy(Control Control)
        {
            var newControl = new Control(Control.ControlPlugin, Control.Board, Control.LoggingEnabled, Control.Location);

            newControl.Pin = Control.Pin;
            newControl.LogValueMenuItem.Checked = Control.Log;
            Control.ControlPlugin.Create(newControl);
            newControl.Text = Control.Text;

            foreach (var setting in Control.Settings)
            {
                newControl.Settings[setting.Key] = setting.Value;
            }

            return newControl;
        }

        public XElement ToConfig(XNamespace Ns)
        {
            var assembly = ControlPlugin.GetType().Assembly.GetName();
            var config = new XElement(Ns + "control",
                new XElement(Ns + "type", assembly.Name),
                new XElement(Ns + "version", assembly.Version),
                new XElement(Ns + "label", Text.Get()),
                new XElement(Ns + "x", Location.X),
                new XElement(Ns + "y", Location.Y),
                new XElement(Ns + "pin", Pin),
                new XElement(Ns + "log", Log));

            foreach (var setting in Settings)
            {
                config.Add(new XElement(Ns + "setting",
                    new XElement(Ns + "label", setting.Key),
                    new XElement(Ns + "value", setting.Value.Get())));
            }

            return config;
        }

        public void Initialize(SerialPort SerialPort)
        {
            this.SerialPort = SerialPort;
            Send("INIT");
        }

        public void WriteValue(string Value)
        {
            Send($"WRITE {Value}");
        }

        public string ReadValue()
        {
            Send("READ");

            return SerialPort.ReadLine();
        }

        private Board Board;
        private SerialPort SerialPort;
        private bool MouseIsDown = false;
        private Point OrigionalControlLocation, OrigionalMousePosition;

        private ToolStripMenuItem ControlLabelMenuItem = new ToolStripMenuItem("Control Label");
        private ToolStripTextBox ControlLabelTextBox = new ToolStripTextBox("Control Label");
        private ToolStripMenuItem SetPinMenuItem = new ToolStripMenuItem("Set Pin");
        private ToolStripComboBox SetPinComboBox = new ToolStripComboBox("Set Pin");
        private ToolStripMenuItem LogValueMenuItem = new ToolStripMenuItem("Log Value");
        private ToolStripMenuItem RemoveControlMenuItem = new ToolStripMenuItem("Remove Control");

        private Control(IControlPlugin ControlPlugin, Board Board, bool LoggingEnabled, Point Location)
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.ControlPlugin = ControlPlugin;
            this.Board = Board;
            this.Location = Location;
            this.LoggingEnabled = LoggingEnabled;
            SetValueDelagate = new SetValueDelagate_T(UpdateValue);

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ControlLabelMenuItem.DropDownItems.Add(ControlLabelTextBox);
            ControlLabelTextBox.TextChanged += new EventHandler(ControlLabelTextBox_TextChanged);
            SetPinMenuItem.DropDownItems.Add(SetPinComboBox);
            SetPinComboBox.SelectedIndexChanged += new EventHandler(SetPinComboBox_SelectedIndexChanged);
            LogValueMenuItem.CheckOnClick = true;
            RemoveControlMenuItem.Click += new EventHandler(RemoveControlMenuItem_Click);

            ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                ControlLabelMenuItem,
                SetPinMenuItem,
                LogValueMenuItem,
                RemoveControlMenuItem
            });
        }

        private void CreateSettingsMenuItems()
        {
            if (Settings.Any())
            {
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
            }

            foreach (var setting in Settings)
            {
                var toolStripMenuItem = new ToolStripMenuItem(setting.Key);
                var toolStripTextBox = new ToolStripTextBox(setting.Key);

                toolStripMenuItem.DropDownItems.Add(toolStripTextBox);

                toolStripTextBox.TextChanged += new EventHandler(Setting_TextChanged);
                toolStripTextBox.Tag = setting.Value;
                toolStripTextBox.Text = setting.Value.Get().ToString();

                ContextMenuStrip.Items.Add(toolStripMenuItem);
            }
        }

        private void Send(string Value)
        {
            SerialPort.WriteLine($"{ControlPlugin.RequiredMode.ToString()} {Pin} {Value}");
        }

        private void UpdateValue(string Value)
        {
            this.Value.Set(Value);
        }

        private void Setting_TextChanged(object Sender, EventArgs EventArgs)
        {
            var toolStripTextBox = (ToolStripTextBox)Sender;
            var setting = (Mapping)toolStripTextBox.Tag;

            try
            {
                setting.Set(toolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void ContextMenuStrip_Opening(object Sender, CancelEventArgs CancelEventArgs)
        {
            CancelEventArgs.Cancel = !ContextMenuStrip.Enabled;
            SetPinComboBox.Items.Clear();

            foreach (var pin in Board.Pins.Where(p => p.Modes.Contains(ControlPlugin.RequiredMode) && !p.Assigned))
            {
                SetPinComboBox.Items.Add(pin.Name);
            }

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                SetPinComboBox.Text = Pin;
            }
        }

        private void ControlLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            Text.Set(ControlLabelTextBox.Text);
        }

        private void SetPinComboBox_SelectedIndexChanged(object Sender, EventArgs EventArgs)
        {
            var pin = SetPinComboBox.Text;

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                Board.Pins.First(p => p.Name == Pin).Assigned = false;
            }

            Board.Pins.First(p => p.Name == pin).Assigned = true;
            Pin = SetPinComboBox.Text;
        }

        private void RemoveControlMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            Parent.Controls.Remove(this);
        }

        private void Control_MouseDown(object Sender, MouseEventArgs MouseEventArgs)
        {
            OrigionalMousePosition = Parent.PointToClient(Cursor.Position);
            OrigionalControlLocation = Location;
            MouseIsDown = true;
        }

        private void Control_MouseUp(object Sender, MouseEventArgs MouseEventArgs)
        {
            MouseIsDown = false;
        }

        private void Control_MouseMove(object Sender, MouseEventArgs MouseEventArgs)
        {
            Point newMousePosition = Parent.PointToClient(Cursor.Position),
                mouseDifference = new Point(OrigionalMousePosition.X - newMousePosition.X,
                    OrigionalMousePosition.Y - newMousePosition.Y);

            Cursor.Current = Cursors.SizeAll;

            if (MouseIsDown)
            {
                Location = Grid.NearestNode(
                    new Point(OrigionalControlLocation.X - mouseDifference.X,
                        OrigionalControlLocation.Y - mouseDifference.Y), Size);
            }
        }
    }
}
