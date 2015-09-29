using System.IO;
using System.Reflection;
using System.Collections.Generic;
using OpenLab;

namespace CSV
{
    public class CSV : ILoggingPlugin
    {
        public string Extension
        {
            get
            {
                return ".csv";
            }
        }

        public void Open(string LogPath, IEnumerable<string> Fields)
        {
            LogFile = new StreamWriter(LogPath);

            WriteLine(Fields);
        }

        public void Write(IEnumerable<string> Values)
        {
            WriteLine(Values);
        }

        public void Close() 
        {
            LogFile.Dispose();
        }

        private StreamWriter LogFile;

        private void WriteLine(IEnumerable<string> Values)
        {
            LogFile.Write(string.Format("{0}\n", string.Join(",", Values)));
        }
    }
}
