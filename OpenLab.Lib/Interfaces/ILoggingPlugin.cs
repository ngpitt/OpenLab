using System.Collections.Generic;

namespace OpenLab.Lib
{
    public interface ILoggingPlugin
    {
        string Extension { get; }
        void Open(string LogPath, IEnumerable<string> Fields);
        void Write(IEnumerable<string> Values);
        void Close();
    }
}
