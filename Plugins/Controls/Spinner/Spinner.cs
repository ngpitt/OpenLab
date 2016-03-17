using OpenLab.Lib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenLab.Plugins.Controls
{
    public partial class Spinner : IControlPlugin
    {
        public PinMode RequiredMode
        {
            get
            {
                return PinMode.ANALOG_OUTPUT;
            }
        }

        public void Create(Lib.Control Control)
        {
            var label = new Label();
            var numericUpDown = new NumericUpDown();

            label.Padding = new Padding(0, 6, 0, 0);
            label.AutoSize = true;

            numericUpDown.Tag = Control;
            numericUpDown.Size = new Size(100, 20);
            numericUpDown.ValueChanged += new EventHandler(NumericUpDown_ValueChanged);
            numericUpDown.Minimum = 0;
            numericUpDown.Maximum = 100;
            numericUpDown.Increment = 1;
            numericUpDown.DecimalPlaces = 0;

            Control.Controls.Add(label);
            Control.Controls.Add(numericUpDown);

            Control.Tag = numericUpDown.Value.ToString();

            Control.Text = new Mapping(() => label.Text.Substring(0, label.Text.Length - 1), Value => label.Text = $"{Value}:");
            Control.Value = new Mapping(() => (string)Control.Tag, Value => Control.Tag = Value);
            Control.Value.Set(numericUpDown.Value.ToString());

            Control.Settings.Add("Minimum", new Mapping(() => numericUpDown.Minimum.ToString(),
                Value => numericUpDown.Minimum = Convert.ToDecimal(Value)));
            Control.Settings.Add("Maximum", new Mapping(() => numericUpDown.Maximum.ToString(),
                Value => numericUpDown.Maximum = Convert.ToDecimal(Value)));
            Control.Settings.Add("Increment", new Mapping(() => numericUpDown.Increment.ToString(),
                Value => numericUpDown.Increment = Convert.ToDecimal(Value)));
            Control.Settings.Add("Decimal Places", new Mapping(() => numericUpDown.DecimalPlaces.ToString(),
                Value => numericUpDown.DecimalPlaces = Convert.ToInt32(Value)));
        }

        private void NumericUpDown_ValueChanged(object Sender, EventArgs EventArgs)
        {
            var numericUpDown = (NumericUpDown)Sender;
            var control = (Lib.Control)numericUpDown.Tag;
            var value = numericUpDown.Value.ToString();

            control.WriteValue(value);
            control.Value.Set(value);
        }
    }
}
