using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using OpenLab.Lib;

namespace OpenLab.Plugins.Controls
{
    public partial class Toggle : IControlPlugin
    {
        public void Create(OpenLab.Lib.Control Control)
        {
            var label = new Label();
            var onButton = new Button();
            var offButton = new Button();
            var padding = new Padding();

            padding.Top = 8;
            label.Padding = padding;
            label.AutoSize = true;
            Control.Controls.Add(label);

            onButton.Tag = Control;
            onButton.Text = "On";
            onButton.Size = new Size(50, 23);
            onButton.Click += new EventHandler(OnButton_Click);
            Control.Controls.Add(onButton);

            offButton.Tag = Control;
            offButton.Text = "Off";
            offButton.Size = new Size(50, 23);
            offButton.Click += new EventHandler(OffButton_Click);
            Control.Controls.Add(offButton);

            Control.Settings.Add("on_command", string.Empty);
            Control.Settings.Add("off_command", string.Empty);
            Control.Settings.Add("state", string.Empty);
        }

        public void LoadSettings(OpenLab.Lib.Control Control)
        {
            // Nothing to do!
        }

        public ToolStripItem[] GetContextMenuItems(OpenLab.Lib.Control Control)
        {
            var onCommandMenuItem = new ToolStripMenuItem("On Command");
            var onCommandTextBox = new ToolStripTextBox("On Command Text Box");
            var offCommandMenuItem = new ToolStripMenuItem("Off Command");
            var offCommandTextBox = new ToolStripTextBox("Off Command Text Box");

            onCommandMenuItem.DropDownItems.Add(onCommandTextBox);
            onCommandTextBox.TextChanged += new EventHandler(OnCommandTextBox_TextChanged);
            onCommandTextBox.Tag = Control;
            offCommandMenuItem.DropDownItems.Add(offCommandTextBox);
            offCommandTextBox.TextChanged += new EventHandler(OffCommandTextBox_TextChanged);
            offCommandTextBox.Tag = Control;

            return new ToolStripItem[]
            {
                onCommandMenuItem,
                offCommandMenuItem
            };
        }

        public string GetLabel(OpenLab.Lib.Control Control)
        {
            var label = Control.Controls.OfType<Label>().First();

            return label.Text.Substring(0, label.Text.Length - 1);
        }

        public void SetLabel(OpenLab.Lib.Control Control, string Label)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}:", Label);
        }

        public string GetValue(OpenLab.Lib.Control Control)
        {
            return Control.Settings["state"];
        }

        public void SetValue(OpenLab.Lib.Control Control, string Value)
        {
            // Nothing to do!
        }

        public void ContextMenuOpening(OpenLab.Lib.Control Control)
        {
            Control.GetContextMenuItem<ToolStripTextBox>("On Command Text Box").Text = Control.Settings["on_command"];
            Control.GetContextMenuItem<ToolStripTextBox>("Off Command Text Box").Text = Control.Settings["off_command"];
        }

        private void OnButton_Click(object Sender, EventArgs EventArgs)
        {
            var button = (Button)Sender;
            var control = (OpenLab.Lib.Control)button.Tag;

            control.SerialPort.WriteLine(control.Settings["on_command"]);
            control.Settings["state"] = "On";
        }

        private void OffButton_Click(object Sender, EventArgs EventArgs)
        {
            var button = (Button)Sender;
            var control = (OpenLab.Lib.Control)button.Tag;

            control.SerialPort.WriteLine(control.Settings["off_command"]);
            control.Settings["state"] = "Off";
        }

        private void OnCommandTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Lib.Control)textBox.Tag;

            control.Settings["on_command"] = textBox.Text;
        }

        private void OffCommandTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Lib.Control)textBox.Tag;

            control.Settings["off_command"] = textBox.Text;
        }
    }
}
