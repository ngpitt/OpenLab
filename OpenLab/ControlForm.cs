using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.IO.Ports;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using OpenLab.Lib;
using OpenLab.Properties;

namespace OpenLab
{
    public partial class ControlForm : Form
    {
        public List<IControlPlugin> ControlPlugins = new List<IControlPlugin>();
        public List<ILoggingPlugin> LoggingPlugins = new List<ILoggingPlugin>();
        private SerialPort SerialPort = new SerialPort();
        private int UpdateInterval;
        private OpenFileDialog OpenConfigDialog = new OpenFileDialog();
        private SaveFileDialog SaveConfigDialog = new SaveFileDialog(), SaveLogDialog = new SaveFileDialog();
        private BackgroundWorker UpdateBackgroundWorker = new BackgroundWorker();
        private Point ContextMenuLocation;

        private ToolStripMenuItem ConfigMenuItem = new ToolStripMenuItem("Config");
        private ToolStripMenuItem NewConfigMenuItem = new ToolStripMenuItem("New");
        private ToolStripMenuItem OpenConfigMenuItem = new ToolStripMenuItem("Open...");
        private ToolStripMenuItem EditConfigMenuItem = new ToolStripMenuItem("Edit");
        private ToolStripMenuItem SaveConfigMenuItem = new ToolStripMenuItem("Save");
        private ToolStripMenuItem SaveConfigAsMenuItem = new ToolStripMenuItem("Save As...");
        private ToolStripMenuItem QuitMenuItem = new ToolStripMenuItem("Quit");

        private ToolStripMenuItem SerialMenuItem = new ToolStripMenuItem("Serial");
        private ToolStripMenuItem ConnectMenuItem = new ToolStripMenuItem("Connect");
        private ToolStripMenuItem DisconnectMenuItem = new ToolStripMenuItem("Disconnect");
        private ToolStripMenuItem PortNameMenuItem = new ToolStripMenuItem("Port Name");
        private ToolStripComboBox PortNameComboBox = new ToolStripComboBox("Port Name");
        private ToolStripMenuItem BaudRateMenuItem = new ToolStripMenuItem("Baud Rate");
        private ToolStripTextBox BaudRateTextBox = new ToolStripTextBox("Baud Rate");
        private ToolStripMenuItem ParityMenuItem = new ToolStripMenuItem("Parity");
        private ToolStripComboBox ParityComboBox = new ToolStripComboBox("Parity");
        private ToolStripMenuItem DataBitsMenuItem = new ToolStripMenuItem("Data Bits");
        private ToolStripTextBox DataBitsTextBox = new ToolStripTextBox("Data Bits");
        private ToolStripMenuItem StopBitsMenuItem = new ToolStripMenuItem("Stop Bits");
        private ToolStripComboBox StopBitsComboBox = new ToolStripComboBox("Stop Bits");
        private ToolStripMenuItem ReadTimeoutMenuItem = new ToolStripMenuItem("Read Timeout");
        private ToolStripTextBox ReadTimeoutTextBox = new ToolStripTextBox("Read Timeout");
        private ToolStripMenuItem WriteTimeoutMenuItem = new ToolStripMenuItem("Write Timeout");
        private ToolStripTextBox WriteTimeoutTextBox = new ToolStripTextBox("Write Timeout");
        private ToolStripMenuItem UpdateIntervalMenuItem = new ToolStripMenuItem("Update Interval");
        private ToolStripTextBox UpdateIntervalTextBox = new ToolStripTextBox("Update Interval");

        private ToolStripMenuItem LoggingMenuItem = new ToolStripMenuItem("Logging");
        private ToolStripMenuItem SaveLogAsMenuItem = new ToolStripMenuItem("Save As...");

        private ToolStripMenuItem AddControlMenuItem = new ToolStripMenuItem("Add Control");

        private ToolStripMenuItem FormLabelMenuItem = new ToolStripMenuItem("Form Label");
        private ToolStripTextBox FormLabelTextBox = new ToolStripTextBox("Form Label");
        private ToolStripMenuItem AddGroupMenuItem = new ToolStripMenuItem("Add Group");

        public ControlForm()
        {
            Load += new EventHandler(ControlForm_Load);
            FormClosing += new FormClosingEventHandler(ControlForm_FormClosing);
        }

