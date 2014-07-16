using System.Xml;
using PluginInterface;

namespace Indicator
{
    public class Indicator : IPlugin
    {
        public string name
        {
            get
            {
                return "Indicator";
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