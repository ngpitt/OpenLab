using System;
using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenLab;
using Tags = System.Collections.Generic.Dictionary<string, string>;

namespace Toggle
{
    public class Toggle : ControlPlugin
    {
        public Toggle(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public string name
        {
            get
            {
                return "Toggle";
            }
        }

        public FlowLayoutPanel create(Point location)
        {
            Tags tags = new Tags();

            tags["label"] = name;
            tags["on_command"] = "";
            tags["off_command"] = "";

            return create(location, tags);
        }

        public FlowLayoutPanel create(XmlNode xml_node)
        {
            int x, y;
            Tags tags = new Tags();

            tags["label"] = xml_node["label"].InnerText;
            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);
            tags["on_command"] = xml_node["on_command"].InnerText;
            tags["off_command"] = xml_node["off_command"].InnerText;

            return create(new Point(x, y), tags);
        }

        public FlowLayoutPanel copy(FlowLayoutPanel source_control)
        {
            return create(source_control.Location, new Tags(source_control.Tag as Tags));
        }

        public void update(FlowLayoutPanel control, SafeSerialPort serial_port)
        {
            // Nothing to do here!
        }

        public List<ToolStripMenuItem> settings(FlowLayoutPanel control)
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();
            Tags tags = control.Tag as Tags;

            ToolStripTextBox on_command_text_box = new ToolStripTextBox();
            on_command_text_box.Text = tags["on_command"];
            on_command_text_box.TextChanged += new EventHandler(onCommandToolStripTextBox_TextChanged);

            ToolStripTextBox off_command_text_box = new ToolStripTextBox();
            off_command_text_box.Text = tags["off_command"];
            off_command_text_box.TextChanged += new EventHandler(offCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem on_command_menu_item = new ToolStripMenuItem();
            on_command_menu_item.Text = "On Command";
            on_command_menu_item.DropDownItems.Add(on_command_text_box);

            ToolStripMenuItem off_command_menu_item = new ToolStripMenuItem();
            off_command_menu_item.Text = "Off Command";
            off_command_menu_item.DropDownItems.Add(off_command_text_box);

            menu_items.Add(on_command_menu_item);
            menu_items.Add(off_command_menu_item);

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

            XmlNode on_command_node = control_config.CreateElement("on_command");
            on_command_node.InnerText = tags["on_command"];
            control_node.AppendChild(on_command_node);

            XmlNode off_command_node = control_config.CreateElement("off_command");
            off_command_node.InnerText = tags["off_command"];
            control_node.AppendChild(off_command_node);

            control_config.AppendChild(control_node);

            return control_config;
        }

        private ControlForm control_form;

        private FlowLayoutPanel create(Point location, Tags tags)
        {
            FlowLayoutPanel control = new FlowLayoutPanel();
            Label label = new Label();
            Padding padding = new Padding();
            Button on_button = new Button();
            Button off_button = new Button();

            tags["plugin"] = name;
            control.Location = location;
            control.AutoSize = true;
            control.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control.Tag = tags;

            padding.Top = 8;
            label.Margin = padding;
            label.AutoSize = true;
            control.Controls.Add(label);
            label.Text = tags["label"] + ": ";

            on_button.Text = "On";
            on_button.Size = new Size(50, 23);
            on_button.Click += new System.EventHandler(onButton_Click);
            control.Controls.Add(on_button);

            off_button.Text = "Off";
            off_button.Size = new Size(50, 23);
            off_button.Click += new System.EventHandler(offButton_Click);
            control.Controls.Add(off_button);

            return control;
        }

        private void onCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            Tags tags = control.Tag as Tags;
            tags["on_command"] = text_box.Text;
        }

        private void offCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            Tags tags = control.Tag as Tags;
            tags["off_command"] = text_box.Text;
        }

        private void onButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Tags tags = button.Parent.Tag as Tags;
            control_form.serialWrite(tags["on_command"]);
        }

        private void offButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Tags tags = button.Parent.Tag as Tags;
            control_form.serialWrite(tags["off_command"]);
        }
    }
}
