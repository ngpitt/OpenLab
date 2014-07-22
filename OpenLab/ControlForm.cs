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
        public Control menuSource()
        {
            return menu_source;
        }

        public ControlForm()
        {
            InitializeComponent();

            FormClosing += new FormClosingEventHandler(controlFormClose);
            editGroupBoxContextMenuStrip.Opening += editGroupBoxContextMenuStrip_Opening;
            editControlContextMenuStrip.Opening += editControlContextMenuStrip_Opening;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            saveAsToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = false;

            config = new XmlDocument();
            load_file_dialog = new OpenFileDialog();
            save_file_dialog = new SaveFileDialog();
            cleanup_delagate = new CleanupDelagate(cleanupForm);
            controls = new Dictionary<string, IControl>();
            group_boxes = new List<GroupBox>();
            panels = new List<Panel>();

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

            ICollection<IControl> control_collection = GenericPluginLoader<IControl>.LoadPlugins("Plugins");
            if (control_collection != null)
            {
                foreach (var control in control_collection)
                {
                    controls.Add(control.name, control);
                    control.init(this);
                }
            }

            foreach (KeyValuePair<string, IControl> control in controls)
            {
                ToolStripMenuItem menu_item = new ToolStripMenuItem();
                menu_item.Name = "add" + control.Key.ToLower() + "ControlToolStripMenuItem";
                menu_item.Text = control.Key;
                menu_item.Tag = control.Value;
                menu_item.Click += new System.EventHandler(addControlToolStripMenuItem_Click);
                addControlToolStripMenuItem.DropDownItems.Add(menu_item);
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
        private XmlDocument config;
        private OpenFileDialog load_file_dialog;
        private SaveFileDialog save_file_dialog;
        private List<GroupBox> group_boxes;
        private List<Panel> panels;
        private string port_name;
        private int baud_rate, data_bits, update_interval, read_timeout, write_timeout;
        private Parity parity;
        private StopBits stop_bits;
        private SafeSerialPort serial_port;
        private bool edit = false, run = false, mouse_down = false, resize = false;
        private Thread update_thread;
        private delegate void CleanupDelagate();
        private CleanupDelagate cleanup_delagate;
        private Control menu_source;
        private Point form_click_location, control_click_location;
        private Size group_box_size;

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

        private void editGroupBoxContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            menu_source = (sender as ContextMenuStrip).SourceControl;
            groupBoxTitleToolStripTextBox.Text = menu_source.Text;
        }

        private void editControlContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            menu_source = (sender as ContextMenuStrip).SourceControl;
            Tags tags = menu_source.Tag as Tags;
            controlTitleToolStripTextBox.Text = tags["text"];
            controlSettingsToolStripMenuItem.DropDownItems.Clear();
            foreach (ToolStripMenuItem menu_item in controls[tags["name"]].settings())
            {
                controlSettingsToolStripMenuItem.DropDownItems.Add(menu_item);
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

            XmlNodeList dependency_nodes = config["config"].GetElementsByTagName("dependency");
            foreach (XmlNode dependency_node in dependency_nodes)
            {
                if (!controls.ContainsKey(dependency_node.InnerText))
                {
                    MessageBox.Show("This config requires the plugin \"" + dependency_node.InnerText + "\".", "OpenLab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Text = config["config"]["text"].InnerText;
            if (!edit)
            {
                Width = Convert.ToInt32(config["config"]["width"].InnerText);
                Height = Convert.ToInt32(config["config"]["height"].InnerText);
            }

            XmlNodeList node_list = config["config"].GetElementsByTagName("groupbox");
            foreach (XmlNode node in node_list)
            {
                GroupBox group_box = new GroupBox();

                x = Convert.ToInt32(node["x"].InnerText);
                y = Convert.ToInt32(node["y"].InnerText);
                width = Convert.ToInt32(node["width"].InnerText);
                height = Convert.ToInt32(node["height"].InnerText);

                group_box.Text = node["text"].InnerText;
                group_box.Location = new Point(x, y);
                group_box.Size = new Size(width, height);
                if (edit)
                {
                    group_box.ContextMenuStrip = editGroupBoxContextMenuStrip;
                    group_box.MouseMove += new MouseEventHandler(editGroupBox_MouseMove);
                    group_box.MouseDown += new MouseEventHandler(editGroupBox_MouseDown);
                    group_box.MouseUp += new MouseEventHandler(editGroupBox_MouseUp);
                }
                else
                {
                    group_box.Enabled = false;
                }
                Controls.Add(group_box);

                XmlElement group_box_element = node as XmlElement;
                XmlNodeList control_nodes = group_box_element.GetElementsByTagName("control");
                foreach (XmlNode control_node in control_nodes)
                {
                    FlowLayoutPanel panel = controls[control_node["name"].InnerText].add(group_box, control_node);
                    if (edit)
                    {
                        foreach (Control control in panel.Controls)
                        {
                            control.Enabled = false;
                        }
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        panel.ContextMenuStrip = editControlContextMenuStrip;
                        panel.MouseMove += new MouseEventHandler(editControl_MouseMove);
                        panel.MouseDown += new MouseEventHandler(editControl_MouseDown);
                        panel.MouseUp += new MouseEventHandler(editControl_MouseUp);
                    }
                    panels.Add(panel);
                }

                group_boxes.Add(group_box);
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GroupBox group_box in group_boxes)
            {
                Controls.Remove(group_box);
            }
            group_boxes.Clear();
            panels.Clear();
            foreach (KeyValuePair<string, IControl> control in controls)
            {
                control.Value.reset();
            }
            Width = 800;
            Height = 600;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = true;
            editToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = true;
            serialToolStripMenuItem.Enabled = false;
            FormBorderStyle = FormBorderStyle.Sizable;
            ContextMenuStrip = editFormContextMenuStrip;
            foreach (GroupBox group_box in group_boxes)
            {
                group_box.Enabled = true;
                group_box.ContextMenuStrip = editGroupBoxContextMenuStrip;
                group_box.MouseMove += new MouseEventHandler(editGroupBox_MouseMove);
                group_box.MouseDown += new MouseEventHandler(editGroupBox_MouseDown);
                group_box.MouseUp += new MouseEventHandler(editGroupBox_MouseUp);
            }
            foreach (FlowLayoutPanel panel in panels)
            {
                foreach (Control control in panel.Controls)
                {
                    control.Enabled = false;
                }
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.ContextMenuStrip = editControlContextMenuStrip;
                panel.MouseMove += new MouseEventHandler(editControl_MouseMove);
                panel.MouseDown += new MouseEventHandler(editControl_MouseDown);
                panel.MouseUp += new MouseEventHandler(editControl_MouseUp);
            }
        }

        private void editFormContextMenuStrip_Opened(object sender, EventArgs e)
        {
            ContextMenuStrip context_menu = sender as ContextMenuStrip;
            Point absolute_click_location = Cursor.Position;
            control_click_location = context_menu.SourceControl.PointToClient(absolute_click_location);
        }

        private void formTitleToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            Text = formTitleToolStripTextBox.Text;
        }

        private void addGroupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupBox group_box = new GroupBox();

            group_box.Text = "New Group Box";
            group_box.Location = control_click_location;
            group_box.Size = new Size(300, 100);
            group_box.ContextMenuStrip = editGroupBoxContextMenuStrip;
            group_box.MouseMove += new MouseEventHandler(editGroupBox_MouseMove);
            group_box.MouseDown += new MouseEventHandler(editGroupBox_MouseDown);
            group_box.MouseUp += new MouseEventHandler(editGroupBox_MouseUp);
            Controls.Add(group_box);
            group_boxes.Add(group_box);
        }

        private void editGroupBox_MouseMove(object sender, MouseEventArgs e)
        {
            GroupBox group_box = sender as GroupBox;
            Point screen_location = Cursor.Position, form_cursor_location, group_box_cursor_location;
            form_cursor_location = PointToClient(screen_location);
            group_box_cursor_location = group_box.PointToClient(screen_location);
            if (group_box_cursor_location.Y > group_box.Height - 10 && group_box_cursor_location.X > group_box.Width - 10 || resize)
            {
                Cursor.Current = Cursors.SizeNWSE;
                if (mouse_down)
                {
                    resize = true;
                    group_box.Height = group_box_size.Height + form_cursor_location.Y - form_click_location.Y;
                    group_box.Width = group_box_size.Width + form_cursor_location.X - form_click_location.X;
                }
            }
            else
            {
                Cursor.Current = Cursors.SizeAll;
                if (mouse_down)
                {
                    group_box.Location = new Point(form_cursor_location.X - control_click_location.X, form_cursor_location.Y - control_click_location.Y);
                }
            }
        }

        private void editGroupBox_MouseDown(object sender, MouseEventArgs e)
        {
            GroupBox group_box = sender as GroupBox;
            Point screen_location = Cursor.Position;
            form_click_location = PointToClient(screen_location);
            control_click_location = e.Location;
            group_box_size = group_box.Size;
            mouse_down = true;
        }

        private void editGroupBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            resize = false;
        }

        private void groupBoxTitleToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            GroupBox group_box = menu_source as GroupBox;
            if (group_box != null)
            {
                group_box.Text = text_box.Text;
            }
        }

        private void addControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            IControl icontrol = menu_item.Tag as IControl;
            GroupBox group_box = menu_source as GroupBox;
            FlowLayoutPanel panel = icontrol.add(group_box, new Point(control_click_location.X, control_click_location.Y));
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.ContextMenuStrip = editControlContextMenuStrip;
            panel.MouseMove += new MouseEventHandler(editControl_MouseMove);
            panel.MouseDown += new MouseEventHandler(editControl_MouseDown);
            panel.MouseUp += new MouseEventHandler(editControl_MouseUp);
            foreach (Control control in panel.Controls)
            {
                control.Enabled = false;
            }
            panels.Add(panel);
        }

        private void removeGroupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupBox group_box = menu_source as GroupBox;
            group_boxes.Remove(group_box);
            Controls.Remove(group_box);
        }

        private void editControl_MouseMove(object sender, MouseEventArgs e)
        {
            FlowLayoutPanel panel = sender as FlowLayoutPanel;
            Cursor.Current = Cursors.SizeAll;
            if (mouse_down)
            {
                Point screen_location = Cursor.Position, group_box_location;
                group_box_location = panel.Parent.PointToClient(screen_location);
                panel.Location = new Point(group_box_location.X - control_click_location.X, group_box_location.Y - control_click_location.Y);
            }
        }

        private void editControl_MouseDown(object sender, MouseEventArgs e)
        {
            Point screen_location = Cursor.Position;
            form_click_location = PointToClient(screen_location);
            control_click_location = e.Location;
            mouse_down = true;
        }

        private void editControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
        }

        private void controlTitleToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox text_box = sender as ToolStripTextBox;
            if (menu_source != null)
            {
                FlowLayoutPanel panel = menu_source as FlowLayoutPanel;
                Tags tags = panel.Tag as Tags;
                tags["text"] = text_box.Text;
                panel.Controls[0].Text = text_box.Text + ": ";
            }
        }

        private void removeControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            FlowLayoutPanel panel = menu_source as FlowLayoutPanel;
            panels.Remove(panel);
            panel.Parent.Controls.Remove(menu_source);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (GroupBox group_box in group_boxes)
            {
                group_box.Enabled = false;
                group_box.ContextMenuStrip = null;
                group_box.MouseMove -= new MouseEventHandler(editGroupBox_MouseMove);
                group_box.MouseDown -= new MouseEventHandler(editGroupBox_MouseDown);
                group_box.MouseUp -= new MouseEventHandler(editGroupBox_MouseUp);
            }
            foreach (FlowLayoutPanel panel in panels)
            {
                panel.BorderStyle = BorderStyle.None;
                panel.ContextMenuStrip = null;
                panel.MouseMove -= new MouseEventHandler(editControl_MouseMove);
                panel.MouseDown -= new MouseEventHandler(editControl_MouseDown);
                panel.MouseUp -= new MouseEventHandler(editControl_MouseUp);
                foreach (Control control in panel.Controls)
                {
                    control.Enabled = true;
                }
            }
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ContextMenuStrip = null;
            editToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = false;
            serialToolStripMenuItem.Enabled = true;
            edit = false;

            save_file_dialog.Filter = "OpenLab Config (*.olc)|*.olc";
            if (save_file_dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            config.RemoveAll();

            XmlNode config_node = config.CreateElement("config");
            config.AppendChild(config_node);

            XmlNode text_node = config.CreateElement("text");
            text_node.InnerText = Text;
            config_node.AppendChild(text_node);

            XmlNode width_node = config.CreateElement("width");
            width_node.InnerText = Convert.ToString(Width);
            config_node.AppendChild(width_node);

            XmlNode height_node = config.CreateElement("height");
            height_node.InnerText = Convert.ToString(Height);
            config_node.AppendChild(height_node);

            foreach (GroupBox group_box in group_boxes)
            {
                XmlNode group_node = config.CreateElement("groupbox");

                XmlNode group_box_text_node = config.CreateElement("text");
                group_box_text_node.InnerText = group_box.Text;
                group_node.AppendChild(group_box_text_node);

                XmlNode group_box_x_node = config.CreateElement("x");
                group_box_x_node.InnerText = Convert.ToString(group_box.Location.X);
                group_node.AppendChild(group_box_x_node);

                XmlNode group_box_y_node = config.CreateElement("y");
                group_box_y_node.InnerText = Convert.ToString(group_box.Location.Y);
                group_node.AppendChild(group_box_y_node);

                XmlNode group_box_width_node = config.CreateElement("width");
                group_box_width_node.InnerText = Convert.ToString(group_box.Width);
                group_node.AppendChild(group_box_width_node);

                XmlNode group_box_height_node = config.CreateElement("height");
                group_box_height_node.InnerText = Convert.ToString(group_box.Height);
                group_node.AppendChild(group_box_height_node);

                foreach (KeyValuePair<string, IControl> control in controls)
                {
                    XmlDocument control_config = control.Value.save(group_box);
                    if (control_config != null)
                    {
                        XmlNodeList control_nodes = control_config["config"].GetElementsByTagName("control");
                        foreach (XmlNode control_node in control_nodes)
                        {
                            XmlNode imported_node = config.ImportNode(control_node, true);
                            group_node.AppendChild(imported_node);
                        }

                        XmlNode dependency_node = config.CreateElement("dependency");
                        dependency_node.InnerText = control.Key;
                        config_node.AppendChild(dependency_node);
                    }
                }
                config_node.AppendChild(group_node);
            }
            config.Save(save_file_dialog.FileName);
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

            update_thread = new Thread(updateThread);
            update_thread.Start();

            fileToolStripMenuItem.Enabled = false;
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = false;
            foreach (GroupBox group_box in group_boxes)
            {
                group_box.Enabled = true;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanupForm();
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
            foreach (string control in controls.Keys)
            {
                message += "    \u2022  " + control + "\n";
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
                    foreach (KeyValuePair<string, IControl> control in controls)
                    {
                        control.Value.update(serial_port);
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

        private void cleanupForm()
        {
            run = false;
            update_thread.Join();
            serial_port.Dispose();

            fileToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = true;
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            foreach (GroupBox group_box in group_boxes)
            {
                group_box.Enabled = false;
            }
        }
    }
}
