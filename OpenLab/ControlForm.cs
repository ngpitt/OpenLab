using OpenLab.Lib;
using OpenLab.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenLab
{
    public partial class ControlForm : Form
    {
        public override string Text
        {
            set
            {
                base.Text = value;
                FormLabelTextBox.Text = value;
            }
        }
        public ICollection<IControlPlugin> ControlPlugins = new List<IControlPlugin>();
        public ICollection<ILoggingPlugin> LoggingPlugins = new List<ILoggingPlugin>();
        public ICollection<Board> Boards = new List<Board>();
        public Board Board
        {
            get
            {
                return Boards.First(b => b.Name == SetBoardComboBox.Text);
            }
            set
            {
                SetBoardComboBox.Text = value.Name;
            }
        }

        public ControlForm()
        {
            Load += new EventHandler(ControlForm_Load);
            FormClosing += new FormClosingEventHandler(ControlForm_FormClosing);
            Resize += new EventHandler(ControlForm_Resize);
        }

        public void SerialWrite(string Message)
        {
            try
            {
                SerialPort.WriteLine(Message);
            }
            catch
            {
                ShowError($"Serial port {SerialPort.PortName} disconnected.");
            }
        }

        public void LoadPlugins()
        {
            foreach (var dllPath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
            {
                try
                {
                    foreach (var type in Assembly.Load(AssemblyName.GetAssemblyName(dllPath)).GetTypes())
                    {
                        var pluginVersion = type.Assembly.GetName().Version.Major;

                        if (pluginVersion != GetType().Assembly.GetName().Version.Major)
                        {
                            throw new Exception($"This plugin requires OpenLab v{pluginVersion}.");
                        }

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
                catch (Exception Ex)
                {
                    ShowError($"Error loading plugin {dllPath}. {Ex.Message}");
                }
            }
        }

        public void LoadBoards()
        {
            foreach (var boardPath in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Boards"), "*.brd"))
            {
                try
                {
                    Boards.Add(Board.FromConfig(XElement.Load(boardPath), "http://www.xphysics.net/OpenLab/Board"));
                }
                catch (Exception Ex)
                {
                    ShowError($"Error loading board {boardPath}. {Ex.Message}");
                }
            }

            SetBoardComboBox.Items.AddRange(Boards.Select(b => b.Name).ToArray());
        }

        public void FromConfig(XElement Config, XNamespace Ns)
        {
            var configVersion = Convert.ToInt32(Config.Element(Ns + "version").Value.Split('.')[0]);
            var boardName = Config.Element(Ns + "board").Value;

            if (configVersion != GetType().Assembly.GetName().Version.Major)
            {
                ShowError($"This config requires OpenLab v{configVersion}.");
                return;
            }

            if (!Boards.Any(b => b.Name == boardName))
            {
                ShowError($"This config requires board {boardName}.");
                return;
            }

            foreach (var controlConfig in Config.Descendants(Ns + "control"))
            {
                var controlType = controlConfig.Element(Ns + "type").Value;
                var controlVersion = Convert.ToInt32(controlConfig.Element(Ns + "version").Value.Split('.')[0]);
                var controlPlugin = ControlPlugins.FirstOrDefault(x => x.GetType().Assembly.GetName().Name == controlType);

                if (controlVersion != controlPlugin?.GetType().Assembly.GetName().Version.Major)
                {
                    ShowError($"This config requires plugin {controlType} v{controlVersion}.");
                    return;
                }
            }

            Text = Config.Element(Ns + "label").Value;
            Width = Convert.ToInt32(Config.Element(Ns + "width").Value);
            Height = Convert.ToInt32(Config.Element(Ns + "height").Value);
            Board = Boards.First(b => b.Name == boardName);

            foreach (var groupConfig in Config.Descendants(Ns + "group"))
            {
                Controls.Add(Group.FromConfig(ControlPlugins, Board, LoggingEnabled, groupConfig, Ns));
            }
        }

        public XElement ToConfig(XNamespace Ns)
        {
            var config = new XElement(Ns + "config",
                new XElement(Ns + "version", GetType().Assembly.GetName().Version),
                new XElement(Ns + "label", Text),
                new XElement(Ns + "width", Width),
                new XElement(Ns + "height", Height),
                new XElement(Ns + "board", Board.Name));

            foreach (var group in Controls.OfType<Group>())
            {
                config.Add(group.ToConfig(Ns));
            }

            return config;
        }

        private SerialPort SerialPort = new SerialPort();
        private int UpdateInterval
        {
            get
            {
                return Convert.ToInt32(UpdateIntervalTextBox.Text);
            }
            set
            {
                UpdateIntervalTextBox.Text = value.ToString();
            }
        }
        private bool LoggingEnabled
        {
            get
            {
                return LoggingMenuItem.Enabled;
            }
        }
        private bool LoggingFileSet
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SaveLogDialog.FileName);
            }
        }
        private bool ConfigFileSet
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SaveConfigDialog.FileName);
            }
        }
        private ILoggingPlugin LoggingPlugin
        {
            get
            {
                return LoggingPlugins.First(p => p.Extension == Path.GetExtension(SaveLogDialog.FileName));
            }
        }
        private bool Editing
        {
            get
            {
                return !EditConfigMenuItem.Enabled;
            }
            set
            {
                EditConfigMenuItem.Enabled = !value;
                SaveConfigAsMenuItem.Enabled = value;
                SaveConfigMenuItem.Enabled = value && ConfigFileSet;
                ConnectMenuItem.Enabled = !value;
                ContextMenuStrip.Enabled = value;

                foreach (var group in Controls.OfType<Group>())
                {
                    group.Editing = value;
                }

                if (value)
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    SaveConfigMenuItem.Enabled = !string.IsNullOrWhiteSpace(SaveConfigDialog.FileName);
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    SaveConfigAsMenuItem.Enabled = false;
                }
            }
        }
        private bool Connected
        {
            set
            {
                NewConfigMenuItem.Enabled = !value;
                OpenConfigMenuItem.Enabled = !value;
                EditConfigMenuItem.Enabled = !value;
                ConnectMenuItem.Enabled = !value;
                DisconnectMenuItem.Enabled = value;
                SerialSettingsEnabled = !value;
                SaveLogAsMenuItem.Enabled = !value;
                ControlsEnabled = value;
            }
        }
        private bool SerialSettingsEnabled
        {
            set
            {
                PortNameMenuItem.Enabled = value;
                BaudRateMenuItem.Enabled = value;
                ParityMenuItem.Enabled = value;
                DataBitsMenuItem.Enabled = value;
                StopBitsMenuItem.Enabled = value;
                ReadTimeoutMenuItem.Enabled = value;
                WriteTimeoutMenuItem.Enabled = value;
                UpdateIntervalMenuItem.Enabled = value;
            }
        }
        private bool ControlsEnabled
        {
            set
            {
                foreach (var group in Controls.OfType<Group>())
                {
                    group.Enabled = value;

                    foreach (var control in group.Controls.OfType<Lib.Control>())
                    {
                        control.Enabled = value;
                    }
                }
            }
        }
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
        private ToolStripMenuItem SetBoardMenuItem = new ToolStripMenuItem("Set Board");
        private ToolStripComboBox SetBoardComboBox = new ToolStripComboBox("Set Board");
        private ToolStripMenuItem AddGroupMenuItem = new ToolStripMenuItem("Add Group");

        private DialogResult ShowError(string Message)
        {
            return MessageBox.Show(Message, "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult ShowConfirmation(string Message)
        {
            return MessageBox.Show(Message, "OpenLab", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        private void OpenConfig(string FilePath)
        {
            try
            {
                FromConfig(XElement.Load(FilePath), "http://www.xphysics.net/OpenLab/Config");
                Editing = false;
                SaveConfigDialog.FileName = FilePath;
            }
            catch
            {
                ShowError($"Error opening config file {FilePath}.");
            }
        }

        private void SaveConfig(string FileName)
        {
            if (Controls.OfType<Group>().Any(g => g.Controls.OfType<Lib.Control>().Any(c => string.IsNullOrWhiteSpace(c.Pin))))
            {
                ShowError("One or more controls are not assigned pins.");
                return;
            }

            try
            {
                ToConfig("http://www.xphysics.net/OpenLab/Config").Save(FileName);
                Editing = false;
            }
            catch
            {
                ShowError($"Error saving config file {FileName}.");
            }
        }

        private bool ResetForm()
        {
            if (!EditConfigMenuItem.Enabled)
            {
                if (ShowConfirmation("You have unsaved changes. Do you want to discard them?") == DialogResult.No)
                {
                    return false;
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
            Editing = false;

            return true;
        }

        private void SaveFileAs()
        {
            if (SaveConfigDialog.ShowDialog() == DialogResult.OK)
            {
                SaveConfig(SaveConfigDialog.FileName);
            }
        }

        private void UpdateBackgroundWorker_DoWork(object Sender, DoWorkEventArgs DoWorkEventArgs)
        {
            int sleep;
            var controls = new List<Lib.Control>();
            var values = new List<string>();
            var stopwatch = new Stopwatch();

            try
            {
                SerialPort.Open();
            }
            catch
            {
                ShowError($"Error opening serial port {SerialPort.PortName}.");
                return;
            }

            if (LoggingFileSet)
            {
                var fields = new List<string>();

                fields.Add("Time");
                fields.AddRange(Controls.OfType<Group>().SelectMany(g => g.Controls.OfType<Lib.Control>().Select(c => c.Text.Get())));

                try
                {
                    LoggingPlugins.First(x => x.Extension == Path.GetExtension(SaveLogDialog.FileName))
                        .Open(SaveLogDialog.FileName, fields);
                }
                catch
                {
                    ShowError($"Error opening log file {SaveLogDialog.FileName}.");
                    return;
                }
            }

            foreach (var group in Controls.OfType<Group>())
            {
                foreach (var control in group.Controls.OfType<Lib.Control>())
                {
                    control.Initialize(SerialPort);
                    controls.Add(control);
                }
            }

            stopwatch.Start();

            while (!UpdateBackgroundWorker.CancellationPending)
            {
                values.Add(stopwatch.ElapsedMilliseconds.ToString());

                foreach (var control in controls)
                {
                    try
                    {
                        var value = control.Value.Get().Trim(' ', '\r', '\n');

                        BeginInvoke(control.SetValueDelagate, value);

                        if (control.Log)
                        {
                            values.Add(value);
                        }
                    }
                    catch
                    {
                        ShowError($"Serial port {SerialPort.PortName} disconnected.");
                        return;
                    }
                }

                if (LoggingFileSet)
                {
                    try
                    {
                        LoggingPlugin.Write(values);
                    }
                    catch
                    {
                        ShowError($"Error writing log file {SaveLogDialog.FileName}.");
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

            if (LoggingFileSet)
            {
                try
                {
                    LoggingPlugin.Close();
                }
                catch
                {
                    ShowError($"Error writing log to {SaveLogDialog.FileName}.");
                }

                SaveLogDialog.FileName = null;
            }

            Connected = false;
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
            SaveConfigAsMenuItem.Click += new EventHandler(SaveFileAsMenuItem_Click);
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
                SetBoardMenuItem,
                AddGroupMenuItem
            });

            FormLabelMenuItem.DropDownItems.Add(FormLabelTextBox);
            FormLabelTextBox.TextChanged += new EventHandler(FormLabelTextBox_TextChanged);
            SetBoardMenuItem.DropDownItems.Add(SetBoardComboBox);
            AddGroupMenuItem.Click += new EventHandler(AddGroupMenuItem_Click);

            OpenConfigDialog.Filter = "OpenLab (*.olc)|*.olc";
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

            LoadPlugins();

            if (!ControlPlugins.Any())
            {
                ShowError("No control plugins found.");
                Close();
                return;
            }

            LoadBoards();

            if (!Boards.Any())
            {
                ShowError("No board configs found.");
                Close();
                return;
            }

            SaveLogDialog.Filter = string.Join("|", LoggingPlugins.Select(
                x => $"{x.GetType().Assembly.GetName().Name} (*{x.Extension})|*{x.Extension}"));
            LoggingMenuItem.Enabled = LoggingPlugins.Any();
            Board = Boards.First();

            UpdateBackgroundWorker.DoWork += new DoWorkEventHandler(UpdateBackgroundWorker_DoWork);
            UpdateBackgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(UpdateBackgroundWorker_RunWorkerCompleted);
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
                    if (ShowConfirmation("The serial port is currently in use. Do you want to quit?") == DialogResult.Yes)
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
                    if (ShowConfirmation("You have unsaved changes. Do you want to quit?") == DialogResult.No)
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

        private void ControlForm_Resize(object Sender, EventArgs EventArgs)
        {
            Size = Grid.NearestNode(Size);
        }

        private void NewFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            ResetForm();
        }

        private void OpenFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (OpenConfigDialog.ShowDialog() == DialogResult.OK)
            {
                if (ResetForm())
                {
                    OpenConfig(OpenConfigDialog.FileName);
                }
            }
        }

        private void EditFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            Editing = true;
        }

        private void SaveFileMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            if (!ConfigFileSet)
            {
                SaveFileAs();
            }
            else
            {
                SaveConfig(SaveConfigDialog.FileName);
            }
        }

        private void SaveFileAsMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            SaveFileAs();
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
            Connected = true;
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
            CancelEventArgs.Cancel = !ContextMenuStrip.Enabled;
            ContextMenuLocation = PointToClient(Cursor.Position);
        }

        private void FormLabelTextBox_TextChanged(object Sender, EventArgs EventArgs)
        {
            base.Text = FormLabelTextBox.Text;
        }

        private void AddGroupMenuItem_Click(object Sender, EventArgs EventArgs)
        {
            var group = Group.FromLocation(ControlPlugins, Board, LoggingEnabled, ContextMenuLocation);

            Controls.Add(group);
            group.BringToFront();
            group.Editing = true;
        }
    }
}
