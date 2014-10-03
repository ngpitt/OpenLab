using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenLab;

namespace CSV
{
    public class CSV : LoggingPlugin
    {
        public CSV(ControlForm control_form)
        {
            this.control_form = control_form;
        }

        public string name
        {
            get
            {
                return "CSV";
            }
        }

        public string extension
        {
            get
            {
                return "csv";
            }
        }

        public void setup(string log_path, List<string> fields)
        {
            log_file = new StreamWriter(log_path);
            for (int i = 0; i < fields.Count; i++)
            {
                if (i == fields.Count - 1)
                {
                    log_file.Write(fields[i] + "\n");
                }
                else
                {
                    log_file.Write(fields[i] + ",");
                }
            }
        }

        public void update(List<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    log_file.Write(values[i] + "\n");
                }
                else
                {
                    log_file.Write(values[i] + ",");
                }
            }
        }

        public void save() 
        {
            log_file.Dispose();
        }

        private ControlForm control_form;
        private StreamWriter log_file;
    }
}
