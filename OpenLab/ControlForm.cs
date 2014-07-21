using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenLab
{
    public partial class ControlForm : Form
    {
        public ControlForm()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(controlFormClose);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            disconnectToolStripMenuItem.Enabled = false;

            load_file_dialog = new OpenFileDialog();
            config = new XmlDocument();
            cleanup_delagate = new CleanupDelagate(cleanupForm);
            controls = new Dictionary<string, IControl>();
            groupboxes = new List<GroupBox>();

            ICollection<IControl> control_collection = GenericPluginLoader<IControl>.LoadPlugins("Plugins");

            if (control_collection != null)
            {
                foreach (var item in control_collection)
                {
                    controls.Add(item.name, item);
                    item.init(this, config);
                }
            }
        }

        public void serialWrite(string message)
        {
            try
            {
                serial_port.WriteLine(message);
            }
            catch
            {
                MessageBox.Show("Serial port disconnected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cleanupForm();
            }
        }

        private Dictionary<string, IControl> controls;
        private OpenFileDialog load_file_dialog;
        private XmlDocument config;
        private List<GroupBox> groupboxes;
        private string port_name = null;
        private int baud_rate = 115200, data_bits = 8, update_interval = 500, read_timeout = 500, write_timeout = 500;
        private Parity parity = Parity.None;
        private StopBits stop_bits = StopBits.One;
        private SafeSerialPort serial_port;
        private bool run = false;
        private Thread update_thread;
        private delegate void CleanupDelagate();
        private CleanupDelagate cleanup_delagate;

        private void controlFormClose(object sender, FormClosingEventArgs e)
        {
            if (run)
            {
                if (MessageBox.Show("The serial port is currently connected.\nDo you really want to quit?", "OpenLab", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cleanupForm();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x, y, width, height;

            load_file_dialog.Filter = "OpenLab Config (*.olc)|*.olc";

            if (load_file_dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            config.Load(load_file_dialog.FileName);

            XmlNodeList dependency_node_list = config["config"].GetElementsByTagName("dependency");

            foreach (XmlNode dependency_node in dependency_node_list)
            {
                if (!controls.ContainsKey(dependency_node.InnerText))
                {
                    MessageBox.Show("Required plugin \"" + dependency_node.InnerText + "\" missing.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            width = Convert.ToInt32(config["config"]["width"].InnerText);
            height = Convert.ToInt32(config["config"]["height"].InnerText);

            this.Text = config["config"]["text"].InnerText;
            this.Size = new Size(width, height);

            XmlNodeList groupbox_node_list = config["config"].GetElementsByTagName("groupbox");

            foreach (XmlNode groupbox_node in groupbox_node_list)
            {
                GroupBox groupbox = new GroupBox();

                x = Convert.ToInt32(groupbox_node["x"].InnerText);
                y = Convert.ToInt32(groupbox_node["y"].InnerText);
                width = Convert.ToInt32(groupbox_node["width"].InnerText);
                height = Convert.ToInt32(groupbox_node["height"].InnerText);

                groupbox.Text = groupbox_node["text"].InnerText;
                groupbox.Location = new Point(x, y);
                groupbox.Size = new Size(width, height);
                groupbox.Enabled = false;

                this.Controls.Add(groupbox);

                XmlElement groupbox_element = groupbox_node as XmlElement;
                XmlNodeList control_node_list = groupbox_element.GetElementsByTagName("control");

                foreach (XmlNode control_node in control_node_list)
                {
                    controls[control_node["name"].InnerText].add(groupbox, control_node);
                }

                groupboxes.Add(groupbox);
            }
        }


        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (port_name == null || baud_rate <= 0 || data_bits <= 0 || update_interval <= 0 || read_timeout <= 0 || write_timeout <= 0)
            {
                MessageBox.Show("Invalid port settings.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            serial_port = new SafeSerialPort(port_name, baud_rate, parity, data_bits, stop_bits);
            serial_port.ReadTimeout = read_timeout;
            serial_port.WriteTimeout = write_timeout;

            try
            {
                serial_port.Open();
            }
            catch
            {
                MessageBox.Show("Unable to open serial device.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            update_thread = new Thread(updateThread);
            update_thread.Start();

            settingsToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;

            foreach (GroupBox groupbox in groupboxes)
            {
                groupbox.Enabled = true;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanupForm();

            disconnectToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = true;

            foreach (GroupBox groupbox in groupboxes)
            {
                groupbox.Enabled = false;
            }
        }

        private void portNameToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            string[] ports = SafeSerialPort.GetPortNames();

            portNameToolStripComboBox.Items.Clear();

            foreach (string port in ports)
            {
                portNameToolStripComboBox.Items.Add(port);
            }
        }

        private void portNameToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            port_name = portNameToolStripComboBox.Text;
        }

        private void baudRateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            baud_rate = Convert.ToInt32(baudRateToolStripTextBox.Text);
        }

        private void parityToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (parityToolStripComboBox.SelectedIndex)
            {
                case 0:
                    parity = Parity.Even;
                    break;
                case 1:
                    parity = Parity.Mark;
                    break;
                case 2:
                    parity = Parity.None;
                    break;
                case 3:
                    parity = Parity.Odd;
                    break;
                case 4:
                    parity = Parity.Space;
                    break;
            }
        }

        private void dataBitsToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            data_bits = Convert.ToInt32(dataBitsToolStripTextBox.Text);
        }

        private void stopBitsToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (stopBitsToolStripComboBox.SelectedIndex)
            {
                case 0:
                    stop_bits = StopBits.One;
                    break;
                case 1:
                    stop_bits = StopBits.OnePointFive;
                    break;
                case 2:
                    stop_bits = StopBits.Two;
                    break;
            }
        }

        private void readTimeoutToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            read_timeout = Convert.ToInt32(readTimeoutToolStripTextBox.Text);
        }

        private void writeTimeoutToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            write_timeout = Convert.ToInt32(writeTimeoutToolStripTextBox.Text);
        }

        private void updateIntervalToolStripMenuItem_TextChanged(object sender, EventArgs e)
        {
            update_interval = Convert.ToInt32(updateIntervalToolStripTextBox.Text);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "OpenLab - Open Source Lab Equipment Control Software\n\n";

            message += "Controls:\n";
            foreach (string control in controls.Keys)
            {
                message += "    " + control + "\n";
            }

            MessageBox.Show(message, "OpenLab");
        }

        private void updateThread()
        {
            long sleep;
            ulong time = 0;
            Stopwatch stopwatch = new Stopwatch();

            run = true;

            while (run)
            {
                stopwatch.Restart();

                try
                {
                    foreach (KeyValuePair<string, IControl> key_value_pair in controls)
                    {
                        key_value_pair.Value.update(serial_port);
                    }
                }
                catch
                {
                    MessageBox.Show("Serial port disconnected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.BeginInvoke(cleanup_delagate);
                    return;
                }

                time += (ulong)update_interval;
                sleep = update_interval - stopwatch.ElapsedMilliseconds;

                if (sleep > 0)
                {
                    Thread.Sleep((int)sleep);
                }
            }
        }

        private void cleanupForm()
        {
            run = false;
            update_thread.Join();
            serial_port.Dispose();
        }
    }
}