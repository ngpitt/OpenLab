using System.Windows.Forms;

namespace OpenLab.Lib
{
    public interface IControlPlugin
    {
        void Create(Control Control);
        void LoadSettings(Control Control);
        ToolStripItem[] GetContextMenuItems(Control Control);
        void ContextMenuOpening(Control Control);
        string GetLabel(Control Control);
        void SetLabel(Control Control, string Label);
        string GetValue(Control Control);
        void SetValue(Control Control, string Value);
    }
}
