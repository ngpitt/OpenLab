namespace OpenLab
{
    partial class ControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portNameToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.baudRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baudRateToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.parityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parityToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.dataBitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataBitsToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.stopBitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopBitsToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.readTimeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readTimeoutToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.writeTimeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeTimeoutToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.updateIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLogAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formNameToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.controlContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainMenuStrip.SuspendLayout();
            this.formContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.serialToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 28;
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveConfigAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.ShowShortcutKeys = false;
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.ShowShortcutKeys = false;
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.ShowShortcutKeys = false;
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.ShowShortcutKeys = false;
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveConfigAsToolStripMenuItem
            // 
            this.saveConfigAsToolStripMenuItem.Name = "saveConfigAsToolStripMenuItem";
            this.saveConfigAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveConfigAsToolStripMenuItem.ShowShortcutKeys = false;
            this.saveConfigAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveConfigAsToolStripMenuItem.Text = "Save As...";
            this.saveConfigAsToolStripMenuItem.Click += new System.EventHandler(this.saveConfigAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.ShowShortcutKeys = false;
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // serialToolStripMenuItem
            // 
            this.serialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem});
            this.serialToolStripMenuItem.Name = "serialToolStripMenuItem";
            this.serialToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.serialToolStripMenuItem.Text = "Serial";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.connectToolStripMenuItem.ShowShortcutKeys = false;
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.disconnectToolStripMenuItem.ShowShortcutKeys = false;
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portNameToolStripMenuItem,
            this.baudRateToolStripMenuItem,
            this.parityToolStripMenuItem,
            this.dataBitsToolStripMenuItem,
            this.stopBitsToolStripMenuItem,
            this.readTimeoutToolStripMenuItem,
            this.writeTimeoutToolStripMenuItem,
            this.updateIntervalToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // portNameToolStripMenuItem
            // 
            this.portNameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portNameToolStripComboBox});
            this.portNameToolStripMenuItem.Name = "portNameToolStripMenuItem";
            this.portNameToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.portNameToolStripMenuItem.Text = "Port Name";
            this.portNameToolStripMenuItem.DropDownOpening += new System.EventHandler(this.portNameToolStripMenuItem_DropDownOpening);
            // 
            // portNameToolStripComboBox
            // 
            this.portNameToolStripComboBox.Name = "portNameToolStripComboBox";
            this.portNameToolStripComboBox.Size = new System.Drawing.Size(75, 23);
            this.portNameToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.portNameToolStripComboBox_SelectedIndexChanged);
            // 
            // baudRateToolStripMenuItem
            // 
            this.baudRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.baudRateToolStripTextBox});
            this.baudRateToolStripMenuItem.Name = "baudRateToolStripMenuItem";
            this.baudRateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.baudRateToolStripMenuItem.Text = "Baud Rate";
            // 
            // baudRateToolStripTextBox
            // 
            this.baudRateToolStripTextBox.MaxLength = 10;
            this.baudRateToolStripTextBox.Name = "baudRateToolStripTextBox";
            this.baudRateToolStripTextBox.Size = new System.Drawing.Size(75, 23);
            this.baudRateToolStripTextBox.TextChanged += new System.EventHandler(this.baudRateToolStripTextBox_TextChanged);
            // 
            // parityToolStripMenuItem
            // 
            this.parityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parityToolStripComboBox});
            this.parityToolStripMenuItem.Name = "parityToolStripMenuItem";
            this.parityToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.parityToolStripMenuItem.Text = "Parity";
            // 
            // parityToolStripComboBox
            // 
            this.parityToolStripComboBox.Items.AddRange(new object[] {
            "Even",
            "Mark",
            "None",
            "Odd",
            "Space"});
            this.parityToolStripComboBox.Name = "parityToolStripComboBox";
            this.parityToolStripComboBox.Size = new System.Drawing.Size(75, 23);
            this.parityToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.parityToolStripComboBox_SelectedIndexChanged);
            // 
            // dataBitsToolStripMenuItem
            // 
            this.dataBitsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataBitsToolStripTextBox});
            this.dataBitsToolStripMenuItem.Name = "dataBitsToolStripMenuItem";
            this.dataBitsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.dataBitsToolStripMenuItem.Text = "Data Bits";
            // 
            // dataBitsToolStripTextBox
            // 
            this.dataBitsToolStripTextBox.MaxLength = 10;
            this.dataBitsToolStripTextBox.Name = "dataBitsToolStripTextBox";
            this.dataBitsToolStripTextBox.Size = new System.Drawing.Size(75, 23);
            this.dataBitsToolStripTextBox.TextChanged += new System.EventHandler(this.dataBitsToolStripTextBox_TextChanged);
            // 
            // stopBitsToolStripMenuItem
            // 
            this.stopBitsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopBitsToolStripComboBox});
            this.stopBitsToolStripMenuItem.Name = "stopBitsToolStripMenuItem";
            this.stopBitsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.stopBitsToolStripMenuItem.Text = "Stop Bits";
            // 
            // stopBitsToolStripComboBox
            // 
            this.stopBitsToolStripComboBox.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.stopBitsToolStripComboBox.Name = "stopBitsToolStripComboBox";
            this.stopBitsToolStripComboBox.Size = new System.Drawing.Size(75, 23);
            this.stopBitsToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.stopBitsToolStripComboBox_SelectedIndexChanged);
            // 
            // readTimeoutToolStripMenuItem
            // 
            this.readTimeoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readTimeoutToolStripTextBox});
            this.readTimeoutToolStripMenuItem.Name = "readTimeoutToolStripMenuItem";
            this.readTimeoutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.readTimeoutToolStripMenuItem.Text = "Read Timeout (ms)";
            // 
            // readTimeoutToolStripTextBox
            // 
            this.readTimeoutToolStripTextBox.MaxLength = 10;
            this.readTimeoutToolStripTextBox.Name = "readTimeoutToolStripTextBox";
            this.readTimeoutToolStripTextBox.Size = new System.Drawing.Size(75, 23);
            this.readTimeoutToolStripTextBox.TextChanged += new System.EventHandler(this.readTimeoutToolStripTextBox_TextChanged);
            // 
            // writeTimeoutToolStripMenuItem
            // 
            this.writeTimeoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.writeTimeoutToolStripTextBox});
            this.writeTimeoutToolStripMenuItem.Name = "writeTimeoutToolStripMenuItem";
            this.writeTimeoutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.writeTimeoutToolStripMenuItem.Text = "Write Timeout (ms)";
            // 
            // writeTimeoutToolStripTextBox
            // 
            this.writeTimeoutToolStripTextBox.MaxLength = 10;
            this.writeTimeoutToolStripTextBox.Name = "writeTimeoutToolStripTextBox";
            this.writeTimeoutToolStripTextBox.Size = new System.Drawing.Size(75, 23);
            this.writeTimeoutToolStripTextBox.TextChanged += new System.EventHandler(this.writeTimeoutToolStripTextBox_TextChanged);
            // 
            // updateIntervalToolStripMenuItem
            // 
            this.updateIntervalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateIntervalToolStripTextBox});
            this.updateIntervalToolStripMenuItem.Name = "updateIntervalToolStripMenuItem";
            this.updateIntervalToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.updateIntervalToolStripMenuItem.Text = "Update Interval (ms)";
            // 
            // updateIntervalToolStripTextBox
            // 
            this.updateIntervalToolStripTextBox.MaxLength = 10;
            this.updateIntervalToolStripTextBox.Name = "updateIntervalToolStripTextBox";
            this.updateIntervalToolStripTextBox.Size = new System.Drawing.Size(75, 23);
            this.updateIntervalToolStripTextBox.TextChanged += new System.EventHandler(this.updateIntervalToolStripMenuItem_TextChanged);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLogAsToolStripMenuItem});
            this.loggingToolStripMenuItem.Enabled = false;
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.loggingToolStripMenuItem.Text = "Logging";
            // 
            // saveLogAsToolStripMenuItem
            // 
            this.saveLogAsToolStripMenuItem.Name = "saveLogAsToolStripMenuItem";
            this.saveLogAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveLogAsToolStripMenuItem.ShowShortcutKeys = false;
            this.saveLogAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveLogAsToolStripMenuItem.Text = "Save As...";
            this.saveLogAsToolStripMenuItem.Click += new System.EventHandler(this.saveLogAsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // formContextMenuStrip
            // 
            this.formContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.addGroupToolStripMenuItem});
            this.formContextMenuStrip.Name = "editFormContextMenuStrip";
            this.formContextMenuStrip.Size = new System.Drawing.Size(153, 70);
            this.formContextMenuStrip.Opened += new System.EventHandler(this.editFormContextMenuStrip_Opened);
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formNameToolStripTextBox});
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nameToolStripMenuItem.Text = "Name";
            // 
            // formNameToolStripTextBox
            // 
            this.formNameToolStripTextBox.Name = "formNameToolStripTextBox";
            this.formNameToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.formNameToolStripTextBox.Text = "OpenLab";
            this.formNameToolStripTextBox.TextChanged += new System.EventHandler(this.formTitleToolStripTextBox_TextChanged);
            // 
            // addGroupToolStripMenuItem
            // 
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addGroupToolStripMenuItem.Text = "Add Group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            // 
            // groupContextMenuStrip
            // 
            this.groupContextMenuStrip.Name = "editGroupBoxContextMenuStrip";
            this.groupContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // controlContextMenuStrip
            // 
            this.controlContextMenuStrip.Name = "editControlContextMenuStrip";
            this.controlContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "ControlForm";
            this.Text = "OpenLab";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.formContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem serialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem baudRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox baudRateToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem parityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataBitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopBitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readTimeoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox readTimeoutToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem writeTimeoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox writeTimeoutToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox updateIntervalToolStripTextBox;
        private System.Windows.Forms.ToolStripTextBox dataBitsToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox parityToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox stopBitsToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox portNameToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip formContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip groupContextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip controlContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox formNameToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem saveConfigAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLogAsToolStripMenuItem;
    }
}
