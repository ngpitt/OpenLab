using System.Xml;

namespace PluginInterface
{
    public interface IPlugin
    {
        string name { get; }
        bool load(XmlDocument config);
        bool save(ref XmlDocument config);
    }
}