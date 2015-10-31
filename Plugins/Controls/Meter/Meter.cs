using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using OpenLab.Lib;

namespace OpenLab.Plugins.Controls
{
    public class Meter : IControlPlugin
    {
        private Regex LabelRegex = new Regex(@"^(.*):.*");

        public void Create(Lib.Control Control)
        {
            var label = new Label();

            label.AutoSize = true;
            Control.Controls.Add(label);

            Control.Settings.Add("get_command", string.Empty);
        }

        public void LoadSettings(Lib.Control Control)
        {
            // Nothing to do!
        }

        public ToolStripItem[] GetContextMenuItems(Lib.Control Control)
        {
            var getCommandMenuItem = new ToolStripMenuItem("Get Command");
            var getCommandTextBox = new ToolStripTextBox("Get Command Text Box");

            getCommandMenuItem.DropDownItems.Add(getCommandTextBox);
            getCommandTextBox.TextChanged += new EventHandler(GetCommandToolStripTextBox_TextChanged);
            getCommandTextBox.Tag = Control;

            return new ToolStripItem[] { getCommandMenuItem };
        }

        public void ContextMenuOpening(Lib.Control Control)
        {
            Control.GetContextMenuItem<ToolStripTextBox>("Get Command Text Box").Text = Control.Settings["get_command"];
        }

        public string GetLabel(Lib.Control Control)
        {
            return ParseLabel(Control);
        }

        public void SetLabel(Lib.Control Control, string Label)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}:", Label);
        }

        public string GetValue(Lib.Control Control)
        {
            Control.SerialPort.Write(Control.Settings["get_command"] + "\n");

            return Control.SerialPort.ReadLine();
        }

        public void SetValue(Lib.Control Control, string Value)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}: {1}", ParseLabel(Control), Value);
        }

        private void GetCommandToolStripTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (Lib.Control)textBox.Tag;

            control.Settings["get_command"] = textBox.Text;
        }

        private string ParseLabel(Lib.Control Control)
        {
            return LabelRegex.Match(Control.Controls.OfType<Label>().First().Text).Groups[1].Value;
        }
    }
}
