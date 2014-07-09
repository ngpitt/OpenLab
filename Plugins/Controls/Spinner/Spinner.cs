using ControlPlugins;

namespace Spinner
{
    public class Spinner : IControl
    {
        public string name
        {
            get
            {
                return "Spinner";
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