using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenLab
{
    public class Group : GroupBox
    {
        private IEnumerable<IControlPlugin> ControlPlugins;
        private bool LoggingEnabled, MouseIsDown = false, Resizing = false;
        private Point ContextMenuLocation, OrigionalControlLocation, OrigionalMousePosition;
        private Size OrigionalControlSize;

        private ToolStripMenuItem GroupLabelMenuItem = new ToolStripMenuItem("Group Label");
        private ToolStripTextBox GroupLabelTextBox = new ToolStripTextBox("Group Label");
        private ToolStripMenuItem AddControlMenuItem = new ToolStripMenuItem("Add Control");
        private ToolStripMenuItem RemoveGroupMenuItem = new ToolStripMenuItem("Remove Group"); 

        public static Group Create(Point Location, IEnumerable<IControlPlugin> ControlPlugins)
        {
            return new Group("Group", Location, new Size(300, 100), ControlPlugins);
        }

        public static Group OpenConfig(XElement Config, IEnumerable<IControlPlugin> ControlPlugins, bool LoggingEnabled)
        {
            var x = Convert.ToInt32(Config.Element("x").Value);
            var y = Convert.ToInt32(Config.Element("y").Value);
            var width = Convert.ToInt32(Config.Element("width").Value);
            var height = Convert.ToInt32(Config.Element("height").Value);
            var group = new Group(Config.Element("label").Value, new Point(x, y), new Size(width, height), ControlPlugins);

            group.LoggingEnabled = LoggingEnabled;

            foreach (var controlConfig in Config.Descendants("control"))
            {
                var controlType = controlConfig.Element("type").Value;
                var controlPlugin = ControlPlugins.First(plugin => plugin.GetType().Assembly.GetName().Name == controlType);
                var control = Control.OpenConfig(controlPlugin, controlConfig, LoggingEnabled);

                group.Controls.Add(control);
            }

            return group;
        }

        public Group Copy(Point Location)
        {
            return new Group(Text, Location, Size, ControlPlugins);
        }

        public XElement Config()
        {
            var config =
                new XElement("group",
                    new XElement("label", Text),
                    new XElement("x", Location.X),
                    new XElement("y", Location.Y),
                    new XElement("width", Width),
                    new XElement("height", Height)
                );

            foreach (var control in Controls.OfType<Control>())
            {
                config.Add(control.Config());
            }

            return config;
        }

        public void EnableEdit()
        {
            ContextMenuStrip.Enabled = true;
            Enabled = true;

            MouseUp += new MouseEventHandler(MouseUpEvent);
            MouseDown += new MouseEventHandler(MouseDownEvent);
            MouseMove += new MouseEventHandler(MouseMoveEvent);

            foreach (var control in Controls.OfType<Control>())
            {
                control.EnableEdit();
            }
        }

        public void DisableEdit()
        {
            ContextMenuStrip.Enabled = false;
            Enabled = false;

            MouseUp -= new MouseEventHandler(MouseUpEvent);
            MouseDown -= new MouseEventHandler(MouseDownEvent);
            MouseMove -= new MouseEventHandler(MouseMoveEvent);

            foreach (var control in Controls.OfType<Control>())
            {
                control.DisableEdit();
            }
        }

        private Group(string Label, Point Location, Size Size, IEnumerable<IControlPlugin> ControlPlugins)
        {
            Text = Label;
            this.Location = Location;
            this.Size = Size;
            this.ControlPlugins = ControlPlugins;

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            GroupLabelMenuItem.DropDownItems.Add(GroupLabelTextBox);
            GroupLabelTextBox.TextChanged += new EventHandler(GroupLabelTextBox_TextChanged);
            
            foreach (var controlPlugin in ControlPlugins)
            {
                var addControlMenuItem = new ToolStripMenuItem(controlPlugin.GetType().Assembly.GetName().Name);

                addControlMenuItem.Tag = controlPlugin;
                addControlMenuItem.Click += new EventHandler(AddControlMenuItem_Click);
                AddControlMenuItem.DropDownItems.Add(addControlMenuItem);
            }

            RemoveGroupMenuItem.Click += new EventHandler(RemoveGroupMenuItem_Click);

            ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                GroupLabelMenuItem,
                AddControlMenuItem,
                RemoveGroupMenuItem
            });
        }

        private void ContextMenuStrip_Opening(object Sender, CancelEventArgs CancelEventArgs)
        {
            if (!ContextMenuStrip.Enabled)
            {
                CancelEventArgs.Cancel = true;
                return;
            }

            ContextMenuLocation = PointToClient(Cursor.Position);
            GroupLabelTextBox.Text = Text;
        }

        private void GroupLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            Text = GroupLabelTextBox.Text;
        }

        private void AddControlMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            var controlPlugin = (IControlPlugin)((ToolStripMenuItem)Sender).Tag;
            var control = Control.Create(controlPlugin, ContextMenuLocation, LoggingEnabled);

            Controls.Add(control);
            control.BringToFront();
            control.EnableEdit();
        }

        private void RemoveGroupMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            Parent.Controls.Remove(this);
        }

        private void MouseDownEvent(object Sender, MouseEventArgs MouseEventArgs)
        {
            OrigionalMousePosition = Parent.PointToClient(Cursor.Position);
            OrigionalControlLocation = Location;
            OrigionalControlSize = Size;
            MouseIsDown = true;

            if (MouseInResizeRegion())
            {
                Resizing = true;
            }
        }

        private void MouseUpEvent(object Sender, MouseEventArgs MouseEventArgs)
        {
            MouseIsDown = false;
            Resizing = false;
        }

        private void MouseMoveEvent(object Sender, MouseEventArgs MouseEventArgs)
        {
            var newMousePosition = Parent.PointToClient(Cursor.Position);
            var mouseDifference = new Point(OrigionalMousePosition.X - newMousePosition.X, OrigionalMousePosition.Y - newMousePosition.Y);

            Cursor.Current = MouseInResizeRegion() ? Cursors.SizeNWSE : Cursors.Default;

            if (Resizing)
            {
                Size = new Size(OrigionalControlSize.Width - mouseDifference.X, OrigionalControlSize.Height - mouseDifference.Y);
            }
            else if (MouseIsDown)
            {
                Location = new Point(OrigionalControlLocation.X - mouseDifference.X, OrigionalControlLocation.Y - mouseDifference.Y);
            }
        }

        private bool MouseInResizeRegion()
        {
            var mousePosition = PointToClient(Cursor.Position);

            return mousePosition.X > Width - 10 && mousePosition.Y > Height - 10;
        }
    }
}
