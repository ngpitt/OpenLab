using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenLab
{
    public interface IControlPlugin
    {
        void Create(Control Control);
        void LoadSettings(Control Control);
        ToolStripItem[] ContextMenuItems(Control Control);
        void ContextMenuOpening(Control Control);
        string GetLabel(Control Control);
        void SetLabel(Control Control, string Label);
        string GetValue(Control Control);
        void SetValue(Control Control, string Value);
    }

    public interface ILoggingPlugin
    {
        string Extension { get; }
        void Open(string LogPath, IEnumerable<string> Fields);
        void Write(IEnumerable<string> Values);
        void Close();
    }
}
