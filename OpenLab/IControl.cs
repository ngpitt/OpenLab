using System.Windows.Forms;
using System.Xml;
using System.IO.Ports;

namespace OpenLab
{
    public interface IControl
    {
        string name { get; }
        void init(ControlForm control_form, XmlDocument config);
        void add(GroupBox group_box, XmlNode xml_node);
        void update(SafeSerialPort serial_port);
        void save();
    }
}