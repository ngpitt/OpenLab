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
    public class Meter : IControl
    {
        public Meter()
        {
            panels = new List<Panel>();
            update_delagate = new UpdateDelagate(updateForm);
        }

        public string name
        {
            get
            {
                return "Meter";
            }
        }

        public void init(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public FlowLayoutPanel add(GroupBox group_box, Point location)
        {
            Tags tags = new Tags();

            tags["text"] = "New Meter";
            tags["get_command"] = "";

            return add(group_box, location, tags);
        }

        public FlowLayoutPanel add(GroupBox group_box, XmlNode xml_node)
        {
            int x, y;
            Tags tags = new Tags();

            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);
            tags["text"] = xml_node["text"].InnerText;
            tags["get_command"] = xml_node["get_command"].InnerText;

            return add(group_box, new Point(x, y), tags);
        }

        public List<ToolStripMenuItem> settings()
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();

            ToolStripTextBox get_command_text_box = new ToolStripTextBox();
            get_command_text_box.Name = "meterGetCommandToolStripTextBox";
            get_command_text_box.TextChanged += new EventHandler(meterGetCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem get_command_menu_item = new ToolStripMenuItem();
            get_command_menu_item.Name = "meterGetCommandToolStripMenuItem";
            get_command_menu_item.Text = "Get Command";
            get_command_menu_item.MouseHover += new EventHandler(meterGetCommandToolStripMenuItem_MouseHover);
            get_command_menu_item.DropDownItems.Add(get_command_text_box);

            menu_items.Add(get_command_menu_item);

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

                    XmlNode get_node = control_config.CreateElement("get_command");
                    get_node.InnerText = tags["get_command"];
                    control_node.AppendChild(get_node);

                    config_node.AppendChild(control_node);
                }
            }

            return control_config;
        }

        public void update(SafeSerialPort serial_port)
        {
            foreach (FlowLayoutPanel panel in panels)
            {
                Tags tags = panel.Tag as Tags;
                serial_port.WriteLine(tags["get_command"] as string);
                tags["value"] = serial_port.ReadLine();
            }
            control_form.BeginInvoke(update_delagate);
        }

        public void reset()
        {
            panels.Clear();
        }

        private ControlForm control_form;
        private List<Panel> panels;
        private delegate void UpdateDelagate();
        private UpdateDelagate update_delagate;

        private FlowLayoutPanel add(GroupBox group_box, Point Location, Tags tags)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Label label = new Label();

            tags["name"] = name;
            panel.Location = Location;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Tag = tags;

            label.AutoSize = true;
            label.Text = tags["text"] + ": ";
            panel.Controls.Add(label);

            group_box.Controls.Add(panel);
            panels.Add(panel);

            return panel;
        }

        private void meterGetCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            tags["get_command"] = text_box.Text;
        }

        private void meterGetCommandToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            menu_item.DropDownItems[0].Text = tags["get_command"];
        }

        private void updateForm()
        {
            foreach (FlowLayoutPanel panel in panels)
            {
                Tags tags = panel.Tag as Tags;
                panel.Controls[0].Text = tags["text"] + ": " + tags["value"];
            }
        }
    }
}
