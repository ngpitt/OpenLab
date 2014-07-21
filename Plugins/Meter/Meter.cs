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
            labels = new List<Label>();
            update_delagate = new UpdateDelagate(updateForm);
        }

        public string name
        {
            get
            {
                return "Meter";
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
            Tags tags = new Tags();

            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);

            tags["text"] = xml_node["text"].InnerText;
            tags["get"] = xml_node["get"].InnerText;

            label.Location = new Point(x, y);
            label.AutoSize = true;
            label.Tag = tags;

            group_box.Controls.Add(label);

            label.Text = tags["text"] + ": ";

            labels.Add(label);
        }

        public void update(SafeSerialPort serial_port)
        {
            foreach (Label label in labels)
            {
                serial_port.WriteLine(((Tags)label.Tag)["get"]);
                control_form.BeginInvoke(update_delagate, label, serial_port.ReadLine());
            }
        }

        public void save()
        {
            // TODO
        }

        private ControlForm control_form;
        private XmlDocument config;
        private List<Label> labels;
        private delegate void UpdateDelagate(Label label, string value);
        private UpdateDelagate update_delagate;

        private void updateForm(Label label, string value)
        {
            label.Text = ((Tags)label.Tag)["text"] + ": " + value;
        }
    }
}