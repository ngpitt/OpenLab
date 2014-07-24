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
    public class Spinner : ControlPlugin
    {
        public Spinner(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public string name
        {
            get
            {
                return "Spinner";
            }
        }

        public FlowLayoutPanel create(Point location)
        {
            Tags tags = new Tags();
            NumericUpDown spinner = new NumericUpDown();

            tags["label"] = name;
            spinner.Minimum = spinner.Maximum = spinner.Increment = 0;
            spinner.DecimalPlaces = 0;
            tags["set_command"] = "";

            return create(location, spinner, tags);
        }

        public FlowLayoutPanel create(XmlNode xml_node)
        {
            int x, y;
            NumericUpDown spinner = new NumericUpDown();
            Tags tags = new Tags();

            tags["label"] = xml_node["label"].InnerText;
            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);
            spinner.Minimum = Convert.ToDecimal(xml_node["minimum"].InnerText);
            spinner.Maximum = Convert.ToDecimal(xml_node["maximum"].InnerText);
            spinner.DecimalPlaces = Convert.ToInt32(xml_node["decimal_places"].InnerText);
            spinner.Increment = Convert.ToDecimal(xml_node["increment"].InnerText);
            tags["set_command"] = xml_node["set_command"].InnerText;

            return create(new Point(x, y), spinner, tags);
        }

        public FlowLayoutPanel copy(FlowLayoutPanel source_control)
        {
            FlowLayoutPanel control = new FlowLayoutPanel();
            Label label = new Label();
            NumericUpDown spinner = new NumericUpDown();
            Padding padding = new Padding();
            NumericUpDown source_spinner = source_control.Controls[1] as NumericUpDown;
            Tags tags = source_control.Tag as Tags;

            control.Location = source_control.Location;
            control.AutoSize = true;
            control.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control.Tag = new Tags(tags);

            padding.Top = 6;
            label.Padding = padding;
            label.AutoSize = true;
            control.Controls.Add(label);
            label.Text = tags["label"] + ": ";

            spinner.Size = new Size(100, 20);
            spinner.Minimum = source_spinner.Minimum;
            spinner.Maximum = source_spinner.Maximum;
            spinner.DecimalPlaces = source_spinner.DecimalPlaces;
            spinner.Increment = source_spinner.Increment;
            spinner.ValueChanged += new System.EventHandler(spinner_ValueChanged);
            control.Controls.Add(spinner);

            return control;
        }

        public void update(FlowLayoutPanel control, SafeSerialPort serial_port)
        {
            // Nothing to do here!
        }

        public List<ToolStripMenuItem> settings(FlowLayoutPanel control)
        {
            List<ToolStripMenuItem> menu_items = new List<ToolStripMenuItem>();
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;
            Tags tags = control.Tag as Tags;

            ToolStripTextBox minimum_text_box = new ToolStripTextBox();
            minimum_text_box.Text = Convert.ToString(spinner.Minimum);
            minimum_text_box.TextChanged += new EventHandler(minimumToolStripTextBox_TextChanged);

            ToolStripTextBox maximum_text_box = new ToolStripTextBox();
            maximum_text_box.Text = Convert.ToString(spinner.Maximum);
            maximum_text_box.TextChanged += new EventHandler(maximumToolStripTextBox_TextChanged);

            ToolStripTextBox decimal_places_text_box = new ToolStripTextBox();
            decimal_places_text_box.Text = Convert.ToString(spinner.DecimalPlaces);
            decimal_places_text_box.TextChanged += new EventHandler(decimalPlacesToolStripTextBox_TextChanged);

            ToolStripTextBox increment_text_box = new ToolStripTextBox();
            increment_text_box.Text = Convert.ToString(spinner.Increment);
            increment_text_box.TextChanged += new EventHandler(incrementToolStripTextBox_TextChanged);

            ToolStripTextBox set_command_text_box = new ToolStripTextBox();
            set_command_text_box.Text = tags["set_command"];
            set_command_text_box.TextChanged += new EventHandler(setCommandToolStripTextBox_TextChanged);

            ToolStripMenuItem minimum_menu_item = new ToolStripMenuItem();
            minimum_menu_item.Text = "Minimum";
            minimum_menu_item.DropDownItems.Add(minimum_text_box);

            ToolStripMenuItem maximum_menu_item = new ToolStripMenuItem();
            maximum_menu_item.Text = "Maximum";
            maximum_menu_item.DropDownItems.Add(maximum_text_box);

            ToolStripMenuItem decimal_places_menu_item = new ToolStripMenuItem();
            decimal_places_menu_item.Text = "Decimal Places";
            decimal_places_menu_item.DropDownItems.Add(decimal_places_text_box);

            ToolStripMenuItem increment_menu_item = new ToolStripMenuItem();
            increment_menu_item.Text = "Increment";
            increment_menu_item.DropDownItems.Add(increment_text_box);

            ToolStripMenuItem set_command_menu_item = new ToolStripMenuItem();
            set_command_menu_item.Text = "Set Command";
            set_command_menu_item.DropDownItems.Add(set_command_text_box);

            menu_items.Add(minimum_menu_item);
            menu_items.Add(maximum_menu_item);
            menu_items.Add(decimal_places_menu_item);
            menu_items.Add(increment_menu_item);
            menu_items.Add(set_command_menu_item);

            return menu_items;
        }

        public XmlDocument save(FlowLayoutPanel control)
        {
            XmlDocument control_config = new XmlDocument();
            Tags tags = control.Tag as Tags;
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;

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

            XmlNode minimum_node = control_config.CreateElement("minimum");
            minimum_node.InnerText = Convert.ToString(spinner.Minimum);
            control_node.AppendChild(minimum_node);

            XmlNode maximum_node = control_config.CreateElement("maximum");
            maximum_node.InnerText = Convert.ToString(spinner.Maximum);
            control_node.AppendChild(maximum_node);

            XmlNode decimal_places_node = control_config.CreateElement("decimal_places");
            decimal_places_node.InnerText = Convert.ToString(spinner.DecimalPlaces);
            control_node.AppendChild(decimal_places_node);

            XmlNode increment_node = control_config.CreateElement("increment");
            increment_node.InnerText = Convert.ToString(spinner.Increment);
            control_node.AppendChild(increment_node);

            XmlNode set_command_node = control_config.CreateElement("set_command");
            set_command_node.InnerText = tags["set_command"];
            control_node.AppendChild(set_command_node);

            control_config.AppendChild(control_node);

            return control_config;
        }

        private ControlForm control_form;

        private FlowLayoutPanel create(Point location, NumericUpDown spinner, Tags tags)
        {
            FlowLayoutPanel control = new FlowLayoutPanel();
            Label label = new Label();
            Padding padding = new Padding();

            tags["plugin"] = name;
            control.Location = location;
            control.AutoSize = true;
            control.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            control.Tag = tags;

            padding.Top = 6;
            label.Padding = padding;
            label.AutoSize = true;
            control.Controls.Add(label);
            label.Text = tags["label"] + ": ";

            spinner.Size = new Size(100, 20);
            spinner.ValueChanged += new System.EventHandler(spinner_ValueChanged);
            control.Controls.Add(spinner);

            return control;
        }

        private void minimumToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;
            spinner.Minimum = Convert.ToDecimal(text_box.Text);
        }

        private void maximumToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;
            spinner.Maximum = Convert.ToDecimal(text_box.Text);
        }

        private void decimalPlacesToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;
            spinner.DecimalPlaces = Convert.ToInt32(text_box.Text);
        }

        private void incrementToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            NumericUpDown spinner = control.Controls[1] as NumericUpDown;
            spinner.Increment = Convert.ToDecimal(text_box.Text);
        }

        private void setCommandToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            FlowLayoutPanel control = control_form.menuSource as FlowLayoutPanel;
            Tags tags = control.Tag as Tags;
            tags["set_command"] = text_box.Text;
        }

        private void spinner_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown spinner = sender as NumericUpDown;
            Tags tags = spinner.Parent.Tag as Tags;
            control_form.serialWrite(tags["set_command"] + spinner.Value);
        }
    }
}