        public void SerialWrite(string Message)
        {
            try
            {
                SerialPort.WriteLine(Message);
            }
            catch
            {
                ShowError("Serial port {0} disconnected.", SerialPort.PortName);
            }
        }

        private DialogResult ShowError(string Format, params object[] Args)
        {
            return MessageBox.Show(string.Format(Format, Args), GetType().Assembly.GetName().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult ShowConfirmation(string Format, params object[] Args)
        {
            return MessageBox.Show(string.Format(Format, Args), GetType().Assembly.GetName().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        public void LoadPlugins()
        {
            foreach (var dllPath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
            {
                try
                {
                    foreach (var type in Assembly.Load(AssemblyName.GetAssemblyName(dllPath)).GetTypes())
                    {
                        if (type.GetInterface(typeof(IControlPlugin).FullName) != null)
                        {
                            ControlPlugins.Add((IControlPlugin)Activator.CreateInstance(type));
                        }
                        else if (type.GetInterface(typeof(ILoggingPlugin).FullName) != null)
                        {
                            LoggingPlugins.Add((ILoggingPlugin)Activator.CreateInstance(type));
                        }
                    }
                }
                catch
                {
                    throw new Exception(string.Format("Error loading plugin {0}.", dllPath));
                }
            }
        }

        private void OpenConfig(string FileName)
        {
            try
            {
                NewFileMenuItem_Click(null, null);
                FromConfig(XElement.Load(FileName));
                DisableEdit();
                SaveConfigDialog.FileName = FileName;
            }
            catch
            {
                ShowError("Error opening config file {0}.", FileName);
            }
        }

        public void FromConfig(XElement Config)
        {
            var configType = Config.Element("type").Value;
            var configVersion = Convert.ToInt32(Config.Element("version").Value.Split('.')[0]);

            if (configType != GetType().Assembly.GetName().Name || configVersion != GetType().Assembly.GetName().Version.Major)
            {
                ShowError("This config requires {0} (v{1})", configType, configVersion);
                return;
            }

            foreach (var controlConfig in Config.Descendants("control"))
            {
                var controlType = controlConfig.Element("type").Value;
                var controlVersion = Convert.ToInt32(controlConfig.Element("version").Value.Split('.')[0]);
                var controlPlugin = ControlPlugins.FirstOrDefault(x => x.GetType().Assembly.GetName().Name == controlType);

                if (controlVersion != controlPlugin?.GetType().Assembly.GetName().Version.Major)
                {
                    ShowError("This config requires plugin {0} (v{1}).", controlType, controlVersion);
                    return;
                }
            }

            Text = Config.Element("label").Value;
            Width = Convert.ToInt32(Config.Element("width").Value);
            Height = Convert.ToInt32(Config.Element("height").Value);

            foreach (var groupConfig in Config.Descendants("group"))
            {
                Controls.Add(Group.FromConfig(ControlPlugins, groupConfig, LoggingPlugins.Any()));
            }
        }

        private void SaveConfig(string FileName)
        {
            try
            {
                GetConfig().Save(FileName);
                DisableEdit();
            }
            catch
            {
                ShowError("Error saving config file {0}.", FileName);
            }
        }

        public XElement GetConfig()
        {
            var config =
                new XElement("config",
                    new XElement("type", GetType().Assembly.GetName().Name),
                    new XElement("version", GetType().Assembly.GetName().Version.ToString()),
                    new XElement("label", Text),
                    new XElement("width", Width),
                    new XElement("height", Height)
                );

            foreach (var group in Controls.OfType<Group>())
            {
                config.Add(group.GetConfig());
            }

            return config;
        }

        private void EnableEdit()
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            OpenConfigMenuItem.Enabled = false;
            EditConfigMenuItem.Enabled = false;
            SaveConfigAsMenuItem.Enabled = true;
            SaveConfigMenuItem.Enabled = !string.IsNullOrWhiteSpace(SaveConfigDialog.FileName);
            ConnectMenuItem.Enabled = false;
            ContextMenuStrip.Enabled = true;

            foreach (var group in Controls.OfType<Group>())
            {
                group.EnableEdit();
            }
        }

        private void DisableEdit()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            OpenConfigMenuItem.Enabled = true;
            EditConfigMenuItem.Enabled = true;
            SaveConfigMenuItem.Enabled = false;
            SaveConfigAsMenuItem.Enabled = false;
            ConnectMenuItem.Enabled = true;
            ContextMenuStrip.Enabled = false;

            foreach (var group in Controls.OfType<Group>())
            {
                group.DisableEdit();
            }
        }

        private void UpdateBackgroundWorker_DoWork(object Sender, DoWorkEventArgs DoWorkEventArgs)
        {
            int sleep;
            var controls = new List<OpenLab.Lib.Control>();
            var values = new List<string>();
            var stopwatch = new Stopwatch();
            var delegates = new HashSet<Delegate>();

            try
            {
                SerialPort.Open();
            }
            catch
            {
                ShowError("Error opening serial port {0}.", SerialPort.PortName);
                return;
            }

            if (!string.IsNullOrWhiteSpace(SaveLogDialog.FileName))
            {
                var fields = new List<string>();

                fields.Add("Time");

                foreach (var group in Controls.OfType<Group>())
                {
                    foreach (var control in group.Controls.OfType<OpenLab.Lib.Control>())
                    {
                        if (control.Log)
                        {
                            fields.Add(control.Text);
                        }
                    }
                }

                try
                {
                    LoggingPlugins.First(x => x.Extension == Path.GetExtension(SaveLogDialog.FileName)).Open(SaveLogDialog.FileName, fields);
                }
                catch
                {
                    ShowError("Error opening log file {0}.", SaveLogDialog.FileName);
                    return;
                }
            }

            foreach (var group in Controls.OfType<Group>())
            {
                foreach (var control in group.Controls.OfType<OpenLab.Lib.Control>())
                {
                    control.SetSerialPort(SerialPort);
                    controls.Add(control);
                }
            }

            stopwatch.Start();

            while (!UpdateBackgroundWorker.CancellationPending)
            {
                values.Add(stopwatch.ElapsedMilliseconds.ToString());

                foreach (var control in controls)
                {
                    var value = default(string);

                    try
                    {
                        value = control.GetValue();
                        BeginInvoke(control.SetValueDelagate, value);
                    }
                    catch
                    {
                        ShowError("Serial port {0} disconnected", SerialPort.PortName);
                        return;
                    }

                    if (control.Log)
                    {
                        values.Add(value.TrimEnd('\r', '\n'));
                    }
                }

                if (!string.IsNullOrWhiteSpace(SaveLogDialog.FileName))
                {
                    try
                    {
                        LoggingPlugins.First(x => x.Extension == Path.GetExtension(SaveLogDialog.FileName)).Write(values);
                    }
                    catch
                    {
                        ShowError("Error writing log file {0}.", SaveLogDialog.FileName);
                        return;
                    }
                }

                values.Clear();
                sleep = UpdateInterval - (int)(stopwatch.ElapsedMilliseconds % UpdateInterval);

                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
            }
        }

        private void UpdateBackgroundWorker_RunWorkerCompleted(object Sender, RunWorkerCompletedEventArgs RunWorkerCompletedEventArgs)
        {
            SerialPort.Close();

            if (!string.IsNullOrWhiteSpace(SaveLogDialog.FileName))
            {
                try
                {
                    LoggingPlugins.First(x => x.Extension == Path.GetExtension(SaveLogDialog.FileName)).Close();
                }
                catch
                {
                    ShowError("Error writing log to {0}.", SaveLogDialog.FileName);
                }

                SaveLogDialog.FileName = null;
            }

            NewConfigMenuItem.Enabled = true;
            OpenConfigMenuItem.Enabled = true;
            EditConfigMenuItem.Enabled = true;
            ConnectMenuItem.Enabled = true;
            DisconnectMenuItem.Enabled = false;
            SerialSettingsEnabled(true);
            SaveLogAsMenuItem.Enabled = true;
            ControlsEnabled(false);
        }

        private void ControlForm_Load(object Sender, EventArgs EventArgs)
        {
            Text = "OpenLab";
            Width = 800;
            Height = 600;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = false;
            MaximizeBox = false;

            MainMenuStrip = new MenuStrip();
            Controls.Add(MainMenuStrip);

            NewConfigMenuItem.Click += new EventHandler(NewFileMenuItem_Click);
            OpenConfigMenuItem.Click += new EventHandler(OpenFileMenuItem_Click);
            EditConfigMenuItem.Click += new EventHandler(EditFileMenuItem_Click);
            SaveConfigMenuItem.Click += new EventHandler(SaveFileMenuItem_Click);
            SaveConfigAsMenuItem.Click += new EventHandler(SaveConfigAsMenuItem_Click);
            QuitMenuItem.Click += new EventHandler(QuitMenuItem_Click);

            ConnectMenuItem.Click += new EventHandler(ConnectMenuItem_Click);
            DisconnectMenuItem.Click += new EventHandler(DisconnectMenuItem_Click);
            PortNameMenuItem.DropDownItems.Add(PortNameComboBox);
            PortNameComboBox.SelectedIndexChanged += new EventHandler(PortNameComboBox_SelectedIndexChanged);
            BaudRateMenuItem.DropDownItems.Add(BaudRateTextBox);
            BaudRateTextBox.TextChanged += new EventHandler(BaudRateTextBox_TextChanged);
            ParityMenuItem.DropDownItems.Add(ParityComboBox);
            ParityComboBox.SelectedIndexChanged += new EventHandler(ParityComboBox_SelectedIndexChanged);
            DataBitsMenuItem.DropDownItems.Add(DataBitsTextBox);
            DataBitsTextBox.TextChanged += new EventHandler(DataBitsTextBox_TextChanged);
            StopBitsMenuItem.DropDownItems.Add(StopBitsComboBox);
            StopBitsComboBox.SelectedIndexChanged += new EventHandler(StopBitsComboBox_SelectedIndexChanged);
            ReadTimeoutMenuItem.DropDownItems.Add(ReadTimeoutTextBox);
            ReadTimeoutTextBox.TextChanged += new EventHandler(ReadTimeoutTextBox_TextChanged);
            WriteTimeoutMenuItem.DropDownItems.Add(WriteTimeoutTextBox);
            WriteTimeoutTextBox.TextChanged += new EventHandler(WriteTimeoutTextBox_TextChanged);
            UpdateIntervalMenuItem.DropDownItems.Add(UpdateIntervalTextBox);
            UpdateIntervalTextBox.TextChanged += new EventHandler(UpdateIntervalMenuItem_TextChanged);

            SaveLogAsMenuItem.Click += new EventHandler(SaveLogAsMenuItem_Click);

            ConfigMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                NewConfigMenuItem,
                OpenConfigMenuItem,
                EditConfigMenuItem,
                SaveConfigMenuItem,
                SaveConfigAsMenuItem,
                new ToolStripSeparator(),
                QuitMenuItem
            });

            SerialMenuItem.DropDownOpening += new EventHandler(SerialMenuItem_DropDownOpening);
            SerialMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                ConnectMenuItem,
                DisconnectMenuItem,
                new ToolStripSeparator(),
                PortNameMenuItem,
                BaudRateMenuItem,
                ParityMenuItem,
                DataBitsMenuItem,
                StopBitsMenuItem,
                ReadTimeoutMenuItem,
                WriteTimeoutMenuItem,
                UpdateIntervalMenuItem
            });

