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
    public class Toggle : IControl
    {
        public Toggle()
        {
            panels = new List<Panel>();
        }

        public string name
        {
            get
            {
                return "Toggle";
            }
        }

        public void init(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public FlowLayoutPanel add(GroupBox group_box, Point location)
        {
            Tags tags = new Tags();

            tags["text"] = "New Toggle";
            tags["on_command"] = "";
            tags["off_command"] = "";

            return add(group_box, location, tags);
        }

        public FlowLayoutPanel add(GroupBox group_box, XmlNode xml_node)
        {
            int x, y;
            Tags tags = new Tags();

            tags["text"] = xml_node["text"].InnerText;
            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);
            tags["on_command"] = xml_node["on_command"].InnerText;
            tags["off_command"] = xml_node["off_command"].InnerText;

            return add(group_box, new Point(x, y), tags);
        }

        public List<ToolStripMenuItem> settings()
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();

            ToolStripTextBox on_command_text_box = new ToolStripTextBox();
            on_command_text_box.Name = "toggleOnCommandToolStripTextBox";
            on_command_text_box.TextChanged += new EventHandler(toggleOnCommandToolStripTextBox_TextChanged);

            ToolStripTextBox off_command_text_box = new ToolStripTextBox();
            off_command_text_box.Name = "toggleOffCommandToolStripTextBox";
            off_command_text_box.TextChanged += new EventHandler(toggleOffCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem on_command_menu_item = new ToolStripMenuItem();
            on_command_menu_item.Name = "toggleOnCommandToolStripMenuItem";
            on_command_menu_item.Text = "On Command";
            on_command_menu_item.MouseHover += new EventHandler(toggleOnCommandToolStripMenuItem_MouseHover);
            on_command_menu_item.DropDownItems.Add(on_command_text_box);

            ToolStripMenuItem off_command_menu_item = new ToolStripMenuItem();
            off_command_menu_item.Name = "toggleOffCommandToolStripMenuItem";
            off_command_menu_item.Text = "Off Command";
            off_command_menu_item.MouseHover += new EventHandler(toggleOffCommandToolStripMenuItem_MouseHover);
            off_command_menu_item.DropDownItems.Add(off_command_text_box);

            menu_items.Add(on_command_menu_item);
            menu_items.Add(off_command_menu_item);

            return menu_items;
        }

        public XmlDocument save(GroupBox group_box)
        {
            XmlDocument control_config = new XmlDocument();

            XmlNode config_node = control_config.CreateElement("config");
            control_config.AppendChild(config_node);
            foreach (FlowLayoutPanel panel in group_box.Controls)
            {
                if (panels.Contains(panel))
                {
                    Tags tags = panel.Tag as Tags;

                    XmlNode control_node = control_config.CreateElement("control");

                    XmlNode name_node = control_config.CreateElement("name");
                    name_node.InnerText = name;
                    control_node.AppendChild(name_node);

                    XmlNode text_node = control_config.CreateElement("text");
                    text_node.InnerText = tags["text"];
                    control_node.AppendChild(text_node);

                    XmlNode x_node = control_config.CreateElement("x");
                    x_node.InnerText = Convert.ToString(panel.Location.X);
                    control_node.AppendChild(x_node);

                    XmlNode y_node = control_config.CreateElement("y");
                    y_node.InnerText = Convert.ToString(panel.Location.Y);
                    control_node.AppendChild(y_node);

                    XmlNode on_node = control_config.CreateElement("on_command");
                    on_node.InnerText = tags["on_command"];
                    control_node.AppendChild(on_node);

                    XmlNode off_node = control_config.CreateElement("off_command");
                    off_node.InnerText = tags["off_command"];
                    control_node.AppendChild(off_node);

                    config_node.AppendChild(control_node);
                }
            }

            return control_config;
        }

        public void update(SafeSerialPort serial_port)
        {
            // Nothing to do here!
        }

        public void reset()
        {
            panels.Clear();
        }

        private ControlForm control_form;
        private List<Panel> panels;

        private FlowLayoutPanel add(GroupBox group_box, Point location, Tags tags)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Label label = new Label();
            Padding padding = new Padding();
            Button on_button = new Button();
            Button off_button = new Button();

            tags["name"] = name;
            panel.Location = location;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Tag = tags;

            padding.Top = 8;
            label.Margin = padding;
            label.AutoSize = true;
            panel.Controls.Add(label);
            label.Text = tags["text"] + ": ";

            on_button.Text = "On";
            on_button.Size = new Size(50, 23);
            on_button.Click += new System.EventHandler(onButton_Click);
            panel.Controls.Add(on_button);

            off_button.Text = "Off";
            off_button.Size = new Size(50, 23);
            off_button.Click += new System.EventHandler(offButton_Click);
            panel.Controls.Add(off_button);

            group_box.Controls.Add(panel);
            panels.Add(panel);

            return panel;
        }

        private void toggleOnCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            tags["on_command"] = text_box.Text;
        }

        private void toggleOffCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            tags["off_command"] = text_box.Text;
        }

        private void toggleOnCommandToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            menu_item.DropDownItems[0].Text = tags["on_command"];
        }

        private void toggleOffCommandToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            menu_item.DropDownItems[0].Text = tags["off_command"];
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
