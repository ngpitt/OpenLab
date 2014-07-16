using System.Xml;
using PluginInterface;

namespace Toggle
{
    public class Toggle : IPlugin
    {
        public string name
        {
            get
            {
                return "Toggle";
            }
        }

        public bool load(XmlDocument config)
        {
            return true;
        }

        public bool save(ref XmlDocument config)
        {
            return true;
        }
    }
}