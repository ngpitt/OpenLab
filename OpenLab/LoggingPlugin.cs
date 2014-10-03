using System.Collections.Generic;

namespace OpenLab
{
    public interface LoggingPlugin
    {
        string name { get; }
        string extension { get; }
        void setup(string log_path, List<string> fields);
        void update(List<string> values);
        void save();
    }
}
