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
            this.serialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateIntervalToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serialToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(442, 24);
            this.menuStrip1.TabIndex = 28;
            // 
            // serialToolStripMenuItem
            // 
            this.serialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem});
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
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.portToolStripMenuItem,
            this.updateIntervalToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // portToolStripMenuItem
            // 
            this.portToolStripMenuItem.Name = "portToolStripMenuItem";
            this.portToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.portToolStripMenuItem.Text = "Port";
            // 
            // updateIntervalToolStripMenuItem
            // 
            this.updateIntervalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateIntervalToolStripMenuItem1,
            this.updateIntervalToolStripMenuItem2,
            this.updateIntervalToolStripMenuItem3,
            this.updateIntervalToolStripMenuItem4,
            this.updateIntervalToolStripMenuItem5});
            this.updateIntervalToolStripMenuItem.Name = "updateIntervalToolStripMenuItem";
            this.updateIntervalToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.updateIntervalToolStripMenuItem.Text = "Update Interval";
            // 
            // updateIntervalToolStripMenuItem1
            // 
            this.updateIntervalToolStripMenuItem1.Name = "updateIntervalToolStripMenuItem1";
            this.updateIntervalToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.updateIntervalToolStripMenuItem1.Tag = "50";
            this.updateIntervalToolStripMenuItem1.Text = "50 ms";
            this.updateIntervalToolStripMenuItem1.Click += new System.EventHandler(this.updateIntervalToolStripMenuItem_Click);
            // 
            // updateIntervalToolStripMenuItem2
            // 
            this.updateIntervalToolStripMenuItem2.Name = "updateIntervalToolStripMenuItem2";
            this.updateIntervalToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.updateIntervalToolStripMenuItem2.Tag = "100";
            this.updateIntervalToolStripMenuItem2.Text = "100 ms";
            this.updateIntervalToolStripMenuItem2.Click += new System.EventHandler(this.updateIntervalToolStripMenuItem_Click);
            // 
            // updateIntervalToolStripMenuItem3
            // 
            this.updateIntervalToolStripMenuItem3.Name = "updateIntervalToolStripMenuItem3";
            this.updateIntervalToolStripMenuItem3.Size = new System.Drawing.Size(117, 22);
            this.updateIntervalToolStripMenuItem3.Tag = "250";
            this.updateIntervalToolStripMenuItem3.Text = "250 ms";
            this.updateIntervalToolStripMenuItem3.Click += new System.EventHandler(this.updateIntervalToolStripMenuItem_Click);
            // 
            // updateIntervalToolStripMenuItem4
            // 
            this.updateIntervalToolStripMenuItem4.Name = "updateIntervalToolStripMenuItem4";
            this.updateIntervalToolStripMenuItem4.Size = new System.Drawing.Size(117, 22);
            this.updateIntervalToolStripMenuItem4.Tag = "500";
            this.updateIntervalToolStripMenuItem4.Text = "500 ms";
            this.updateIntervalToolStripMenuItem4.Click += new System.EventHandler(this.updateIntervalToolStripMenuItem_Click);
            // 
            // updateIntervalToolStripMenuItem5
            // 
            this.updateIntervalToolStripMenuItem5.Name = "updateIntervalToolStripMenuItem5";
            this.updateIntervalToolStripMenuItem5.Size = new System.Drawing.Size(117, 22);
            this.updateIntervalToolStripMenuItem5.Tag = "1000";
            this.updateIntervalToolStripMenuItem5.Text = "1000 ms";
            this.updateIntervalToolStripMenuItem5.Click += new System.EventHandler(this.updateIntervalToolStripMenuItem_Click);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 368);
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
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem updateIntervalToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem serialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
    }
}

