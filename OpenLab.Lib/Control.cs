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
    public class Control : FlowLayoutPanel
    {
        public override string Text
        {
            get
            {
                return ControlPlugin.GetLabel(this);
            }
            set
            {
                ControlPlugin.SetLabel(this, value);
            }
        }
        public bool Log
        {
            get
            {
                return LogValueMenuItem.Checked;
            }
        }
        public Dictionary<string, string> Settings { get; private set; }
        public SerialPort SerialPort { get; private set; }
        public IControlPlugin ControlPlugin { get; private set; }
        public delegate void SetValueDelagate_T(string Value);
        public SetValueDelagate_T SetValueDelagate;

        private bool MouseIsDown = false;
        private Point OrigionalControlLocation, OrigionalMousePosition;

        private ToolStripMenuItem ControlLabelMenuItem = new ToolStripMenuItem("Control Label");
        private ToolStripTextBox ControlLabelTextBox = new ToolStripTextBox("Control Label");
        private ToolStripMenuItem LogValueMenuItem = new ToolStripMenuItem("Log Value");
        private ToolStripMenuItem RemoveControlMenuItem = new ToolStripMenuItem("Remove Control");

        public static Control FromLocation(IControlPlugin Plugin, Point Location, bool LoggingEnabled)
        {
            var control = new Control(Plugin, Location);

            control.LogValueMenuItem.Enabled = LoggingEnabled;
            control.LogValueMenuItem.Checked = false;
            Plugin.Create(control);
            Plugin.LoadSettings(control);
            control.Text = Plugin.GetType().Assembly.GetName().Name;

            return control;
        }

        public static Control FromConfig(IControlPlugin Plugin, XElement Config, bool LoggingEnabled)
        {
            var x = Convert.ToInt32(Config.Element("x").Value);
            var y = Convert.ToInt32(Config.Element("y").Value);
            var control = new Control(Plugin, new Point(x, y));

            control.LogValueMenuItem.Enabled = LoggingEnabled;
            control.LogValueMenuItem.Checked = bool.Parse(Config.Element("log").Value);
            Plugin.Create(control);

            foreach (var setting in Config.Element("settings").Elements())
            {
                control.Settings[setting.Name.LocalName] = setting.Value;
            }

            Plugin.LoadSettings(control);
            control.Text = Config.Element("label").Value;

            return control;
        }

        public static Control Copy(Control Control)
        {
            var newControl = new Control(Control.ControlPlugin, Control.Location);

            newControl.LogValueMenuItem.Checked = Control.Log;
            Control.ControlPlugin.Create(newControl);

            foreach (var setting in Control.Settings)
            {
                newControl.Settings[setting.Key] = setting.Value;
            }

            newControl.Text = Control.Text;

            return newControl;
        }

        public XElement GetConfig()
        {
            var settingsConfig = new List<XElement>();

            foreach (var setting in Settings)
            {
                settingsConfig.Add(new XElement(setting.Key, setting.Value));
            }

            return
                new XElement("control",
                    new XElement("type", ControlPlugin.GetType().Assembly.GetName().Name),
                    new XElement("version", ControlPlugin.GetType().Assembly.GetName().Version),
                    new XElement("label", Text),
                    new XElement("x", Location.X),
                    new XElement("y", Location.Y),
                    new XElement("log", Log),
                    new XElement("settings", settingsConfig)
                );
        }

        public void EnableEdit()
        {
            BorderStyle = BorderStyle.FixedSingle;
            ContextMenuStrip.Enabled = true;
            Enabled = true;

            MouseDown += new MouseEventHandler(Control_MouseDown);
            MouseMove += new MouseEventHandler(Control_MouseMove);
            MouseUp += new MouseEventHandler(Control_MouseUp);

            foreach (System.Windows.Forms.Control control in Controls)
            {
                control.Enabled = false;
            }
        }

        public void DisableEdit()
        {
            BorderStyle = BorderStyle.None;
            ContextMenuStrip.Enabled = false;
            Enabled = false;

            MouseDown -= new MouseEventHandler(Control_MouseDown);
            MouseMove -= new MouseEventHandler(Control_MouseMove);
            MouseUp -= new MouseEventHandler(Control_MouseUp);

            foreach (System.Windows.Forms.Control control in Controls)
            {
                control.Enabled = true;
            }
        }

        public void SetSerialPort(SerialPort SerialPort)
        {
            this.SerialPort = SerialPort;
        }

        public string GetValue()
        {
            return ControlPlugin.GetValue(this);
        }

        public T GetContextMenuItem<T>(string Name) where T : ToolStripItem
        {
            return (T)ContextMenuStrip.Items.Find(Name, true).First();
        }

        private void SetValue(string Value)
        {
            ControlPlugin.SetValue(this, Value);
        }

        private Control(IControlPlugin Plugin, Point Location)
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.ControlPlugin = Plugin;
            this.Location = Location;
            Settings = new Dictionary<string, string>();
            SetValueDelagate = new SetValueDelagate_T(SetValue);

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ControlLabelMenuItem.DropDownItems.Add(ControlLabelTextBox);
            ControlLabelTextBox.TextChanged += new EventHandler(ControlLabelTextBox_TextChanged);
            LogValueMenuItem.CheckOnClick = true;
            RemoveControlMenuItem.Click += new EventHandler(RemoveControlMenuItem_Click);

            ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                ControlLabelMenuItem,
                LogValueMenuItem,
                RemoveControlMenuItem,
                new ToolStripSeparator()
            });

            ContextMenuStrip.Items.AddRange(Plugin.GetContextMenuItems(this));
        }

        private void ContextMenuStrip_Opening(object Sender, CancelEventArgs CancelEventArgs)
        {
            if (!ContextMenuStrip.Enabled)
            {
                CancelEventArgs.Cancel = true;
                return;
            }

            ControlLabelTextBox.Text = Text;
            ControlPlugin.ContextMenuOpening(this);
        }

        private void ControlLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            Text = ControlLabelTextBox.Text;
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
                mouseDifference = new Point(OrigionalMousePosition.X - newMousePosition.X, OrigionalMousePosition.Y - newMousePosition.Y);

            if (MouseIsDown)
            {
                Location = new Point(OrigionalControlLocation.X - mouseDifference.X, OrigionalControlLocation.Y - mouseDifference.Y);
            }
        }
    }
}
