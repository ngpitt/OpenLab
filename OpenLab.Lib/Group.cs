using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenLab.Lib
{
    public class Group : GroupBox
    {
        public override string Text
        {
            set
            {
                base.Text = value;
                GroupLabelTextBox.Text = value;
            }
        }
        public bool Editing
        {
            set
            {
                ContextMenuStrip.Enabled = value;
                Enabled = value;

                foreach (var control in Controls.OfType<Control>())
                {
                    control.Editing = value;
                }

                if (value)
                {
                    MouseUp += new MouseEventHandler(Group_MouseUp);
                    MouseDown += new MouseEventHandler(Group_MouseDown);
                    MouseMove += new MouseEventHandler(Group_MouseMove);
                }
                else
                {
                    MouseUp -= new MouseEventHandler(Group_MouseUp);
                    MouseDown -= new MouseEventHandler(Group_MouseDown);
                    MouseMove -= new MouseEventHandler(Group_MouseMove);
                }
            }
        }
        public ICollection<IControlPlugin> ControlPlugins;
        public Board Board;

        public static Group FromLocation(ICollection<IControlPlugin> ControlPlugins, Board Board, bool LoggingEnabled,
            Point Location)
        {
            return new Group("Group", Location, new Size(300, 100), ControlPlugins, Board, LoggingEnabled);
        }

        public static Group FromConfig(ICollection<IControlPlugin> ControlPlugins, Board Board, bool LoggingEnabled,
            XElement Config, XNamespace Ns)
        {
            var group = new Group(Config.Element(Ns + "label").Value,
                new Point(Convert.ToInt32(Config.Element(Ns + "x").Value), Convert.ToInt32(Config.Element(Ns + "y").Value)),
                new Size(Convert.ToInt32(Config.Element(Ns + "width").Value), Convert.ToInt32(Config.Element(Ns + "height").Value)),
                ControlPlugins, Board, LoggingEnabled);

            foreach (var controlConfig in Config.Descendants(Ns + "control"))
            {
                var controlType = controlConfig.Element(Ns + "type").Value;
                var controlPlugin = ControlPlugins.First(plugin =>
                    plugin.GetType().Assembly.GetName().Name == controlType);
                var control = Control.FromConfig(controlPlugin, Board, LoggingEnabled, controlConfig, Ns);

                group.Controls.Add(control);
            }

            return group;
        }

        public static Group Copy(Group Group)
        {
            var newGroup = new Group(Group.Text, Group.Location, Group.Size, Group.ControlPlugins, Group.Board,
                Group.LoggingEnabled);

            newGroup.Controls.AddRange(Group.Controls.OfType<Control>().Select(c => Control.Copy(c)).ToArray());

            return newGroup;
        }

        public XElement ToConfig(XNamespace Ns)
        {
            var config = new XElement(Ns + "group",
                new XElement(Ns + "label", Text),
                new XElement(Ns + "x", Location.X),
                new XElement(Ns + "y", Location.Y),
                new XElement(Ns + "width", Width),
                new XElement(Ns + "height", Height));

            foreach (var control in Controls.OfType<Control>())
            {
                config.Add(control.ToConfig(Ns));
            }

            return config;
        }

        private bool LoggingEnabled, MouseIsDown = false, Resizing = false;
        private Point ContextMenuLocation, OrigionalControlLocation, OrigionalMousePosition;
        private Size OrigionalControlSize;

        private ToolStripMenuItem GroupLabelMenuItem = new ToolStripMenuItem("Group Label");
        private ToolStripTextBox GroupLabelTextBox = new ToolStripTextBox("Group Label");
        private ToolStripMenuItem AddControlMenuItem = new ToolStripMenuItem("Add Control");
        private ToolStripMenuItem RemoveGroupMenuItem = new ToolStripMenuItem("Remove Group");

        private Group(string Label, Point Location, Size Size, ICollection<IControlPlugin> ControlPlugins, Board Board,
            bool LoggingEnabled)
        {
            Text = Label;
            this.Location = Location;
            this.Size = Size;
            this.ControlPlugins = ControlPlugins;
            this.Board = Board;
            this.LoggingEnabled = LoggingEnabled;

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            GroupLabelMenuItem.DropDownItems.Add(GroupLabelTextBox);
            GroupLabelTextBox.TextChanged += new EventHandler(GroupLabelTextBox_TextChanged);

            foreach (var controlPlugin in ControlPlugins)
            {
                var addControlMenuItem =
                    new ToolStripMenuItem(controlPlugin.GetType().Assembly.GetName().Name);

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
            CancelEventArgs.Cancel = !ContextMenuStrip.Enabled;
            ContextMenuLocation = PointToClient(Cursor.Position);
        }

        private void GroupLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            base.Text = GroupLabelTextBox.Text;
        }

        private void AddControlMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            var controlPlugin = (IControlPlugin)((ToolStripMenuItem)Sender).Tag;
            var control = Control.FromLocation(controlPlugin, Board, LoggingEnabled, ContextMenuLocation);

            Controls.Add(control);
            control.BringToFront();
            control.Editing = true;
        }

        private void RemoveGroupMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            Parent.Controls.Remove(this);
        }

        private void Group_MouseDown(object Sender, MouseEventArgs MouseEventArgs)
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

        private void Group_MouseUp(object Sender, MouseEventArgs MouseEventArgs)
        {
            MouseIsDown = false;
            Resizing = false;
        }

        private void Group_MouseMove(object Sender, MouseEventArgs MouseEventArgs)
        {
            var newMousePosition = Parent.PointToClient(Cursor.Position);
            var mouseDifference = new Point(OrigionalMousePosition.X - newMousePosition.X,
                OrigionalMousePosition.Y - newMousePosition.Y);

            Cursor.Current = MouseInResizeRegion() ? Cursors.SizeNWSE : Cursors.SizeAll;

            if (Resizing)
            {
                Size = Grid.NearestNode(new Size(OrigionalControlSize.Width - mouseDifference.X,
                    OrigionalControlSize.Height - mouseDifference.Y));
            }
            else if (MouseIsDown)
            {
                Location = Grid.NearestNode(new Point(OrigionalControlLocation.X - mouseDifference.X,
                    OrigionalControlLocation.Y - mouseDifference.Y));
            }
        }

        private bool MouseInResizeRegion()
        {
            var mousePosition = PointToClient(Cursor.Position);

            return mousePosition.X > Width - 10 && mousePosition.Y > Height - 10;
        }
    }
}
