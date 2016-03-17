using OpenLab.Lib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenLab.Plugins.Controls
{
    public partial class Toggle : IControlPlugin
    {
        public PinMode RequiredMode
        {
            get
            {
                return PinMode.DIGITAL_OUTPUT;
            }
        }

        public void Create(Lib.Control Control)
        {
            var label = new Label();
            var onButton = new Button();
            var offButton = new Button();

            label.Padding = new Padding(0, 8, 0, 0);
            label.AutoSize = true;

            onButton.Tag = Control;
            onButton.Text = "On";
            onButton.Size = new Size(50, 23);
            onButton.Click += new EventHandler(OnButton_Click);

            offButton.Tag = Control;
            offButton.Text = "Off";
            offButton.Size = new Size(50, 23);
            offButton.Click += new EventHandler(OffButton_Click);

            Control.Controls.Add(label);
            Control.Controls.Add(onButton);
            Control.Controls.Add(offButton);

            Control.Text = new Mapping(() => label.Text.Substring(0, label.Text.Length - 1),
                Value => label.Text = string.Format("{0}:", Value));
            Control.Value = new Mapping(() => (string)Control.Tag, Value => Control.Tag = Value);
            Control.Value.Set("0");
        }

        private void OnButton_Click(object Sender, EventArgs EventArgs)
        {
            var button = (Button)Sender;
            var control = (Lib.Control)button.Tag;

            control.WriteValue("1");
            control.Value.Set("1");
        }

        private void OffButton_Click(object Sender, EventArgs EventArgs)
        {
            var button = (Button)Sender;
            var control = (Lib.Control)button.Tag;

            control.WriteValue("0");
            control.Value.Set("0");
        }
    }
}
