using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using Tags = System.Collections.Generic.Dictionary<string, string>;

namespace OpenLab
{
    public partial class ControlForm : Form
    {
        public Control menuSource
        {
            get
            {
                return menu_source;
            }
        }

        public ControlForm()
        {
            InitializeComponent();

            FormClosing += new FormClosingEventHandler(controlForm_FormClosing);
            KeyDown += new KeyEventHandler(controlForm_KeyDown);
            groupContextMenuStrip.Opening += groupContextMenuStrip_Opening;
            controlContextMenuStrip.Opening += controlContextMenuStrip_Opening;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            disconnectToolStripMenuItem.Enabled = false;

            config = new XmlDocument();
            load_file_dialog = new OpenFileDialog();
            save_file_dialog = new SaveFileDialog();
            cleanup_delagate = new CleanupDelagate(cleanup);
            control_plugins = new Dictionary<string, ControlPlugin>();

            load_file_dialog.Filter = save_file_dialog.Filter = "OpenLab Config (*.olc)|*.olc";
            portNameToolStripComboBox.Text = port_name = Settings.Default.port_name;
            baud_rate = Settings.Default.baud_rate;
            baudRateToolStripTextBox.Text = Convert.ToString(baud_rate);
            parity = Settings.Default.parity;
            switch (parity)
            {
                case Parity.Even:
                    parityToolStripComboBox.SelectedIndex = 0;
                    break;
                case Parity.Mark:
                    parityToolStripComboBox.SelectedIndex = 1;
                    break;
                case Parity.None:
                    parityToolStripComboBox.SelectedIndex = 2;
                    break;
                case Parity.Odd:
                    parityToolStripComboBox.SelectedIndex = 3;
                    break;
                case Parity.Space:
                    parityToolStripComboBox.SelectedIndex = 4;
                    break;
            }
            data_bits = Settings.Default.data_bits;
            dataBitsToolStripTextBox.Text = Convert.ToString(data_bits);
            stop_bits = Settings.Default.stop_bits;
            switch (stop_bits)
            {
                case StopBits.One:
                    stopBitsToolStripComboBox.SelectedIndex = 0;
                    break;
                case StopBits.OnePointFive:
                    stopBitsToolStripComboBox.SelectedIndex = 1;
                    break;
                case StopBits.Two:
                    stopBitsToolStripComboBox.SelectedIndex = 2;
                    break;
            }
            read_timeout = Settings.Default.read_timeout;
            readTimeoutToolStripTextBox.Text = Convert.ToString(read_timeout);
            write_timeout = Settings.Default.write_timeout;
            writeTimeoutToolStripTextBox.Text = Convert.ToString(write_timeout);
            update_interval = Settings.Default.update_interval;
            updateIntervalToolStripTextBox.Text = Convert.ToString(update_interval);

            string binary_directory = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

            ICollection<ControlPlugin> control_plugin_collection = PluginLoader<ControlPlugin>.LoadPlugins(binary_directory + "\\Plugins", this);
            if (control_plugin_collection != null)
            {
                foreach (ControlPlugin control_plugin in control_plugin_collection)
                {
                    control_plugins.Add(control_plugin.name, control_plugin);
                }
            }

            ToolStripMenuItem group_label = new ToolStripMenuItem();
            ToolStripTextBox text_box = new ToolStripTextBox();
            ToolStripMenuItem remove_group = new ToolStripMenuItem();

            text_box.TextChanged += new EventHandler(groupLabelToolStripTextBox_TextChanged);

            group_label.Text = "Group Label";
            group_label.DropDownItems.Add(text_box);

            remove_group.Text = "Remove Group";
            remove_group.Click += new EventHandler(removeGroupToolStripMenuItem_Click);

            groupContextMenuStrip.Items.Add(group_label);
            foreach (KeyValuePair<string, ControlPlugin> control_plugin in control_plugins)
            {
                ToolStripMenuItem menu_item = new ToolStripMenuItem();
                menu_item.Text = "Add " + control_plugin.Key;
                menu_item.Tag = control_plugin.Value;
                menu_item.Click += new System.EventHandler(addControlToolStripMenuItem_Click);
                groupContextMenuStrip.Items.Add(menu_item);
            }
            groupContextMenuStrip.Items.Add(remove_group);

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                open(Environment.GetCommandLineArgs()[1]);
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
                cleanup();
            }
        }

