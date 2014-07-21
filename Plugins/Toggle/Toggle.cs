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
        public string name
        {
            get
            {
                return "Toggle";
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
            Button on_button = new Button();
            Button off_button = new Button();
            Tags on_tags = new Tags();
            Tags off_tags = new Tags();

            x = Convert.ToInt32(xml_node["x"].InnerText);
            y = Convert.ToInt32(xml_node["y"].InnerText);

            label.Location = new Point(x, y);
            label.AutoSize = true;

            group_box.Controls.Add(label);

            label.Text = xml_node["text"].InnerText + ": ";

            on_tags["on"] = xml_node["on"].InnerText;

            on_button.Text = "On";
            on_button.Location = new Point(x + label.Size.Width, y - 5);
            on_button.Size = new Size(63, 23);
            on_button.Tag = on_tags;
            on_button.Click += new System.EventHandler(onButton_Click);
                
            group_box.Controls.Add(on_button);

            off_tags["off"] = xml_node["off"].InnerText;

            off_button.Text = "Off";
            off_button.Location = new Point(x + label.Size.Width + on_button.Size.Width + 5, y - 5);
            off_button.Size = new Size(63, 23);
            off_button.Tag = off_tags;
            off_button.Click += new System.EventHandler(offButton_Click);

            group_box.Controls.Add(off_button);
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

        private void onButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            control_form.serialWrite(((Tags)button.Tag)["on"]);
        }

        private void offButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            control_form.serialWrite(((Tags)button.Tag)["off"]);
        }
    }
}