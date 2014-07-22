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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFormContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.formTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formTitleToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.addGroupBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editGroupBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBoxTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxTitleToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.addControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeGroupBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editControlContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.controlTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlTitleToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.controlSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.editFormContextMenuStrip.SuspendLayout();
            this.editGroupBoxContextMenuStrip.SuspendLayout();
            this.editControlContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serialToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 28;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
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
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(130, 6);
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
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
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
            this.baudRateToolStripTextBox.Text = "115200";
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
            this.parityToolStripComboBox.Text = "None";
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
            this.dataBitsToolStripTextBox.Text = "8";
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
            this.stopBitsToolStripComboBox.Text = "1";
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
            this.readTimeoutToolStripTextBox.Text = "500";
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
            this.writeTimeoutToolStripTextBox.Text = "500";
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
            this.updateIntervalToolStripTextBox.Text = "500";
            this.updateIntervalToolStripTextBox.TextChanged += new System.EventHandler(this.updateIntervalToolStripMenuItem_TextChanged);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // editFormContextMenuStrip
            // 
            this.editFormContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formTitleToolStripMenuItem,
            this.addGroupBoxToolStripMenuItem});
            this.editFormContextMenuStrip.Name = "editFormContextMenuStrip";
            this.editFormContextMenuStrip.Size = new System.Drawing.Size(155, 48);
            this.editFormContextMenuStrip.Opened += new System.EventHandler(this.editFormContextMenuStrip_Opened);
            // 
            // formTitleToolStripMenuItem
            // 
            this.formTitleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formTitleToolStripTextBox});
            this.formTitleToolStripMenuItem.Name = "formTitleToolStripMenuItem";
            this.formTitleToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.formTitleToolStripMenuItem.Text = "Form Title";
            // 
            // formTitleToolStripTextBox
            // 
            this.formTitleToolStripTextBox.Name = "formTitleToolStripTextBox";
            this.formTitleToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.formTitleToolStripTextBox.Text = "OpenLab";
            this.formTitleToolStripTextBox.TextChanged += new System.EventHandler(this.formTitleToolStripTextBox_TextChanged);
            // 
            // addGroupBoxToolStripMenuItem
            // 
            this.addGroupBoxToolStripMenuItem.Name = "addGroupBoxToolStripMenuItem";
            this.addGroupBoxToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.addGroupBoxToolStripMenuItem.Text = "Add Group Box";
            this.addGroupBoxToolStripMenuItem.Click += new System.EventHandler(this.addGroupBoxToolStripMenuItem_Click);
            // 
            // editGroupBoxContextMenuStrip
            // 
            this.editGroupBoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupBoxTitleToolStripMenuItem,
            this.addControlToolStripMenuItem,
            this.removeGroupBoxToolStripMenuItem});
            this.editGroupBoxContextMenuStrip.Name = "editGroupBoxContextMenuStrip";
            this.editGroupBoxContextMenuStrip.Size = new System.Drawing.Size(176, 70);
            // 
            // groupBoxTitleToolStripMenuItem
            // 
            this.groupBoxTitleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupBoxTitleToolStripTextBox});
            this.groupBoxTitleToolStripMenuItem.Name = "groupBoxTitleToolStripMenuItem";
            this.groupBoxTitleToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.groupBoxTitleToolStripMenuItem.Text = "Group Box Title";
            // 
            // groupBoxTitleToolStripTextBox
            // 
            this.groupBoxTitleToolStripTextBox.Name = "groupBoxTitleToolStripTextBox";
            this.groupBoxTitleToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.groupBoxTitleToolStripTextBox.TextChanged += new System.EventHandler(this.groupBoxTitleToolStripTextBox_TextChanged);
            // 
            // addControlToolStripMenuItem
            // 
            this.addControlToolStripMenuItem.Name = "addControlToolStripMenuItem";
            this.addControlToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.addControlToolStripMenuItem.Text = "Add Control";
            // 
            // removeGroupBoxToolStripMenuItem
            // 
            this.removeGroupBoxToolStripMenuItem.Name = "removeGroupBoxToolStripMenuItem";
            this.removeGroupBoxToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.removeGroupBoxToolStripMenuItem.Text = "Remove Group Box";
            this.removeGroupBoxToolStripMenuItem.Click += new System.EventHandler(this.removeGroupBoxToolStripMenuItem_Click);
            // 
            // editControlContextMenuStrip
            // 
            this.editControlContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlTitleToolStripMenuItem,
            this.controlSettingsToolStripMenuItem,
            this.removeControlToolStripMenuItem});
            this.editControlContextMenuStrip.Name = "editControlContextMenuStrip";
            this.editControlContextMenuStrip.Size = new System.Drawing.Size(161, 70);
            // 
            // controlTitleToolStripMenuItem
            // 
            this.controlTitleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlTitleToolStripTextBox});
            this.controlTitleToolStripMenuItem.Name = "controlTitleToolStripMenuItem";
            this.controlTitleToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.controlTitleToolStripMenuItem.Text = "Control Title";
            // 
            // controlTitleToolStripTextBox
            // 
            this.controlTitleToolStripTextBox.Name = "controlTitleToolStripTextBox";
            this.controlTitleToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.controlTitleToolStripTextBox.TextChanged += new System.EventHandler(this.controlTitleToolStripTextBox_TextChanged);
            // 
            // controlSettingsToolStripMenuItem
            // 
            this.controlSettingsToolStripMenuItem.Name = "controlSettingsToolStripMenuItem";
            this.controlSettingsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.controlSettingsToolStripMenuItem.Text = "Control Settings";
            // 
            // removeControlToolStripMenuItem
            // 
            this.removeControlToolStripMenuItem.Name = "removeControlToolStripMenuItem";
            this.removeControlToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.removeControlToolStripMenuItem.Text = "Remove Control";
            this.removeControlToolStripMenuItem.Click += new System.EventHandler(this.removeControlToolStripMenuItem_Click);
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
            this.editFormContextMenuStrip.ResumeLayout(false);
            this.editGroupBoxContextMenuStrip.ResumeLayout(false);
            this.editControlContextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip editFormContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addGroupBoxToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip editGroupBoxContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem groupBoxTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addControlToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip editControlContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem controlTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox groupBoxTitleToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem removeGroupBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox formTitleToolStripTextBox;
        private System.Windows.Forms.ToolStripTextBox controlTitleToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
    }
}
