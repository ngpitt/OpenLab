using System.Xml;
using PluginInterface;

namespace DigitalMeter
{
    public class DigitalMeter : IPlugin
    {
        public string name
        {
            get
            {
                return "Digital Meter";
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