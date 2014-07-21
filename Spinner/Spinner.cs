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
    public class Toggle : IControl
    {
        public string name
        {
            get
            {
                return "Spinner";
            }
        }

        public void init(ControlForm control_form, XmlDocument config)
        {
            this.control_form = control_form;
            this.config = config;
        }

        public void add(GroupBox group_box, XmlNode xml_node)
        {
            int x, y;
            Label label = new Label();
            NumericUpDown spinner = new NumericUpDown();
            Tags tags = new Tags();

            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);

            label.Location = new Point(x, y);
            label.AutoSize = true;

            group_box.Controls.Add(label);

            label.Text = xml_node["text"].InnerText + ": ";

            tags["set"] = xml_node["set"].InnerText;

            spinner.Location = new Point(x + label.Size.Width, y - 2);
            spinner.Size = new Size(63, 23);
            spinner.Minimum = Convert.ToDecimal(xml_node["minimum"].InnerText);
            spinner.Maximum = Convert.ToDecimal(xml_node["maximum"].InnerText);
            spinner.DecimalPlaces = Convert.ToInt32(xml_node["decimals"].InnerText);
            spinner.Increment = Convert.ToDecimal(xml_node["increment"].InnerText);
            spinner.Tag = tags;
            spinner.ValueChanged += new System.EventHandler(spinner_ValueChanged);

            group_box.Controls.Add(spinner);
        }

        public void update(SafeSerialPort serial_port)
        {
            // Nothing to do here!
        }

        public void save()
        {
            // TODO
        }

        private ControlForm control_form;
        private XmlDocument config;

        private void spinner_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown spinner = (NumericUpDown)sender;
            control_form.serialWrite(((Tags)spinner.Tag)["set"] + spinner.Value);
        }
    }
}