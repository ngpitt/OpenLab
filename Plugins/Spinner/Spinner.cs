using System;
using System.Drawing;
using System.Windows.Forms;
using OpenLab;
using System.Linq;

namespace Spinner
{
    public partial class Spinner : IControlPlugin
    {

        public void Create(OpenLab.Control Control)
        {
            var label = new Label();
            var padding = new Padding();
            var numericUpDown = new NumericUpDown();

            padding.Top = 6;
            label.Padding = padding;
            label.AutoSize = true;
            Control.Controls.Add(label);

            numericUpDown.Tag = Control;
            numericUpDown.Size = new Size(100, 20);
            numericUpDown.ValueChanged += new EventHandler(NumericUpDown_ValueChanged);
            Control.Controls.Add(numericUpDown);

            Control.Settings.Add("minimum", "0");
            Control.Settings.Add("maximum", "100");
            Control.Settings.Add("increment", "1");
            Control.Settings.Add("decimal_places", "0");
            Control.Settings.Add("set_command", string.Empty);
        }

        public void LoadSettings(OpenLab.Control Control)
        {
            var numericUpDown = Control.Controls.OfType<NumericUpDown>().First();

            numericUpDown.Minimum = Convert.ToDecimal(Control.Settings["minimum"]);
            numericUpDown.Maximum = Convert.ToDecimal(Control.Settings["maximum"]);
            numericUpDown.Increment = Convert.ToDecimal(Control.Settings["increment"]);
            numericUpDown.DecimalPlaces = Convert.ToInt32(Control.Settings["decimal_places"]);
        }

        public ToolStripItem[] ContextMenuItems(OpenLab.Control Control)
        {
            var minimumMenuItem = new ToolStripMenuItem("Minimum");
            var minimumTextBox = new ToolStripTextBox("Minimum Text Box");
            var maximumMenuItem = new ToolStripMenuItem("Maximum");
            var maximumTextBox = new ToolStripTextBox("Maximum Text Box");
            var incrementMenuItem = new ToolStripMenuItem("Increment");
            var incrementTextBox = new ToolStripTextBox("Increment Text Box");
            var decimalPlacesMenuItem = new ToolStripMenuItem("Decimal Places");
            var decimalPlacesTextBox = new ToolStripTextBox("Decimal Places Text Box");
            var setCommandMenuItem = new ToolStripMenuItem("Set Command");
            var setCommandTextBox = new ToolStripTextBox("Set Command Text Box");

            minimumMenuItem.DropDownItems.Add(minimumTextBox);
            minimumTextBox.TextChanged += new EventHandler(MinimumTextBox_TextChanged);
            minimumTextBox.Tag = Control;
            maximumMenuItem.DropDownItems.Add(maximumTextBox);
            maximumTextBox.TextChanged += new EventHandler(MaximumTextBox_TextChanged);
            maximumTextBox.Tag = Control;
            incrementMenuItem.DropDownItems.Add(incrementTextBox);
            incrementTextBox.TextChanged += new EventHandler(IncrementTextBox_TextChanged);
            incrementTextBox.Tag = Control;
            decimalPlacesMenuItem.DropDownItems.Add(decimalPlacesTextBox);
            decimalPlacesTextBox.TextChanged += new EventHandler(DecimalPlacesTextBox_TextChanged);
            decimalPlacesTextBox.Tag = Control;
            setCommandMenuItem.DropDownItems.Add(setCommandTextBox);
            setCommandTextBox.TextChanged += new EventHandler(SetCommandTextBox_TextChanged);
            setCommandTextBox.Tag = Control;

            return new ToolStripItem[]
            {
                minimumMenuItem,
                maximumMenuItem,
                incrementMenuItem,
                decimalPlacesMenuItem,
                setCommandMenuItem
            };
        }

        public void ContextMenuOpening(OpenLab.Control Control)
        {
            Control.GetContextMenuItem<ToolStripTextBox>("Minimum Text Box").Text = Control.Settings["minimum"];
            Control.GetContextMenuItem<ToolStripTextBox>("Maximum Text Box").Text = Control.Settings["maximum"];
            Control.GetContextMenuItem<ToolStripTextBox>("Increment Text Box").Text = Control.Settings["increment"];
            Control.GetContextMenuItem<ToolStripTextBox>("Decimal Places Text Box").Text = Control.Settings["decimal_places"];
            Control.GetContextMenuItem<ToolStripTextBox>("Set Command Text Box").Text = Control.Settings["set_command"];
        }

        public string GetLabel(OpenLab.Control Control)
        {
            var label = Control.Controls.OfType<Label>().First();

            return label.Text.Substring(0, label.Text.Length - 1);
        }

        public void SetLabel(OpenLab.Control Control, string Label)
        {
            Control.Controls.OfType<Label>().First().Text = string.Format("{0}:", Label);
        }

        public string GetValue(OpenLab.Control Control)
        {
            return Control.Controls.OfType<NumericUpDown>().First().Value.ToString();
        }

        public void SetValue(OpenLab.Control Control, string Value)
        {
            // Nothing to do!
        }

        private void NumericUpDown_ValueChanged(object Sender, EventArgs EventArgs)
        {
            var numericUpDown = (NumericUpDown)Sender;
            var control = (OpenLab.Control)numericUpDown.Tag;

            control.SerialPort.WriteLine(string.Format("{0}{1}", control.Settings["set_command"], numericUpDown.Value));
        }

        private void MinimumTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["minimum"] = textBox.Text;
        }

        private void MaximumTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["maximum"] = textBox.Text;
        }

        private void IncrementTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["increment"] = textBox.Text;
        }

        private void DecimalPlacesTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["decimal_places"] = textBox.Text;
        }

        private void SetCommandTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            var textBox = (ToolStripTextBox)Sender;
            var control = (OpenLab.Control)textBox.Tag;

            control.Settings["set_command"] = textBox.Text;
        }
    }
}
