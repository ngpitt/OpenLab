using System;
using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenLab;
using Tags = System.Collections.Generic.Dictionary<string, string>;

namespace Spinner
{
    public class Spinner : IControl
    {
        public Spinner()
        {
            panels = new List<Panel>();
        }

        public string name
        {
            get
            {
                return "Spinner";
            }
        }

        public void init(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public FlowLayoutPanel add(GroupBox group_box, Point location)
        {
            Tags tags = new Tags();
            NumericUpDown spinner = new NumericUpDown();

            tags["text"] = "New Spinner";
            spinner.Minimum = spinner.Maximum = spinner.Increment = 0;
            spinner.DecimalPlaces = 0;
            tags["set_command"] = "";

            return add(group_box, location, spinner, tags);
        }

        public FlowLayoutPanel add(GroupBox group_box, XmlNode xml_node)
        {
            int x, y;
            NumericUpDown spinner = new NumericUpDown();
            Tags tags = new Tags();

            tags["text"] = xml_node["text"].InnerText;
            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);
            spinner.Minimum = Convert.ToDecimal(xml_node["minimum"].InnerText);
            spinner.Maximum = Convert.ToDecimal(xml_node["maximum"].InnerText);
            spinner.DecimalPlaces = Convert.ToInt32(xml_node["decimals"].InnerText);
            spinner.Increment = Convert.ToDecimal(xml_node["increment"].InnerText);
            tags["set_command"] = xml_node["set_command"].InnerText;

            return add(group_box, new Point(x, y), spinner, tags);
        }

        public List<ToolStripMenuItem> settings()
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();

            ToolStripTextBox minimum_text_box = new ToolStripTextBox();
            minimum_text_box.Name = "spinnerMinimumToolStripTextBox";
            minimum_text_box.TextChanged += new EventHandler(spinnerMinimumToolStripTextBox_TextChanged);

            ToolStripTextBox maximum_text_box = new ToolStripTextBox();
            maximum_text_box.Name = "spinnerMaximumToolStripTextBox";
            maximum_text_box.TextChanged += new EventHandler(spinnerMaximumToolStripTextBox_TextChanged);

            ToolStripTextBox decimals_text_box = new ToolStripTextBox();
            decimals_text_box.Name = "spinnerDecimalsToolStripTextBox";
            decimals_text_box.TextChanged += new EventHandler(spinnerDecimalsToolStripTextBox_TextChanged);

            ToolStripTextBox increment_text_box = new ToolStripTextBox();
            increment_text_box.Name = "spinnerIncrementToolStripTextBox";
            increment_text_box.TextChanged += new EventHandler(spinnerIncrementToolStripTextBox_TextChanged);

