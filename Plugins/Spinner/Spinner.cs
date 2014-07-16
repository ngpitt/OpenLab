using System.Xml;
using PluginInterface;

namespace Spinner
{
    public class Spinner : IPlugin
    {
        public string name
        {
            get
            {
                return "Spinner";
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