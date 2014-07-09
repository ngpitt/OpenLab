using ControlPlugins;

namespace Indicator
{
    public class Indicator : IControl
    {
        public string name
        {
            get
            {
                return "Indicator";
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