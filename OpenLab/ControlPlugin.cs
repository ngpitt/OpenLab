using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenLab
{
    public interface ControlPlugin
    {
        string name { get; }
        FlowLayoutPanel create(Point location);
        FlowLayoutPanel create(XmlNode config);
        FlowLayoutPanel copy(FlowLayoutPanel source_control);
        void update(FlowLayoutPanel control, SafeSerialPort serial_port);
        List<ToolStripMenuItem> settings(FlowLayoutPanel control);
        XmlDocument save(FlowLayoutPanel control);
    }
}
