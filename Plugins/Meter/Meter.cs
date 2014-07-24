using System;
using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenLab;
using Tags = System.Collections.Generic.Dictionary<string, string>;

namespace Meter
{
    public class Meter : ControlPlugin
    {
        public Meter(ControlForm control_form)
        {
            this.control_form = control_form;
            update_delagate = new UpdateDelagate(updateForm);
        }

        public string name
        {
            get
            {
                return "Meter";
            }
        }

        public FlowLayoutPanel create(Point location)
        {
            Tags tags = new Tags();

            tags["label"] = name;
            tags["get_command"] = "";

            return create(location, tags);
        }

        public FlowLayoutPanel create(XmlNode config_node)
        {
            int x, y;
            Tags tags = new Tags();

            x = Convert.ToInt32(config_node["x"].InnerText);
            y = Convert.ToInt32(config_node["y"].InnerText);
            tags["label"] = config_node["label"].InnerText;
            tags["get_command"] = config_node["get_command"].InnerText;

            return create(new Point(x, y), tags);
        }

        public void update(FlowLayoutPanel control, SafeSerialPort serial_port)
        {
            Tags tags = control.Tag as Tags;
            serial_port.WriteLine(tags["get_command"] as string);
            tags["value"] = serial_port.ReadLine();
            control_form.BeginInvoke(update_delagate, control);
        }

        public FlowLayoutPanel copy(FlowLayoutPanel source_control)
        {
            FlowLayoutPanel control = new FlowLayoutPanel();
            Label label = new Label();
            Tags tags = source_control.Tag as Tags;

            control.Location = source_control.Location;
            control.AutoSize = true;
            control.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control.Tag = new Tags(tags);

            label.AutoSize = true;
            label.Text = tags["label"] + ": ";
            control.Controls.Add(label);

            return control;
        }

        public List<ToolStripMenuItem> settings(FlowLayoutPanel control)
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();
            Tags tags = control.Tag as Tags;

            ToolStripTextBox get_command_text_box = new ToolStripTextBox();
            get_command_text_box.Text = tags["get_command"];
            get_command_text_box.TextChanged += new EventHandler(getCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem get_command_menu_item = new ToolStripMenuItem();
            get_command_menu_item.Text = "Get Command";
            get_command_menu_item.DropDownItems.Add(get_command_text_box);

            menu_items.Add(get_command_menu_item);

            return menu_items;
        }

        public XmlDocument save(FlowLayoutPanel control)
        {
            XmlDocument control_config = new XmlDocument();
            Tags tags = control.Tag as Tags;

            XmlNode control_node = control_config.CreateElement("control");

            XmlNode plugin_node = control_config.CreateElement("plugin");
            plugin_node.InnerText = name;
            control_node.AppendChild(plugin_node);

            XmlNode label_node = control_config.CreateElement("label");
            label_node.InnerText = tags["label"];
            control_node.AppendChild(label_node);

            XmlNode x_node = control_config.CreateElement("x");
            x_node.InnerText = Convert.ToString(control.Location.X);
            control_node.AppendChild(x_node);

            XmlNode y_node = control_config.CreateElement("y");
            y_node.InnerText = Convert.ToString(control.Location.Y);
            control_node.AppendChild(y_node);

            XmlNode get_command_node = control_config.CreateElement("get_command");
            get_command_node.InnerText = tags["get_command"];
            control_node.AppendChild(get_command_node);

            control_config.AppendChild(control_node);

            return control_config;
        }

        private ControlForm control_form;
        private delegate void UpdateDelagate(FlowLayoutPanel control);
        private UpdateDelagate update_delagate;

        private FlowLayoutPanel create(Point Location, Tags tags)
        {
            FlowLayoutPanel control = new FlowLayoutPanel();
            Label label = new Label();

            tags["plugin"] = name;
            control.Location = Location;
            control.AutoSize = true;
            control.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control.Tag = tags;

            label.AutoSize = true;
            label.Text = tags["label"] + ": ";
            control.Controls.Add(label);

            return control;
        }

        private void getCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            Tags tags = control.Tag as Tags;
            tags["get_command"] = text_box.Text;
        }

        private void updateForm(FlowLayoutPanel control)
        {
            Tags tags = control.Tag as Tags;
            control.Controls[0].Text = tags["label"] + ": " + tags["value"];
        }
    }
}
