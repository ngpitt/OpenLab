using ControlPlugins;

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