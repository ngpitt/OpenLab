using OpenLab.Lib;
using System.Windows.Forms;

namespace OpenLab.Plugins.Controls
{
    public class Meter : IControlPlugin
    {
        public PinMode RequiredMode
        {
            get
            {
                return PinMode.ANALOG_INPUT;
            }
        }

        public void Create(Lib.Control Control)
        {
            var label = new Label();
            var value = new Label();

            label.AutoSize = true;
            value.AutoSize = true;
            value.Margin = new Padding(2, 0, 0, 0);

            Control.Controls.Add(label);
            Control.Controls.Add(value);

            Control.Text = new Mapping(() => label.Text.Substring(0, label.Text.Length - 1),
                Value => label.Text = string.Format("{0}:", Value));
            Control.Value = new Mapping(() => Control.ReadValue(), Value => value.Text = Value);
        }
    }
}
