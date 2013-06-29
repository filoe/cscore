namespace Recorder
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.appContainer = new System.Windows.Forms.SplitContainer();
            this.deviceslist = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDriverVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chbOutput = new System.Windows.Forms.CheckBox();
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.peakLeft = new System.Windows.Forms.ProgressBar();
            this.peakRight = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.appContainer)).BeginInit();
            this.appContainer.Panel1.SuspendLayout();
            this.appContainer.Panel2.SuspendLayout();
            this.appContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // appContainer
            // 
            this.appContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appContainer.Location = new System.Drawing.Point(0, 0);
            this.appContainer.Name = "appContainer";
            // 
            // appContainer.Panel1
            // 
            this.appContainer.Panel1.Controls.Add(this.deviceslist);
            // 
            // appContainer.Panel2
            // 
            this.appContainer.Panel2.Controls.Add(this.peakRight);
            this.appContainer.Panel2.Controls.Add(this.peakLeft);
            this.appContainer.Panel2.Controls.Add(this.chbOutput);
            this.appContainer.Panel2.Controls.Add(this.btnRefreshDevices);
            this.appContainer.Panel2.Controls.Add(this.btnStop);
            this.appContainer.Panel2.Controls.Add(this.btnStart);
            this.appContainer.Size = new System.Drawing.Size(683, 258);
            this.appContainer.SplitterDistance = 450;
            this.appContainer.TabIndex = 0;
            // 
            // deviceslist
            // 
            this.deviceslist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnChannels,
            this.columnDriverVersion});
            this.deviceslist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceslist.Location = new System.Drawing.Point(0, 0);
            this.deviceslist.MultiSelect = false;
            this.deviceslist.Name = "deviceslist";
            this.deviceslist.Size = new System.Drawing.Size(450, 258);
            this.deviceslist.TabIndex = 0;
            this.deviceslist.UseCompatibleStateImageBehavior = false;
            this.deviceslist.View = System.Windows.Forms.View.Details;
            this.deviceslist.SelectedIndexChanged += new System.EventHandler(this.deviceslist_SelectedIndexChanged);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 250;
            // 
            // columnChannels
            // 
            this.columnChannels.Text = "Kanäle";
            // 
            // columnDriverVersion
            // 
            this.columnDriverVersion.Text = "Treiberversion";
            this.columnDriverVersion.Width = 80;
            // 
            // chbOutput
            // 
            this.chbOutput.AutoSize = true;
            this.chbOutput.Enabled = false;
            this.chbOutput.Location = new System.Drawing.Point(14, 74);
            this.chbOutput.Name = "chbOutput";
            this.chbOutput.Size = new System.Drawing.Size(112, 17);
            this.chbOutput.TabIndex = 3;
            this.chbOutput.Text = "Realtime Ausgabe";
            this.chbOutput.UseVisualStyleBackColor = true;
            this.chbOutput.CheckedChanged += new System.EventHandler(this.chbOutput_CheckedChanged);
            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDevices.Location = new System.Drawing.Point(14, 223);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(203, 23);
            this.btnRefreshDevices.TabIndex = 2;
            this.btnRefreshDevices.Text = "Geräte aktualisieren";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(14, 44);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(203, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Aufnahme beenden";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(14, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(203, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Aufnahme starten";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // peakLeft
            // 
            this.peakLeft.Location = new System.Drawing.Point(14, 98);
            this.peakLeft.Maximum = 10000;
            this.peakLeft.Name = "peakLeft";
            this.peakLeft.Size = new System.Drawing.Size(203, 13);
            this.peakLeft.TabIndex = 4;
            // 
            // peakRight
            // 
            this.peakRight.Location = new System.Drawing.Point(14, 117);
            this.peakRight.Maximum = 10000;
            this.peakRight.Name = "peakRight";
            this.peakRight.Size = new System.Drawing.Size(203, 13);
            this.peakRight.TabIndex = 5;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 258);
            this.Controls.Add(this.appContainer);
            this.Name = "MainWindow";
            this.Text = "CSCore - Recorder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.appContainer.Panel1.ResumeLayout(false);
            this.appContainer.Panel2.ResumeLayout(false);
            this.appContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appContainer)).EndInit();
            this.appContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer appContainer;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView deviceslist;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnChannels;
        private System.Windows.Forms.ColumnHeader columnDriverVersion;
        private System.Windows.Forms.CheckBox chbOutput;
        private System.Windows.Forms.ProgressBar peakRight;
        private System.Windows.Forms.ProgressBar peakLeft;
    }
}

