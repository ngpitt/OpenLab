using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using ControlPlugins;

namespace OpenLab
{
    public partial class ControlForm : Form
    {

        public ControlForm()
        {
            InitializeComponent();

            controls = new Dictionary<string, IControl>();
            ICollection<IControl> control_collection = GenericPluginLoader<IControl>.LoadPlugins("Plugins");

            if (control_collection != null)
            {
                foreach (var item in control_collection)
                {
                    controls.Add(item.name, item);
                }
            }

            this.FormClosing += new FormClosingEventHandler(controlFormClose);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            load_file_dialog = new OpenFileDialog();
            config = new XmlDocument();

            update_delagate = new UpdateDelagate(updateForm);
            cleanup_delagate = new CleanupDelagate(cleanupForm);
        }

        private Dictionary<string, IControl> controls;
        private List<System.Windows.Forms.Label> labels;
        private List<System.Windows.Forms.Button> buttons;
        private OpenFileDialog load_file_dialog;
        private XmlDocument config;
        private string port_name;
        private int baud_rate, data_bits, update_interval, read_timeout, write_timeout;
        private Parity parity;
        private StopBits stop_bits;
        private SafeSerialPort serial_port;
        private bool run = false;
        private Thread update_thread;
        private delegate void UpdateDelagate(bool pump, int pressure, bool hv, int voltage, int current, int count);
        private delegate void CleanupDelagate();
        private UpdateDelagate update_delagate;
        private CleanupDelagate cleanup_delagate;

        private void controlFormClose(object sender, FormClosingEventArgs e)
        {
            if (run)
            {
                if (MessageBox.Show("The program is currently running.\nDo you really want to quit?", "OpenLab", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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

            load_file_dialog.Filter = "OpenLab Config (*.olc)|*.olc";

            if (load_file_dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            config.Load(load_file_dialog.FileName);

            string xmlcontents = config.InnerXml;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanupForm();
        }

        private void portNameToolStripMenuItem_MouseHover(object sender, EventArgs e)
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

            message += "Control Plugins:\n";
            foreach (string plugin in controls.Keys)
            {
                message += "    " + plugin + "\n";
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

                    //this.BeginInvoke(update_delagate, ...);

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

        private void updateForm(bool pump, int pressure, bool hv, int voltage, int current, int count)
        {

            // Update indicators and meters

        }

        private void cleanupForm()
        {
            run = false;
            update_thread.Join();
            serial_port.Dispose();
        }

        private void serialWrite(string message)
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

        private class SafeSerialPort : SerialPort
        {
            private Stream theBaseStream;

            public SafeSerialPort(string port_name, int baud_rate, Parity parity, int data_bits, StopBits stop_bits)
                : base(port_name, baud_rate, parity, data_bits, stop_bits)
            {

            }

            public new void Open()
            {
                try
                {
                    base.Open();
                    theBaseStream = BaseStream;
                    GC.SuppressFinalize(BaseStream);
                }
                catch
                {

                }
            }

            public new void Dispose()
            {
                Dispose(true);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (base.Container != null))
                {
                    base.Container.Dispose();
                }
                try
                {
                    if (theBaseStream.CanRead)
                    {
                        theBaseStream.Close();
                        GC.ReRegisterForFinalize(theBaseStream);
                    }
                }
                catch
                {
                    // ignore exception - bug with USB - serial adapters.
                }
                base.Dispose(disposing);
            }
        }
    }
}