            LoggingMenuItem.DropDownItems.Add(SaveLogAsMenuItem);

            MainMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                ConfigMenuItem,
                SerialMenuItem,
                LoggingMenuItem
            });

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ContextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                FormLabelMenuItem,
                AddGroupMenuItem
            });

            FormLabelMenuItem.DropDownItems.Add(FormLabelTextBox);
            FormLabelTextBox.TextChanged += new EventHandler(FormLabelTextBox_TextChanged);
            AddGroupMenuItem.Click += new EventHandler(AddGroupMenuItem_Click);

            OpenConfigDialog.Filter = string.Format("{0} (*{1})|*{1}", GetType().Assembly.GetName().Name, ".olc");
            SaveConfigDialog.Filter = OpenConfigDialog.Filter;

            SaveConfigMenuItem.Enabled = false;
            SaveConfigAsMenuItem.Enabled = false;
            DisconnectMenuItem.Enabled = false;
            ContextMenuStrip.Enabled = false;

            SerialPort.PortName = Settings.Default.PortName;
            SerialPort.BaudRate = Settings.Default.BaudRate;
            SerialPort.Parity = Settings.Default.Parity;
            SerialPort.DataBits = Settings.Default.DataBits;
            SerialPort.StopBits = Settings.Default.StopBits;
            SerialPort.ReadTimeout = Settings.Default.ReadTimeout;
            SerialPort.WriteTimeout = Settings.Default.WriteTimeout;
            UpdateInterval = Settings.Default.UpdateInterval;

            var currentPluginPath = string.Empty;

            try
            {
                LoadPlugins();
            }
            catch (Exception Ex)
            {
                ShowError(Ex.Message);
            }

            SaveLogDialog.Filter = string.Join("|", LoggingPlugins.Select(x => string.Format("{0} (*{1})|*{1}", x.GetType().Assembly.GetName().Name, x.Extension)));
            LoggingMenuItem.Enabled = LoggingPlugins.Any();

            UpdateBackgroundWorker.DoWork += new DoWorkEventHandler(UpdateBackgroundWorker_DoWork);
            UpdateBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateBackgroundWorker_RunWorkerCompleted);
            UpdateBackgroundWorker.WorkerSupportsCancellation = true;

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                OpenConfig(Environment.GetCommandLineArgs()[1]);
            }
        }

        private void ControlForm_FormClosing(object Sender, FormClosingEventArgs EventArgs)
        {
            if (!EditConfigMenuItem.Enabled)
            {
                if (UpdateBackgroundWorker.IsBusy)
                {
                    if (ShowConfirmation("The serial port is currently in use.\nDo you want to quit?") == DialogResult.Yes)
                    {
                        UpdateBackgroundWorker.CancelAsync();
                    }
                    else
                    {
                        EventArgs.Cancel = true;
                        return;
                    }
                }
                else
                {
                    if (ShowConfirmation("You have unsaved changes.\nDo you want to quit?") == DialogResult.No)
                    {
                        EventArgs.Cancel = true;
                        return;
                    }
                }
            }

            Settings.Default.PortName = SerialPort.PortName;
            Settings.Default.BaudRate = SerialPort.BaudRate;
            Settings.Default.Parity = SerialPort.Parity;
            Settings.Default.DataBits = SerialPort.DataBits;
            Settings.Default.StopBits = SerialPort.StopBits;
            Settings.Default.ReadTimeout = SerialPort.ReadTimeout;
            Settings.Default.WriteTimeout = SerialPort.WriteTimeout;
            Settings.Default.UpdateInterval = UpdateInterval;
            Settings.Default.Save();
        }

        private void NewFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (!EditConfigMenuItem.Enabled)
            {
                if (ShowConfirmation("You have unsaved changes.\nDo you want to discard them?") == DialogResult.No)
                {
                    return;
                }
            }

            foreach (var group in Controls.OfType<Group>())
            {
                Controls.Remove(group);
            }

            Text = GetType().Assembly.GetName().Name;
            Width = 800;
            Height = 600;
            SaveConfigDialog.FileName = null;
            DisableEdit();
        }

        private void OpenFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (OpenConfigDialog.ShowDialog() == DialogResult.OK)
            {
                OpenConfig(OpenConfigDialog.FileName);
            }
        }

        private void EditFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            EnableEdit();
        }

        private void SaveFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (string.IsNullOrWhiteSpace(OpenConfigDialog.FileName))
            {
                SaveConfigAsMenuItem_Click(null, null);
            }
            else
            {
                SaveConfig(SaveConfigDialog.FileName);
            }
        }

        private void SaveConfigAsMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (SaveConfigDialog.ShowDialog() == DialogResult.OK)
            {
                SaveConfig(SaveConfigDialog.FileName);
            }
        }

        private void QuitMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            Close();
        }

        private void SerialMenuItem_DropDownOpening(object Sender, EventArgs EventArgs)
        {
            if (!UpdateBackgroundWorker.IsBusy)
            {
                var ports = SerialPort.GetPortNames();

                PortNameComboBox.Items.Clear();

                foreach (var port in ports)
                {
                    PortNameComboBox.Items.Add(port);
                }

                PortNameComboBox.Text = SerialPort.PortName;
                BaudRateTextBox.Text = SerialPort.BaudRate.ToString();
                ParityComboBox.Items.Clear();

                foreach (var parity in Enum.GetNames(typeof(Parity)))
                {
                    ParityComboBox.Items.Add(parity);
                }

                ParityComboBox.Text = SerialPort.Parity.ToString();
                DataBitsTextBox.Text = SerialPort.DataBits.ToString();
                StopBitsComboBox.Items.Clear();

                foreach (var stopBit in Enum.GetNames(typeof(StopBits)))
                {
                    StopBitsComboBox.Items.Add(stopBit);
                }

                StopBitsComboBox.Text = SerialPort.StopBits.ToString();
                ReadTimeoutTextBox.Text = SerialPort.ReadTimeout.ToString();
                WriteTimeoutTextBox.Text = SerialPort.WriteTimeout.ToString();
                UpdateIntervalTextBox.Text = UpdateInterval.ToString();
            }
        }

        private void ConnectMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            NewConfigMenuItem.Enabled = false;
            OpenConfigMenuItem.Enabled = false;
            EditConfigMenuItem.Enabled = false;
            ConnectMenuItem.Enabled = false;
            DisconnectMenuItem.Enabled = true;
            SerialSettingsEnabled(false);
            SaveLogAsMenuItem.Enabled = false;
            ControlsEnabled(true);

            UpdateBackgroundWorker.RunWorkerAsync();
        }

        private void DisconnectMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            UpdateBackgroundWorker.CancelAsync();
        }

        private void PortNameComboBox_SelectedIndexChanged(object Sender, EventArgs EventArgs)
        {
            SerialPort.PortName = PortNameComboBox.Text;
        }

        private void BaudRateTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            try
            {
                SerialPort.BaudRate = Convert.ToInt32(BaudRateTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void ParityComboBox_SelectedIndexChanged(object Sender, EventArgs EventArgs)
        {
            SerialPort.Parity = (Parity)ParityComboBox.SelectedIndex;
        }

        private void DataBitsTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            try
            {
                SerialPort.DataBits = Convert.ToInt32(DataBitsTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void StopBitsComboBox_SelectedIndexChanged(object Sender, EventArgs EventArgs)
        {
            SerialPort.StopBits = (StopBits)StopBitsComboBox.SelectedIndex;
        }

        private void ReadTimeoutTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            try
            {
                SerialPort.ReadTimeout = Convert.ToInt32(ReadTimeoutTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void WriteTimeoutTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            try
            {
                SerialPort.WriteTimeout = Convert.ToInt32(WriteTimeoutTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void UpdateIntervalMenuItem_TextChanged(object Sender, EventArgs EventArgs)
        {
            try
            {
                UpdateInterval = Convert.ToInt32(UpdateIntervalTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void SaveLogAsMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            SaveLogDialog.ShowDialog();
        }

        private void ContextMenuStrip_Opening(object Sender, CancelEventArgs CancelEventArgs)
        {
            if (!ContextMenuStrip.Enabled)
            {
                CancelEventArgs.Cancel = true;
                return;
            }

            ContextMenuLocation = PointToClient(Cursor.Position);
            FormLabelTextBox.Text = Text;
        }

        private void FormLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            Text = FormLabelTextBox.Text;
        }

        private void AddGroupMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            var group = Group.FromLocation(ControlPlugins, ContextMenuLocation);

            Controls.Add(group);
            group.BringToFront();
            group.EnableEdit();
        }

        private void SerialSettingsEnabled(bool Enabled)
        {
            PortNameMenuItem.Enabled = Enabled;
            BaudRateMenuItem.Enabled = Enabled;
            ParityMenuItem.Enabled = Enabled;
            DataBitsMenuItem.Enabled = Enabled;
            StopBitsMenuItem.Enabled = Enabled;
            ReadTimeoutMenuItem.Enabled = Enabled;
            WriteTimeoutMenuItem.Enabled = Enabled;
            UpdateIntervalMenuItem.Enabled = Enabled;
        }

        private void ControlsEnabled(bool Enabled)
        {
            foreach (var group in Controls.OfType<Group>())
            {
                group.Enabled = Enabled;

                foreach (var control in group.Controls.OfType<OpenLab.Lib.Control>())
                {
                    control.Enabled = Enabled;
                }
            }
        }
    }
}
