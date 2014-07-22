using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenLab
{
    public interface IControl
    {
        string name { get; }
        void init(ControlForm control_form);
        FlowLayoutPanel add(GroupBox group_box, Point location);
        FlowLayoutPanel add(GroupBox group_box, XmlNode xml_node);
        List<ToolStripMenuItem> settings();
        XmlDocument save(GroupBox group_box);
        void update(SafeSerialPort serial_port);
        void reset();
    }
}