            ToolStripTextBox set_command_text_box = new ToolStripTextBox();
            set_command_text_box.Name = "spinnerSetCommandToolStripTextBox";
            set_command_text_box.TextChanged += new EventHandler(spinnerSetCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem minimum_menu_item = new ToolStripMenuItem();
            minimum_menu_item.Name = "spinnerMinimumToolStripMenuItem";
            minimum_menu_item.Text = "Minimum";
            minimum_menu_item.MouseHover += new EventHandler(spinnerMinimumToolStripMenuItem_MouseHover);
            minimum_menu_item.DropDownItems.Add(minimum_text_box);

            ToolStripMenuItem maximum_menu_item = new ToolStripMenuItem();
            maximum_menu_item.Name = "spinnerMaximumToolStripMenuItem";
            maximum_menu_item.Text = "Maximum";
            maximum_menu_item.MouseHover += new EventHandler(spinnerMaximumToolStripMenuItem_MouseHover);
            maximum_menu_item.DropDownItems.Add(maximum_text_box);

            ToolStripMenuItem decimals_menu_item = new ToolStripMenuItem();
            decimals_menu_item.Name = "spinnerDecimalsToolStripMenuItem";
            decimals_menu_item.Text = "Decimals";
            decimals_menu_item.MouseHover += new EventHandler(spinnerDecimalsToolStripMenuItem_MouseHover);
            decimals_menu_item.DropDownItems.Add(decimals_text_box);

            ToolStripMenuItem increment_menu_item = new ToolStripMenuItem();
            increment_menu_item.Name = "spinnerIncrementToolStripMenuItem";
            increment_menu_item.Text = "Increment";
            increment_menu_item.MouseHover += new EventHandler(spinnerIncrementToolStripMenuItem_MouseHover);
            increment_menu_item.DropDownItems.Add(increment_text_box);

            ToolStripMenuItem set_command_menu_item = new ToolStripMenuItem();
            set_command_menu_item.Name = "spinnerSetCommandToolStripMenuItem";
            set_command_menu_item.Text = "Set Command";
            set_command_menu_item.MouseHover += new EventHandler(spinnerSetCommandToolStripMenuItem_MouseHover);
            set_command_menu_item.DropDownItems.Add(set_command_text_box);

            menu_items.Add(minimum_menu_item);
            menu_items.Add(maximum_menu_item);
            menu_items.Add(decimals_menu_item);
            menu_items.Add(increment_menu_item);
            menu_items.Add(set_command_menu_item);

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
                    NumericUpDown spinner = panel.Controls[1] as NumericUpDown;

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

                    XmlNode minimum_node = control_config.CreateElement("minimum");
                    minimum_node.InnerText = Convert.ToString(spinner.Minimum);
                    control_node.AppendChild(minimum_node);

                    XmlNode maximum_node = control_config.CreateElement("maximum");
                    maximum_node.InnerText = Convert.ToString(spinner.Maximum);
                    control_node.AppendChild(maximum_node);

                    XmlNode decimals_node = control_config.CreateElement("decimals");
                    decimals_node.InnerText = Convert.ToString(spinner.DecimalPlaces);
                    control_node.AppendChild(decimals_node);

                    XmlNode increment_node = control_config.CreateElement("increment");
                    increment_node.InnerText = Convert.ToString(spinner.Increment);
                    control_node.AppendChild(increment_node);

                    XmlNode set_node = control_config.CreateElement("set_command");
                    set_node.InnerText = tags["set_command"];
                    control_node.AppendChild(set_node);

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

        private FlowLayoutPanel add(GroupBox group_box, Point location, NumericUpDown spinner, Tags tags)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            Label label = new Label();
            Padding padding = new Padding();

            tags["name"] = name;
            panel.Location = location;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Tag = tags;

            padding.Top = 6;
            label.Padding = padding;
            label.AutoSize = true;
            panel.Controls.Add(label);
            label.Text = tags["text"] + ": ";

            spinner.Size = new Size(100, 20);
            spinner.ValueChanged += new System.EventHandler(spinner_ValueChanged);
            panel.Controls.Add(spinner);

            group_box.Controls.Add(panel);
            panels.Add(panel);

            return panel;
        }

        private void spinnerMinimumToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            (panel.Controls[1] as NumericUpDown).Minimum = Convert.ToDecimal(text_box.Text);
        }

        private void spinnerMaximumToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            (panel.Controls[1] as NumericUpDown).Maximum = Convert.ToDecimal(text_box.Text);
        }

        private void spinnerDecimalsToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            (panel.Controls[1] as NumericUpDown).DecimalPlaces = Convert.ToInt32(text_box.Text);
        }

        private void spinnerIncrementToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            (panel.Controls[1] as NumericUpDown).Increment = Convert.ToDecimal(text_box.Text);
        }

        private void spinnerSetCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            (panel.Tag as Tags)["set_command"] = text_box.Text;
        }

        private void spinnerMinimumToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            NumericUpDown spinner = panel.Controls[1] as NumericUpDown;
            menu_item.DropDownItems[0].Text = Convert.ToString(spinner.Minimum);
        }

        private void spinnerMaximumToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            NumericUpDown spinner = panel.Controls[1] as NumericUpDown;
            menu_item.DropDownItems[0].Text = Convert.ToString(spinner.Maximum);
        }

        private void spinnerDecimalsToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            NumericUpDown spinner = panel.Controls[1] as NumericUpDown;
            menu_item.DropDownItems[0].Text = Convert.ToString(spinner.DecimalPlaces);
        }

        private void spinnerIncrementToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            NumericUpDown spinner = panel.Controls[1] as NumericUpDown;
            menu_item.DropDownItems[0].Text = Convert.ToString(spinner.Increment);
        }

        private void spinnerSetCommandToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = control_form.menuSource() as FlowLayoutPanel;
            Tags tags = panel.Tag as Tags;
            menu_item.DropDownItems[0].Text = tags["set_command"];
        }

        private void spinner_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown spinner = sender as NumericUpDown;
            Tags tags = spinner.Parent.Tag as Tags;
            control_form.serialWrite(tags["set_command"] + spinner.Value);
        }
    }
}
