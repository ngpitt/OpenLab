using System;
using System.Windows.Forms;
using OpenLab;
using System.Text.RegularExpressions;
using System.Linq;

namespace Spinner
{
    public class Meter : IControlPlugin
    {
        private Regex LabelRegex = new Regex(@"^(.*):.*");

        public void Create(OpenLab.Control Control)
        {
            var label = new Label();

            label.AutoSize = true;
            Control.Controls.Add(label);

            Control.Settings.Add("get_command", string.Empty);
        }

        public void LoadSettings(OpenLab.Control Control)
        {
            // Nothing to do!
        }

        public ToolStripItem[] ContextMenuItems(OpenLab.Control Control)
        {
            var getCommandMenuItem = new ToolStripMenuItem("Get Command");
            var getCommandTextBox = new ToolStripTextBox("Get Command Text Box");

            getCommandMenuItem.DropDownItems.Add(getCommandTextBox);
            getCommandTextBox.TextChanged += new EventHandler(GetCommandToolStripTextBox_TextChanged);
            getCommandTextBox.Tag = Control;

            return new ToolStripItem[] { getCommandMenuItem };
        }

        public void ContextMenuOpening(OpenLab.Control Control)
        {
            Control.GetContextMenuItem<ToolStripTextBox>("Get Command Text Box").Text = Control.Settings["get_command"];
        }

        public string GetLabel(OpenLab.Control Control)
        {
            return ParseLabel(Control);
        }

        public void SetLabel(OpenLab.Control Control, string Label)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}:", Label);
        }

        public string GetValue(OpenLab.Control Control)
        {
            Control.SerialPort.WriteLine(Control.Settings["get_command"]);

            return Control.SerialPort.ReadLine();
        }

        public void SetValue(OpenLab.Control Control, string Value)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}: {1}", ParseLabel(Control), Value);
        }

        private void GetCommandToolStripTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["get_command"] = textBox.Text;
        }

        private string ParseLabel(OpenLab.Control Control)
        {
            return LabelRegex.Match(Control.Controls.OfType<Label>().First().Text).Groups[1].Value;
        }
    }
}
