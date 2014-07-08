using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using ControlPlugins;

namespace OpenLab
{
    public partial class ControlForm : Form
    {
        public Dictionary<string, IControl> controls;
        public string port_name;
        public uint baud_rate;
        public ushort update_interval;
        public SerialPort serial_port;

        public ControlForm()
        {
            InitializeComponent();

            Dictionary<string, IControl> controls = new Dictionary<string, IControl>();
            ICollection<IControl> control_collection = GenericPluginLoader<IControl>.LoadPlugins("Plugins");

            if (control_collection != null)
            {
                foreach (var item in control_collection)
                {
                    controls.Add(item.name, item);
                }
            }

            this.Shown += new EventHandler(controlFormShown);
            this.FormClosing += new FormClosingEventHandler(controlFormClose);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            update_delagate = new UpdateDelagate(updateForm);
            cleanup_delagate = new CleanupDelagate(cleanupForm);
            log_file_dialog = new SaveFileDialog();
        }

        public void setMenu(bool value)
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                recurseMenu(value, item);
            }
            serialToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = !value;
        }

        private bool run = false;
        private SaveFileDialog log_file_dialog;
        private StreamWriter log_file;
        private Thread update_thread;
        private delegate void UpdateDelagate(bool pump, int pressure, bool hv, int voltage, int current, int count);
        private delegate void CleanupDelagate();
        private UpdateDelagate update_delagate;
        private CleanupDelagate cleanup_delagate;

        private void controlFormShown(object sender, EventArgs e)
        {
            log_file_dialog.Filter = "CSV (*.csv)|*.csv";
            log_file_dialog.DefaultExt = "csv";

            foreach (ToolStripMenuItem item in updateIntervalToolStripMenuItem.DropDownItems)
            {
                item.Click += new EventHandler(updateIntervalToolStripMenuItem_Click);

                if (Convert.ToInt32(item.Tag) == update_interval)
                {
                    item.Checked = true;
                }
            }

            refreshPorts(false);
            setControls(false);
            setMenu(true);
        }

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

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (port_name == null)
            {
                settingsToolStripMenuItem.ShowDropDown();
                portToolStripMenuItem.ShowDropDown();
                portToolStripMenuItem.Select();
                MessageBox.Show("No serial port selected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (update_interval == 0)
            {
                settingsToolStripMenuItem.ShowDropDown();
                updateIntervalToolStripMenuItem.ShowDropDown();
                updateIntervalToolStripMenuItem.Select();
                MessageBox.Show("No interval selected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            serial_port = new SerialPort(port_name, 115200);
            serial_port.ReadTimeout = 1000;
            serial_port.WriteTimeout = 1000;

            try
            {
                serial_port.Open();
            }
            catch
            {
                MessageBox.Show("Serial device not found.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (File.Exists(log_file_dialog.FileName))
            {
                if (MessageBox.Show("The current log file aready exists.\nDo you want to overwrite it?", "OpenLab", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    if (log_file_dialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }

            try
            {
                log_file = new StreamWriter(log_file_dialog.FileName);
            }
            catch
            {
                MessageBox.Show("Error opening log file.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            setControls(true);
            setMenu(false);

            update_thread = new Thread(updateThread);
            update_thread.Start();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanupForm();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshPorts(true);
        }

        private void portToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem port = (ToolStripMenuItem)sender;

            port_name = port.Tag.ToString();

            foreach (ToolStripMenuItem item in portToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }

            port.Checked = true;
        }

        private void updateIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem interval = (ToolStripMenuItem)sender;

            update_interval = Convert.ToUInt16(interval.Tag);

            foreach (ToolStripMenuItem item in updateIntervalToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }

            interval.Checked = true;
        }

        private void refreshPorts(bool value)
        {
            bool found = false;
            string[] ports = SerialPort.GetPortNames();
            ToolStripMenuItem[] items = new ToolStripMenuItem[ports.Length + 1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "refreshToolStripMenuItem";
            items[0].Text = "Refresh";
            items[0].ShortcutKeys = Keys.Control | Keys.R;
            items[0].Click += new EventHandler(refreshToolStripMenuItem_Click);

            for (int i = 1; i < items.Length; i++)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = "portToolStripMenuItem" + i;
                items[i].Text = ports[i - 1];
                items[i].Tag = ports[i - 1];
                items[i].Click += new EventHandler(portToolStripMenuItem_Click);

                if (ports[i - 1] == port_name)
                {
                    items[i].Checked = true;
                    found = true;
                }
            }

            if (!found)
            {
                port_name = null;
            }

            portToolStripMenuItem.DropDownItems.Clear();
            portToolStripMenuItem.DropDownItems.AddRange(items);

            if (value)
            {
                settingsToolStripMenuItem.ShowDropDown();
                portToolStripMenuItem.ShowDropDown();
            }
        }

        private void setControls(bool value)
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = value;
            }
            menuStrip1.Enabled = true;
        }

        private void recurseMenu(bool value, ToolStripMenuItem item)
        {
            if (item.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem child_item in item.DropDownItems)
                {
                    recurseMenu(value, child_item);
                }
            }
            item.Enabled = value;
        }

        private void updateThread()
        {
            ushort sleep;
            ulong time = 0;
            Stopwatch stopwatch = new Stopwatch();

            run = true;

            while (run)
            {
                stopwatch.Restart();

                try
                {

                    //this.BeginInvoke(update_delagate, ...);

                    //log_file.WriteLine(...);
                }
                catch
                {
                    MessageBox.Show("Serial port disconnected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.BeginInvoke(cleanup_delagate);
                    return;
                }

                time += (ulong)update_interval;
                sleep = (ushort)(update_interval - stopwatch.ElapsedMilliseconds);

                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
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
            setControls(false);
            setMenu(true);
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
    }
}