        private Dictionary<string, ControlPlugin> control_plugins;
        private XmlDocument config;
        private OpenFileDialog load_file_dialog;
        private SaveFileDialog save_file_dialog;
        private string port_name, file_name;
        private int baud_rate, data_bits, update_interval, read_timeout, write_timeout;
        private Parity parity;
        private StopBits stop_bits;
        private SafeSerialPort serial_port;
        private bool editing = false, run = false, mouse_down = false, resizing = false;
        private Thread update_thread;
        private delegate void CleanupDelagate();
        private CleanupDelagate cleanup_delagate;
        private Control menu_source, mouse_over, clipboard;
        private Point form_click_location, control_click_location;
        private Size group_size;

        private List<T> get<T>(Control parent) where T : Control
        {
            List<T> controls = new List<T>();

            foreach (var control in parent.Controls)
            {
                if (control.GetType() == typeof(T))
                {
                    controls.Add(control as T);
                }
            }

            return controls;
        }

        private void open(string file_name)
        {
            int x, y, width, height;

            this.file_name = file_name;
            config.Load(file_name);

            XmlNodeList dependency_nodes = config["config"].GetElementsByTagName("dependency");
            foreach (XmlNode dependency_node in dependency_nodes)
            {
                if (!control_plugins.ContainsKey(dependency_node.InnerText))
                {
                    MessageBox.Show("This config requires the plugin \"" + dependency_node.InnerText + "\".", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!editing)
            {
                Text = config["config"]["name"].InnerText;
                Width = Convert.ToInt32(config["config"]["width"].InnerText);
                Height = Convert.ToInt32(config["config"]["height"].InnerText);
            }

            XmlNodeList group_nodes = config["config"].GetElementsByTagName("group");
            foreach (XmlNode group_node in group_nodes)
            {
                GroupBox group = new GroupBox();

                x = Convert.ToInt32(group_node["x"].InnerText);
                y = Convert.ToInt32(group_node["y"].InnerText);
                width = Convert.ToInt32(group_node["width"].InnerText);
                height = Convert.ToInt32(group_node["height"].InnerText);

                group.Text = group_node["label"].InnerText;
                group.Location = new Point(x, y);
                group.Size = new Size(width, height);
                if (editing)
                {
                    editGroup(group);
                }
                else
                {
                    group.Enabled = false;
                }
                Controls.Add(group);

                XmlElement group_element = group_node as XmlElement;
                XmlNodeList control_nodes = group_element.GetElementsByTagName("control");
                foreach (XmlNode control_node in control_nodes)
                {
                    FlowLayoutPanel control = control_plugins[control_node["plugin"].InnerText].create(control_node);
                    if (editing)
                    {
                        editControl(control);
                    }
                    group.Controls.Add(control);
                }
            }
        }

        private void edit()
        {
            editing = true;
            editToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            FormBorderStyle = FormBorderStyle.Sizable;
            ContextMenuStrip = formContextMenuStrip;
            foreach (GroupBox group in get<GroupBox>(this))
            {
                editGroup(group);
                foreach (FlowLayoutPanel control in get<FlowLayoutPanel>(group))
                {
                    editControl(control);
                }
            }
        }

        private void editGroup(GroupBox group)
        {
            group.Enabled = true;
            group.ContextMenuStrip = groupContextMenuStrip;
            group.MouseMove += new MouseEventHandler(group_MouseMove);
            group.MouseDown += new MouseEventHandler(group_MouseDown);
            group.MouseUp += new MouseEventHandler(group_MouseUp);
            group.MouseEnter += new EventHandler(group_MouseEnter);
            group.MouseLeave += new EventHandler(group_MouseLeave);
        }

        private void editControl(FlowLayoutPanel control)
        {
            foreach (Control element in control.Controls)
            {
                element.Enabled = false;
            }
            control.BorderStyle = BorderStyle.FixedSingle;
            control.ContextMenuStrip = controlContextMenuStrip;
            control.MouseMove += new MouseEventHandler(control_MouseMove);
            control.MouseDown += new MouseEventHandler(control_MouseDown);
            control.MouseUp += new MouseEventHandler(control_MouseUp);
            control.MouseEnter += new EventHandler(control_MouseEnter);
            control.MouseLeave += new EventHandler(control_MouseLeave);
        }

        private void saveGroup(GroupBox group)
        {
            group.Enabled = false;
            group.ContextMenuStrip = null;
            group.MouseMove -= new MouseEventHandler(group_MouseMove);
            group.MouseDown -= new MouseEventHandler(group_MouseDown);
            group.MouseUp -= new MouseEventHandler(group_MouseUp);
            group.MouseEnter += new EventHandler(group_MouseEnter);
            group.MouseLeave += new EventHandler(group_MouseLeave);
        }

        private void saveControl(FlowLayoutPanel control)
        {
            foreach (Control element in control.Controls)
            {
                element.Enabled = true;
            }
            control.BorderStyle = BorderStyle.None;
            control.ContextMenuStrip = null;
            control.MouseMove -= new MouseEventHandler(control_MouseMove);
            control.MouseDown -= new MouseEventHandler(control_MouseDown);
            control.MouseUp -= new MouseEventHandler(control_MouseUp);
            control.MouseEnter -= new EventHandler(control_MouseEnter);
            control.MouseLeave -= new EventHandler(control_MouseLeave);
        }

        private bool saveFileDialog()
        {
            if (save_file_dialog.ShowDialog() == DialogResult.Cancel)
            {
                return false;
            }
            file_name = save_file_dialog.FileName;

            return true;
        }

        private void update()
        {
            long sleep;
            ulong time = 0;
            List<FlowLayoutPanel> controls = new List<FlowLayoutPanel>();
            Stopwatch stopwatch = new Stopwatch();

            foreach (GroupBox group in get<GroupBox>(this))
            {
                foreach (FlowLayoutPanel control in get<FlowLayoutPanel>(group))
                {
                    controls.Add(control);
                }
            }

            run = true;
            while (run)
            {
                stopwatch.Restart();
                try
                {
                    foreach (FlowLayoutPanel control in controls)
                    {
                        Tags tags = control.Tag as Tags;
                        control_plugins[tags["plugin"]].update(control, serial_port);
                    }
                }
                catch
                {
                    MessageBox.Show("Serial port disconnected.", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BeginInvoke(cleanup_delagate);
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

        private void cleanup()
        {
            run = false;
            update_thread.Join();
            serial_port.Dispose();

            newToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = true;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            foreach (GroupBox group in get<GroupBox>(this))
            {
                group.Enabled = false;
            }
        }

        private void groupContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            menu_source = (sender as ContextMenuStrip).SourceControl;
            ToolStripMenuItem control_label = groupContextMenuStrip.Items[0] as ToolStripMenuItem;
            control_label.DropDownItems[0].Text = menu_source.Text;
        }

        private void controlContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ToolStripMenuItem control_title = new ToolStripMenuItem();
            ToolStripTextBox text_box = new ToolStripTextBox();
            ToolStripMenuItem remove_control = new ToolStripMenuItem();
            menu_source = (sender as ContextMenuStrip).SourceControl;
            Tags tags = menu_source.Tag as Tags;

            controlContextMenuStrip.Items.Clear();

            text_box.Text = tags["label"];
            text_box.TextChanged += new EventHandler(controlLabelToolStripTextBox_TextChanged);

            control_title.Text = "Label";
            control_title.DropDownItems.Add(text_box);

            remove_control.Text = "Remove";
            remove_control.Click += new EventHandler(removeControlToolStripMenuItem_Click);

            controlContextMenuStrip.Items.Add(control_title);
            foreach (ToolStripMenuItem menu_item in control_plugins[tags["plugin"]].settings(menu_source as FlowLayoutPanel))
            {
                controlContextMenuStrip.Items.Add(menu_item);
            }
            controlContextMenuStrip.Items.Add(remove_control);
        }

        private void controlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (run)
            {
                if (MessageBox.Show("The serial port is currently connected.\nDo you really want to quit?", "OpenLab", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cleanup();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            Settings.Default.port_name = port_name;
            Settings.Default.baud_rate = baud_rate;
            Settings.Default.parity = parity;
            Settings.Default.data_bits = data_bits;
            Settings.Default.stop_bits = stop_bits;
            Settings.Default.read_timeout = read_timeout;
            Settings.Default.write_timeout = write_timeout;
            Settings.Default.update_interval = update_interval;
            Settings.Default.Save();
        }

        private void controlForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (editing)
            {
                if (mouse_over != null)
                {
                    if (e.Control && e.KeyCode == Keys.C)
                    {
                        clipboard = mouse_over;
                    }
                    else if (e.Control && e.KeyCode == Keys.V && clipboard != null)
                    {
                        if (clipboard.GetType() == typeof(FlowLayoutPanel))
                        {
                            Control parent = mouse_over;
                            if (mouse_over.GetType() != typeof(GroupBox))
                            {
                                parent = mouse_over.Parent;
                            }
                            FlowLayoutPanel control = clipboard as FlowLayoutPanel;
                            Tags tags = control.Tag as Tags;
                            FlowLayoutPanel control_copy = control_plugins[tags["plugin"]].copy(control);
                            control_copy.Location = parent.PointToClient(Cursor.Position);
                            editControl(control_copy);
                            parent.Controls.Add(control_copy);
                            control_copy.BringToFront();
                        }
                    }
                    else
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                                mouse_over.Location = new Point(mouse_over.Location.X - 1, mouse_over.Location.Y);
                                break;
                            case Keys.Right:
                                mouse_over.Location = new Point(mouse_over.Location.X + 1, mouse_over.Location.Y);
                                break;
                            case Keys.Up:
                                mouse_over.Location = new Point(mouse_over.Location.X, mouse_over.Location.Y - 1);
                                break;
                            case Keys.Down:
                                mouse_over.Location = new Point(mouse_over.Location.X, mouse_over.Location.Y + 1);
                                break;
                            case Keys.Delete:
                                if (mouse_over.GetType() == typeof(GroupBox))
                                {
                                    Controls.Remove(mouse_over);
                                }
                                else if (mouse_over.GetType() == typeof(FlowLayoutPanel))
                                {
                                    mouse_over.Parent.Controls.Remove(mouse_over);
                                }
                                break;
                        }
                    }
                }
                if (e.Control && e.KeyCode == Keys.V && clipboard != null)
                {
                    if (clipboard.GetType() == typeof(GroupBox))
                    {
                        GroupBox group = clipboard as GroupBox;
                        GroupBox group_copy = new GroupBox();
                        group_copy.Text = group.Text;
                        group_copy.Location = PointToClient(Cursor.Position);
                        group_copy.Size = group.Size;
                        foreach (FlowLayoutPanel control in group.Controls)
                        {
                            Tags tags = control.Tag as Tags;
                            FlowLayoutPanel control_copy = control_plugins[tags["plugin"]].copy(control);
                            editControl(control_copy);
                            group_copy.Controls.Add(control_copy);
                        }
                        editGroup(group_copy);
                        Controls.Add(group_copy);
                        group_copy.BringToFront();
                    }
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GroupBox group in get<GroupBox>(this))
            {
                Controls.Remove(group);
            }
            Text = "OpenLab";
            Width = 800;
            Height = 600;
            file_name = null;
            edit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (load_file_dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            open(load_file_dialog.FileName);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HashSet<string> dependencies = new HashSet<string>();

            if (file_name == null)
            {
                if (!saveFileDialog())
                {
                    return;
                }
            }

            editing = false;
            editToolStripMenuItem.Enabled = true;
            connectToolStripMenuItem.Enabled = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ContextMenuStrip = null;
            foreach (GroupBox group in get<GroupBox>(this))
            {
                saveGroup(group);
                foreach (FlowLayoutPanel control in get<FlowLayoutPanel>(group))
                {
                    saveControl(control);
                }
            }

            config.RemoveAll();

            XmlComment comment = config.CreateComment("OpenLab Config");
            config.AppendChild(comment);

            XmlNode config_node = config.CreateElement("config");
            config.AppendChild(config_node);

            XmlNode name_node = config.CreateElement("name");
            name_node.InnerText = Text;
            config_node.AppendChild(name_node);

            XmlNode width_node = config.CreateElement("width");
            width_node.InnerText = Convert.ToString(Width);
            config_node.AppendChild(width_node);

            XmlNode height_node = config.CreateElement("height");
            height_node.InnerText = Convert.ToString(Height);
            config_node.AppendChild(height_node);

            foreach (GroupBox group in get<GroupBox>(this))
            {
                XmlNode group_node = config.CreateElement("group");

                XmlNode group_label_node = config.CreateElement("label");
                group_label_node.InnerText = group.Text;
                group_node.AppendChild(group_label_node);

                XmlNode group_x_node = config.CreateElement("x");
                group_x_node.InnerText = Convert.ToString(group.Location.X);
                group_node.AppendChild(group_x_node);

                XmlNode group_y_node = config.CreateElement("y");
                group_y_node.InnerText = Convert.ToString(group.Location.Y);
                group_node.AppendChild(group_y_node);

                XmlNode group_width_node = config.CreateElement("width");
                group_width_node.InnerText = Convert.ToString(group.Width);
                group_node.AppendChild(group_width_node);

                XmlNode group_height_node = config.CreateElement("height");
                group_height_node.InnerText = Convert.ToString(group.Height);
                group_node.AppendChild(group_height_node);

                foreach (FlowLayoutPanel control in group.Controls)
                {
                    Tags tags = control.Tag as Tags;
                    XmlDocument control_config = control_plugins[tags["plugin"]].save(control);
                    XmlNode imported_node = config.ImportNode(control_config.DocumentElement, true);
                    group_node.AppendChild(imported_node);
                    dependencies.Add(tags["plugin"]);
                }
                config_node.AppendChild(group_node);
            }

            foreach (string plugin in dependencies)
            {
                XmlNode dependency_node = config.CreateElement("dependency");
                dependency_node.InnerText = plugin;
                config_node.InsertBefore(dependency_node, name_node);
            }

            config.Save(file_name);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (port_name == "" || baud_rate <= 0 || data_bits <= 0 || update_interval <= 0 || read_timeout <= 0 || write_timeout <= 0)
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

            update_thread = new Thread(update);
            update_thread.Start();

            newToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            editToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = false;
            foreach (GroupBox group in get<GroupBox>(this))
            {
                group.Enabled = true;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanup();
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
            try
            {
                baud_rate = Convert.ToInt32(baudRateToolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
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
            try
            {
                data_bits = Convert.ToInt32(dataBitsToolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
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
            try
            {
                read_timeout = Convert.ToInt32(readTimeoutToolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void writeTimeoutToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                write_timeout = Convert.ToInt32(writeTimeoutToolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void updateIntervalToolStripMenuItem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                update_interval = Convert.ToInt32(updateIntervalToolStripTextBox.Text);
            }
            catch
            {
                // Ignore exception
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "OpenLab - Open Source Lab Equipment Control Software\nVersion 1.0\n\n";

            message += "Control Plugins:\n";
            foreach (string plugin_name in control_plugins.Keys)
            {
                message += "    \u2022  " + plugin_name + "\n";
            }
            MessageBox.Show(message, "OpenLab");
        }


        private void editFormContextMenuStrip_Opened(object sender, EventArgs e)
        {
            ContextMenuStrip context_menu = sender as ContextMenuStrip;
            Point absolute_click_location = Cursor.Position;
            control_click_location = context_menu.SourceControl.PointToClient(absolute_click_location);
        }

        private void formTitleToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            Text = formNameToolStripTextBox.Text;
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupBox group = new GroupBox();

            group.Text = "Group";
            group.Location = control_click_location;
            group.Size = new Size(300, 100);
            group.ContextMenuStrip = groupContextMenuStrip;
            group.MouseMove += new MouseEventHandler(group_MouseMove);
            group.MouseDown += new MouseEventHandler(group_MouseDown);
            group.MouseUp += new MouseEventHandler(group_MouseUp);
            group.MouseEnter += new EventHandler(group_MouseEnter);
            group.MouseLeave += new EventHandler(group_MouseLeave);
            Controls.Add(group);
        }

        private void group_MouseMove(object sender, MouseEventArgs e)
        {
            GroupBox group = sender as GroupBox;
            Point screen_location = Cursor.Position, form_cursor_location, group_cursor_location;
            form_cursor_location = PointToClient(screen_location);
            group_cursor_location = group.PointToClient(screen_location);
            if (group_cursor_location.Y > group.Height - 10 && group_cursor_location.X > group.Width - 10 || resizing)
            {
                Cursor.Current = Cursors.SizeNWSE;
                if (mouse_down)
                {
                    resizing = true;
                    group.Height = group_size.Height + form_cursor_location.Y - form_click_location.Y;
                    group.Width = group_size.Width + form_cursor_location.X - form_click_location.X;
                }
            }
            else
            {
                if (mouse_down)
                {
                    Cursor.Current = Cursors.Default;
                    group.Location = new Point(form_cursor_location.X - control_click_location.X, form_cursor_location.Y - control_click_location.Y);
                }
                else
                {
                    Cursor.Current = Cursors.SizeAll;
                }
            }
        }

        private void group_MouseDown(object sender, MouseEventArgs e)
        {
            GroupBox group = sender as GroupBox;
            Cursor.Current = Cursors.SizeAll;
            Point screen_location = Cursor.Position;
            form_click_location = PointToClient(screen_location);
            control_click_location = e.Location;
            group_size = group.Size;
            mouse_down = true;
        }

        private void group_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            resizing = false;
        }

        private void group_MouseEnter(object sender, EventArgs e)
        {
            mouse_over = sender as GroupBox;
        }

        private void group_MouseLeave(object sender, EventArgs e)
        {
            mouse_over = null;
        }

        private void groupLabelToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            if (menu_source != null)
            {
                menu_source.Text = text_box.Text;
            }
        }

        private void addControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            ControlPlugin control_plugin = menu_item.Tag as ControlPlugin;
            FlowLayoutPanel control = control_plugin.create(new Point(control_click_location.X, control_click_location.Y));
            control.BorderStyle = BorderStyle.FixedSingle;
            control.ContextMenuStrip = controlContextMenuStrip;
            control.MouseMove += new MouseEventHandler(control_MouseMove);
            control.MouseDown += new MouseEventHandler(control_MouseDown);
            control.MouseUp += new MouseEventHandler(control_MouseUp);
            control.MouseEnter += new EventHandler(control_MouseEnter);
            control.MouseLeave += new EventHandler(control_MouseLeave);
            foreach (Control element in control.Controls)
            {
                element.Enabled = false;
            }
            menu_source.Controls.Add(control);
        }

        private void removeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controls.Remove(menu_source);
        }

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            FlowLayoutPanel control = sender as FlowLayoutPanel;
            Cursor.Current = Cursors.SizeAll;
            if (mouse_down)
            {
                Cursor.Current = Cursors.Default;
                Point screen_location = Cursor.Position, group_location;
                group_location = control.Parent.PointToClient(screen_location);
                control.Location = new Point(group_location.X - control_click_location.X, group_location.Y - control_click_location.Y);
            }
            else
            {
                Cursor.Current = Cursors.SizeAll;
            }
        }

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
            Point screen_location = Cursor.Position;
            form_click_location = PointToClient(screen_location);
            control_click_location = e.Location;
            mouse_down = true;
        }

        private void control_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
        }

        private void control_MouseEnter(object sender, EventArgs e)
        {
            mouse_over = sender as FlowLayoutPanel;
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            mouse_over = null;
        }

        private void controlLabelToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            if (menu_source != null)
            {
                FlowLayoutPanel control = menu_source as FlowLayoutPanel;
                Tags tags = control.Tag as Tags;
                tags["label"] = text_box.Text;
                control.Controls[0].Text = text_box.Text + ": ";
            }
        }

        private void removeControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel control = menu_source as FlowLayoutPanel;
            control.Parent.Controls.Remove(menu_source);
        }
    }
}
