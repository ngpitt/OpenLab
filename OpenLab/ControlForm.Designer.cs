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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serialToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 28;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
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
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
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
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
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
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ControlForm";
            this.Text = "OpenLab";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
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
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

