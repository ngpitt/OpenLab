using ControlPlugins;

namespace DigitalMeter
{
    public class DigitalMeter : IControl
    {
        public string name
        {
            get
            {
                return "Digital Meter";
            }
        }
        
        public Label[] labels
        {
            get
            {
                return null;
            }
        }

        public Button[] buttons
        {
            get
            {
                return null;
            }
        }
    }
}